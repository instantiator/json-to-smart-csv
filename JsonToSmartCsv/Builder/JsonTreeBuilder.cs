using System.Collections.Generic;
using System.Collections.Immutable;
using System.Data;
using System.Text;
using JsonToSmartCsv.Rules.Json;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace JsonToSmartCsv.Builder;

public class JsonTreeBuilder
{
  private JsonRuleSet rules;

  public JsonTreeBuilder(JsonRuleSet rules)
  {
    this.rules = rules;
  }

  public IEnumerable<DataTree> BuildTree(JToken source, bool asJsonLines = false)
  {
    var path = rules.root ?? "$";
    var token = source.SelectToken(path);
    if (asJsonLines)
    {
      var array = source.ToArray();
      var trees = new List<DataTree>();
      return array.Select((item, index) => BuildDataTree(item, rules.rules, index));
    }
    else
    {
      return new List<DataTree> { BuildDataTree(token, rules.rules, null) };
    }
  }

  private DataTree BuildDataTree(JToken? token, IEnumerable<JsonRule>? rules, object? index = null)
  {
    var tree = new DataTree() { Index = index };
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
        case JsonInterpretation.AsIndex:
          tree.Items.Add(rule.target!, tree.Index);
          break;
        case JsonInterpretation.IterateListItems:
          var asArray = selectedToken as JArray;
          foreach (var item in asArray?.Select((item, index) => new Tuple<int, object?>(index, item)) ?? new Tuple<int, object?>[0])
          {
            if (!tree.Items.ContainsKey(rule.target!)) tree.Items.Add(rule.target!, new List<DataTree>());
            ((List<DataTree>)tree.Items[rule.target!]!).Add(BuildDataTree(item.Item2 as JToken, rule.children, item.Item1));
          }
          break;
        case JsonInterpretation.IteratePropertiesAsList:
          var asObject = selectedToken as JObject;
          foreach (var prop in asObject?.Properties() ?? new JProperty[0])
          {
            if (!tree.Items.ContainsKey(rule.target!)) tree.Items.Add(rule.target!, new List<DataTree>());
            ((List<DataTree>)tree.Items[rule.target!]!).Add(BuildDataTree(prop.Value, rule.children, prop.Name));
          }
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
          var nonNull = GetNonNullValues(countTree);
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
    numbers.AddRange(tree.Items
        .Where(i => i.Value != null && i.Value is List<DataTree>)
        .SelectMany(list => (list.Value as List<DataTree>)!.SelectMany(GetNumbers)));
    return numbers;
  }

  private IEnumerable<KeyValuePair<string, object?>> GetNonNullValues(DataTree tree)
  {
    var items = new List<KeyValuePair<string, object?>>();
    foreach (var item in tree.Items)
    {
      if (item.Value is DataTree)
      {
        items.AddRange(GetNonNullValues((DataTree)item.Value));
      }
      else if (item.Value is List<DataTree>)
      {
        items.AddRange(((List<DataTree>)item.Value).SelectMany(GetNonNullValues));
      }
      else
      {
        items.Add(KeyValuePair.Create(item.Key, item.Value));
      }
    }
    return items;
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

}
