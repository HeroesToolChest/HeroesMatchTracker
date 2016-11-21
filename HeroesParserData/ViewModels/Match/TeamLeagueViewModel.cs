using Heroes.ReplayParser;
using HeroesParserData.DataQueries;
using System.Collections.ObjectModel;

namespace HeroesParserData.ViewModels.Match
{
    public class TeamLeagueViewModel : MatchOverviewContext
    {
        public TeamLeagueViewModel()
            :base()
        {

        }

        protected override void ExecuteLoadMatchListCommmand()
        {
            MatchListCollection = new ObservableCollection<Models.DbModels.Replay>(Query.Replay.ReadGameModeRecords(GameMode.TeamLeague, this));
            RowsReturned = MatchListCollection.Count;
        }
    }
}
