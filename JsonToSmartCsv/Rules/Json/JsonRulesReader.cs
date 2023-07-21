using Newtonsoft.Json;

namespace JsonToSmartCsv.Rules.Json;

public class JsonRulesReader
{
    public static JsonRuleSet FromFile(string path) => FromString(File.ReadAllText(path));
    public static JsonRuleSet FromString(string json) => JsonConvert.DeserializeObject<JsonRuleSet>(json)!;
}