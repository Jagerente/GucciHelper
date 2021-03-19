using DiscordBot.Configuration;
using System.Collections.Generic;

namespace DiscordBot.Modules.Quotes
{
    public class QuotesProvider
    {
        private List<QuotesConfiguration> _quotes;
        public IReadOnlyList<QuotesConfiguration> AllQuotes => _quotes;

        public QuotesProvider(string dir)
        {
            _quotes = new List<QuotesConfiguration>();
            string[] files = System.IO.Directory.GetFiles(dir);
            foreach (string path in files)
            {
                _quotes.Add(JsonStorage.RestoreObject<QuotesConfiguration>(path));
            }
        }
    }
}
