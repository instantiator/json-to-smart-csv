using System.Globalization;
using CsvHelper;

namespace JsonToSmartCsv.Rules.Csv;

public class CsvRulesReader
{
    public static CsvRulesSet FromFile(string path)
    {
        using (var reader = new StreamReader(path))
        using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
        {
            var records = csv.GetRecords<CsvColumnRule>().ToList();
            return new CsvRulesSet(records);
        }
    }
}