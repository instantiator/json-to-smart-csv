using Newtonsoft.Json.Linq;

namespace JsonToSmartCsv.Reader
{
  public class SmartJsonReader
  {
    public static JToken Read(string filePath, bool asJsonLines = false)
    {
      if (asJsonLines)
      {
        var source = File.ReadAllLines(filePath);
        var jsonArray = new JArray();
        foreach (var line in source)
        {
          jsonArray.Add(JToken.Parse(line));
        }
        return jsonArray;
      }
      else
      {
        var source = File.ReadAllText(filePath);
        return JToken.Parse(source);
      }
    }
  }
}