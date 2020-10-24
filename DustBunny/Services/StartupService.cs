using System;
using System.Reflection;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using DustBunny.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace DustBunny.Services
{
    public class StartupService
    {
        private readonly DiscordSocketClient _discord;
        private readonly CommandService _commands;
        private readonly DustBunnySettings _config;
        private readonly IServiceProvider _serviceProvider;

        public StartupService(
            DiscordSocketClient discord,
            CommandService commands,
            IOptions<DustBunnySettings> config, IServiceProvider serviceProvider)
        {
            _config = config.Value;
            _serviceProvider = serviceProvider;
            _discord = discord;
            _commands = commands;
        }

        public async Task StartAsync()
        {
            string discordToken = _config.Tokens.Discord; 
            if (string.IsNullOrWhiteSpace(discordToken))
                throw new Exception("Please enter your bot's token into the `BotConfig.json` file found in the applications root directory.");

            await _discord.LoginAsync(TokenType.Bot, discordToken);     
            await _discord.StartAsync();                                

            await _commands.AddModulesAsync(Assembly.GetEntryAssembly(), _serviceProvider);
        }
    }
}