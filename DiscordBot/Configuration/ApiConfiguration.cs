using Newtonsoft.Json;

namespace DiscordBot.Configuration
{
    public class ApiConfiguration
    {
        [JsonProperty("token")]
        public string Token { get; set; }
    }
}
