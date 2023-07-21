namespace JsonToSmartCsv.Rules.Csv;

public enum CsvSourceInterpretation
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