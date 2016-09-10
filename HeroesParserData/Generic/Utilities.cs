using System;

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

        public static Tuple<int?, int?> GetSeasonReplayBuild(string season)
        {
            // item 1: beginning
            // item 2: end

            if (season == "Season 2")
                return new Tuple<int?, int?>(45949, 99999);
            else if (season == "Season 1")
                return new Tuple<int?, int?>(43571, 45949);
            else if (season == "Preseason")
                return new Tuple<int?, int?>(0, 43571);
            else
                return null;
        }
    }
}
