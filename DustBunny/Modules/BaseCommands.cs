using Discord;
using Discord.Commands;
using Discord.WebSocket;
using DustBunny.Models;
using DustBunny.Services;
using Microsoft.Extensions.Options;
using System.Threading.Tasks;

namespace DustBunny.Modules
{
    public class BaseCommands : ModuleBase
    {
        [Command("Ping")]
        public async Task Ping()
        {
            await ReplyAsync("Pong!");
        }
    }
}