using Heroes.ReplayParser;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using static Heroes.ReplayParser.DataParser;

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
                throw new ArgumentNullException("season");

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
                throw new ArgumentNullException("gameMode");

            GameMode gameModeEnum;
            gameMode = Regex.Replace(gameMode, @"\s+", "");

            if (gameMode == "AllTypes" || gameMode == "AllGameModes")
                return GameMode.Cooperative;
            else if (Enum.TryParse(gameMode, true, out gameModeEnum))
                return gameModeEnum;
            else
                throw new Exception("GetSeasonFromString failed to convert to Season Enum");
        }

        public static ReplayParseResult GetReplayParseResultFromString(string replayParseResult)
        {
            if (string.IsNullOrEmpty(replayParseResult))
                throw new ArgumentNullException("replayParseResult");

            ReplayParseResult replayParseResultEnum;
            replayParseResult = Regex.Replace(replayParseResult, @"\s+", "");

            if (Enum.TryParse(replayParseResult, true, out replayParseResultEnum))
                return replayParseResultEnum;
            else
                throw new Exception("GetReplayParseResultFromString failed to convert to ReplayParseResult Enum");
        }

        public static int CalculateWinPercentage(int wins, double total)
        {
            return total != 0 ? (int)(Math.Round(wins / total, 2) * 100) : 0;
        }

        public static List<string> GetSeasonList()
        {
            List<string> list = new List<string>();
            list.Add("Preseason");
            list.Add("Season 1");
            list.Add("Season 2");
            list.Add("Season 3");

            return list;
        }

        public static List<string> GetGameModeList()
        {
            List<string> list = new List<string>();
            list.Add("Quick Match");
            list.Add("Unranked Draft");
            list.Add("Hero League");
            list.Add("Team League");

            return list;
        }

        public static List<string> GetBuildsList()
        {
            List<string> list = App.HeroesInfo.GetListOfHeroesBuilds().ConvertAll(x => x.ToString());
            
            list.Add("47219"); list.Add("47133"); list.Add("46889");
            list.Add("46869"); list.Add("46787"); list.Add("46690"); list.Add("46446");
            list.Add("46158"); list.Add("45635"); list.Add("45228"); list.Add("44941");
            list.Add("44797"); list.Add("44468"); list.Add("44468"); list.Add("44124");
            list.Add("43905"); list.Add("43571"); list.Add("43259"); list.Add("43170");
            list.Add("43051"); list.Add("42958"); list.Add("42590"); list.Add("42506");
            list.Add("42406"); list.Add("42273"); list.Add("42178"); list.Add("41810");
            list.Add("41504"); list.Add("41393"); list.Add("41150"); list.Add("40798");
            list.Add("40697"); list.Add("40431"); list.Add("40322"); list.Add("40087");
            list.Add("39951"); list.Add("39709"); list.Add("39595"); list.Add("39445");

            // end here, no need to add any earlier builds
            return list;
        }

        public static Tuple<int?, int?> GetSeasonReplayBuild(Season season)
        {
            // item 1: beginning (inclusive)
            // item 2: end (exclusive)

            switch (season)
            {
                case Season.Season3:
                    return new Tuple<int?, int?>(48760, 99999);
                case Season.Season2:
                    return new Tuple<int?, int?>(45949, 48760);
                case Season.Season1:
                    return new Tuple<int?, int?>(43571, 45949);
                case Season.Preseason:
                    return new Tuple<int?, int?>(0, 43571);
                case Season.Lifetime:
                    return new Tuple<int?, int?>(0, 99999);
                default:
                    return null;
            }
        }
    }
}
