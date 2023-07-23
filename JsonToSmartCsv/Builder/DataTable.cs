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

    public DataTable AddItem(string key, object? value)
    {
        var duplicate = Rows == 0 ? AddRow(DataRow.Empty) : Duplicate();
        foreach (var row in duplicate.Data)
        {
            row[key] = value;
        }
        return duplicate;
    }

    public DataTable AddRow(DataRow newRow)
    {
        var rows = new List<DataRow>(Data)
        {
            newRow
        };

        return new DataTable(rows);
    }

    public int Rows => Data.Count();

    public int Cols => Headers.Count();

    public DataTable Duplicate()
    {
        return new DataTable(Data.Select(row => row.Duplicate()).ToList());
    }

    public DataTable Append(DataTable incoming)
    {
        return DataTable.Concat(this, incoming);
    }

    public DataTable Join(DataTable incoming)
    {
        return DataTable.Join(this, incoming);
    }

    public static DataTable Empty => new DataTable();

    public static DataTable Concat(params DataTable[] tables)
        => new DataTable(new List<DataRow>(tables.SelectMany(table => table.Data)));

    public static DataTable Join(DataTable original, DataTable incoming)
    {
        if (original.Rows == 0) { return incoming.Duplicate(); }
        if (incoming.Rows == 0) { return original.Duplicate(); }

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