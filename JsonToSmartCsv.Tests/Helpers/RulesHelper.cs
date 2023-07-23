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

    public static JsonRuleSet NestedObjectRules => new JsonRuleSet()
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

    public static JsonRuleSet NestedObjectAggregateRules => new JsonRuleSet()
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
            new JsonRule {
                path = "$", target = "avg-amount", interpretation = JsonInterpretation.AsAggregateAvg,
                children = new List<JsonRule>
                {
                    new JsonRule
                    {
                        path = "$.balloons", target = "balloons", interpretation = JsonInterpretation.IterateListItems,
                        children = new List<JsonRule>
                        {
                            new JsonRule { path = "$.amount", target = "amount", interpretation = JsonInterpretation.AsNumber },
                        }
                    }
                }
            },
            new JsonRule {
                path = "$", target = "sum-amount", interpretation = JsonInterpretation.AsAggregateSum,
                children = new List<JsonRule>
                {
                    new JsonRule
                    {
                        path = "$.balloons", target = "balloons", interpretation = JsonInterpretation.IterateListItems,
                        children = new List<JsonRule>
                        {
                            new JsonRule { path = "$.amount", target = "amount", interpretation = JsonInterpretation.AsNumber },
                        }
                    }
                }
            },
            new JsonRule {
                path = "$", target = "min-amount", interpretation = JsonInterpretation.AsAggregateMin,
                children = new List<JsonRule>
                {
                    new JsonRule
                    {
                        path = "$.balloons", target = "balloons", interpretation = JsonInterpretation.IterateListItems,
                        children = new List<JsonRule>
                        {
                            new JsonRule { path = "$.amount", target = "amount", interpretation = JsonInterpretation.AsNumber },
                        }
                    }
                }
            },
            new JsonRule {
                path = "$", target = "max-amount", interpretation = JsonInterpretation.AsAggregateMax,
                children = new List<JsonRule>
                {
                    new JsonRule
                    {
                        path = "$.balloons", target = "balloons", interpretation = JsonInterpretation.IterateListItems,
                        children = new List<JsonRule>
                        {
                            new JsonRule { path = "$.amount", target = "amount", interpretation = JsonInterpretation.AsNumber },
                        }
                    }
                }
            },
            new JsonRule {
                path = "$", target = "count-balloon-types", interpretation = JsonInterpretation.AsAggregateCount,
                children = new List<JsonRule>
                {
                    new JsonRule
                    {
                        path = "$.balloons", target = "balloons", interpretation = JsonInterpretation.IterateListItems,
                        children = new List<JsonRule>
                        {
                            new JsonRule { path = "$", target = "balloon-json", interpretation = JsonInterpretation.AsJson },
                        }
                    }
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