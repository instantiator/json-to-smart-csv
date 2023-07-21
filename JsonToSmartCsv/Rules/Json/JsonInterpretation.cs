namespace JsonToSmartCsv.Rules.Json;

public enum JsonInterpretation
{
    AsString,
    AsNumber,
    AsJson,
    WithPropertiesAsColumns, // uses the target column as a prefix
    IteratePropertiesAsList,
    IterateListItems
}
