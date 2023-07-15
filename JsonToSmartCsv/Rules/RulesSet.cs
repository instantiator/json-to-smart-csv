namespace JsonToSmartCsv.Rules;

public class RulesSet
{
    public RulesSet(IEnumerable<ColumnRule> rules)
    {
        ColumnRules = rules;
    }
    public IEnumerable<ColumnRule> ColumnRules { get; }
}