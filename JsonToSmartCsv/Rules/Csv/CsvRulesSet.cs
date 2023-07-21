namespace JsonToSmartCsv.Rules.Csv;

public class CsvRulesSet
{
    public CsvRulesSet(IEnumerable<CsvColumnRule> rules)
    {
        ColumnRules = rules;
    }
    public IEnumerable<CsvColumnRule> ColumnRules { get; }
}