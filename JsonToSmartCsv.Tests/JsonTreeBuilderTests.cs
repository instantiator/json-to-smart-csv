using JsonToSmartCsv.Builder;
using JsonToSmartCsv.Tests.Helpers;
using Newtonsoft.Json.Linq;

namespace JsonToSmartCsv.Tests;

public class JsonTreeBuilderTests
{
    [Fact]
    public void CanInterpretSimpleObject()
    {
        var rules = RulesHelper.SimpleRules;
        var json = SampleDataHelper.SimpleJson;
        var data = JToken.Parse(json);
        var builder = new JsonTreeBuilder(rules);
        var tree = builder.BuildTree(data);

        Assert.NotNull(tree);
        Assert.Equal(2, tree.Items.Count());
        Assert.Equal("John", tree.Items["name"]);
        Assert.Equal("Basic clown", tree.Items["description"]);
    }

    [Fact]
    public void CanInterpretSimpleNestedObject()
    {
        var rules = RulesHelper.NestedRules;
        var json = SampleDataHelper.SimpleNestedObject;
        var data = JToken.Parse(json);
        var builder = new JsonTreeBuilder(rules);
        var tree = builder.BuildTree(data);

        Assert.NotNull(tree);
        Assert.Equal(3, tree.Items.Count());
        Assert.Equal("John", tree.Items["name"]);
        Assert.Equal("Basic clown", tree.Items["description"]);
        Assert.Equal(3, (tree.Items["balloons"]! as List<DataTree>)!.Count());
        Assert.Equal("red", (tree.Items["balloons"]! as List<DataTree>)![0].Items["colour"]);
        Assert.Equal("green", (tree.Items["balloons"]! as List<DataTree>)![1].Items["colour"]);
        Assert.Equal("blue", (tree.Items["balloons"]! as List<DataTree>)![2].Items["colour"]);
    }

    [Fact]
    public void CanInterpretSimpleNestedStringList()
    {
        var rules = RulesHelper.NestedStringListRules;
        var json = SampleDataHelper.SimpleNestedStringList;
        var data = JToken.Parse(json);
        var builder = new JsonTreeBuilder(rules);
        var tree = builder.BuildTree(data);

        Assert.NotNull(tree);
        Assert.Single(tree.Items);
        var clowns = tree.Items["clowns"] as List<DataTree>;
        Assert.NotNull(clowns);
        Assert.Equal(2, clowns.Count());

        Assert.Equal("John", clowns[0].Items["name"]);
        Assert.Equal("Basic clown", clowns[0].Items["description"]);
        Assert.Equal(3, (clowns[0].Items["balloons"]! as List<DataTree>)!.Count());
        Assert.Equal("red-string", (clowns[0].Items["balloons"]! as List<DataTree>)![0].Items["string-colour"]);
        Assert.Equal("green-string", (clowns[0].Items["balloons"]! as List<DataTree>)![1].Items["string-colour"]);
        Assert.Equal("blue-string", (clowns[0].Items["balloons"]! as List<DataTree>)![2].Items["string-colour"]);

        Assert.Equal("Lisa", clowns[1].Items["name"]);
        Assert.Equal("Advanced clown", clowns[1].Items["description"]);
        Assert.Equal(3, (clowns[1].Items["balloons"]! as List<DataTree>)!.Count());
        Assert.Equal("magenta-string", (clowns[1].Items["balloons"]! as List<DataTree>)![0].Items["string-colour"]);
        Assert.Equal("cyan-string", (clowns[1].Items["balloons"]! as List<DataTree>)![1].Items["string-colour"]);
        Assert.Equal("yellow-string", (clowns[1].Items["balloons"]! as List<DataTree>)![2].Items["string-colour"]);
    }

    [Fact]
    public void CanInterpretSimpleNestedObjectList()
    {
        var rules = RulesHelper.NestedObjectListRules;
        var json = SampleDataHelper.SimpleNestedObjectList;
        var data = JToken.Parse(json);
        var builder = new JsonTreeBuilder(rules);
        var tree = builder.BuildTree(data);

        Assert.NotNull(tree);
        Assert.Single(tree.Items);
        var clowns = tree.Items["clowns"] as List<DataTree>;
        Assert.NotNull(clowns);
        Assert.Equal(2, clowns.Count());

        Assert.Equal("John", clowns[0].Items["name"]);
        Assert.Equal("Basic clown", clowns[0].Items["description"]);
        Assert.Equal(3, (clowns[0].Items["balloons"]! as List<DataTree>)!.Count());
        Assert.Equal("red", (clowns[0].Items["balloons"]! as List<DataTree>)![0].Items["colour"]);
        Assert.Equal("green", (clowns[0].Items["balloons"]! as List<DataTree>)![1].Items["colour"]);
        Assert.Equal("blue", (clowns[0].Items["balloons"]! as List<DataTree>)![2].Items["colour"]);

        Assert.Equal("Lisa", clowns[1].Items["name"]);
        Assert.Equal("Advanced clown", clowns[1].Items["description"]);
        Assert.Equal(3, (clowns[1].Items["balloons"]! as List<DataTree>)!.Count());
        Assert.Equal("magenta", (clowns[1].Items["balloons"]! as List<DataTree>)![0].Items["colour"]);
        Assert.Equal("cyan", (clowns[1].Items["balloons"]! as List<DataTree>)![1].Items["colour"]);
        Assert.Equal("yellow", (clowns[1].Items["balloons"]! as List<DataTree>)![2].Items["colour"]);
    }

    [Fact]
    public void CanInterpretSimplePropertyList()
    {
        var rules = RulesHelper.NestedPropertyListRules;
        var json = SampleDataHelper.SimpleNestedPropertyCollection;
        var data = JToken.Parse(json);
        var builder = new JsonTreeBuilder(rules);
        var tree = builder.BuildTree(data);

        Assert.NotNull(tree);
        Assert.Single(tree.Items);
        var clowns = tree.Items["clowns"] as List<DataTree>;
        Assert.NotNull(clowns);
        Assert.Equal(2, clowns.Count());

        Assert.Equal("John", clowns[0].Items["name"]);
        Assert.Equal("Basic clown", clowns[0].Items["description"]);
        Assert.Equal(3, (clowns[0].Items["balloons"]! as List<DataTree>)!.Count());

        Assert.Equal("best", (clowns[0].Items["balloons"]! as List<DataTree>)![0].Items["qualifier"]);
        Assert.Equal("bright-red", (clowns[0].Items["balloons"]! as List<DataTree>)![0].Items["colour"]);
        Assert.Equal("in-between", (clowns[0].Items["balloons"]! as List<DataTree>)![1].Items["qualifier"]);
        Assert.Equal("medium-green", (clowns[0].Items["balloons"]! as List<DataTree>)![1].Items["colour"]);
        Assert.Equal("worst", (clowns[0].Items["balloons"]! as List<DataTree>)![2].Items["qualifier"]);
        Assert.Equal("dull-blue", (clowns[0].Items["balloons"]! as List<DataTree>)![2].Items["colour"]);

        Assert.Equal("Lisa", clowns[1].Items["name"]);
        Assert.Equal("Advanced clown", clowns[1].Items["description"]);
        Assert.Equal(3, (clowns[1].Items["balloons"]! as List<DataTree>)!.Count());

        Assert.Equal("best", (clowns[1].Items["balloons"]! as List<DataTree>)![0].Items["qualifier"]);
        Assert.Equal("bright-magenta", (clowns[1].Items["balloons"]! as List<DataTree>)![0].Items["colour"]);
        Assert.Equal("in-between", (clowns[1].Items["balloons"]! as List<DataTree>)![1].Items["qualifier"]);
        Assert.Equal("medium-cyan", (clowns[1].Items["balloons"]! as List<DataTree>)![1].Items["colour"]);
        Assert.Equal("worst", (clowns[1].Items["balloons"]! as List<DataTree>)![2].Items["qualifier"]);
        Assert.Equal("dull-yellow", (clowns[1].Items["balloons"]! as List<DataTree>)![2].Items["colour"]);
    }
}

