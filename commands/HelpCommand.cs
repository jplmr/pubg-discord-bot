using System.Collections.Generic;
using System.Threading.Tasks;

namespace DiscordBot {
public class HelpCommand : ICommand
{
    private string _command;
    private static Dictionary<string, string> _helpDictionary = new Dictionary<string, string>(){
        { "**wins** playerName _solo|duo|squad_ _fpp|tpp_", "number of wins this season" },
        { "**losses** playerName _solo|duo|squad_ _fpp|tpp_", "number of losses this sason" },
        { "**kills** playerName _solo|duo|squad_ _fpp|tpp_", "number of kills this season" },
        { "**deaths** playerName _solo|duo|squad_ _fpp|tpp_", "number of deaths this season (approximate)" },
        { "**assists** playerName _solo|duo|squad_ _fpp|tpp_", "number of assists this season" },
        { "**kd** playerName _solo|duo|squad_ _fpp|tpp_", "kill/death ratio" },
        { "**kda** playerName _solo|duo|squad_ _fpp|tpp_", "(kill+assist)/death ratio" },
        { "**help** _command_", "display this help message" },
    };

    public HelpCommand(string[] commandArgs)
    {

    }

    public Task<string> Execute()
    {
    }

    private string GetAllCommands(){
        var message = "Here is a list of available commands. Things in _italics_ are optional, to scope down the command.\n";

        foreach(var helpItem in _helpDictionary)
        {
            message += string.Format("{0}: {1}\n", helpItem.Key, helpItem.Value);
        }

        return message;
    }

}

}