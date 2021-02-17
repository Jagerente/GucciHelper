using Newtonsoft.Json;

namespace DiscordBot.Configuration
{
    public class DiscordConfiguration : ApiConfiguration
    {
        [JsonProperty("prefix")]
        public string Prefix { get; set; }
        [JsonProperty("name")]
        public string Name { get; set; }
    }
}
