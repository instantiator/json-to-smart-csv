using JsonToSmartCsv.Rules.Json;
namespace JsonToSmartCsv.Tests;

public class RulesHelper
{
    public static JsonRuleSet SimpleRules => new JsonRuleSet()
    {
        root = "$",
        rules = new List<JsonRule>
        {
            new JsonRule { path = "$.name", target = "name", interpretation = JsonInterpretation.AsString },
            new JsonRule { path = "$.description", target = "description", interpretation = JsonInterpretation.AsString },
        }
    };

    public static JsonRuleSet NestedRules => new JsonRuleSet()
    {
        root = "$",
        rules = new List<JsonRule>
        {
            new JsonRule { path = "$.name", target = "name", interpretation = JsonInterpretation.AsString },
            new JsonRule { path = "$.description", target = "description", interpretation = JsonInterpretation.AsString },
            new JsonRule { 
                path = "$.balloons", target = "balloons", interpretation = JsonInterpretation.IterateListItems,
                children = new List<JsonRule>
                {
                    new JsonRule { path = "$.colour", target = "colour", interpretation = JsonInterpretation.AsString },
                }
            },
        }
    };

    public static JsonRuleSet NestedListRules => new JsonRuleSet()
    {
        root = "$",
        rules = new List<JsonRule>
        {
            new JsonRule {
                path = "$", target = "clowns", interpretation = JsonInterpretation.IterateListItems,
                children = new List<JsonRule>
                {
                    new JsonRule { path = "${id}", target = "clown-index", interpretation = JsonInterpretation.AsNumber },
                    new JsonRule { path = "$.name", target = "name", interpretation = JsonInterpretation.AsString },
                    new JsonRule { path = "$.description", target = "description", interpretation = JsonInterpretation.AsString },
                    new JsonRule {
                        path = "$.balloons", target = "balloons", interpretation = JsonInterpretation.IterateListItems,
                        children = new List<JsonRule>
                        {
                            new JsonRule { path = "$.colour", target = "colour", interpretation = JsonInterpretation.AsString },
                        }
                    },
                }
            }
        }
    };

    public static JsonRuleSet NestedPropertyListRules => new JsonRuleSet()
    {
        root = "$",
        rules = new List<JsonRule>
        {
            new JsonRule {
                path = "$", target = "clowns", interpretation = JsonInterpretation.IterateListItems,
                children = new List<JsonRule>
                {
                    new JsonRule { path = "$.name", target = "name", interpretation = JsonInterpretation.AsString },
                    new JsonRule { path = "$.description", target = "description", interpretation = JsonInterpretation.AsString },
                    new JsonRule {
                        path = "$.balloons", target = "balloons", interpretation = JsonInterpretation.IteratePropertiesAsList,
                        children = new List<JsonRule>
                        {
                            new JsonRule { path = "${id}", target = "qualifier", interpretation = JsonInterpretation.AsString },
                            new JsonRule { path = "$.colour", target = "colour", interpretation = JsonInterpretation.AsString },
                            new JsonRule { path = "$.colour", target = "colour-${id}", interpretation = JsonInterpretation.AsString },
                        }
                    },
                }
            }
        }
    };

}