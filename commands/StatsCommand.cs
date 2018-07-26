using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Pubg.Net;
using Pubg.Net.Exceptions;
using Pubg.Net.Models.Stats;

namespace DiscordBot
{
    public class StatsCommand : ICommand
    {
        private StatType _stat;
        private string _playerName;
        private TeamSize? _teamSize;
        private Perspective? _perspective;

        public StatsCommand(string[] commandArguments){
            this._stat = StatHelper.StatTypeMap[commandArguments[0].ToLower()];
            this._playerName = commandArguments[1];
            this._teamSize = TypeHelper.GetTeamSize(commandArguments[2]);
            this._perspective = TypeHelper.GetPerspective(commandArguments[3]);
        }

        public async Task<string> Execute()
        {
            var gameModeStats = await SeasonHelper.GetCurrentSeasonStatsForPlayer(this._playerName, PubgRegion.PCNorthAmerica, this._perspective, this._teamSize);
            if (gameModeStats == null)
            {
                return "error - no stats for active season";
            }

            var statResult = StatHelper.GetStats(this._stat, gameModeStats);
            var message = string.Format(StatHelper.StatTypeFormatMap[this._stat], this._playerName, statResult);

            if (this._perspective.HasValue || this._teamSize.HasValue)
            {
                message += " in ";
                message += this._perspective.HasValue ? TypeHelper.PerspectiveToString(this._perspective) + " " : "";
                message += this._teamSize.HasValue ? TypeHelper.TeamSizeToString(this._teamSize): "";
            }
            
            return message.Trim() + " this sesason";
        }
    }
}