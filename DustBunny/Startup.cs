using System;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using DustBunny.Models;
using DustBunny.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DustBunny
{
    class Startup
    {
        public IConfigurationRoot Configuration { get; }

        public Startup(string[] args)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(AppContext.BaseDirectory)
                .AddJsonFile("BotConfig.json");
            Configuration = builder.Build();
        }

        public static async Task RunAsync(string[] args)
        {
            var startup = new Startup(args);
            await startup.RunAsync();
        }

        public async Task RunAsync()
        {
            var services = new ServiceCollection();
            ConfigureServices(services);

            var provider = services.BuildServiceProvider();
            var logger = provider.GetRequiredService<LoggingService>();
            provider.GetRequiredService<CommandHandler>();

            await provider.GetRequiredService<StartupService>().StartAsync();
            var discord = provider.GetRequiredService<DiscordSocketClient>();

            // Don't progress until our user is completely logged in.
            while (discord.CurrentUser == null)
            {
                logger.LogToConsole("Waiting for User to Log In...");
                await Task.Delay(10000);
            }

            // Main entry point for bot/monitor functionality.
            provider.GetRequiredService<QueueService>().Startup();
            await Task.Delay(-1);
        }

        private void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton(new DiscordSocketClient(new DiscordSocketConfig
            {
                LogLevel = LogSeverity.Verbose,
                MessageCacheSize = 1000
            }))
                .AddSingleton(new CommandService(new CommandServiceConfig
                {
                    LogLevel = LogSeverity.Verbose,
                    DefaultRunMode = RunMode.Async,
                    CaseSensitiveCommands = false
                }))
                .AddSingleton<StartupService>()
                .AddSingleton<LoggingService>()
                .AddSingleton<CommandHandler>()
                .AddSingleton<QueueService>()
                .AddSingleton<BotService>();

            services.AddOptions();
            services.Configure<DustBunnySettings>(Configuration);
        }
    }
}
