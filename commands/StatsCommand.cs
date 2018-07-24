using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Pubg.Net;

namespace DiscordBot
{
    public class StatsCommand : ICommand
    {
        private string _userId;
        private PubgPlayerService _playerService;

        public StatsCommand(string[] commandArguments){
            this._userId = commandArguments[0];
            this._playerService = new PubgPlayerService();
        }

        public async Task<string> Execute()
        {
            var pubgPlayer = await _playerService.GetPlayerSeasonAsync(PubgRegion.PCNorthAmerica, _userId, "2018-07");
            return string.Format("{0} has {1} solo FFP NA win(s)", this._userId, pubgPlayer.GameModeStats.SoloFPP.Wins);
        }
    }
}