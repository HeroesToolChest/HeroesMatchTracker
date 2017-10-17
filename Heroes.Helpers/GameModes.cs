using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace Heroes.Helpers
{
    [Flags]
    public enum GameMode
    {
        Unknown = 1 << 0,
        [Description("Custom")]
        Custom = 1 << 1,
        [Description("Quick Match")]
        QuickMatch = 1 << 5,
        [Description("Hero League")]
        HeroLeague = 1 << 6,
        [Description("Team League")]
        TeamLeague = 1 << 7,
        [Description("Unranked Draft")]
        UnrankedDraft = 1 << 8,
        [Description("Brawl")]
        Brawl = 1 << 9,

        [Description("All Game Modes")]
        AllGameModes = Custom | QuickMatch | HeroLeague | TeamLeague | UnrankedDraft | Brawl,
        [Description("Normal Game Modes")]
        NormalGameModes = QuickMatch | HeroLeague | TeamLeague | UnrankedDraft,
        [Description("Draft Modes")]
        DraftModes = HeroLeague | TeamLeague | UnrankedDraft | Custom,
        [Description("Ranked Modes")]
        RankedModes = HeroLeague | TeamLeague,
    }

    public static partial class HeroesHelpers
    {
        public static class GameModes
        {
            /// <summary>
            /// Returns a collection of the Game Modes
            /// </summary>
            /// <returns></returns>
            public static ICollection<string> GetGameModesList()
            {
                List<string> list = new List<string>();

                foreach (GameMode gameMode in Enum.GetValues(typeof(GameMode)))
                {
                    if (gameMode != GameMode.Unknown)
                        list.Add(gameMode.GetFriendlyName());
                }

                return list;
            }

            /// <summary>
            /// Returns a collection of all the game modes (qm, hl, tl, ud, brawl, and custom)
            /// </summary>
            /// <returns></returns>
            public static ICollection<string> GetAllGameModesList()
            {
                List<string> list = new List<string>()
                {
                    GameMode.QuickMatch.GetFriendlyName(),
                    GameMode.HeroLeague.GetFriendlyName(),
                    GameMode.TeamLeague.GetFriendlyName(),
                    GameMode.UnrankedDraft.GetFriendlyName(),
                    GameMode.Brawl.GetFriendlyName(),
                    GameMode.Custom.GetFriendlyName(),
                };

                return list;
            }
        }
    }
}
