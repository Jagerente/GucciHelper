using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using System;
using System.Threading.Tasks;
using DiscordBot.Modules.Quotes;
using System.Linq;

namespace DiscordBot.Commands
{
    public class UnsortedCommands : BaseCommandModule
    {
        public Random rnd { private get; set; }

        public QuotesProvider quotes { private get; set; }

        [Command("roll")]
        [Description("Rolls a number.")]
        public async Task Roll(CommandContext ctx, [Description("Range min-max.")] string range = "0-100")
        {
            var min = Convert.ToInt32(range.Split('-')[0]);
            var max = Convert.ToInt32(range.Split('-')[1]);
            await ctx.Channel.SendMessageAsync(rnd.Next(min, max).ToString());
        }

        [Command("roll")]
        [Description("Rolls a number.")]
        public async Task Roll(CommandContext ctx, [Description("Maximum number.")]int max = 100)
        {
            await ctx.Channel.SendMessageAsync(rnd.Next(max).ToString());
        }

        [Command("pick")]
        public async Task Pick(CommandContext ctx, params string[] message)
        {
            Random random = new Random();
            var selection = message[random.Next(0, message.Length)];

            var embed = new DiscordEmbedBuilder();
            embed.WithAuthor("I choose...");
            embed.WithTitle(selection);
            embed.WithDescription($"Options:\n{string.Join("\n", message)}");
            embed.WithFooter($"picked by {ctx.User.Username}");
            embed.WithTimestamp(DateTimeOffset.Now);

            await ctx.Channel.SendMessageAsync(embed.Build());
        }

        [Command("clean"), Aliases("clear", "purge", "почисти", "очисти")]
        [Description("Cleans chat")]
        public async Task Clean(CommandContext ctx, 
            [Description("Messages count. Default is 100.")] int count = 100, 
            [Description("If not empty, cleans only <userId> messages.")] DSharpPlus.Entities.DiscordUser user = null)
        {
            var k = count;
            await ctx.Message.DeleteAsync();
            while (k != 0)
            { 
            var messages = ctx.Channel.GetMessagesAsync(k).Result;
                foreach (var message in messages)
                {
                    if (user != null && message.Author == user)
                    {
                        await ctx.Channel.DeleteMessageAsync(message);
                        k--;
                    }
                    else if (user == null)
                    {
                        await ctx.Channel.DeleteMessageAsync(message);
                        k--;
                    }
                }
            }
        }

        [Command("quotes")]
        public async Task Quote(CommandContext ctx, string person)
        {
            var qt = quotes.AllQuotes.FirstOrDefault(x=> x.Keys.Any(key => key.ToLower() == person.ToLower()));
            
            //var qt = QuotesModule.Find(person);
            var embed = new DiscordEmbedBuilder();

            if (qt == null)
            {
                embed.WithAuthor("Цитаты Великих Людей");
                embed.WithDescription("Доступные цитаты:");
                foreach (var quote in quotes.AllQuotes)
                {
                    var keyString = string.Empty;
                    foreach (var key in quote.Keys)
                        keyString += key + "\n";
                    embed.AddField(quote.Author, keyString);
                }
                await ctx.Channel.SendMessageAsync(embed.Build());
                return;
            }
            embed.WithAuthor("Цитаты Великих Людей");
            embed.WithDescription(qt.Quotes[rnd.Next(0, qt.Quotes.Length)]);
            embed.WithImageUrl(qt.Image);
            embed.WithFooter(qt.Author);
            embed.WithTimestamp(DateTimeOffset.Now);

            await ctx.Channel.SendMessageAsync(embed.Build());
        }

        [Command("quotes")]
        public async Task Quote(CommandContext ctx)
        {
            var qt = quotes.AllQuotes[rnd.Next(0, quotes.AllQuotes.Count)];
            if (qt == null)
            {
                return;
            }
            var embed = new DiscordEmbedBuilder();
            embed.WithAuthor("Цитаты Великих Людей");
            embed.WithDescription(qt.Quotes[rnd.Next(0, qt.Quotes.Length)]);
            embed.WithImageUrl(qt.Image);
            embed.WithFooter(qt.Author);
            embed.WithTimestamp(DateTimeOffset.Now);

            await ctx.Channel.SendMessageAsync(embed.Build());
        }

    }
}
