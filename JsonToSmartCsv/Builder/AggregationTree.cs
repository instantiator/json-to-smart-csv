using System;
namespace JsonToSmartCsv.Builder
{
	public class DataTree
	{
		public Dictionary<string, object?> Items { get; set; } = new DataRow();
		//public Dictionary<string, DataTree> Children { get; set; } = new Dictionary<string, DataTree>();
	}
}

