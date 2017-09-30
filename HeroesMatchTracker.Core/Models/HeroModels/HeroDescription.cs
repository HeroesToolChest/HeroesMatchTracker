using Heroes.Helpers;
using Heroes.Icons.Models;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Media.Imaging;

namespace HeroesMatchTracker.Core.Models.HeroModels
{
    public class HeroDescription
    {
        public string HeroName { get; set; }

        public string Description { get; set; }

        public BitmapImage Franchise { get; set; }

        public HeroType Type { get; set; }

        public List<HeroRole> Roles { get; set; }

        public HeroDifficulty Difficulty { get; set; }

        /// <summary>
        /// Type, roles, difficulty
        /// </summary>
        public string SubDescription
        {
            get
            {
                if (Roles.Count > 1)
                    return $"{Type} {Roles[0]} ({Roles[1]} | {Roles[2]}) | Difficulty: {Difficulty.GetFriendlyName()}";
                else
                    return $"{Type} {Roles.FirstOrDefault()} | Difficulty: {Difficulty.GetFriendlyName()}";
            }
        }
    }
}
