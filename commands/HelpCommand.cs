using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DiscordBot {
public class HelpCommand : ICommand
{
    private string _command;

    private static Dictionary<string, Tuple<string, string>> _helpDictionary = new Dictionary<string, Tuple<string, string>>(){
        { "wins", new Tuple<string, string>("playerName (solo|duo|squad) (fpp|tpp)", "number of wins") },
        { "losses", new Tuple<string, string>("playerName (solo|duo|squad) (fpp|tpp)", "number of losses") },
        { "kills", new Tuple<string, string>("playerName (solo|duo|squad) (fpp|tpp)", "number of kills") },
        { "deaths", new Tuple<string, string>("playerName (solo|duo|squad) (fpp|tpp)", "number of deaths") },
        { "assists", new Tuple<string, string>("playerName (solo|duo|squad) (fpp|tpp)", "number of assists") },
        { "kd", new Tuple<string, string>("playerName (solo|duo|squad) (fpp|tpp)", "kill/death ratio") },
        { "kda", new Tuple<string, string>("playerName (solo|duo|squad) (fpp|tpp)", "(kill+assist)/death ratio") },
        { "matches", new Tuple<string, string>("playerName (solo|duo|squad) (fpp|tpp)", "number of matches played") },
        { "compare", new Tuple<string, string>("playerName playerName (solo|duo|squad) (fpp|tpp)", "compare all available stats for given players") },
        { "help", new Tuple<string, string>("(command)", "get help") },
    };

    public HelpCommand(string[] commandArgs)
    {
        this._command = commandArgs[1];
    }

    public Task<string> Execute()
    {
        var message = "";

        if (this._command != null && this._command.Length != 0)
        {
            message = GetCommandHelp(this._command);

            if (message != null)
            {
                return Task.FromResult(message);
            }
            else
            {
                message = "Sorry, we don't support that command. ";
            }
        }

        message += GetAllCommandsHelp();
        return Task.FromResult(message);
    }

    private static string GetAllCommandsHelp()
    {
        var message = "Here is a list of available commands. Things in parenthesis () can be provided to scope down the command.\n\n";

        foreach(var helpItem in _helpDictionary)
        {
            message += GetCommandHelp(helpItem.Key);
        }

        return message;
    }

    private static string GetCommandHelp(string command)
    {
        if(!_helpDictionary.ContainsKey(command))
        {
            return null;
        }

        var helpItem = _helpDictionary[command];
        return string.Format("`{0} {1}`: {2}\n\n", command, helpItem.Item1, helpItem.Item2);
    }

}

}