using Heroes.ReplayParser;
using HeroesParserData.DataQueries;
using System.Collections.ObjectModel;

namespace HeroesParserData.ViewModels.Match
{
    public class QuickMatchViewModel : MatchOverviewContext
    {
        public QuickMatchViewModel()
            :base()
        {

        }

        protected override void ExecuteLoadMatchListCommmand()
        {
            MatchListCollection = new ObservableCollection<Models.DbModels.Replay>(Query.Replay.ReadGameModeRecords(GameMode.QuickMatch, GetSelectedSeason));
            RowsReturned = MatchListCollection.Count;
        }
    }
}
