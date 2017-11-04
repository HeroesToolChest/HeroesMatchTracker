using System.Collections.Generic;
using System.IO;
using System.Linq;

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

        public Dictionary<string, Ability> Abilities { get; set; }

        public Dictionary<string, Talent> Talents { get; set; }

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

        /// <summary>
        /// Returns a talent object given the reference name
        /// </summary>
        /// <param name="referenceName">reference name of the talent</param>
        /// <returns></returns>
        public Talent GetTalent(string referenceName)
        {
            if (string.IsNullOrEmpty(referenceName))
            {
                return Talents[string.Empty]; // no pick
            }

            if (Talents.TryGetValue(referenceName, out Talent talent))
            {
                return talent;
            }
            else
            {
                talent = Talents["NotFound"];
                talent.Name = referenceName;
                return talent;
            }
        }

        /// <summary>
        /// Returns a collection of all the talents in the selected tier
        /// </summary>
        /// <param name="tier">The talent tier</param>
        /// <returns></returns>
        public ICollection<Talent> GetTierTalents(TalentTier tier)
        {
            return Talents.Values.Where(x => x.Tier == tier).ToArray();
        }
    }
}
