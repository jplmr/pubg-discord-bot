using System;
using System.Threading.Tasks;
using Pubg.Net;

namespace DiscordBot
{

class CompareCommand : ICommand
{
    private string _player1, _player2;
    private Perspective? _perspective;
    private TeamSize? _teamSize;

    public CompareCommand(string[] commandArguments)
    {
        this._player1 = commandArguments[1];
        this._player2 = commandArguments[2];
        this._teamSize = TypeHelper.GetTeamSize(commandArguments[3]);
        this._perspective = TypeHelper.GetPerspective(commandArguments[4]);
    }

    public async Task<string> Execute()
    {
        var message = string.Format("Let's see how {0} and {1} compare:\n\n", this._player1, this._player2);

        var player1Stats = await SeasonHelper.GetCurrentSeasonStatsForPlayer(this._player1, PubgRegion.PCNorthAmerica, this._perspective, this._teamSize);
        var player2Stats = await SeasonHelper.GetCurrentSeasonStatsForPlayer(this._player2, PubgRegion.PCNorthAmerica, this._perspective, this._teamSize);

        foreach(StatType statType in Enum.GetValues(typeof(StatType)))
        {
            var player1statResult = StatHelper.GetStats(statType, player1Stats);
            var player1message = string.Format(StatHelper.StatTypeFormatMap[statType], this._player1, player1statResult);

            var player2statResult = StatHelper.GetStats(statType, player2Stats);
            var player2message = string.Format(StatHelper.StatTypeFormatMap[statType], this._player2, player2statResult);

            message += string.Format("{0}: {1} and {2}\n", statType.ToString(), player1message, player2message);
        }

        return message;
    }
}

}