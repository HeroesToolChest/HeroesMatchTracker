using HeroesParserData.DataQueries.ReplayData;
using HeroesParserData.Models.DbModels;
using System.Threading.Tasks;
using System.Windows.Input;

namespace HeroesParserData.ViewModels.Match
{
    public class LastGameViewModel : MatchContext
    {
        public LastGameViewModel()
            :base()
        {

        }

        public new ICommand Refresh
        {
            get { return new DelegateCommand(async () => await RefreshExecute()); }
        }

        protected override async Task RefreshExecute()
        {
            await LastGameQueryGameDetails();
        }

        private async Task LastGameQueryGameDetails()
        {
            var replays = await Query.Replay.ReadLastRecordsAsync(1);

            if (replays.Count > 0)
            {
                await QuerySummaryDetails(replays[0].ReplayId);
            }                        
        }
    }
}
