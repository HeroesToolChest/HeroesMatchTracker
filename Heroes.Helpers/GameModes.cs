using Heroes.ReplayParser;
using System.Collections.Generic;

namespace Heroes.Helpers
{
    public static partial class HeroesHelpers
    {
        public static class GameModes
        {
            public static List<string> GetAllGameModeList()
            {
                List<string> list = new List<string>();
                list.Add("Quick Match");
                list.Add("Unranked Draft");
                list.Add("Hero League");
                list.Add("Team League");
                list.Add("Brawl");
                list.Add("Custom Game");

                return list;
            }

            public static List<string> GetNormalGameModeList()
            {
                List<string> list = new List<string>();
                list.Add("Quick Match");
                list.Add("Unranked Draft");
                list.Add("Hero League");
                list.Add("Team League");

                return list;
            }

            public static string GetStringFromGameMode(GameMode gameMode)
            {
                switch (gameMode)
                {
                    case GameMode.QuickMatch:
                        return "Quick Match";
                    case GameMode.UnrankedDraft:
                        return "Unranked Draft";
                    case GameMode.HeroLeague:
                        return "Hero League";
                    case GameMode.TeamLeague:
                        return "Team League";
                    default:
                        return gameMode.ToString();
                }
            }
        }
    }
}
