using System;
using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;

namespace DiscordBot
{
    public class DiscordBot : DiscordSocketClient
    {
        private Configuration _config;

        public DiscordBot(Configuration config)
        {
            this._config = config;
            this.Ready += this.HandleReadyAsync;
            this.MessageReceived += this.HandleMessageReceivedAsync;
        }

        public async void LoginAndStartAsync()
        {
            await LoginAsync(TokenType.Bot, this._config.discordToken);
            await StartAsync();
        }

        private async Task HandleMessageReceivedAsync(SocketMessage arg)
        {
            if (!arg.Author.IsBot && arg.Author.Id != this.CurrentUser.Id)
            {
                await arg.Channel.SendMessageAsync("You said: " + arg.Content);
            }
        }

        private async Task HandleReadyAsync()
        {
            var enumerator = this.Guilds.GetEnumerator();
            enumerator.MoveNext();

            var channels = enumerator.Current.TextChannels.GetEnumerator();
            while (channels.MoveNext() && channels.Current.Name.ToString() != "general") {}
            var chan = enumerator.Current.GetTextChannel(channels.Current.Id);
            await chan.SendMessageAsync("Connected!");
        }
    }
}