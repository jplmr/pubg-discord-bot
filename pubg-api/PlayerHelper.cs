using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Pubg.Net;

namespace DiscordBot
{

public static class PlayerHelper
{
    private static TimeSpan cacheTimeout = new TimeSpan(0, 5, 0);
    private static PubgPlayerService _playerService = new PubgPlayerService(Configuration.ActiveConfiguration.pubgToken);
    private static Dictionary<string, Tuple<PubgPlayer, DateTime>> cachedPlayers = new Dictionary<string, Tuple<PubgPlayer, DateTime>>();

    public static async Task<PubgPlayer> GetPlayerFromName(string playerName, PubgRegion region)
    {
        if (cachedPlayers.ContainsKey(playerName) && DateTime.Now - cachedPlayers[playerName].Item2 < cacheTimeout)
        {
            System.Console.WriteLine("Cache hit for player {0}", playerName);
            return cachedPlayers[playerName].Item1;
        }

        var playerFilter = new GetPubgPlayersRequest()
        {
            PlayerNames = new string[] { playerName }
        };

        var pubgPlayers = await _playerService.GetPlayersAsync(region, playerFilter);
        using (var pubgPlayersEnumerator = pubgPlayers.GetEnumerator())
        {
            if (pubgPlayersEnumerator.MoveNext())
            {
                var pubgPlayer = pubgPlayersEnumerator.Current;
                cachedPlayers[playerName] = new Tuple<PubgPlayer, DateTime>(pubgPlayer, DateTime.Now);
                return pubgPlayer;
            }
        }

        return null;
    }

    public static async Task<PubgPlayerSeason> GetPlayerSeason(PubgPlayer player, PubgRegion region, PubgSeason season)
    {
        return await _playerService.GetPlayerSeasonAsync(PubgRegion.PCNorthAmerica, player.Id, season.Id);
    }

}

}