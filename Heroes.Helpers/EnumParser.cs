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

                season = Regex.Replace(season, @"\s+", string.Empty);
                if (Enum.TryParse(season, true, out Season seasonEnum))
                    return seasonEnum;
                else
                    throw new ArgumentException(nameof(season));
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

            public static Region ConvertRegionStringtoEnum(string region)
            {
                if (string.IsNullOrEmpty(region))
                    throw new ArgumentNullException(nameof(region));

                region = Regex.Replace(region, @"\s+", string.Empty);

                if (Enum.TryParse(region, true, out Region regionEnum))
                    return regionEnum;
                else
                    throw new ArgumentException(nameof(region));
            }
        }
    }
}
