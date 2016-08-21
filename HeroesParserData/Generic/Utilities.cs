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
    }
}
