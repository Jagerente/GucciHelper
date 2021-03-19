using Newtonsoft.Json;

namespace DiscordBot.Configuration
{
    public class QuotesConfiguration
    {
        [JsonProperty("author")]
        public string Author { get; set; }
        [JsonProperty("image")]
        public string Image { get; set; }
        [JsonProperty("keys")]
        public string[] Keys { get; set; }
        [JsonProperty("quotes")]
        public string[] Quotes { get; set; }
    }
}
