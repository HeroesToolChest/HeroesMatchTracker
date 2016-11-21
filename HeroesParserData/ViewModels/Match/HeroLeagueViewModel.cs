using Heroes.ReplayParser;
using HeroesParserData.DataQueries;
using System.Collections.ObjectModel;

namespace HeroesParserData.ViewModels.Match
{
    public class HeroLeagueViewModel : MatchOverviewContext
    {
        public HeroLeagueViewModel()
            :base()
        {

        }

        protected override void ExecuteLoadMatchListCommmand()
        {
            MatchListCollection = new ObservableCollection<Models.DbModels.Replay>(Query.Replay.ReadGameModeRecords(GameMode.HeroLeague, this));
            RowsReturned = MatchListCollection.Count;
        }
    }
}
