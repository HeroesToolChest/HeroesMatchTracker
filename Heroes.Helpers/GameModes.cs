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

        AllGameMode = Custom | QuickMatch | HeroLeague | TeamLeague | UnrankedDraft | Brawl,
        NormalGameMode = QuickMatch | HeroLeague | TeamLeague | UnrankedDraft,
        DraftMode = HeroLeague | TeamLeague | UnrankedDraft | Custom,
        RankedMode = HeroLeague | TeamLeague,
    }

    public static partial class HeroesHelpers
    {
        public static class GameModes
        {
            public static ICollection<string> GetAllGameModeList()
            {
                List<string> list = new List<string>();

                foreach (GameMode gameMode in Enum.GetValues(GameMode.AllGameMode.GetType()))
                {
                    list.Add(gameMode.GetFriendlyName());
                }

                return list;
            }

            public static ICollection<string> GetNormalGameModeList()
            {
                List<string> list = new List<string>();

                foreach (GameMode gameMode in Enum.GetValues(GameMode.NormalGameMode.GetType()))
                {
                    list.Add(gameMode.GetFriendlyName());
                }

                return list;
            }
        }
    }
}
