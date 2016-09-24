using Heroes.ReplayParser;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace HeroesParserData
{
    public static class Utilities
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

        public static bool LikeOperatorInputCheck(string operand, string input)
        {
            if (operand == "LIKE" && (input.Length == 1 || (input.Length >= 2 && input[0] != '%' && input[input.Length - 1] != '%')))
                return true;
            else
                return false;                  
        }

        public static Season GetSeasonFromString(string season)
        {
            if (string.IsNullOrEmpty(season))
                throw new Exception("GetSeasonFromString is null");

            Season seasonEnum;
            season = Regex.Replace(season, @"\s+", "");
            if (Enum.TryParse(season, true, out seasonEnum))
                return seasonEnum;
            else
                throw new Exception("GetSeasonFromString failed to convert to Season Enum");
        }

        public static GameMode GetGameModeFromString(string gameMode)
        {
            if (string.IsNullOrEmpty(gameMode))
                throw new Exception("GetSeasonFromString is null");

            GameMode gameModeEnum;
            gameMode = Regex.Replace(gameMode, @"\s+", "");
            if (Enum.TryParse(gameMode, true, out gameModeEnum))
                return gameModeEnum;
            else
                throw new Exception("GetSeasonFromString failed to convert to Season Enum");
        }

        public static List<string> GetSeasonList()
        {
            List<string> list = new List<string>();
            list.Add("Preseason");
            list.Add("Season 1");
            list.Add("Season 2");

            return list;
        }

        public static List<string> GetGameModes()
        {
            List<string> list = new List<string>();
            //list.Add("All Types");
            list.Add("Quick Match");
            list.Add("Unranked Draft");
            list.Add("Hero League");
            list.Add("Team League");

            return list;
        }

        public static Tuple<int?, int?> GetSeasonReplayBuild(Season season)
        {
            // item 1: beginning
            // item 2: end

            if (season == Season.Season2)
                return new Tuple<int?, int?>(45949, 99999);
            else if (season == Season.Season1)
                return new Tuple<int?, int?>(43571, 45949);
            else if (season == Season.Preseason)
                return new Tuple<int?, int?>(0, 43571);
            else
                return null;
        }
    }
}
