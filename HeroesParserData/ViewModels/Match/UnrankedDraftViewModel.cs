using Heroes.ReplayParser;
using HeroesParserData.DataQueries;
using System.Collections.ObjectModel;

namespace HeroesParserData.ViewModels.Match
{
    public class UnrankedDraftViewModel : MatchOverviewContext
    {
        public UnrankedDraftViewModel()
            :base()
        {

        }

        protected override void ExecuteLoadMatchListCommmand()
        {
            MatchListCollection = new ObservableCollection<Models.DbModels.Replay>(Query.Replay.ReadGameModeRecords(GameMode.UnrankedDraft, this));
            RowsReturned = MatchListCollection.Count;
        }
    }
}
