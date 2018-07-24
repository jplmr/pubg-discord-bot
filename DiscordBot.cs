using System;
using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;
using Pubg.Net;

namespace DiscordBot
{
    public class DiscordBot : DiscordSocketClient
    {
        private Configuration _config;
        private CommandParser _commandParser;
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

        private async Task HandleMessageReceivedAsync(SocketMessage message)
        {
            if (!message.Author.IsBot && message.Author.Id != this.CurrentUser.Id)
            {
                var parsedCommand = this._commandParser.ParseCommand(message);
                if (parsedCommand == null)
                {
                    return;
                }

                var response = await parsedCommand.Execute();
                await message.Channel.SendMessageAsync(response);
            }
        }

        private async Task HandleReadyAsync()
        {
            this._commandParser = new CommandParser(this.CurrentUser.Id);

            var enumerator = this.Guilds.GetEnumerator();
            enumerator.MoveNext();

            var channels = enumerator.Current.TextChannels.GetEnumerator();
            while (channels.MoveNext() && channels.Current.Name.ToString() != "general") {}
            var chan = enumerator.Current.GetTextChannel(channels.Current.Id);
            await chan.SendMessageAsync("Connected!");
        }
    }
}