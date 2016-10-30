using Heroes.ReplayParser;
using HeroesParserData.DataQueries;
using System.Collections.ObjectModel;

namespace HeroesParserData.ViewModels.Match
{
    public class BrawlViewModel : MatchContext
    {
        public BrawlViewModel()
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
            MatchList = new ObservableCollection<Models.DbModels.Replay>(Query.Replay.ReadGameModeRecords(GameMode.Brawl, GetSelectedSeason));
            RowsReturned = MatchList.Count;
        }
    }
}
