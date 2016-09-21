using HeroesParserData.DataQueries.ReplayData;
using System;
using System.Threading.Tasks;
using System.Windows.Input;

namespace HeroesParserData.ViewModels.Match
{
    public class LastMatchViewModel : MatchContext
    {
        public LastMatchViewModel()
            :base()
        {

        }

        public new ICommand Refresh
        {
            get
            {
                return new DelegateCommand(async () =>
                {
                    try
                    {
                        QueryStatus = "Waiting for query...";
                        await RefreshExecute();
                        QueryStatus = "Match details queried successfully";
                    }
                    catch (Exception)
                    {
                        QueryStatus = "Match details queried failed";
                    }
                });
            }
        }

        protected override async Task RefreshExecute()
        {
            var replay = await Query.Replay.ReadLastRecordsAsync(1);
            if (replay.Count > 0)
                await QuerySummaryDetails(replay[0].ReplayId);
        }
    }
}
