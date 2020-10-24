using System;
using System.Threading;
using Discord;
using Discord.WebSocket;
using DustBunny.Models;
using Microsoft.Extensions.Options;

namespace DustBunny.Services
{
    public class QueueService : IDisposable
    {
        private Timer _botOnlineTimer;
        private Timer _processRunningTimer;

        private readonly BotService _botService;
        private readonly DiscordSocketClient _discord;
        private readonly DustBunnySettings _config;

        public QueueService(BotService botService, DiscordSocketClient discord, IOptions<DustBunnySettings> config)
        {
            _botService = botService;
            _discord = discord;
            _config = config.Value;
        }

        public void Startup()
        {
            _botOnlineTimer = new Timer(async (e) =>
            {
                // Start looping through Bot configurations to determine online status.
                foreach (var bot in _config.Bots)
                {
                    // Get the channel to announce in if we have an issue.
                    var announceChannel = (IMessageChannel)_discord.GetChannel(ulong.Parse(bot.AnnounceChannel));

                    // Get the status of the bot via Discord.
                    var online = _botService.CheckIfBotIsOnline(bot.Id);

                    // Get the status of the bot via system process.
                    var processRunning = _botService.CheckIfProcessIsRunning(bot.ProcessName, bot.Path);

                    // If the bot previously went down, but we're online now .. let them know all is well.
                    if (bot.OfflineTime.HasValue && online)
                    {
                        bot.OfflineTime = null;
                        await announceChannel?.SendMessageAsync($"{bot.Name} is now online. IT'S ALL OK GUYS.");
                    }
                    // If the bot previously went down, but we're still offline .. send another update.
                    else if (bot.OfflineTime.HasValue && !online)
                    {
                        var span = DateTime.UtcNow.Subtract(bot.OfflineTime.Value);

                        if (((int)span.TotalMinutes) % 15 == 0)
                        {
                            await announceChannel?.SendMessageAsync($"{bot.Name} has been offline for {(int)span.TotalMinutes} minutes. We are aware of the situation, and attempting to get it resolved.");
                        }
                    }
                    // If the bot wasn't down, but we're offline now .. announce the bot has gone down.
                    else if (!bot.OfflineTime.HasValue && !online)
                    {
                        bot.OfflineTime = DateTime.UtcNow;
                        await announceChannel?.SendMessageAsync(
                            $"It appears {bot.Name} has gone offline. I've contacted <@{_config.Ids.Owner}> and let him know. I'm also trying to restore service myself. Yes. I'm that good.");
                        
                        // Sometimes Discord has issues and boots the bot. If we don't handle reconnect OR reconnect fails, this will cycle the process.
                        var process = _botService.GetProcessIfRunning(bot.ProcessName);
                        process?.Kill();

                        _botService.StartProcess(bot.Path);
                    }
                }
            }, null, 0, 60000);

            _processRunningTimer = new Timer(async (e) =>
            {
                // Start looping through system processes to determine running status.
                foreach (var process in _config.Processes)
                {
                    // Get the channel to announce in if we have an issue.
                    var announceChannel = (IMessageChannel)_discord.GetChannel(ulong.Parse(process.AnnounceChannel));

                    // Check the system process to determine running status
                    var processRunning = _botService.CheckIfProcessIsRunning(process.ProcessName, process.Path);

                    // If the process was previously down, but it's now running, send the all clear.
                    if (process.OfflineTime.HasValue && processRunning)
                    {
                        process.OfflineTime = null;
                        await announceChannel.SendMessageAsync($"{process.Name} is now online. IT'S ALL OK GUYS.");
                    }
                    // If the process was previously down, and it's still not running - send an update.
                    else if (process.OfflineTime.HasValue && !processRunning)
                    {
                        var span = DateTime.UtcNow.Subtract(process.OfflineTime.Value);

                        if (((int)span.TotalMinutes) % 15 == 0)
                        {
                            await announceChannel.SendMessageAsync($"{process.Name} has been offline for {(int)span.TotalMinutes} minutes. We are aware of the situation, and attempting to get it resolved.");
                        }
                    }
                    // If the system process wasn't down, but we're offline now .. announce the process has gone down.
                    else if (!process.OfflineTime.HasValue && !processRunning)
                    {
                        process.OfflineTime = DateTime.UtcNow;
                        await announceChannel.SendMessageAsync(
                            $"It appears {process.Name} has gone offline. I've contacted <@{_config.Ids.Owner}> and let him know. I'm also trying to restore service myself. Yes. I'm that good.");

                        _botService.StartProcess(process.Path);
                    }
                }
            }, null, 0, 60000);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if(disposing)
            {
                _botOnlineTimer.Dispose();
                _processRunningTimer.Dispose();
            }
        }
    }
}