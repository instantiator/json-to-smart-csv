# Generating aggregate values

Define an aggregate rule, and then use its child rules to retrieve the values to aggregate over.

## Input

For example, you have some JSON data:

```jsonc
[
    {
        "name": "Daisy",
        "eggs": [ 1, 1, 0, 1, 2, 0, 1 ]
        "legs": 2
    },
    {
        "name": "Phoebe",
        "eggs": [ 0, 0, 0, 0, 0, 1, 0 ]
        "legs": 2
    },
    {
        "name": "Willow",
        "eggs": [ 0, 0, 0, 0, 0, 0, 0 ],
        "legs": 4
    }
]
```

## Rules

Some simple rules to generate some aggregate data:

```jsonc
{
    "root": "$",
    "rules":
    [
        {
            "path": "$",
            "target": "creatures", // target not actually used
            "interpretation": "IterateListItems",
            "children":
            [
                {
                    "path": "$.name",
                    "target": "name",
                    "interpretation": "AsString"
                },
                {
                    "path": "$.legs",
                    "target": "legs",
                    "interpretation": "AsNumber"
                },
                {
                    "path": "$", // path not actually used
                    "target": "total-eggs",
                    "interpretation": "AsAggregateSum",
                    "children":
                    [
                        {
                            "path": "$",
                            "target": "eggs", // target not actually used
                            "interpretation": "IterateListItems",
                            "children":
                            [
                                {
                                    "path": "$",
                                    "target": "eggs-today",
                                    "interpretation": "AsNumber"
                                }
                            ]
                        }
                    ]
                }
            ]
        }
    ]
}
```


In this example, the aggregation is over the number of eggs produced by each creature.

You can see that the `total-eggs` aggregate value operates over values returned by its child rules.
Those rules do not create columns in the table in their own right - instead they return data which is used by the aggregator to create an aggregate value.
All _numerical_ values returned by the child rules are used for the aggregation.

NB. For this example, the root object is a list. See: [Processing a JSON list](example-json-list.md)