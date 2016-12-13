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

            IsLeftChangeButtonEnabled = false;
            IsRightChangeButtonEnabled = false;
            IsLeftChangeButtonVisible = false;
            IsRightChangeButtonVisible = false;
        }

        protected override void ReceiveMessage(MatchSummaryMessage action)
        {
            if (action.MatchSummary == MatchSummary.LastMatch)
            {
                if (!IsRefreshLastMatchDataOn)
                {
                    IsRefreshLastMatchDataOn = true;

                    var lastRecord = Query.Replay.ReadLastRecords(1);
                    if (lastRecord.Count == 0)
                    {
                        IsRefreshLastMatchDataOn = false;
                        return;
                    }

                    // dont reload if already loaded
                    if (CurrentReplay?.ReplayId != lastRecord[0].ReplayId)
                    {
                        CurrentReplay = lastRecord[0];
                        ClearSummaryDetails();
                        QuerySummaryDetails();
                    }

                    if (App.IsProcessingReplays)
                    {
                        Task.Run(async () =>
                        {
                            while (App.IsProcessingReplays && App.CurrentSelectedMainTab == MainTab.LastMatch)
                            {
                                await Task.Delay(4000);
                                await Application.Current.Dispatcher.InvokeAsync(delegate
                                {
                                    IsRefreshLastMatchDataOn = true;

                                    var replay = Query.Replay.ReadLastRecords(1)[0];
                                    if (CurrentReplay.ReplayId != replay.ReplayId)
                                    {
                                        CurrentReplay = replay;
                                        ClearSummaryDetails();
                                        QuerySummaryDetails();
                                    }
                                });
                            }
                            IsRefreshLastMatchDataOn = false;
                        });
                    }
                    IsRefreshLastMatchDataOn = false;
                }
            }
        }
    }
}
