using Discord.WebSocket;

namespace DiscordBot
{
    public class CommandParser
    {
        private ulong _botId;

        public CommandParser(ulong botId)
        {
            this._botId = botId;
        }

        public ICommand ParseCommand(SocketMessage message)
        {
            if (!IsAtMe(message))
            {
                return null;
            }

            return StatsCommand.TryCreate(message.Content);
        }

        private bool IsAtMe(SocketMessage message)
        {
            using (var usersEnumerator = message.MentionedUsers.GetEnumerator())
            {
                while (usersEnumerator.MoveNext())
                {
                    if (usersEnumerator.Current.Id == this._botId)
                    {
                        return true;
                    }
                }

                return false;
            }
        }

    }

}