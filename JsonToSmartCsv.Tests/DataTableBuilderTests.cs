using System;
using JsonToSmartCsv.Builder;
using JsonToSmartCsv.Tests.Helpers;
using Newtonsoft.Json.Linq;

namespace JsonToSmartCsv.Tests
{
	public class DataTableBuilderTests
	{
        [Fact]
        public void CanConvertSimpleObjectTreeToTable()
        {
            var rules = RulesHelper.SimpleRules;
            var json = SampleDataHelper.SimpleJson;
            var data = JToken.Parse(json);
            var builder = new JsonTreeBuilder(rules);
            var tree = builder.BuildTree(data);
            var table = DataTableBuilder.BuildTableFromTree(tree);
            Assert.NotNull(table);
            Assert.Equal(1, table.Rows);
            Assert.Equal(2, table.Data.ElementAt(0).Count());
            Assert.Equal("John", table.Data.ElementAt(0)["name"]);
            Assert.Equal("Basic clown", table.Data.ElementAt(0)["description"]);
        }

        [Fact]
        public void CanConvertSimpleNestedObjectTreeToTable()
        {
            var rules = RulesHelper.NestedObjectRules;
            var json = SampleDataHelper.SimpleNestedObject;
            var data = JToken.Parse(json);
            var builder = new JsonTreeBuilder(rules);
            var tree = builder.BuildTree(data);
            var table = DataTableBuilder.BuildTableFromTree(tree);
            Assert.NotNull(table);
            Assert.Equal(3, table.Rows);
            var colours = new[] { "red", "green", "blue" };
            for (int r = 0; r < 3; r++)
            {
                Assert.Equal(3, table.Data.ElementAt(r).Count());
                Assert.Equal("John", table.Data.ElementAt(r)["name"]);
                Assert.Equal("Basic clown", table.Data.ElementAt(r)["description"]);
                Assert.Equal(colours[r], table.Data.ElementAt(r)["colour"]);
            }
        }

        [Fact]
        public void CanConvertSimpleNestedObjectTreeWithAggregatesToTable()
        {
            var rules = RulesHelper.NestedObjectAggregateRules;
            var json = SampleDataHelper.SimpleNestedObjectWithAmounts;
            var data = JToken.Parse(json);
            var builder = new JsonTreeBuilder(rules);
            var tree = builder.BuildTree(data);
            var table = DataTableBuilder.BuildTableFromTree(tree);
            Assert.NotNull(table);
            Assert.Equal(3, table.Rows);
            var colours = new[] { "red", "green", "blue" };
            for (int r = 0; r < 3; r++)
            {
                Assert.Equal(8, table.Data.ElementAt(r).Count());
                Assert.Equal("John", table.Data.ElementAt(r)["name"]);
                Assert.Equal("Basic clown", table.Data.ElementAt(r)["description"]);
                Assert.Equal(4m, table.Data.ElementAt(r)["min-amount"]);
                Assert.Equal(23m, table.Data.ElementAt(r)["max-amount"]);
                Assert.Equal(12m, table.Data.ElementAt(r)["avg-amount"]);
                Assert.Equal(36m, table.Data.ElementAt(r)["sum-amount"]);
                Assert.Equal(3, table.Data.ElementAt(r)["count-balloon-types"]);
                Assert.Equal(colours[r], table.Data.ElementAt(r)["colour"]);
            }
        }

        [Fact]
        public void CanConvertSimpleNestedStringListTreeToTable()
        {
            var rules = RulesHelper.NestedStringListRules;
            var json = SampleDataHelper.SimpleNestedStringList;
            var data = JToken.Parse(json);
            var builder = new JsonTreeBuilder(rules);
            var tree = builder.BuildTree(data);
            var table = DataTableBuilder.BuildTableFromTree(tree);
            Assert.NotNull(table);
            Assert.Equal(6, table.Rows);

            var colours = new[] {
                "red-string", "green-string", "blue-string",
                "magenta-string", "cyan-string", "yellow-string",
            };
            for (int r = 0; r < 3; r++)
            {
                Assert.Equal(4, table.Data.ElementAt(r).Count());
                Assert.Equal(0, table.Data.ElementAt(r)["clown-index"]);
                Assert.Equal("John", table.Data.ElementAt(r)["name"]);
                Assert.Equal("Basic clown", table.Data.ElementAt(r)["description"]);
                Assert.Equal(colours[r], table.Data.ElementAt(r)["string-colour"]);
            }
            for (int r = 3; r < 6; r++)
            {
                Assert.Equal(4, table.Data.ElementAt(r).Count());
                Assert.Equal(1, table.Data.ElementAt(r)["clown-index"]);
                Assert.Equal("Lisa", table.Data.ElementAt(r)["name"]);
                Assert.Equal("Advanced clown", table.Data.ElementAt(r)["description"]);
                Assert.Equal(colours[r], table.Data.ElementAt(r)["string-colour"]);
            }
        }

        [Fact]
        public void CanConvertSimpleNestedObjectListTreeToTable()
        {
            var rules = RulesHelper.NestedObjectListRules;
            var json = SampleDataHelper.SimpleNestedObjectList;
            var data = JToken.Parse(json);
            var builder = new JsonTreeBuilder(rules);
            var tree = builder.BuildTree(data);
            var table = DataTableBuilder.BuildTableFromTree(tree);
            Assert.NotNull(table);

            var colours = new[] {
                "red", "green", "blue",
                "magenta", "cyan", "yellow",
            };
            for (int r = 0; r < 3; r++)
            {
                Assert.Equal(4, table.Data.ElementAt(r).Count());
                Assert.Equal(0, table.Data.ElementAt(r)["clown-index"]);
                Assert.Equal("John", table.Data.ElementAt(r)["name"]);
                Assert.Equal("Basic clown", table.Data.ElementAt(r)["description"]);
                Assert.Equal(colours[r], table.Data.ElementAt(r)["colour"]);
            }
            for (int r = 3; r < 6; r++)
            {
                Assert.Equal(4, table.Data.ElementAt(r).Count());
                Assert.Equal(1, table.Data.ElementAt(r)["clown-index"]);
                Assert.Equal("Lisa", table.Data.ElementAt(r)["name"]);
                Assert.Equal("Advanced clown", table.Data.ElementAt(r)["description"]);
                Assert.Equal(colours[r], table.Data.ElementAt(r)["colour"]);
            }
        }

        [Fact]
        public void CanConvertSimplePropertyListTreeToTable()
        {
            var rules = RulesHelper.NestedPropertyListRules;
            var json = SampleDataHelper.SimpleNestedPropertyCollection;
            var data = JToken.Parse(json);
            var builder = new JsonTreeBuilder(rules);
            var tree = builder.BuildTree(data);
            var table = DataTableBuilder.BuildTableFromTree(tree);
            Assert.NotNull(table);

            var colours = new[] {
                "bright-red", "medium-green", "dull-blue",
                "bright-magenta", "medium-cyan", "dull-yellow",
            };
            var qualifiers = new[]
            {
                "best", "in-between", "worst",
                "best", "in-between", "worst"
            };
            for (int r = 0; r < 3; r++)
            {
                Assert.Equal(4, table.Data.ElementAt(r).Count());
                Assert.Equal("John", table.Data.ElementAt(r)["name"]);
                Assert.Equal("Basic clown", table.Data.ElementAt(r)["description"]);
                Assert.Equal(colours[r], table.Data.ElementAt(r)["colour"]);
                Assert.Equal(qualifiers[r], table.Data.ElementAt(r)["qualifier"]);
            }
            for (int r = 3; r < 6; r++)
            {
                Assert.Equal(4, table.Data.ElementAt(r).Count());
                Assert.Equal("Lisa", table.Data.ElementAt(r)["name"]);
                Assert.Equal("Advanced clown", table.Data.ElementAt(r)["description"]);
                Assert.Equal(colours[r], table.Data.ElementAt(r)["colour"]);
                Assert.Equal(qualifiers[r], table.Data.ElementAt(r)["qualifier"]);
            }

        }
    }
}

