using Heroes.ReplayParser;
using HeroesParserData.DataQueries;
using System.Collections.ObjectModel;

namespace HeroesParserData.ViewModels.Match
{
    public class UnrankedDraftViewModel : MatchContext
    {
        public UnrankedDraftViewModel()
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
            MatchList = new ObservableCollection<Models.DbModels.Replay>(Query.Replay.ReadGameModeRecords(GameMode.UnrankedDraft, GetSelectedSeason));
            RowsReturned = MatchList.Count;
        }
    }
}
