using Discord;
using Discord.WebSocket;
using System;
using System.IO;
using System.Threading.Tasks;

namespace DiscordBot
{
    class Program
    {
        static void Main(string[] args) => new Program().MainAsync().GetAwaiter().GetResult();

        public async Task MainAsync()
        {
            var config = Configuration.Create();
            var bot = new DiscordBot(config);
            bot.LoginAndStartAsync();

            await Task.Delay(-1);
        }

    }
}
