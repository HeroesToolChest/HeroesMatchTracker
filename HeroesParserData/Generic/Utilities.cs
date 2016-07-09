using System;

namespace HeroesParserData
{
    public class Utilities
    {
        public static string GetBattleTagName(string playerName, int playerTagNumber)
        {
            return $"{playerName}#{playerTagNumber}";
        }

        public static string GetNameFromBattleTagName(string battleTagName)
        {
            return battleTagName.Substring(0, battleTagName.IndexOf('#'));
        }

        public static int GetTagFromBattleTagName(string battleTagName)
        {
            return Convert.ToInt32(battleTagName.Substring(battleTagName.IndexOf('#') + 1));
        }

        /// <summary>
        /// Returns the season for the given build
        /// </summary>
        /// <param name="replayBuild">The replay build number of the replay</param>
        /// <returns>The season for the replay</returns>
        //public static string GetSeasonForGame(int replayBuild)
        //{
        //    // current: 43051

        //    string season = String.Empty;
        //    if (replayBuild <= 48000)
        //        season = "Preseason";
        //    else if (replayBuild <= 55000)
        //        season = "Season 1";
        //    else
        //    {
        //        season = "Unknown";
        //    }

        //    return season;
        //}

        //public static decimal CalculateWinPercentage(int win, int lose)
        //{
        //    int total = win + lose;
        //    return (total > 0) ? (win / total) * 100 : (decimal)0.00;        
        //}
    }
}
