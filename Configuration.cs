using Newtonsoft.Json;
using System.IO;

namespace DiscordBot
{
    public class Configuration
    {
        public string discordToken { get; set; }
        public string pubgToken { get; set; }

        public static Configuration Create()
        {
            using (StreamReader file = File.OpenText(Constants.ConfigFileName))
            {
                var serializer = new JsonSerializer();
                var config = (Configuration) serializer.Deserialize(file, typeof(Configuration));
                return config;
            }
        }
    }
}