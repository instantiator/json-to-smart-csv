using System.Globalization;
using CsvHelper;

namespace JsonToSmartCsv.Rules;

public class RulesReader
{
    public static RulesSet FromFile(string path)
    {
        using (var reader = new StreamReader(path))
        using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
        {
            var records = csv.GetRecords<ColumnRule>().ToList();
            return new RulesSet(records);
        }
    }
}