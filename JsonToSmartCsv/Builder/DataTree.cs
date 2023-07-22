using System;
namespace JsonToSmartCsv.Builder
{
	public class DataTree
	{
		public object? Index { get; set; }
		public Dictionary<string, object?> Items { get; set; } = new DataRow();
	}
}

