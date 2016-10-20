using Heroes.ReplayParser;
using HeroesParserData.DataQueries;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace HeroesParserData.ViewModels.Match
{
    public class CustomGameViewModel : MatchContext
    {
        public CustomGameViewModel()
            : base()
        {

        }

        protected override void RefreshExecute()
        {
            QueryMatchList();
        }

        private void QueryMatchList()
        {
            MatchList = new ObservableCollection<Models.DbModels.Replay>(Query.Replay.ReadGameModeRecords(GameMode.Custom, GetSelectedSeason));
            RowsReturned = MatchList.Count;
        }
    }
}
