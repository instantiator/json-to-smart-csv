using CommandLine;
using JsonToSmartCsv.Builder;
using JsonToSmartCsv.Reader;
using JsonToSmartCsv.Rules.Json;
using JsonToSmartCsv.Writer;

namespace JsonToSmartCsv;

public enum ProcessingMode
{
    Append,
    Create
}

public class Options
{
    [Option('c', "columns", Required = true, HelpText = "Column definitions CSV file.")]
    public string? ColumnsFile { get; set; }

    [Option('s', "source", Required = true, HelpText = "Source data JSON file.")]
    public string? SourceFile { get; set; }

    [Option('t', "target", Required = true, HelpText = "Target CSV file.")]
    public string? TargetFile { get; set; }

    [Option('m', "mode", Required = false, HelpText = "Write mode (Append or Create)", Default = ProcessingMode.Create)]
    public ProcessingMode Mode { get; set; }
}

public class Program
{
    public static void Main(string[] args)
    {
        CommandLine.Parser.Default.ParseArguments<Options>(args)
            .WithParsed(RunOptions)
            .WithNotParsed(HandleParseError);
    }

    private static void HandleParseError(IEnumerable<Error> errs)
    {
        Console.WriteLine(@"
Modes:

1. Create = create a new target file, backup any existing file
2. Append = append to the target file (if it exists)

Provide column configuration as a JSON file:

{
    ""root"": <string>, // topmost object to process, default: ""$""
    ""rules"":          // array of rules defining columns
    [
        {
            ""path"": <string>           // relative path to the field in the current object
            ""target"": <string>         // name of the column in the target CSV file
            ""interpretation"": <string> // how to interpret the value of the field (see below)
            ""children"": []             // optional array of rules to apply to nested objects and lists
        }
    ]
}

Interpretations:

""AsString""                - interpret this value as a string
""AsNumber""                - interpret this value as an integer or decimal number
""AsBoolean""               - interpret this value as a boolean (true/false)
""AsJson""                  - convert this object or list to a JSON string
""IterateListItems""        - apply child rules to the items in this list
""IteratePropertiesAsList"" - apply child rules to the object properties, as if a list
""AsIndex""                 - item's index (IterateListItems), or property (IteratePropertiesAsList)
""AsAggregateSum""          - aggregate and sum all numeric values from child rules
""AsAggregateMax""          - aggregate and find the max of numeric values from child rules
""AsAggregateMin""          - aggregate and find the min of numeric values from child rules
""AsAggregateAvg""          - aggregate and find the mean of numeric values from child rules

Coming soon:

""WithPropertiesAsColumns"" - Not yet implemented, a shortcut to transform an object to columns

Data types:

The root item of a JSON document is either an object or a list.

1. If an object, you can start to apply rules with ""$.property"" paths
2. If it's a list indexed by properties, you might use ""IteratePropertiesAsList""
2. If it's a regular list, you'll want to apply an ""IterateListItems"" rule

Further help:

For more information and examples, see:
https://github.com/instantiator/json-to-smart-csv
");
    }

    private static void RunOptions(Options options)
    {
        var colsFile_json = options.ColumnsFile;
        var sourceFile_json = options.SourceFile;
        var targetFile_csv = options.TargetFile;
        var mode = options.Mode;

        if (string.IsNullOrWhiteSpace(colsFile_json) || !File.Exists(colsFile_json))
        {
            Console.WriteLine("Please provide a column specification json config file.");
            return;
        }

        if (string.IsNullOrWhiteSpace(sourceFile_json) || !File.Exists(sourceFile_json))
        {
            Console.WriteLine("Please provide a source json file.");
            return;
        }

        if (string.IsNullOrWhiteSpace(targetFile_csv))
        {
            Console.WriteLine("Please provide a target file to create.");
            return;
        }

        Console.WriteLine($"Reading rules: {colsFile_json}");
        var rules = JsonRulesReader.FromFile(colsFile_json);

        Console.WriteLine($"Reading source json: {sourceFile_json}");
        var source = SmartJsonReader.Read(sourceFile_json);

        Console.WriteLine($"Preparing intermediary tree...");
        var tree = new JsonTreeBuilder(rules).BuildTree(source);

        Console.WriteLine($"Preparing table...");
        var table = DataTableBuilder.BuildTableFromTree(tree);
        Console.WriteLine($"Created table with {table.Rows} rows.");

        if (mode == ProcessingMode.Create && File.Exists(targetFile_csv))
        {
            var backup = $"{targetFile_csv}.backup";
            Console.WriteLine($"Backing up existing target to: {backup}...");
            if (File.Exists(backup)) { File.Delete(backup); }
            File.Move(targetFile_csv, backup);
        }

        Console.WriteLine($"Writing output CSV to: {targetFile_csv}");
        SmartCsvWriter.Write(targetFile_csv, table, mode);
    }
}

