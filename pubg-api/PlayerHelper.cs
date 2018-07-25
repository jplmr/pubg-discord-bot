using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Pubg.Net;

namespace DiscordBot
{

using PlayerSeasonKey = Tuple<string, PubgRegion, string>;

public static class PlayerHelper
{
    private static TimeSpan cacheTimeout = new TimeSpan(0, 1, 0);
    private static PubgPlayerService _playerService = new PubgPlayerService(Configuration.ActiveConfiguration.pubgToken);
    private static Dictionary<string, Tuple<PubgPlayer, DateTime>> cachedPlayers = new Dictionary<string, Tuple<PubgPlayer, DateTime>>();
    private static Dictionary<PlayerSeasonKey, Tuple<PubgPlayerSeason, DateTime>> cachedPlayerSeasons = new Dictionary<PlayerSeasonKey, Tuple<PubgPlayerSeason, DateTime>>();

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
        var cacheKey = new PlayerSeasonKey(player.Id, region, season.Id);
        if (cachedPlayerSeasons.ContainsKey(cacheKey) && DateTime.Now - cachedPlayerSeasons[cacheKey].Item2 < cacheTimeout)
        {
            System.Console.WriteLine("Cache hit for player season {0}", player.Name);
            return cachedPlayerSeasons[cacheKey].Item1;
        }

        var pubgPlayerSeason = await _playerService.GetPlayerSeasonAsync(region, player.Id, season.Id);
        cachedPlayerSeasons[cacheKey] = new Tuple<PubgPlayerSeason, DateTime>(pubgPlayerSeason, DateTime.Now);
        return pubgPlayerSeason;
    }

}

}