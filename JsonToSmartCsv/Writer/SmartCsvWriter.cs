using System.Globalization;
using CsvHelper;
using JsonToSmartCsv.Rules;

namespace JsonToSmartCsv.Writer;

public class SmartCsvWriter
{
    public static void Write(string path, IEnumerable<IEnumerable<object?>> records, RulesSet rules, ProcessingMode mode)
    {
        var targetExists = File.Exists(path);
        var append = mode == ProcessingMode.Append;
        var createHeader = !append || !targetExists;

        using (var writer = new StreamWriter(path, append))
        using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
        {
            // add header
            if (createHeader)
            {
                foreach (var col in rules.ColumnRules)
                {
                    csv.WriteField(col.TargetColumn);
                }
                csv.NextRecord();
            }

            // add records
            foreach (var record in records)
            {
                if (record != null) {
                    foreach (var field in record)
                    {
                        csv.WriteField(field);
                    }
                    csv.NextRecord();
                }
            }

            csv.Flush();
        }    
    }
}
