using Heroes.Helpers;
using System.Collections.Generic;

namespace HeroesMatchTracker.Data.Queries.Replays
{
    public class ReplayFilter
    {
        public Season SelectedSeason { get; set; }
        public long SelectedReplayId { get; set; }
        public string SelectedMapOption { get; set; }
        public string SelectedBuildOption { get; set; }
        public FilterGameTimeOption SelectedGameTimeOption { get; set; }
        public FilterGameDateOption SelectedGameDateOption { get; set; }
        public string SelectedBattleTag { get; set; }
        public string SelectedCharacter { get; set; }
        public bool IsGivenBattleTagOnlyChecked { get; set; }
        public List<string> BuildOptionsList { get; set; }
        public List<string> MapOptionsList { get; set; }
        public List<string> HeroesList { get; set; }
    }
}
