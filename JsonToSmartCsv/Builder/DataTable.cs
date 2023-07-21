using System.Collections.Immutable;

namespace JsonToSmartCsv.Builder;

public class DataTable
{
    public DataTable() {}
    public DataTable(IEnumerable<DataRow> data)
    {
        Data = data;
    }

    public IEnumerable<string> Headers => Data.SelectMany(row => row.Headers).Distinct();

    public IEnumerable<DataRow> Data { get; set; } = new List<DataRow>();

    public void AddRow(DataRow newRow)
    {
        var rows = new List<DataRow>(Data)
        {
            newRow
        };
        Data = rows;
    }

    public int Rows => Data.Count();

    public int Cols => Headers.Count();

    public DataTable Duplicate()
    {
        return new DataTable(Data.Select(row => row.Duplicate()).ToList());
    }

    public DataTable ConcatenatedWith(DataTable incoming)
    {
        return DataTable.Concat(this, incoming);
    }

    public DataTable CombinedWith(DataTable incoming)
    {
        return DataTable.Combine(this, incoming);
    }

    public static DataTable Empty => new DataTable();

    public static DataTable Concat(params DataTable[] tables)
        => new DataTable(new List<DataRow>(tables.SelectMany(table => table.Data)));

    public static DataTable Combine(DataTable original, DataTable incoming)
    {
        // combine headers from each source
        var newHeaders = ImmutableList.Create(original.Headers.ToArray()).AddRange(incoming.Headers).Distinct();

        // combine rows from each source
        var newData = new List<DataRow>();
        foreach (var originalRow in original.Data)
        {
            foreach (var incomingRow in incoming.Data)
            {
                var newRowData = new DataRow();
                
                foreach (var item in originalRow)
                {
                    newRowData.Add(item.Key,item.Value);
                }

                // overwrite or add new values
                foreach (var item in incomingRow)
                {
                    if (newRowData.ContainsKey(item.Key))
                    {
                        newRowData[item.Key] = item.Value;
                    }
                    else
                    {
                        newRowData.Add(item.Key,item.Value);
                    }
                }

                // append each merged row to the new data set
                newData.Add(newRowData);
            }
        }

        return new DataTable(newData);
    }

}