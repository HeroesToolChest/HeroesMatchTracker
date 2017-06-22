using Heroes.ReplayParser;
using System;
using System.Text.RegularExpressions;
using static Heroes.Helpers.HeroesHelpers.Regions;
using static Heroes.ReplayParser.DataParser;

namespace Heroes.Helpers
{
    public static partial class HeroesHelpers
    {
        public static class EnumParser
        {
            public static Season ConvertSeasonStringToEnum(string season)
            {
                if (string.IsNullOrEmpty(season))
                    throw new ArgumentNullException(nameof(season));

                switch (season)
                {
                    case Seasons.LifeTime:
                        return Season.Lifetime;
                    case Seasons.Preseason:
                        return Season.Preseason;
                    case Seasons.Year2016Season1:
                        return Season.Year2016Season1;
                    case Seasons.Year2016Season2:
                        return Season.Year2016Season2;
                    case Seasons.Year2016Season3:
                        return Season.Year2016Season3;
                    case Seasons.Year2017Season1:
                        return Season.Year2017Season1;
                    default:
                        throw new ArgumentException($"paramter {season} not found", nameof(season));
                }
            }

            public static ReplayParseResult ConvertReplayParseResultStringToEnum(string replayParseResult)
            {
                if (string.IsNullOrEmpty(replayParseResult))
                    throw new ArgumentNullException(nameof(replayParseResult));

                replayParseResult = Regex.Replace(replayParseResult, @"\s+", string.Empty);

                if (Enum.TryParse(replayParseResult, true, out ReplayParseResult replayParseResultEnum))
                    return replayParseResultEnum;
                else
                    throw new ArgumentException($"paramter {replayParseResult} not found", nameof(replayParseResult));
            }

            public static Region ConvertRegionStringToEnum(string region)
            {
                if (string.IsNullOrEmpty(region))
                    throw new ArgumentNullException(nameof(region));

                region = Regex.Replace(region, @"\s+", string.Empty);

                if (Enum.TryParse(region, true, out Region regionEnum))
                    return regionEnum;
                else
                    throw new ArgumentException($"parameter {region} not found", nameof(region));
            }

            public static GameMode ConvertGameModeStringToEnum(string gameMode)
            {
                switch (gameMode)
                {
                    case "Quick Match":
                        return GameMode.QuickMatch;
                    case "Unranked Draft":
                        return GameMode.UnrankedDraft;
                    case "Hero League":
                        return GameMode.HeroLeague;
                    case "Team League":
                        return GameMode.TeamLeague;
                    case "Custom Game":
                        return GameMode.Custom;
                    case "Brawl":
                        return GameMode.Brawl;
                    default:
                        throw new ArgumentException($"parameter {gameMode} not found", nameof(gameMode));
                }
            }

            public static OverviewHeroStatOption ConvertHeroStatOptionToEnum(string overviewHeroStatOption)
            {
                switch (overviewHeroStatOption)
                {
                    case "Highest Win Rate As":
                        return OverviewHeroStatOption.HighestWinRate;
                    case "Most Wins as":
                        return OverviewHeroStatOption.MostWins;
                    case "Most Deaths as":
                        return OverviewHeroStatOption.MostDeaths;
                    case "Most Kills as":
                        return OverviewHeroStatOption.MostKills;
                    case "Most Assists as":
                        return OverviewHeroStatOption.MostAssists;
                    default:
                        throw new ArgumentException($"parameter {overviewHeroStatOption} not found", nameof(overviewHeroStatOption));
                }
            }
        }
    }
}
