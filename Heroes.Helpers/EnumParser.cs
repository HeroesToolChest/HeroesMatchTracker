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
                    case "Lifetime":
                        return Season.Lifetime;
                    case "Preseason":
                        return Season.Preseason;
                    case "2016 Season 1":
                        return Season.Year2016Season1;
                    case "2016 Season 2":
                        return Season.Year2016Season2;
                    case "2016 Season 3":
                        return Season.Year2016Season3;
                    case "2017 Season 1":
                        return Season.Year2017Season1;
                    default:
                        throw new ArgumentException(nameof(season));
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
                    throw new ArgumentException(nameof(replayParseResult));
            }

            public static Region ConvertRegionStringToEnum(string region)
            {
                if (string.IsNullOrEmpty(region))
                    throw new ArgumentNullException(nameof(region));

                region = Regex.Replace(region, @"\s+", string.Empty);

                if (Enum.TryParse(region, true, out Region regionEnum))
                    return regionEnum;
                else
                    throw new ArgumentException(nameof(region));
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
                        throw new ArgumentException(nameof(gameMode));
                }
            }
        }
    }
}
