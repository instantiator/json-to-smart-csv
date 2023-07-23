using JsonToSmartCsv.Rules.Json;
namespace JsonToSmartCsv.Tests.Helpers;

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

    public static JsonRuleSet NestedObjectListRules => new JsonRuleSet()
    {
        root = "$",
        rules = new List<JsonRule>
        {
            new JsonRule {
                path = "$", target = "clowns", interpretation = JsonInterpretation.IterateListItems,
                children = new List<JsonRule>
                {
                    new JsonRule { path = "$", target = "clown-index", interpretation = JsonInterpretation.AsIndex },
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

    public static JsonRuleSet NestedStringListRules => new JsonRuleSet()
    {
        root = "$",
        rules = new List<JsonRule>
        {
            new JsonRule {
                path = "$", target = "clowns", interpretation = JsonInterpretation.IterateListItems,
                children = new List<JsonRule>
                {
                    new JsonRule { path = "$", target = "clown-index", interpretation = JsonInterpretation.AsIndex },
                    new JsonRule { path = "$.name", target = "name", interpretation = JsonInterpretation.AsString },
                    new JsonRule { path = "$.description", target = "description", interpretation = JsonInterpretation.AsString },
                    new JsonRule {
                        path = "$.balloons", target = "balloons", interpretation = JsonInterpretation.IterateListItems,
                        children = new List<JsonRule>
                        {
                            new JsonRule { path = "$", target = "string-colour", interpretation = JsonInterpretation.AsString },
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
                            new JsonRule { path = "$", target = "qualifier", interpretation = JsonInterpretation.AsIndex },
                            new JsonRule { path = "$.colour", target = "colour", interpretation = JsonInterpretation.AsString },
                        }
                    },
                }
            }
        }
    };

}