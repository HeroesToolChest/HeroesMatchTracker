using HeroesParserData.DataQueries;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace HeroesParserData.ViewModels.Match
{
    public class LatestMatchesViewModel : MatchContext
    {
        public LatestMatchesViewModel()
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
