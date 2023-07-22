using Newtonsoft.Json.Linq;
using JsonToSmartCsv.Builder.Json;

namespace JsonToSmartCsv.Tests;

public class JsonRulesRecordBuilderTests
{


    [Fact]
    public void CanInterpretSimpleObject()
    {
        var rules = RulesHelper.SimpleRules;
        var json = 
@"{
    ""name"": ""John"",
    ""description"": ""Basic clown"",
}";

        var data = JObject.Parse(json);
        var builder = new JsonRuleRecordBuilder(rules);
        var table = builder.BuildRecords(data);

        Assert.Equal(1, table.Rows);
        Assert.Equal("John", table.Data.Single()["name"]);
        Assert.Equal("Basic clown", table.Data.Single()["description"]);
    }


    [Fact]
    public void CanInterpretSimpleNestedObject()
    {
        var rules = RulesHelper.NestedRules;
        var json =
@"{
    ""name"": ""John"",
    ""description"": ""Basic clown"",
    ""balloons"":
    [
        { ""colour"": ""red"" },
        { ""colour"": ""green"" },
        { ""colour"": ""blue"" }
    ]
}";

        var data = JObject.Parse(json);
        var builder = new JsonRuleRecordBuilder(rules);
        var table = builder.BuildRecords(data);

        Assert.Equal(3, table.Rows);
        var colours = new[] { "red", "green", "blue" };
        for (int i = 0; i < 2; i++)
        {
            Assert.Equal("John", table.Data.ElementAt(i)["name"]);
            Assert.Equal("Basic clown", table.Data.ElementAt(i)["description"]);
            Assert.Equal(colours[i], table.Data.ElementAt(i)["colour"]);
        }
    }

    [Fact]
    public void CanInterpretSimpleNestedList()
    {
        var rules = RulesHelper.NestedListRules;
        var json =
@"[
    {
        ""name"": ""John"",
        ""description"": ""Basic clown"",
        ""balloons"":
        [
            { ""colour"": ""red"" },
            { ""colour"": ""green"" },
            { ""colour"": ""blue"" }
        ]
    },
    {
        ""name"": ""Lisa"",
        ""description"": ""Advanced clown"",
        ""balloons"":
        [
            { ""colour"": ""magenta"" },
            { ""colour"": ""cyan"" },
            { ""colour"": ""yellow"" }
        ]
    },

]";

        var data = JArray.Parse(json);
        var builder = new JsonRuleRecordBuilder(rules);
        var table = builder.BuildRecords(data);

        Assert.Equal(6, table.Rows);
        var colours = new[] { "red", "green", "blue", "magenta", "cyan", "yellow" };
        for (int i = 0; i < 2; i++)
        {
            Assert.Equal("John", table.Data.ElementAt(i)["name"]);
            Assert.Equal("Basic clown", table.Data.ElementAt(i)["description"]);
            Assert.Equal(colours[i], table.Data.ElementAt(i)["colour"]);
            Assert.Equal(0, table.Data.ElementAt(i)["clown-index"]);
        }
        for (int i = 3; i < 6; i++)
        {
            Assert.Equal("Lisa", table.Data.ElementAt(i)["name"]);
            Assert.Equal("Advanced clown", table.Data.ElementAt(i)["description"]);
            Assert.Equal(colours[i], table.Data.ElementAt(i)["colour"]);
            Assert.Equal(1, table.Data.ElementAt(i)["clown-index"]);
        }
    }

    [Fact]
    public void CanInterpretSimplePropertyList()
    {
        var rules = RulesHelper.NestedPropertyListRules;
        var json =
@"[
    {
        ""name"": ""John"",
        ""description"": ""Basic clown"",
        ""balloons"":
        {
            ""best"": { ""colour"": ""red"" },
            ""in-between"": { ""colour"": ""green"" },
            ""worst"": { ""colour"": ""blue"" }
        }
    },
    {
        ""name"": ""Lisa"",
        ""description"": ""Advanced clown"",
        ""balloons"":
        {
            ""best"": { ""colour"": ""magenta"" },
            ""in-between"": { ""colour"": ""cyan"" },
            ""worst"": { ""colour"": ""yellow"" }
        }
    },

]";

        var data = JArray.Parse(json);
        var builder = new JsonRuleRecordBuilder(rules);
        var table = builder.BuildRecords(data);

        Assert.Equal(6, table.Rows);
        var colours = new[] { "red", "green", "blue", "magenta", "cyan", "yellow" };
        var qualifiers = new[] { "best", "in-between", "worst", "best", "in-between", "worst" };
        for (int i = 0; i < 2; i++)
        {
            Assert.Equal("John", table.Data.ElementAt(i)["name"]);
            Assert.Equal("Basic clown", table.Data.ElementAt(i)["description"]);
            Assert.Equal(colours[i], table.Data.ElementAt(i)["colour"]);
            Assert.Equal(qualifiers[i], table.Data.ElementAt(i)["qualifier"]);
        }
        for (int i = 3; i < 6; i++)
        {
            Assert.Equal("Lisa", table.Data.ElementAt(i)["name"]);
            Assert.Equal("Advanced clown", table.Data.ElementAt(i)["description"]);
            Assert.Equal(colours[i], table.Data.ElementAt(i)["colour"]);
            Assert.Equal(qualifiers[i], table.Data.ElementAt(i)["qualifier"]);
        }
    }

}

