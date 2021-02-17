using DSharpPlus;
using DSharpPlus.Entities;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DiscordBot.Extensions
{
    public static class GuildExtensions
    {
        public static async Task<DiscordEmoji> GetByName(this DiscordGuild guild, string name)
        {
            IReadOnlyList<DiscordGuildEmoji> emojis = await guild.GetEmojisAsync();
            return emojis.FirstOrDefault(emoji => emoji.Name == name);
        }
    }
}