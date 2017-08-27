using System.Collections.Generic;

namespace HeroesMatchTracker.Core.Models.MatchModels
{
    public class PlayerTag
    {
        public string PlayerName { get; set; }
        public string AccountLevel { get; set; }
        public int TotalSeen { get; set; }
        public string LastSeenBefore { get; set; }
        public List<string> FormerPlayerNames { get; set; }
        public string Notes { get; set; }

        public string PlayerNameAliases
        {
            get
            {
                string names = string.Empty;

                foreach (var name in FormerPlayerNames)
                {
                    names += $"{name}, ";
                }

                return names.TrimEnd(' ', ',');
            }
        }
    }
}
