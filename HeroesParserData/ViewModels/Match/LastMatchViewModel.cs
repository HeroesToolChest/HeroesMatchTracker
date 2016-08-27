using HeroesParserData.DataQueries.ReplayData;
using System.Threading.Tasks;

namespace HeroesParserData.ViewModels.Match
{
    public class LastMatchViewModel : MatchContext
    {
        public LastMatchViewModel()
            :base()
        {

        }

        protected override async Task RefreshExecute()
        {
            var replay = await Query.Replay.ReadLatestRecordsAsync(1);
            if (replay[0] != null)
                await QuerySummaryDetails(replay[0].ReplayId);
        }
    }
}
