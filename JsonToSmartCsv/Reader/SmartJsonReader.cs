using Newtonsoft.Json.Linq;

namespace JsonToSmartCsv.Reader
{
    public class SmartJsonReader
    {
        public static JToken Read(string filePath)
        {
            var json = File.ReadAllText(filePath);
            return JToken.Parse(json);
        }
    }
}