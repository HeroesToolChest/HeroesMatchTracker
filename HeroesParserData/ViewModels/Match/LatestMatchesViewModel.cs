using HeroesParserData.DataQueries;
using System.Collections.ObjectModel;

namespace HeroesParserData.ViewModels.Match
{
    public class LatestMatchesViewModel : MatchContext
    {
        public LatestMatchesViewModel()
            :base()
        {

        }

        protected override void RefreshExecute()
        {
            QueryMatchList();
        }

        private void QueryMatchList()
        {
            MatchList = new ObservableCollection<Models.DbModels.Replay>(Query.Replay.ReadLatestRecords(30));
            RowsReturned = MatchList.Count;
        }
    }
}
