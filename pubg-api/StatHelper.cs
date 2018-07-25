using System;
using System.Collections.Generic;
using Pubg.Net.Models.Stats;

namespace DiscordBot
{

using GetStat = Func<PubgGameModeStats, object>;
using StatAggregator = Func<PubgGameModeStats[], Func<PubgGameModeStats, object>, object>;

public enum StatType
{
    Wins,
    Losses,
    Kills,
    Assists,
    Deaths,
    KillDeathRatio,
    KillAssistDeathRatio,
    Matches
}

public static class StatHelper
{
    public static Dictionary<string, StatType> StatTypeMap = new Dictionary<string, StatType>(){
        { "wins", StatType.Wins },
        { "losses", StatType.Losses },
        { "kills", StatType.Kills },
        { "assists", StatType.Assists },
        { "deaths", StatType.Deaths },
        { "kd", StatType.KillDeathRatio },
        { "kda", StatType.KillAssistDeathRatio },
        { "matches", StatType.Matches },
    };

    public static Dictionary<StatType, string> StatTypeFormatMap = new Dictionary<StatType, string>(){
        { StatType.Wins, "{0} has {1} wins" },
        { StatType.Losses, "{0} has {1} losses" },
        { StatType.Assists, "{0} has {1} assists" },
        { StatType.Kills, "{0} has {1} kills" },
        { StatType.Deaths, "{0} has {1} deaths" },
        { StatType.KillDeathRatio, "{0}'s KD ratio is {1}" },
        { StatType.KillAssistDeathRatio, "{0}'s KDA ratio is {1}" },
        { StatType.Matches, "{0} has played {1} match(es)" },
    };

    private static Dictionary<StatType, Tuple<StatAggregator, GetStat>> _statAggregators = new Dictionary<StatType, Tuple<StatAggregator, GetStat>>()
    {
        { StatType.Wins, new Tuple<StatAggregator, GetStat>(SumIntegers, (gameModeStats) => { return gameModeStats.Wins; }) },
        { StatType.Losses, new Tuple<StatAggregator, GetStat>(SumIntegers, (gameModeStats) => { return gameModeStats.Losses; }) },
        { StatType.Kills, new Tuple<StatAggregator, GetStat>(SumIntegers, (gameModeStats) => { return gameModeStats.Kills; }) },
        { StatType.Assists, new Tuple<StatAggregator, GetStat>(SumIntegers, (gameModeStats) => { return gameModeStats.Assists; }) },
        { StatType.Deaths, new Tuple<StatAggregator, GetStat>(SumIntegers, (gameModeStats) => { return gameModeStats.RoundsPlayed - gameModeStats.Wins; }) },
        { StatType.Matches, new Tuple<StatAggregator, GetStat>(SumIntegers, (gameModeStats) => { return gameModeStats.RoundsPlayed; }) },
        { StatType.KillDeathRatio, new Tuple<StatAggregator, GetStat>(CaluclateKillDeathRatio, null) },
        { StatType.KillAssistDeathRatio, new Tuple<StatAggregator, GetStat>(CaluclateKillAssistDeathRatio, null) },
    };

    private static object CaluclateKillAssistDeathRatio(PubgGameModeStats[] stats, Func<PubgGameModeStats, object> unused)
    {
        var kills = GetStats(StatType.Kills, stats);
        var assists = GetStats(StatType.Assists, stats);
        var deaths = GetStats(StatType.Deaths, stats);
        return Math.Round(Convert.ToDouble((int) kills + (int) assists) / Convert.ToDouble(deaths), 3);
    }

    private static object CaluclateKillDeathRatio(PubgGameModeStats[] stats, Func<PubgGameModeStats, object> unused)
    {
        var kills = GetStats(StatType.Kills, stats);
        var deaths = GetStats(StatType.Deaths, stats);
        return Math.Round(Convert.ToDouble(kills) / Convert.ToDouble(deaths), 3);
    }

    private static object SumIntegers(PubgGameModeStats[] stats, Func<PubgGameModeStats, object> getInteger)
    {
        int sum = 0;

        foreach (var stat in stats)
        {
            sum += (int) getInteger(stat);
        }

        return sum;
    }

    public static object GetStats(StatType statType, PubgGameModeStats[] gameModeStats)
    {
        if (!_statAggregators.ContainsKey(statType))
        {
            return null;
        }

        return _statAggregators[statType].Item1(gameModeStats, _statAggregators[statType].Item2);
    }

}


}