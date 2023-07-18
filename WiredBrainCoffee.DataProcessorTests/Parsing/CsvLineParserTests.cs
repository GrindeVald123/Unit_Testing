namespace WiredBrainCoffee.DataProcessor.Parsing;

public class CsvLineParserTests
{
    [Fact]
    public void ShouldParseValidLine()
    {
        // Arrange.
        string[] csvLines = new[] { "Copuchino;07/08/2023 13:23:38" };

        // Act.
        var machineDataItems = CsvLineParser.Parse(csvLines);

        // Assert.
        Assert.NotNull(machineDataItems);
        Assert.Single(machineDataItems);
        Assert.Equal("Copuchino", machineDataItems[0].CoffeeType);
        Assert.Equal(new DateTime(2023, 08, 07, 13, 23, 38), machineDataItems[0].CreatedAt);
    }

    [Fact]
    public void ShouldSkipEmptyLines()
    {
        // Arrange.
        string[] csvLines = new[] { "", " " };

        // Act.
        var machineDataItems = CsvLineParser.Parse(csvLines);

        // Assert.
        Assert.NotNull(machineDataItems);
        Assert.Empty(machineDataItems);
    }

    [InlineData("Copuchino", "Invalid csv line")]
    [InlineData("Copuchino;InvalidDateTime", "Invalid datetime in csv line")]
    [Theory]
    public void ShouldThrowExceptionForInvalidLine(string csvLine, string expectedMessagePrefix)
    {
        // Arrange.
        var csvLines = new[] { csvLine };

        // Act and Assert.
        var exception = Assert.Throws<Exception>(()=>  CsvLineParser.Parse(csvLines));

        Assert.Equal($"{expectedMessagePrefix}: {csvLine}", exception.Message);
    }
}