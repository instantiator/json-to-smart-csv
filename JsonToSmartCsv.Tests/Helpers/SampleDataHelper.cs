using System;
namespace JsonToSmartCsv.Tests.Helpers
{
	public class SampleDataHelper
	{
        public static string SimpleJson =
@"{
    ""name"": ""John"",
    ""description"": ""Basic clown"",
}";

        public static string SimpleNestedObject =
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

        public static string SimpleNestedObjectList =
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

        public static string SimpleNestedStringList =
@"[
    {
        ""name"": ""John"",
        ""description"": ""Basic clown"",
        ""balloons"":
        [
            ""red-string"",
            ""green-string"",
            ""blue-string""
        ]
    },
    {
        ""name"": ""Lisa"",
        ""description"": ""Advanced clown"",
        ""balloons"":
        [
            ""magenta-string"",
            ""cyan-string"",
            ""yellow-string""
        ]
    },
]";

        public static string SimpleNestedPropertyCollection =
@"[
    {
        ""name"": ""John"",
        ""description"": ""Basic clown"",
        ""balloons"":
        {
            ""best"": { ""colour"": ""bright-red"" },
            ""in-between"": { ""colour"": ""medium-green"" },
            ""worst"": { ""colour"": ""dull-blue"" }
        }
    },
    {
        ""name"": ""Lisa"",
        ""description"": ""Advanced clown"",
        ""balloons"":
        {
            ""best"": { ""colour"": ""bright-magenta"" },
            ""in-between"": { ""colour"": ""medium-cyan"" },
            ""worst"": { ""colour"": ""dull-yellow"" }
        }
    },
]";

    }
}

