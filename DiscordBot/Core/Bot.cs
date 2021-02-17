using DSharpPlus;
using DSharpPlus.EventArgs;
using DSharpPlus.CommandsNext;
using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Newtonsoft.Json;
using DSharpPlus.Interactivity;
using DSharpPlus.Interactivity.Extensions;
using DSharpPlus.Entities;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using DSharpPlus.CommandsNext.Exceptions;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using DiscordBot.Commands;

namespace DiscordBot
{
    class Bot
    {
        private const string DiscordConfigPath = "discordConfig.json";

        public const string BotName = "GucciBot";
        internal static EventId BotInventId { get; } = new EventId(83, BotName);
        public DiscordClient Client { get; private set; }
        public InteractivityExtension Interactivity { get; private set; }
        public CommandsNextExtension Commands { get; private set; }

        public async Task RunAsync()
        {
            Setup();

            var discordConfig = await JsonStorage.RestoreObjectAsync<Configuration.DiscordConfiguration>(DiscordConfigPath);

            var config = new DiscordConfiguration
            {
                Token = discordConfig.Token,
                TokenType = TokenType.Bot,
                AutoReconnect = true,
                MinimumLogLevel = LogLevel.Debug
            };

            Client = new DiscordClient(config);
            Client.Ready += OnClientReady;
            Client.MessageCreated += OnMessageCreated;
            Client.GuildAvailable += OnGuildAvailable;

            Client.UseInteractivity(new InteractivityConfiguration());

            var commandsConfig = new CommandsNextConfiguration
            {
                StringPrefixes = new string[] { discordConfig.Prefix },
                EnableMentionPrefix = false,
                EnableDms = true,
                CaseSensitive = false,
                Services = GetServiceProvider()
            };
            Commands = Client.UseCommandsNext(commandsConfig);
            Commands.CommandErrored += OnCommandErrored;
            Commands.CommandExecuted += OnCommandExecuted;
            Commands.RegisterCommands<UnsortedCommands>();

            await Client.ConnectAsync(new DiscordActivity("like a Gucci", ActivityType.Playing));

            await Task.Delay(-1);
        }

        private void Setup()
        {
            if (!File.Exists(DiscordConfigPath))
            {
                var cfg = new Configuration.DiscordConfiguration()
                {
                    Name = string.Empty,
                    Token = string.Empty,
                    Prefix = string.Empty
                };
                JsonStorage.StoreObject(cfg, DiscordConfigPath);

                foreach (var property in JObject.Parse(JsonConvert.SerializeObject(cfg)))
                {
                    Console.WriteLine($"Set Discord {property.Key}:");
                    JsonStorage.SetValue(DiscordConfigPath, property.Key, Console.ReadLine());
                }
            }
        }

        private Task OnGuildAvailable(DiscordClient client, GuildCreateEventArgs e)
        {
            client.Logger.LogInformation(BotInventId, "Guild available: '{0}'", e.Guild.Name);
            return Task.CompletedTask;
        }

        private async Task OnCommandErrored(CommandsNextExtension cnext, CommandErrorEventArgs e)
        {
            if (e.Exception is CommandNotFoundException && (e.Command == null || e.Command.QualifiedName != "help"))
                return;

            e.Context.Client.Logger.LogError(BotInventId, e.Exception, "Exception occurred during {0}'s invocation of '{1}'", e.Context.User.Username, e.Context.Command.QualifiedName);

            var exs = new List<Exception>();
            if (e.Exception is AggregateException ae)
                exs.AddRange(ae.InnerExceptions);
            else
                exs.Add(e.Exception);

            foreach (var ex in exs)
            {
                if (ex is CommandNotFoundException && (e.Command == null || e.Command.QualifiedName != "help"))
                    return;

                var embed = new DiscordEmbedBuilder
                {
                    Color = new DiscordColor("#FF0000"),
                    Title = "An exception occurred when executing a command",
                    Description = $"`{e.Exception.GetType()}` occurred when executing `{e.Command.QualifiedName}`.",
                    Timestamp = DateTime.UtcNow
                };
                embed.WithFooter(Client.CurrentUser.Username, Client.CurrentUser.AvatarUrl)
                    .AddField("Message", ex.Message);
                await e.Context.RespondAsync(embed: embed.Build());
            }
        }

        private async Task OnMessageCreated(DiscordClient sender, MessageCreateEventArgs e)
        {
            Console.WriteLine($"{e.Guild.Name}/#{e.Channel.Name.TrimStart("Channel".ToCharArray())}/{e.Author.Username}: {e.Message.Content}");

            await Task.CompletedTask;
        }

        private async Task OnCommandExecuted(CommandsNextExtension sender, CommandExecutionEventArgs e)
        {
            Console.WriteLine($"{e.Context.Guild.Name}/#{e.Context.Channel.Name.TrimStart("Channel".ToCharArray())}/{e.Context.User.Username} successfully executed '{e.Command.QualifiedName}'");
            await Task.CompletedTask;
        }

        private async Task OnClientReady(DiscordClient sender, ReadyEventArgs e)
        {
            Console.WriteLine($"{BotName} is ready");
            await Task.CompletedTask;
        }

        //Initializing services here
        private IServiceProvider GetServiceProvider()
        {
            var collection = new ServiceCollection();

            collection.AddSingleton<Random>();

            return collection.BuildServiceProvider();
        }
    }
}