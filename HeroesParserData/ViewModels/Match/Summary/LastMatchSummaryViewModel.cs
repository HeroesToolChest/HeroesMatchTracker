using HeroesParserData.DataQueries;
using HeroesParserData.Messages;
using System.Threading.Tasks;
using System.Windows;

namespace HeroesParserData.ViewModels.Match.Summary
{
    public class LastMatchSummaryViewModel : MatchSummaryContext
    {
        private bool IsRefreshLastMatchDataOn;

        public LastMatchSummaryViewModel()
            :base()
        {
            IsRefreshLastMatchDataOn = false;
        }

        protected override void ReceiveMessage(MatchSummaryMessage action)
        {
            if (action.MatchSummary == MatchSummary.LastMatch)
            {
                if (!IsRefreshLastMatchDataOn)
                {
                    IsRefreshLastMatchDataOn = true;
                    QuerySummaryDetails(Query.Replay.ReadLastRecords(1)[0].ReplayId);

                    Task.Run(async () =>
                    {
                        while (App.IsProcessingReplays)
                        {
                            await Task.Delay(5000);
                            await Application.Current.Dispatcher.InvokeAsync(delegate
                            {
                                QuerySummaryDetails(Query.Replay.ReadLastRecords(1)[0].ReplayId);
                            });
                        }

                        IsRefreshLastMatchDataOn = false;
                    });
                }
            }
        }
    }
}
