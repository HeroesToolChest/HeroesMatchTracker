using System.Collections.Generic;
using System.IO;

namespace HeroesMatchTracker.Core.Models.HeroModels
{
    public class HeroDescription
    {
        public string HeroName { get; set; }

        public string Description { get; set; }

        public Stream Franchise { get; set; }

        public string Type { get; set; }

        public List<string> Roles { get; set; }

        public string ExpandedRole { get; set; }

        public string Difficulty { get; set; }

        /// <summary>
        /// Type, roles, difficulty.
        /// </summary>
        public string SubDescription => $"{ExpandedRole} | Difficulty: {Difficulty}";
    }
}
