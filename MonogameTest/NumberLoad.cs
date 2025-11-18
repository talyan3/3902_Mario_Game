using System.IO;
using System.Text.Json;

namespace MonogameTest
{
    public static class NumberLoad
    {
        public static NumberStructure Numbers { get; private set; }

        public static void Load()
        {
            string json = File.ReadAllText("MagicData.json");

            Numbers = JsonSerializer.Deserialize<NumberStructure>(json,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        }   
    }
}