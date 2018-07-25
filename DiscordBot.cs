using System;
using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;
using Pubg.Net;
using Pubg.Net.Exceptions;

namespace DiscordBot
{
    public class DiscordBot : DiscordSocketClient
    {
        private CommandParser _commandParser;
        public DiscordBot()
        {
            this.Ready += this.HandleReadyAsync;
            this.MessageReceived += this.HandleMessageReceivedAsync;
        }

        public async void LoginAndStartAsync()
        {
            await LoginAsync(TokenType.Bot, Configuration.ActiveConfiguration.discordToken);
            await StartAsync();
        }

        private async Task HandleMessageReceivedAsync(SocketMessage message)
        {
            if (!CommandParser.IsAtMe(message, this.CurrentUser.Id))
            {
                return;
            }

            if (!message.Author.IsBot && message.Author.Id != this.CurrentUser.Id)
            {
                var parsedCommand = this._commandParser.ParseCommand(message);
                if (parsedCommand == null)
                {
                    await message.Channel.SendMessageAsync("sorry, I don't understand that yet");
                }

                try
                {
                    var response = await parsedCommand.Execute();
                    await message.Channel.SendMessageAsync(response);
                }
                catch (PubgTooManyRequestsException)
                {
                    await message.Channel.SendMessageAsync("sorry, we're being rate limited, please try again in one minute");
                }
                catch (PubgNotFoundException)
                {
                    await message.Channel.SendMessageAsync("sorry, we couldn't find anything for that player");
                }
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