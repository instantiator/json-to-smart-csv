namespace JsonToSmartCsv.Rules;

public class ColumnRule
{
    public string? TargetColumn { get; set; }
    public string? SourcePath { get; set; }
    public SourceInterpretation? Interpretation { get; set; }
    public string? InterpretationRule {get; set; }
}
