using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Pubg.Net;
using Pubg.Net.Exceptions;

namespace DiscordBot
{
    public class StatsCommand : ICommand
    {
        private string _playerName;
        private PubgPlayerService _playerService;
        private PubgSeasonService _seasonService;

        public StatsCommand(string playerName){
            this._playerName = playerName;
            this._playerService = new PubgPlayerService();
            this._seasonService = new PubgSeasonService();
        }

        public async Task<string> Execute()
        {
            PubgSeason currentSeason = null;
            using (var pubgSeasons = _seasonService.GetSeasons(PubgRegion.PCNorthAmerica).GetEnumerator())
            {
                while (pubgSeasons.MoveNext())
                {
                    if (pubgSeasons.Current.IsCurrentSeason)
                    {
                        currentSeason = pubgSeasons.Current;
                        break;
                    }
                }
            }

            if (currentSeason == null)
            {
                return "error";
            }

            // get ID from player name
            var playerFilter = new GetPubgPlayersRequest()
            {
                PlayerNames = new string[] { this._playerName }
            };
            
            try
            {
                var pubgPlayers = await _playerService.GetPlayersAsync(PubgRegion.PCNorthAmerica, playerFilter);
                using (var pubgPlayersEnumerator = pubgPlayers.GetEnumerator())
                {
                    if (pubgPlayersEnumerator.MoveNext())
                    {
                        var playerId = pubgPlayersEnumerator.Current.Id;
                        var pubgPlayer = await _playerService.GetPlayerSeasonAsync(PubgRegion.PCNorthAmerica, playerId, currentSeason.Id);
                        return string.Format("{0} has {1} win(s) in duo FPP this season", this._playerName, pubgPlayer.GameModeStats.DuoFPP.Wins);
                    }
                }
            } 
            catch (PubgNotFoundException)
            {
                return string.Format("error - could not find stats for player {0}", this._playerName);
            }

            return string.Format("error - could not find stats for player {0}", this._playerName);
        }

        public static StatsCommand TryCreate(string input)
        {
            string pattern = @"wins\s+(\w+)";
            Regex r = new Regex(pattern, RegexOptions.IgnoreCase);
            Match m = r.Match(input);

            if (m.Groups.Count == 2)
            {
                return new StatsCommand(m.Groups[1].Value);
            }

            return null;
        }
    }
}