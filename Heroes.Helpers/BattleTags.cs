using System;

namespace Heroes.Helpers
{
    public static partial class HeroesHelpers
    {
        public static class BattleTags
        {
            public static string GetBattleTagName(string playerName, int playerTagNumber)
            {
                if (string.IsNullOrEmpty(playerName))
                    return null;

                return $"{playerName}#{playerTagNumber}";
            }

            public static string GetNameFromBattleTagName(string battleTagName)
            {
                if (!battleTagName.Contains("#"))
                    return null;

                string name = battleTagName.Substring(0, battleTagName.IndexOf('#'));

                if (name == string.Empty)
                    return null;
                else
                    return name;
            }

            public static int GetTagFromBattleTagName(string battleTagName)
            {
                if (!battleTagName.Contains("#") || battleTagName[battleTagName.Length - 1] == '#')
                    return 0;

                return Convert.ToInt32(battleTagName.Substring(battleTagName.IndexOf('#') + 1));
            }
        }
    }
}
