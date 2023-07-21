using CommandLine;
using JsonToSmartCsv.Builder;
using JsonToSmartCsv.Builder.Json;
using JsonToSmartCsv.Reader;
using JsonToSmartCsv.Rules;
using JsonToSmartCsv.Rules.Json;
using JsonToSmartCsv.Writer;
using Newtonsoft.Json.Linq;

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

    [Option('r', "root", Required = false, HelpText = "Root path", Default = "$")]
    public string? Root { get; set; }
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
Path:
  Provide a JSON path to the root note to process. The default is: $ROOT
  If this points to a single object, {}, the object will be processed to a single row.
  If this points to an array, [], each object in the array will be processed to a row.

Modes:
  Create = create a new target file, backup any existing file
  Append = append to the target file (if it exists)

Provide the following columns in your column definitions CSV file:
  TargetColumn         = the name of the column in the target CSV file (string, eg. ""id"")
  SourcePath           = a relative path to a field in the current JSON object (string, eg. ""$.id"")
  SourceInterpretation = how to interpret the value of the field (see below)
  InterpretationArg1   = supplementary information about how to interpret the field (optional)
  InterpretationArg2   = supplementary information about how to interpret the field (optional)

Source interpretations:
  AsString             = as a standard string
  AsDecimal            = as a decimal number
  AsInteger            = as an integer
  AsBoolean            = as a boolean (true/false)
  AsJson               = as a JSON string, representing the object or list (array)
  AsCount              = as the number of items in the list (array) or 1 (object) or 0 (not present)
  AsConcatenation      = as a concatenation of strings found inside the element (array) or a single string (object)
    InterpretationArg1 = a a relative path inside each element to retrieve values
    InterpretationArg2 = the separator to use between values
  AsAggregation        = as an aggregate of sub-elements inside this object or array
    InterpretationArg1 = a a relative path inside each element to retrieve values
    InterpretationArg2 = the aggregation (Sum, Avg, Min, Max, Count)
");
    }

    private static void RunOptions(Options options)
    {
        var colsFile_json = options.ColumnsFile;
        var sourceFile_json = options.SourceFile;
        var root_path = options.Root;
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

        var root = root_path ?? "$";

        Console.WriteLine($"Reading rules: {colsFile_json}");
        var rules = JsonRulesReader.FromFile(colsFile_json);

        Console.WriteLine($"Reading source json: {sourceFile_json}");
        var source = SmartJsonReader.Read(sourceFile_json, root);

        Console.WriteLine($"Preparing transient records...");
        var records = new JsonRuleRecordBuilder(rules).BuildRecords(source);
        Console.WriteLine($"Created {records.Rows} records.");

        // if (mode == ProcessingMode.Create && File.Exists(targetFile_csv))
        // {
        //     var backup = $"{targetFile_csv}.backup";
        //     Console.WriteLine($"Backing up existing target to: {backup}");
        //     if (File.Exists(backup)) { File.Delete(backup); }
        //     File.Move(targetFile_csv, backup);
        // }

        // Console.WriteLine($"Writing {records.Rows} records to target: {targetFile_csv}");
        // SmartCsvWriter.Write(targetFile_csv, records, rules, mode);
    }
}

