using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Pubg.Net;
using Pubg.Net.Exceptions;

namespace DiscordBot
{
    public class StatsCommand : ICommand
    {
        private StatType _stat;
        private string _playerName;
        private TeamSize? _teamSize;
        private Perspective? _perspective;

        private PubgPlayerService _playerService;

        public StatsCommand(string[] commandArguments){
            this._stat = StatHelper.StatTypeMap[commandArguments[0]];
            this._playerName = commandArguments[1];
            this._teamSize = TypeHelper.GetTeamSize(commandArguments[2]);
            this._perspective = TypeHelper.GetPerspective(commandArguments[3]);

            this._playerService = new PubgPlayerService();
        }

        public async Task<string> Execute()
        {
            var currentSeason = SeasonHelper.GetCurrentSeason();
            if (currentSeason == null)
            {
                return "error - no active season";
            }

            var pubgPlayer = await PlayerHelper.GetPlayerFromName(this._playerName, PubgRegion.PCNorthAmerica);
            var pubgPlayerSeason = await PlayerHelper.GetPlayerSeason(pubgPlayer, PubgRegion.PCNorthAmerica, currentSeason);
            var gameModeStats = SeasonHelper.GetGameModeStats(pubgPlayerSeason.GameModeStats, this._perspective, this._teamSize);

            var statResult = StatHelper.GetStats(this._stat, gameModeStats);
            var message = string.Format(StatHelper.StatTypeFormatMap[this._stat], this._playerName, statResult);

            if (this._perspective.HasValue || this._teamSize.HasValue)
            {
                message += " in ";
                message += this._perspective.HasValue ? TypeHelper.PerspectiveToString(this._perspective) + " " : "";
                message += this._teamSize.HasValue ? TypeHelper.TeamSizeToString(this._teamSize): "";
            }

            return message.Trim();
        }
    }
}