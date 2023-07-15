using JsonToSmartCsv.Rules;
using Newtonsoft.Json.Linq;
using System.Linq;

namespace JsonToSmartCsv.Builder;

public class RecordBuilder
{
    public static IEnumerable<string> GetHeaders(RulesSet rules)
    {
        return rules.ColumnRules.Select(col => col.TargetColumn!);
    }

    public static IEnumerable<IEnumerable<object?>> BuildRecords(JToken source, RulesSet rules)
    {
        if (source is JArray)
        {
            return (source as JArray)!.Select(item => BuildRecord(item, rules));
        }
        else if (source is JObject)
        {
            return new[] { BuildRecord(source, rules) };
        }
        else
        {
            throw new Exception($"Unexpected source type: {source.GetType().Name}");
        }
    }

    private static IEnumerable<object?> BuildRecord(JToken root, RulesSet rules)
    {
        var record = new List<object?>();
        foreach (var col in rules.ColumnRules)
        {
            var token = root.SelectToken(col.SourcePath!);
            if (token == null) { throw new Exception($"Source path {col.SourcePath} not found in source."); }

            switch (col.Interpretation)
            {
                case SourceInterpretation.AsString:
                    record.Add(token?.Value<string?>());
                    break;
                case SourceInterpretation.AsDecimal:
                    record.Add(token?.Value<decimal>());
                    break;
                case SourceInterpretation.AsInteger:
                    record.Add(token?.Value<int>());
                    break;
                case SourceInterpretation.AsBoolean:
                    record.Add(token?.Value<bool>());
                    break;
                case SourceInterpretation.AsObjectAsJson:
                    record.Add(token?.ToString(Newtonsoft.Json.Formatting.Indented));
                    break;
                case SourceInterpretation.AsListAsJson:
                    record.Add(token?.ToString(Newtonsoft.Json.Formatting.Indented));
                    break;
                case SourceInterpretation.AsListCount:
                    record.Add((token as JArray)?.Count);
                    break;
                default:
                    throw new Exception($"Unexpected interpretation: {col.Interpretation}");
            }
        }
        return record;
    }

}