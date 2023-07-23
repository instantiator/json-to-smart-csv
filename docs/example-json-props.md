# Processing JSON properties as a list

You may, on occasion, find that you need to work through a JSON object whose properties are _effectively_ a list.

## Input

For example, a JSON source file that looks like this:

```jsonc
{
    "Willow":
    {
        "paws": 4,
        "fleas": 0,
        "fur": "long"
    },
    "Sasha":
    {
        "paws": 4,
        "fleas": 0,
        "fur": "short"
    }
}
```

## Rules

This input _could_ have been a list, but it wasn't. You can process the properties of the object (`Willow`, and `Sasha`) as if they are list.

You can also gain access to the index (ie. the name of the property) by use of a special interpretation: `Asindex`

Here's an example:

```jsonc
{
    "root": "$",
    "rules":
    [
        {
            "path": "$",
            "target": "props", // target not actually used
            "interpretation": "IteratePropertiesAsList",
            "children":
            [
                {
                    "path": "$", // path not actually used
                    "target": "kitty",
                    "interpretation": "AsIndex"
                },
                {
                    "path": "$.paws",
                    "target": "paws",
                    "interpretation": "AsNumber"
                },
                {
                    "path": "$.fleas",
                    "target": "fleas",
                    "interpretation": "AsNumber"
                },
                {
                    "path": "$.fur",
                    "target": "fur",
                    "interpretation": "AsString"
                }
            ]
        }
    ]
}
```

