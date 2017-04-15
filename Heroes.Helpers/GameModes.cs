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
                List<string> list = new List<string>
                {
                    "Quick Match",
                    "Unranked Draft",
                    "Hero League",
                    "Team League",
                    "Brawl",
                    "Custom Game",
                };

                return list;
            }

            public static List<string> GetNormalGameModeList()
            {
                List<string> list = new List<string>
                {
                    "Quick Match",
                    "Unranked Draft",
                    "Hero League",
                    "Team League",
                };

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
                    case GameMode.Custom:
                        return "Custom";
                    case GameMode.Brawl:
                        return "Brawl";
                    default:
                        return gameMode.ToString();
                }
            }
        }
    }
}
