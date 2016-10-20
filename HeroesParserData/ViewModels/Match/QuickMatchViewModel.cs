using Heroes.ReplayParser;
using HeroesParserData.DataQueries;
using System.Collections.ObjectModel;

namespace HeroesParserData.ViewModels.Match
{
    public class QuickMatchViewModel : MatchContext
    {
        public QuickMatchViewModel()
            :base()
        {
            HasObservers = false;
            HasBans = false;
        }

        protected override void RefreshExecute()
        {
            QueryMatchList();
        }

        private void QueryMatchList()
        {
            MatchList = new ObservableCollection<Models.DbModels.Replay>(Query.Replay.ReadGameModeRecords(GameMode.QuickMatch, GetSelectedSeason));
            RowsReturned = MatchList.Count;
        }
    }
}
