using HeroesParserData.DataQueries.ReplayData;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace HeroesParserData.ViewModels.Match
{
    public class LastGameViewModel : MatchContext
    {
        public LastGameViewModel()
            :base()
        {

        }

        protected override async Task RefreshExecute()
        {
            await QueryMatchList();
        }

        private async Task QueryMatchList()
        {
            MatchList = new ObservableCollection<Models.DbModels.Replay>(await Query.Replay.ReadLatestRecordsAsync(30));
            RowsReturned = MatchList.Count;
        }
    }
}
