namespace JsonToSmartCsv.Rules.Json;

public enum JsonInterpretation
{
    AsString,
    AsNumber,
    AsBoolean,
    AsJson,
    AsAggregateSum,
    AsAggregateMax,
    AsAggregateMin,
    AsAggregateAvg,
    AsAggregateCount,
    IterateListItems,
    IteratePropertiesAsList,
    WithPropertiesAsColumns, // uses the target column as a prefix
}
