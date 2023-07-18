using WiredBrainCoffee.DataProcessor.Data;
using WiredBrainCoffee.DataProcessor.Model;

namespace WiredBrainCoffee.DataProcessor.Processing;

public class MachineDataProcessorTests : IDisposable
{
    private readonly FakeCoffeeCountStore _coffeeCountStore;
    private readonly MachineDataProcessor _machineDataProcessor;

    public MachineDataProcessorTests() 
    {
        _coffeeCountStore = new FakeCoffeeCountStore();
        _machineDataProcessor = new MachineDataProcessor(_coffeeCountStore);
    }


    [Fact]
    public void ShouldIgnoreItemsThatAreNotNewer()
    {
        // Arrange.
        var items = new[]
        {
            new MachineDataItem("Espresso", new DateTime(2023, 08, 07, 13, 0, 0)),
            new MachineDataItem("Espresso", new DateTime(2023, 08, 07, 12, 0, 0)),
            new MachineDataItem("Espresso", new DateTime(2023, 08, 07, 12, 10, 0)),
            new MachineDataItem("Espresso", new DateTime(2023, 08, 07, 15, 0, 0)),
            new MachineDataItem("Cappuccino", new DateTime(2023, 08, 07, 19, 0, 0)),
            new MachineDataItem("Cappuccino", new DateTime(2023, 08, 07, 19, 0, 0))
        };

        //Act
        _machineDataProcessor.ProcessItems(items);

        //Assert
        Assert.Equal(2, _coffeeCountStore.SavedItems.Count);

        var item = _coffeeCountStore.SavedItems[0];
        Assert.Equal("Espresso", item.CoffeeType);
        Assert.Equal(2, item.Count);

        item = _coffeeCountStore.SavedItems[1];
        Assert.Equal("Cappuccino", item.CoffeeType);
        Assert.Equal(1, item.Count);
    }

    [Fact]
    public void ShouldSaveCountPerCoffeeType()
    {
        // Arrange.
        var items = new[]
        {
            new MachineDataItem("Cappuccino", new DateTime(2023, 08, 07, 12, 0, 0)),
            new MachineDataItem("Cappuccino", new DateTime(2023, 08, 07, 13, 0, 0)),
            new MachineDataItem("Cappuccino", new DateTime(2023, 08, 07, 14, 0, 0)),
            new MachineDataItem("Espresso", new DateTime(2023, 08, 07, 15, 0, 0)),
        };

        //Act
        _machineDataProcessor.ProcessItems(items);

        //Assert
        Assert.Equal(2, _coffeeCountStore.SavedItems.Count);

        var item = _coffeeCountStore.SavedItems[0];
        Assert.Equal("Cappuccino", item.CoffeeType);
        Assert.Equal(3, item.Count);

        item = _coffeeCountStore.SavedItems[1];
        Assert.Equal("Espresso", item.CoffeeType);
        Assert.Equal(1, item.Count);
    }

    [Fact]
    public void ShouldClearPreviosCoffeeCount()
    {
        // Arrange.
        var items = new[]
        {
            new MachineDataItem("Espresso", new DateTime(2023, 08, 07, 13, 0, 0)),
        };

        //Act
        _machineDataProcessor.ProcessItems(items);
        _machineDataProcessor.ProcessItems(items);

        // Assert
        Assert.Equal(2, _coffeeCountStore.SavedItems.Count);
        foreach (var item in _coffeeCountStore.SavedItems)
        {
            Assert.Equal("Espresso", item.CoffeeType);
            Assert.Equal(1, item.Count);
        }
    }

    public void Dispose()
    {
        // This runs after every test.
    }
}

public class FakeCoffeeCountStore : ICoffeeCountStore
{
    public List<CoffeeCountItem> SavedItems { get; } = new();
    public void Save(CoffeeCountItem item)
    {
        SavedItems.Add(item);
    }
}
