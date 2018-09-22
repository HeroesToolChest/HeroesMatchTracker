using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace HeroesMatchTracker.Core.Models.HeroModels
{
    public class HeroDescription
    {
        public string HeroName { get; set; }

        public string Description { get; set; }

        public Stream Franchise { get; set; }

        public string Type { get; set; }

        public List<string> Roles { get; set; }

        public string Difficulty { get; set; }

        /// <summary>
        /// Type, roles, difficulty.
        /// </summary>
        public string SubDescription
        {
            get
            {
                if (Roles.Count > 1)
                    return $"{Type} {Roles[0]} ({Roles[1]} | {Roles[2]}) | Difficulty: {Difficulty}";
                else
                    return $"{Type} {Roles.FirstOrDefault()} | Difficulty: {Difficulty}";
            }
        }
    }
}
