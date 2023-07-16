using JsonToSmartCsv.Builder;
using JsonToSmartCsv.Reader;
using JsonToSmartCsv.Rules;
using JsonToSmartCsv.Writer;
using Newtonsoft.Json.Linq;

namespace JsonToSmartCsv;

public enum ProcessingMode
{
    Append,
    Create
}

public class Program
{
    public static void Main(string[] args)
    {
        var colsFile_csv = args[0];
        var sourceFile_json = args[1];
        var root_path = args[2];
        var targetFile_csv = args[3];
        var modeStr = args[4];

        if (string.IsNullOrWhiteSpace(colsFile_csv) || !File.Exists(colsFile_csv))
        {
            Console.WriteLine("Please provide a column specification config file.");
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

        ProcessingMode mode;
        var modeOk = Enum.TryParse<ProcessingMode>(modeStr, out mode);
        if (!modeOk)
        {
            Console.WriteLine($"Please provide a valid processing mode. Choices are: {string.Join(", ", Enum.GetValues<ProcessingMode>())}");
            return;
        }

        var root = root_path ?? "$";

        Console.WriteLine($"Reading rules: {colsFile_csv}");
        var rules = RulesReader.FromFile(colsFile_csv);

        Console.WriteLine($"Reading source json: {sourceFile_json}");
        var source = SmartJsonReader.Read(sourceFile_json, root);

        Console.WriteLine($"Preparing records...");
        var records = RecordBuilder.BuildRecords(source, rules);
        Console.WriteLine($"Created {records.Count()} records.");

        if (mode == ProcessingMode.Create && File.Exists(targetFile_csv))
        {
            var backup = $"{targetFile_csv}.backup";
            Console.WriteLine($"Backing up existing target to: {backup}");
            if (File.Exists(backup)) { File.Delete(backup); }
            File.Move(targetFile_csv, backup);
        }

        Console.WriteLine($"Writing {records.Count()} records to target: {targetFile_csv}");
        SmartCsvWriter.Write(targetFile_csv, records, rules, mode);
    }
}

