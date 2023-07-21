namespace JsonToSmartCsv.Rules.Json;

public class JsonRuleSet
{
    public string? root { get; set; }
    public IEnumerable<JsonRule>? rules { get; set; }
}