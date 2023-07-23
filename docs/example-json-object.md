# Processing a JSON object

This is a very simple example, showing a scenario where there's a single, simple JSON object at the root of your source document.

## Input

You have an object that looks like this:

```json
{
    "name": "John",
    "surname": "Doe",
    "contacts":
    {
        "telephone": "1234567890",
        "email": "john@doe"
    }
}
```

## Rules

A simple rule set to extract the names and contacts from this list:

```jsonc
{
    "root": "$",
    "rules": [
        {
            "path": "$.name",
            "target": "first-name",
            "interpretation": "AsString"
        },
        {
            "path": "$.surname",
            "target": "last-name",
            "interpretation": "AsString"
        },
        {
            "path": "$.contacts.telephone",
            "target": "phone-number",
            "interpretation": "AsString"
        },
        {
            "path": "$.contacts.email",
            "target": "email-address",
            "interpretation": "AsString"
        }
    ]
}
```

As you can see, there are 4 rules here, and they will extract all 4 details to a column each.

## See also

If your root object is a list, see: [Processing a JSON list](example-json-list.md)
