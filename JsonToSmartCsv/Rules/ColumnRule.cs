namespace JsonToSmartCsv.Rules;

public class ColumnRule
{
    public string? TargetColumn { get; set; }
    public string? SourcePath { get; set; }
    public SourceInterpretation? Interpretation { get; set; }
    public string? InterpretationArg1 {get; set; }
    public string? InterpretationArg2 {get; set; }
}
