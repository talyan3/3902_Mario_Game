using System.IO;
using System.Text.Json;

namespace MonogameTest
{
    public static class ConfigLoader
    {
        public static GameConfig Config { get; private set; }

        public static void Load(string path = "GameConfig.json")
        {
            string json = File.ReadAllText(path);

            Config = JsonSerializer.Deserialize<GameConfig>(
                json,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true }
            );
        }
    }
}
