using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace Heroes.Icons.Models
{
    public class Hero
    {
        public string Name { get; set; }

        public string ShortName { get; set; }

        public string UnitName { get; set; }

        public string AttributeId { get; set; }

        public string Description { get; set; }

        public HeroType Type { get; set; }

        public HeroDifficulty Difficulty { get; set; }

        /// <summary>
        /// Build that the hero is added in, in terms of this application, not HOTS
        /// </summary>
        public int BuildAvailable { get; set; }

        public HeroFranchise Franchise { get; set; }

        public string HeroPortrait { get; set; }

        public string LoadingPortrait { get; set; }

        public string LeaderboardPortrait { get; set; }

        public HeroMana ManaType { get; set; }

        /// <summary>
        /// Different language names of the hero name
        /// </summary>
        public List<string> Aliases { get; set; } = new List<string>();

        /// <summary>
        /// Roles of the hero, multiclass will be first if hero has multiple roles
        /// </summary>
        public List<HeroRole> Roles { get; set; } = new List<HeroRole>();

        public Stream GetHeroPortrait()
        {
            return Assembly.GetExecutingAssembly().GetManifestResourceStream(HeroPortrait);
        }

        public Stream GetLoadingPortrait()
        {
            return Assembly.GetExecutingAssembly().GetManifestResourceStream(LoadingPortrait);
        }

        public Stream GetLeaderboardPortrait()
        {
            return Assembly.GetExecutingAssembly().GetManifestResourceStream(LeaderboardPortrait);
        }
    }
}
