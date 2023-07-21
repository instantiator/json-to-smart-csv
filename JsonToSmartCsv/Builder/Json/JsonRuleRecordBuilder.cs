using System.Collections.Immutable;
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
        switch (rule.interpretation)
        {
            case JsonInterpretation.AsString:
                if (newTable.Rows == 0) { newTable.AddRow(DataRow.Empty); }
                foreach (var row in newTable.Data)
                {
                    if (!row.ContainsKey(rule.target!)) { row.Add(rule.target!, null); }
                    row[rule.target!] = token?.SelectToken(rule.path!)?.Value<string?>();
                }
                return newTable;

            case JsonInterpretation.AsNumber:
                if (newTable.Rows == 0) { newTable.AddRow(DataRow.Empty); }
                foreach (var row in newTable.Data)
                {
                    if (!row.ContainsKey(rule.target!)) { row.Add(rule.target!, null); }
                    row[rule.target!] = token?.SelectToken(rule.path!)?.Value<decimal?>();
                }
                return newTable;

            case JsonInterpretation.AsJson:
                if (newTable.Rows == 0) { newTable.AddRow(DataRow.Empty); }
                foreach (var row in newTable.Data)
                {
                    if (!row.ContainsKey(rule.target!)) { row.Add(rule.target!, null); }
                    row[rule.target!] = token?.SelectToken(rule.path!)?.ToString(Formatting.Indented);
                }
                return newTable;

            case JsonInterpretation.IterateListItems:
                var asArray = token?.SelectToken(rule.path!) as JArray;
                if (asArray == null)
                { 
                    throw new Exception($"Token at {rule.path} is not an array, or not found."); 
                }
                else
                {
                    // lots of children - they should all be concatenated together
                    var itemTables = new List<DataTable>();
                    for (int index = 0; index < asArray.Count(); index++)
                    {
                        var item = asArray.ElementAt(index);
                        var itemTable = new DataTable();

                        foreach (var indexedChildRule in rule.children!)
                        {
                            var specificChildRule = new JsonRule()
                            {
                                path = indexedChildRule.path,
                                target = indexedChildRule.target!.Replace("${id}", index.ToString()),
                                interpretation = indexedChildRule.interpretation,
                                children = indexedChildRule.children
                            };
                            if (specificChildRule.path == "${id}" && specificChildRule.interpretation == JsonInterpretation.AsNumber)
                            {
                                // special case - use ${id} with AsString to get the property key
                                specificChildRule.path = "$";
                                var itemKey = new JValue(index);
                                itemTable = ApplyRule(itemTable, itemKey, specificChildRule);
                            }
                            else
                            {
                                // regular case
                                itemTable = ApplyRule(itemTable, item, specificChildRule);
                            }
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

            case JsonInterpretation.IteratePropertiesAsList:
                var asObject = token?.SelectToken(rule.path!) as JObject;
                if (asObject == null)
                {
                    throw new Exception($"Token at {rule.path} is not an object, or not found.");
                }
                else
                {
                    var propertyTables = new List<DataTable>();
                    foreach (var property in asObject.Properties())
                    {
                        var propertyTable = new DataTable();
                        foreach (var childRule in rule.children!)
                        {
                            var specificChildRule = new JsonRule()
                            {
                                path = childRule.path,
                                target = childRule.target!.Replace("${id}", property.Name),
                                interpretation = childRule.interpretation,
                                children = childRule.children
                            };
                            if (specificChildRule.path == "${id}" && specificChildRule.interpretation == JsonInterpretation.AsString)
                            {
                                // special case - use ${id} with AsString to get the property key
                                specificChildRule.path = "$";
                                var propertyKey = new JValue(property.Name);
                                propertyTable = ApplyRule(propertyTable, propertyKey, specificChildRule);
                            }
                            else
                            {
                                // regular case
                                propertyTable = ApplyRule(propertyTable, property.Value, specificChildRule);
                            }
                        }
                        propertyTables.Add(propertyTable);
                    }
                    var propertiesConcatTable = DataTable.Concat(propertyTables.ToArray());
                    return newTable.Rows == 0
                        ? propertiesConcatTable
                        : propertiesConcatTable.Rows == 0
                            ? newTable
                            : newTable.CombinedWith(propertiesConcatTable);
                }

            case JsonInterpretation.WithPropertiesAsColumns:
                throw new NotImplementedException("This is shorthand - not implemented yet");

            default:
                throw new NotImplementedException($"Unknown interpretation: {rule.interpretation}");
        }
    }    


}
