using System;
using System.Text.RegularExpressions;
using static Heroes.ReplayParser.DataParser;

namespace HeroesStatTracker.Core
{
    public class EnumParser
    {
        public static ReplayParseResult GetReplayParseResultFromString(string replayParseResult)
        {
            if (string.IsNullOrEmpty(replayParseResult))
                throw new ArgumentNullException("replayParseResult");

            ReplayParseResult replayParseResultEnum;
            replayParseResult = Regex.Replace(replayParseResult, @"\s+", string.Empty);

            if (Enum.TryParse(replayParseResult, true, out replayParseResultEnum))
                return replayParseResultEnum;
            else
                throw new Exception("GetReplayParseResultFromString failed to convert to ReplayParseResult Enum");
        }
    }
}
