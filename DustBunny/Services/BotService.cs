using Discord;
using Discord.WebSocket;
using DustBunny.Models;
using Microsoft.Extensions.Options;
using System;
using System.Linq;
using Process = System.Diagnostics.Process;

namespace DustBunny.Services
{
    public class BotService
    {
        private readonly DiscordSocketClient _discord;
        private readonly DustBunnySettings _config;

        public BotService(DiscordSocketClient discord, IOptions<DustBunnySettings> config)
        {
            _discord = discord;
            _config = config.Value;
        }

        /// <summary>
        /// Check status of our bot withint Discord.
        /// </summary>
        /// <param name="botId">This is the ulong bot ID, in string form, for the bot we want to check status of.</param>
        /// <returns>If it can't detect the status of the bot, it's offline, and returns false. If it can, all is swell.</returns>
        public bool CheckIfBotIsOnline(string botId)
        {
            var server = _discord.GetGuild(ulong.Parse(_config.Ids.Server));
            var serverUser = server.GetUser(ulong.Parse(botId));

            try
            {
                return serverUser.Status == UserStatus.Online;
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// Check Windows processes to determine if the bot is up and running.
        /// </summary>
        /// <param name="processName">The name of the Process to check.</param>
        /// <param name="filePath">The file path of the process to ensure we have the right one.</param>
        /// <returns></returns>
        public bool CheckIfProcessIsRunning(string processName, string filePath)
        {
            var processes = Process.GetProcesses();
            var p = processes.FirstOrDefault(x => x.ProcessName.Equals(processName));

            return p != null && p.MainModule.FileName.Equals(filePath, StringComparison.CurrentCultureIgnoreCase);
        }

        /// <summary>
        /// Return the system process if it's currently running.
        /// </summary>
        /// <param name="processName">The name of the Process to check.</param>
        /// <returns></returns>
        public Process GetProcessIfRunning(string processName)
        {
            var processes = Process.GetProcesses();
            var p = processes.FirstOrDefault(x => x.ProcessName.Equals(processName));

            return p;
        }

        /// <summary>
        /// If the bot is offline, this will be called to start the process.
        /// </summary>
        /// <param name="path">Path to the process that we want to launch.</param>
        public void StartProcess(string path)
        {
            Process.Start(path);
        }
    }
}