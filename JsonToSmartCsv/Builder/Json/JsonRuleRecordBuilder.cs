using System.Collections.Immutable;
using System.Data;
using System.Text;
using JsonToSmartCsv.Rules.Json;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace JsonToSmartCsv.Builder.Json;

public class JsonRuleRecordBuilder
{
    private JsonRuleSet rules;

    public JsonRuleRecordBuilder(JsonRuleSet rules)
    {
        this.rules = rules;
    }

    public DataTable BuildRecords(JToken source)
    {
        var path = rules.root ?? "$";
        var token = source.SelectToken(path);

        // special case - if the root is an object, give it a single empty row, otherwise an empty table
        // it should be populated as a list as the first instruction
        DataTable data = (token is JObject) ? new DataTable(new List<DataRow> { new DataRow() }) : new DataTable();

        // apply each top level rule
        foreach (var rule in rules.rules!)
        {
            if (token != null)
            {
                data = ApplyRule(data, token, rule);
            }
            else
            {
                throw new Exception($"Token at {path} not found in source.");
            }
        }

        return data;
    }

    private DataTable ApplyRule(DataTable table, JToken token, JsonRule rule)
    {
        var newTable = table.Duplicate();
        var selectedToken = token?.SelectToken(rule.path!);
        switch (rule.interpretation)
        {
            case JsonInterpretation.AsString:
                return ApplySingleValue(newTable, rule.target!, selectedToken?.Value<string?>());

            case JsonInterpretation.AsNumber:
                return ApplySingleValue(newTable, rule.target!, selectedToken?.Value<decimal?>());

            case JsonInterpretation.AsBoolean:
                var value = InterpretBool(selectedToken);
                return ApplySingleValue(newTable, rule.target!, value);

            case JsonInterpretation.AsJson:
                var json = selectedToken?.ToString(Formatting.Indented);
                return ApplySingleValue(newTable, rule.target!, json);

            case JsonInterpretation.IterateListItems:
                var items = GetListItems(selectedToken, rule.path!);
                return BuildAndConcatAndApplyItemTables(newTable, items, rule);

            case JsonInterpretation.IteratePropertiesAsList:
                var props = GetObjectProperties(selectedToken, rule.path!);
                return BuildAndConcatAndApplyItemTables(newTable, props, rule);

            //case JsonInterpretation.AsAggregateSum:
            //    var sumTable = BuildAggregationTable(selectedToken, rule.children);
            //    var sumItemRows = GetNumericItemRows(sumTable);
            //    return ApplySingleValue(newTable, rule.target!, sumItems.Sum());

            //case JsonInterpretation.AsAggregateMax:
            //    var maxTable = BuildAggregationTable(selectedToken, rule.children);
            //    var maxItemRows = GetNumericItemRows(maxTable);
            //    return ApplySingleValue(newTable, rule.target!, maxItems.Max());

            //case JsonInterpretation.AsAggregateMin:
            //    var minTable = BuildAggregationTable(selectedToken, rule.children);
            //    var minItemsRows = GetNumericItemRows(minTable);
            //    return ApplySingleValue(newTable, rule.target!, minItems.Min());

            //case JsonInterpretation.AsAggregateAvg:
            //    var avgTable = BuildAggregationTable(selectedToken, rule.children);
            //    var avgItemRows = GetNumericItemRows(avgTable);
            //    var avgItems = avgItemRows.SelectMany(i => i);
            //    return ApplySingleValue(newTable, rule.target!, avgItems.Average());

            case JsonInterpretation.WithPropertiesAsColumns:
                throw new NotImplementedException("This is shorthand - not implemented yet");

            default:
                throw new NotImplementedException($"Unknown interpretation: {rule.interpretation}");
        }
    }    

    private DataTree BuildDataTree(JToken? token, IEnumerable<JsonRule>? rules)
    {
        var tree = new DataTree();
        if (token == null) { return tree; }
        foreach (var rule in rules ?? new JsonRule[0])
        {
            var selectedToken = token?.SelectToken(rule.path!);
            switch (rule.interpretation)
            {
                case JsonInterpretation.AsString:
                    tree.Items.Add(rule.target!, selectedToken?.Value<string?>());
                    break;
                case JsonInterpretation.AsNumber:
                    tree.Items.Add(rule.target!, selectedToken?.Value<decimal?>());
                    break;
                case JsonInterpretation.AsBoolean:
                    tree.Items.Add(rule.target!, InterpretBool(selectedToken));
                    break;
                case JsonInterpretation.AsJson:
                    tree.Items.Add(rule.target!, selectedToken?.ToString(Formatting.Indented));
                    break;
                case JsonInterpretation.IterateListItems:
                    var asArray = selectedToken as JArray;
                    foreach (var item in asArray ?? new JArray())
                        tree.Items.Add(rule.target!, BuildDataTree(item, rule.children));
                    break;
                case JsonInterpretation.IteratePropertiesAsList:
                    var asObject = selectedToken as JObject;
                    foreach (var prop in asObject?.Properties() ?? new JProperty[0])
                        tree.Items.Add(rule.target!, BuildDataTree(prop.Value, rule.children));
                    break;
                case JsonInterpretation.AsAggregateAvg:
                    var avgTree = BuildDataTree(selectedToken, rule.children);
                    var avgNumbers = GetNumbers(avgTree);
                    tree.Items.Add(rule.target!, avgNumbers.Average());
                    break;
                case JsonInterpretation.AsAggregateSum:
                    var sumTree = BuildDataTree(selectedToken, rule.children);
                    var sumNumbers = GetNumbers(sumTree);
                    tree.Items.Add(rule.target!, sumNumbers.Sum());
                    break;
                case JsonInterpretation.AsAggregateMax:
                    var maxTree = BuildDataTree(selectedToken, rule.children);
                    var maxNumbers = GetNumbers(maxTree);
                    tree.Items.Add(rule.target!, maxNumbers.Max());
                    break;
                case JsonInterpretation.AsAggregateMin:
                    var minTree = BuildDataTree(selectedToken, rule.children);
                    var minNumbers = GetNumbers(minTree);
                    tree.Items.Add(rule.target!, minNumbers.Min());
                    break;
                case JsonInterpretation.AsAggregateCount:
                    var countTree = BuildDataTree(selectedToken, rule.children);
                    var nonNull = GetNonNulls(countTree);
                    tree.Items.Add(rule.target!, nonNull.Count());
                    break;
                default:
                    throw new NotImplementedException($"{rule.interpretation}");
            }
        }
        return tree;
    }

    private IEnumerable<decimal> GetNumbers(DataTree tree)
    {
        var numbers = tree.Items
            .Where(i => i.Value != null && (i.Value is decimal || i.Value is int))
            .Select(i => (decimal)((i.Value as decimal?) ?? (i.Value as int?)!))
            .ToList();
        numbers.AddRange(tree.Items
            .Where(i => i.Value != null && i.Value is DataTree)
            .SelectMany(subtree => GetNumbers((DataTree)subtree.Value!)));
        return numbers;
    }

    private IEnumerable<object> GetNonNulls(DataTree tree)
    {
        var nonNull = tree.Items.Where(i => i.Value != null).Select(i => (object)i.Value!).ToList();
        nonNull.AddRange(tree.Items
            .Where(i => i.Value != null && i.Value is DataTree)
            .SelectMany(subtree => GetNonNulls((DataTree)subtree.Value!)));
        return nonNull;
    }

    private DataTable BuildAggregationTable(JToken? token, IEnumerable<JsonRule>? rules)
    {
        var aggTable = new DataTable();
        if (token == null) { return aggTable; }
        foreach (var rule in rules ?? new JsonRule[0])
        {
            aggTable = ApplyRule(aggTable, token, rule);
        }
        return aggTable;
    }

    private bool? InterpretBool(JToken? token)
    {
        if (token == null) { return null; }
        var asBool = token?.Value<bool?>();
        var asString = token?.Value<string?>();
        var asInt = token?.Value<int?>();
        var value = asBool;
        if (value == null && !string.IsNullOrWhiteSpace(asString)) value = bool.Parse(asString);
        if (value == null && asInt != null) value = asInt != 0; // non-zero = true
        return value;
    }

    private DataTable BuildAndConcatAndApplyItemTables(DataTable newTable, IEnumerable<Tuple<object, JToken>> items, JsonRule rule)
    {
        var itemTables = new List<DataTable>();
        foreach (var item in items)
        {
            var itemTable = new DataTable();
            foreach (var childRule in rule.children ?? new JsonRule[0])
            {
                if (childRule.path == "${id}") // special case
                    itemTable = ApplySingleValue(itemTable, childRule.target!, item.Item1);
                else
                    itemTable = ApplyRule(itemTable, item.Item2, childRule);
            }
            itemTables.Add(itemTable);
        }
        var itemsConcatTable = DataTable.Concat(itemTables.ToArray());
        return newTable.Rows == 0
            ? itemsConcatTable
            : itemsConcatTable.Rows == 0
                ? newTable
                : newTable.CombinedWith(itemsConcatTable);
    }

    private IEnumerable<Tuple<object,JToken>> GetListItems(JToken? token, string path)
    {
        var asArray = token as JArray;
        if (asArray == null) throw new Exception($"Token is not an array, or not found."); 
        return asArray.Select((item, index) => new Tuple<object, JToken>(index, item));
    }

    private IEnumerable<Tuple<object,JToken>> GetObjectProperties(JToken? token, string path)
    {
        var asObject = token as JObject;
        if (asObject == null) throw new Exception($"Token at {path} is not an object, or not found."); 
        return asObject.Properties().Select((prop) => new Tuple<object, JToken>(prop.Name, prop.Value));
    }


    private DataTable ApplySingleValue(DataTable table, string column, object? value)
    {
        var newTable = table.Duplicate();
        if (newTable.Rows == 0) { newTable.AddRow(DataRow.Empty); }
        foreach (var row in newTable.Data)
        {
            if (!row.ContainsKey(column)) { row.Add(column, null); }
            row[column] = value;
        }
        return newTable;
    }


}
