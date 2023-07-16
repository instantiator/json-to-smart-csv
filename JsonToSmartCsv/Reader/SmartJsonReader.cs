using Newtonsoft.Json.Linq;

namespace JsonToSmartCsv.Reader
{
    public class SmartJsonReader
    {
        public static JToken Read(string filePath, string jsonPath)
        {
            var json = File.ReadAllText(filePath);
            var doc = JToken.Parse(json);
            return doc.SelectToken(jsonPath)!;
        }
    }
}