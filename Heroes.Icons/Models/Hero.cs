using System;
using System.Collections.Generic;
using System.Windows.Media.Imaging;

namespace Heroes.Icons.Models
{
    public class Hero : IconBase
    {
        public string Name { get; set; }

        public string AltName { get; set; }

        public string AttributeId { get; set; }

        public string Description { get; set; }

        public HeroType Type { get; set; }

        public HeroDifficulty Difficulty { get; set; }

        /// <summary>
        /// Build that the hero is added in, in terms of this application, not HOTS
        /// </summary>
        public int BuildAvailable { get; set; }

        public HeroFranchise Franchise { get; set; }

        public Uri HeroPortrait { get; set; }

        public Uri LoadingPortrait { get; set; }

        public Uri LeaderboardPortrait { get; set; }

        /// <summary>
        /// Different language names of the hero name
        /// </summary>
        public List<string> Aliases { get; set; } = new List<string>();

        /// <summary>
        /// Roles of the hero, multiclass will be first if hero has multiple roles
        /// </summary>
        public List<HeroRole> Roles { get; set; } = new List<HeroRole>();

        public BitmapImage GetPortrait()
        {
            if (HeroPortrait == null)
                return null;

            return CreateBitmapImage(HeroPortrait);
        }

        public BitmapImage GetLoadingPortrait()
        {
            if (LoadingPortrait == null)
                return null;

            return CreateBitmapImage(LoadingPortrait);
        }

        public BitmapImage GetLeaderboardPortrait()
        {
            if (LeaderboardPortrait == null)
                return null;

            return CreateBitmapImage(LeaderboardPortrait);
        }
    }
}
