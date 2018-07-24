using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Pubg.Net;
using Pubg.Net.Exceptions;

namespace DiscordBot
{
    public class StatsCommand : ICommand
    {
        private string _stat;
        private string _playerName;
        private TeamSize? _teamSize;
        private Perspective? _perspective;

        private PubgPlayerService _playerService;

        public StatsCommand(string[] commandArguments){
            this._stat = commandArguments[0];
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

            switch(this._stat)
            {
                case "wins":
                {
                    var wins = SeasonHelper.GetWins(pubgPlayerSeason.GameModeStats, this._perspective, this._teamSize);

                    var message = string.Format("{0} has {1} wins", this._playerName, wins);

                    if (this._perspective.HasValue || this._teamSize.HasValue)
                    {
                        message += " in ";
                        message += this._perspective.HasValue ? TypeHelper.PerspectiveToString(this._perspective) + " " : "";
                        message += this._teamSize.HasValue ? TypeHelper.TeamSizeToString(this._teamSize): "";
                    }

                    return message.Trim();
                }

                default:
                    return string.Format("stat {0} not supported", this._stat);
            }
        }

    }
}