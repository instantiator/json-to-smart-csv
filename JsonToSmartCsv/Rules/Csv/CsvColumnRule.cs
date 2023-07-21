namespace JsonToSmartCsv.Rules.Csv;

public class CsvColumnRule
{
    public string? TargetColumn { get; set; }
    public string? SourcePath { get; set; }
    public CsvSourceInterpretation? Interpretation { get; set; }
    public string? InterpretationArg1 {get; set; }
    public string? InterpretationArg2 {get; set; }
}
