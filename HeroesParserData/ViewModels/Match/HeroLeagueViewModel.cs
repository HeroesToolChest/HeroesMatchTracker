using Heroes.ReplayParser;
using HeroesParserData.DataQueries;
using System.Collections.ObjectModel;

namespace HeroesParserData.ViewModels.Match
{
    public class HeroLeagueViewModel : MatchContext
    {
        public HeroLeagueViewModel()
            :base()
        {
            HasObservers = false;
            HasBans = true;
        }

        protected override void RefreshExecute()
        {
            QueryMatchList();
        }

        private void QueryMatchList()
        {
            MatchList = new ObservableCollection<Models.DbModels.Replay>(Query.Replay.ReadGameModeRecords(GameMode.HeroLeague, GetSelectedSeason));
            RowsReturned = MatchList.Count;
        }
    }
}
