namespace JsonToSmartCsv.Rules;

public enum SourceInterpretation
{
    AsString,
    AsDecimal,
    AsInteger,
    AsBoolean,
    AsJson,
    AsCount,
    AsConcatenation,
    AsAggregate,
}