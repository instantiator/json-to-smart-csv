# Processing a JSON list

## Input

You have a list that looks like this:

```json
[
    {
        "name": "John",
        "stats":
        {
            "level": 4,
            "speciality": "woodwork"
        } 
    },
    { 
        "name": "Lisa",
        "stats":
        {
            "level": 6,
            "speciality": "horticulture"
        }
    }
]
```

## Rules

A simple rule set to extract the names and stats from this list:

```jsonc
{
    "root": "$",
    "rules": [
        {
            "path": "$",
            "target": "list", // target not actually used
            "interpretation": "IterateListItems",
            "children":
            [
                {
                    "path": "$.name",
                    "target": "name",
                    "interpretation": "AsString"
                },
                {
                    "path": "$.stats.level",
                    "target": "level",
                    "interpretation": "AsNumber"
                },
                {
                    "path": "$.stats.speciality",
                    "target": "speciality",
                    "interpretation": "AsString"
                }
            ]
        }
    ]
}
```