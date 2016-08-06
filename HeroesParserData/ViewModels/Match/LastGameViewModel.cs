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
            MatchInfoTeam1.Clear();
            MatchInfoTeam2.Clear();

            Replay replay = (await Query.Replay.ReadLastRecordsAsync(1))[0];
            await QueryGameDetails(replay.ReplayId);          
        }
    }
}
