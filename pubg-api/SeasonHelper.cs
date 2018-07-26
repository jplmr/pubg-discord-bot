using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Pubg.Net;
using Pubg.Net.Models.Stats;

namespace DiscordBot
{
public static class SeasonHelper
{
    private static PubgSeasonService _seasonService = new PubgSeasonService(Configuration.ActiveConfiguration.pubgToken);

    public static PubgSeason GetCurrentSeason()
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

        return currentSeason;
    }

    public static PubgGameModeStats[] GetGameModeStats(PubgSeasonStats seasonStats, Perspective? perspective, TeamSize? teamSize)
    {
        Perspective[] perspectives = perspective.HasValue ? new Perspective[] { perspective.Value } : (Perspective[]) Enum.GetValues(typeof(Perspective));
        TeamSize[] teamSizes = teamSize.HasValue ? new TeamSize[] { teamSize.Value } : (TeamSize[]) Enum.GetValues(typeof(TeamSize));

        var gameModeStats = new List<PubgGameModeStats>();

        foreach (Perspective p in perspectives)
        {
            foreach (TeamSize t in teamSizes)
            {
                if (p.Equals(Perspective.FirstPerson) && t.Equals(TeamSize.Solo)) { gameModeStats.Add(seasonStats.SoloFPP); }
                if (p.Equals(Perspective.ThirdPesron) && t.Equals(TeamSize.Solo)) { gameModeStats.Add(seasonStats.Solo); }
                if (p.Equals(Perspective.FirstPerson) && t.Equals(TeamSize.Duo)) { gameModeStats.Add(seasonStats.DuoFPP); }
                if (p.Equals(Perspective.ThirdPesron) && t.Equals(TeamSize.Duo)) { gameModeStats.Add(seasonStats.Duo); }
                if (p.Equals(Perspective.FirstPerson) && t.Equals(TeamSize.Squad)) { gameModeStats.Add(seasonStats.SquadFPP); }
                if (p.Equals(Perspective.ThirdPesron) && t.Equals(TeamSize.Squad)) { gameModeStats.Add(seasonStats.Squad); }
            }
        }

        return gameModeStats.ToArray();
    }
    public static async Task<PubgGameModeStats[]> GetCurrentSeasonStatsForPlayer(string playerName, PubgRegion region, Perspective? perspective, TeamSize? teamSize)
    {
        var currentSeason = SeasonHelper.GetCurrentSeason();
        if (currentSeason == null)
        {
            return null;
        }

        var pubgPlayer = await PlayerHelper.GetPlayerFromName(playerName, PubgRegion.PCNorthAmerica);
        var pubgPlayerSeason = await PlayerHelper.GetPlayerSeason(pubgPlayer, PubgRegion.PCNorthAmerica, currentSeason);
        var gameModeStats = SeasonHelper.GetGameModeStats(pubgPlayerSeason.GameModeStats, perspective, teamSize);

        return gameModeStats;
    }

}

}