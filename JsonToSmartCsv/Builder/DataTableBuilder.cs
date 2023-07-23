using System;
namespace JsonToSmartCsv.Builder
{
	public class DataTableBuilder
	{
		public static DataTable BuildTableFromTree(DataTree tree)
		{
			return AddTree(new DataTable(), tree);
		}

		private static DataTable AddTree(DataTable table, DataTree tree)
		{
			foreach (var item in tree.Items)
			{
				if (item.Value is List<DataTree>)
				{
					// concat all item tables
					var concatTable = new DataTable();
					foreach (var subItem in (item.Value as List<DataTree>)!)
					{
                        concatTable = concatTable.Append(BuildTableFromTree(subItem));
					}
                    // join to the main table
                    table = table.Join(concatTable);
                }
				else
				{
					table = table.AddItem(item.Key, item.Value);
				}
			}
			return table;
		}
	}
}

