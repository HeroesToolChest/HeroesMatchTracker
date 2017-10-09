using System.Collections.Generic;
using System.IO;

namespace Heroes.Icons.Models
{
    public class Hero
    {
        public string Name { get; set; }

        public string ShortName { get; set; }

        /// <summary>
        /// Unit name of the hero, available build 57797
        /// </summary>
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
        /// Roles of the hero, multiclass will be first if hero has multiple roles
        /// </summary>
        public List<HeroRole> Roles { get; set; } = new List<HeroRole>();

        public Stream GetHeroPortrait()
        {
            return HeroesIcons.GetHeroesIconsAssembly().GetManifestResourceStream(HeroPortrait);
        }

        public Stream GetLoadingPortrait()
        {
            return HeroesIcons.GetHeroesIconsAssembly().GetManifestResourceStream(LoadingPortrait);
        }

        public Stream GetLeaderboardPortrait()
        {
            return HeroesIcons.GetHeroesIconsAssembly().GetManifestResourceStream(LeaderboardPortrait);
        }
    }
}
