using JsonToSmartCsv.Rules.Csv;
using Newtonsoft.Json.Linq;
using System.Linq;

namespace JsonToSmartCsv.Builder.Csv;

public enum Aggregation { Sum, Avg, Max, Min, Count }

public class CsvRuleRecordBuilder
{
    public static IEnumerable<string> GetHeaders(CsvRulesSet rules)
    {
        return rules.ColumnRules.Select(col => col.TargetColumn!);
    }

    public static IEnumerable<IEnumerable<object?>> BuildRecords(JToken source, CsvRulesSet rules)
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

    private static IEnumerable<object?> BuildRecord(JToken root, CsvRulesSet rules)
    {
        var record = new List<object?>();
        foreach (var col in rules.ColumnRules)
        {
            var token = root.SelectToken(col.SourcePath!);
            if (token == null) { throw new Exception($"Source path {col.SourcePath} not found in source."); }

            switch (col.Interpretation)
            {
                case CsvSourceInterpretation.AsString:
                    record.Add(token?.Value<string?>());
                    break;
                case CsvSourceInterpretation.AsDecimal:
                    record.Add(token?.Value<decimal>());
                    break;
                case CsvSourceInterpretation.AsInteger:
                    record.Add(token?.Value<int>());
                    break;
                case CsvSourceInterpretation.AsBoolean:
                    record.Add(token?.Value<bool>());
                    break;
                case CsvSourceInterpretation.AsJson:
                    record.Add(token?.ToString(Newtonsoft.Json.Formatting.Indented));
                    break;
                case CsvSourceInterpretation.AsCount:
                    record.Add(CountToken(token));
                    break;
                case CsvSourceInterpretation.AsConcatenation:
                    record.Add(ConcatenateElements(token, col.InterpretationArg1!, col.InterpretationArg2!));
                    break;
                case CsvSourceInterpretation.AsAggregate:
                    record.Add(AggregateElements(token, col.InterpretationArg1!, col.InterpretationArg2!));
                    break;
                default:
                    throw new Exception($"Unexpected interpretation: {col.Interpretation}");
            }
        }
        return record;
    }

    private static int? CountToken(JToken? token)
    {
        if (token is JArray) {
            return (token as JArray)?.Count;
        } else {
            return token == null ? 0 : 1;
        }
    }

    public static string? ConcatenateElements(JToken? token, string path, string separator)
    {
        if (token == null) { return null; }
        if (token is JArray)
        {
            return string.Join(separator, (token as JArray)!.Select(item => item.SelectToken(path)?.Value<string?>()));
        }
        else
        {
            return token.SelectToken(path)?.Value<string?>();
        }
    }

    public static decimal AggregateElements(JToken? token, string path, string aggStr)
    {
        var aggregation = Enum.Parse<Aggregation>(aggStr, true);

        if (token == null) { return 0; }
        if (token is JArray)
        {
            switch (aggregation)
            {
                case Aggregation.Sum:
                    return (token as JArray)!.Sum(item => item.SelectToken(path)?.Value<decimal>() ?? 0);
                case Aggregation.Avg:
                    return (token as JArray)!.Average(item => item.SelectToken(path)?.Value<decimal>() ?? 0);
                case Aggregation.Max:
                    return (token as JArray)!.Max(item => item.SelectToken(path)?.Value<decimal>() ?? 0);
                case Aggregation.Min:
                    return (token as JArray)!.Min(item => item.SelectToken(path)?.Value<decimal>() ?? 0);
                case Aggregation.Count:
                    return (token as JArray)!.Count();
                default:
                    throw new Exception($"Unexpected aggregation: {aggregation}");
            }
        }
        else
        {
            switch (aggregation)
            {
                case Aggregation.Sum:
                case Aggregation.Avg:
                case Aggregation.Max:
                case Aggregation.Min:
                    return token.SelectToken(path)?.Value<decimal>() ?? 0;
                case Aggregation.Count:
                    return token.SelectToken(path)?.Value<decimal>() == null ? 0 : 1;
                default:
                    throw new Exception($"Unexpected aggregation: {aggregation}");
            }
        }
    }
}