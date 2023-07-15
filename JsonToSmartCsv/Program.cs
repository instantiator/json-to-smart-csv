using JsonToSmartCsv.Builder;
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
        var configFile_csv = args[0];
        if (string.IsNullOrWhiteSpace(configFile_csv) || !File.Exists(configFile_csv))
        {
            Console.WriteLine("Please provide a column specification config file.");
            return;
        }

        var sourceFile_json = args[1];
        if (string.IsNullOrWhiteSpace(sourceFile_json) || !File.Exists(sourceFile_json))
        {
            Console.WriteLine("Please provide a source json file.");
            return;
        }

        var targetFile_csv = args[2];
        if (string.IsNullOrWhiteSpace(targetFile_csv))
        {
            Console.WriteLine("Please provide a target file to create.");
            return;
        }

        ProcessingMode mode;
        var modeOk = Enum.TryParse<ProcessingMode>(args[3], out mode);
        if (!modeOk)
        {
            Console.WriteLine($"Please provide a valid processing mode: {string.Join(", ", Enum.GetValues<ProcessingMode>())}");
            return;
        }

        Console.WriteLine($"Reading rules: {configFile_csv}");
        var rules = RulesReader.FromFile(configFile_csv);

        Console.WriteLine($"Reading source json: {sourceFile_json}");
        var source = JToken.Parse(File.ReadAllText(sourceFile_json));

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

