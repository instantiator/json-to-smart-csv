using JsonToSmartCsv.Rules.Json;
using JsonToSmartCsv.Tests.Helpers;
using Newtonsoft.Json;

namespace JsonToSmartCsv.Tests;

public class JsonRulesReaderTests
{
    [Fact]
    public void RulesReaderCanReadSimpleRules()
    {
        var rules = RulesHelper.SimpleRules;
        var json = JsonConvert.SerializeObject(rules);
        var readRules = JsonRulesReader.FromString(json);
        Assert.Equal(rules.root, readRules.root);
        Assert.Equal(rules.rules!.Count(), readRules.rules!.Count());
    }

    [Fact]
    public void RulesReaderCanReadNestedRules()
    {
        var rules = RulesHelper.NestedRules;
        var json = JsonConvert.SerializeObject(rules);
        var readRules = JsonRulesReader.FromString(json);
        Assert.Equal(rules.root, readRules.root);
        Assert.Equal(rules.rules!.Count(), readRules.rules!.Count());
    }

    [Fact]
    public void RulesReaderCanReadNestedStringListRules()
    {
        var rules = RulesHelper.NestedStringListRules;
        var json = JsonConvert.SerializeObject(rules);
        var readRules = JsonRulesReader.FromString(json);
        Assert.Equal(rules.root, readRules.root);
        Assert.Equal(rules.rules!.Count(), readRules.rules!.Count());
    }

    [Fact]
    public void RulesReaderCanReadNestedObjectListRules()
    {
        var rules = RulesHelper.NestedObjectListRules;
        var json = JsonConvert.SerializeObject(rules);
        var readRules = JsonRulesReader.FromString(json);
        Assert.Equal(rules.root, readRules.root);
        Assert.Equal(rules.rules!.Count(), readRules.rules!.Count());
    }

    [Fact]
    public void RulesReaderCanReadPropertyListRules()
    {
        var rules = RulesHelper.NestedPropertyListRules;
        var json = JsonConvert.SerializeObject(rules);
        var readRules = JsonRulesReader.FromString(json);
        Assert.Equal(rules.root, readRules.root);
        Assert.Equal(rules.rules!.Count(), readRules.rules!.Count());
    }
}
