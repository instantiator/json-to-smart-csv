namespace JsonToSmartCsv.Rules;

public enum SourceInterpretation
{
    AsString,
    AsDecimal,
    AsInteger,
    AsBoolean,
    AsObjectAsJson,
    AsListAsJson,
    AsListCount
}