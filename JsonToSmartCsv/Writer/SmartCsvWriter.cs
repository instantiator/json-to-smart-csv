using System.Globalization;
using CsvHelper;
using JsonToSmartCsv.Builder;

namespace JsonToSmartCsv.Writer;

public class SmartCsvWriter
{
    public static void Write(string path, DataTable table, ProcessingMode mode)
    {
        var targetExists = File.Exists(path);
        var append = mode == ProcessingMode.Append;
        var createHeader = !append || !targetExists;

        var headers = table.Headers;

        using (var writer = new StreamWriter(path, append))
        using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
        {
            // add header
            if (createHeader)
            {
                foreach (var col in headers)
                {
                    csv.WriteField(col);
                }
                csv.NextRecord();
            }

            // add records
            foreach (var record in table.Data.Where(r => r != null))
            {
                foreach (var col in headers)
                {
                    csv.WriteField(record.ContainsKey(col) ? record[col] : null);
                }
                csv.NextRecord();
            }
            csv.Flush();
        }    
    }
}
