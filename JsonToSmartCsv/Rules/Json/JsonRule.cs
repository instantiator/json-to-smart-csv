namespace JsonToSmartCsv.Rules.Json;

public class JsonRule
{
    public string? path { get; set; }
    public string? target { get; set; }
    public JsonInterpretation? interpretation { get; set; }
    public IEnumerable<JsonRule>? children { get; set; }
}