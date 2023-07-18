using WiredBrainCoffee.DataProcessor.Data;
using WiredBrainCoffee.DataProcessor.Model;
using WiredBrainCoffee.DataProcessor.Parsing;
using WiredBrainCoffee.DataProcessor.Processing;

Console.WriteLine("_______________________________________");
Console.WriteLine("  Wired Brain Coffee - Data Processor  ");
Console.WriteLine("_______________________________________");
Console.WriteLine();

const string fileName = "CoffeeMachineData.csv";
string[] scvLines = File.ReadAllLines(fileName);

MachineDataItem[] machineDataItems = CsvLineParser.Parse(scvLines);

var machineDataProcessor = new MachineDataProcessor(new ConsoleCoffeeCountStore());
machineDataProcessor.ProcessItems(machineDataItems);

Console.WriteLine();
Console.WriteLine($"File {fileName} was successfully processed");