using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Discord.WebSocket;

namespace DiscordBot {

public class CommandParser
{
    private ulong _botId;

    public static Dictionary<Regex, Type> Commands = new Dictionary<Regex, Type>()
    {
        { new Regex(@"(wins|losses|kills|assits|deaths|kd|kda)\s+(\w+)\s*(solo|duo|squad)?\s*(fpp|tpp)?", RegexOptions.IgnoreCase), typeof(StatsCommand) },
        { new Regex(@"(help)", RegexOptions.IgnoreCase), typeof(HelpCommand) },
    };

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

        foreach (KeyValuePair<Regex, Type> command in CommandParser.Commands)
        {
            Match matches = command.Key.Match(message.Content);
            if (matches.Groups.Count > 1)
            {
                string[] commandArguments = new string[matches.Groups.Count - 1];
                for(int i = 0; i < matches.Groups.Count - 1; ++i)
                {
                    commandArguments[i] = matches.Groups[i + 1].Value;
                }

                return (ICommand) Activator.CreateInstance(command.Value, new object[] { commandArguments });
            }
        }

        return null;
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