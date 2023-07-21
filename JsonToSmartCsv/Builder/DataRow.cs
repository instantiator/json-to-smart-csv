namespace JsonToSmartCsv.Builder;

public class DataRow : Dictionary<string,object?>
{
    public IEnumerable<string> Headers => Keys;

    public DataRow Duplicate()
    {
        var newRow = new DataRow();
        foreach (var key in Keys)
        {
            newRow.Add(key, this[key]);
        }
        return newRow;
    }

    public static DataRow Empty => new DataRow();
}