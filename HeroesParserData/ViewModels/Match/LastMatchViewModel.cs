using HeroesParserData.DataQueries;
using System;
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
                return new DelegateCommand(() =>
                {
                    try
                    {
                        QueryStatus = "Waiting for query...";
                        RefreshExecute();
                        QueryStatus = "Match details queried successfully";
                    }
                    catch (Exception)
                    {
                        QueryStatus = "Match details queried failed";
                    }
                });
            }
        }

        protected override void RefreshExecute()
        {
            var replay = Query.Replay.ReadLastRecords(1);
            if (replay.Count > 0)
                QuerySummaryDetails(replay[0].ReplayId);
        }
    }
}
