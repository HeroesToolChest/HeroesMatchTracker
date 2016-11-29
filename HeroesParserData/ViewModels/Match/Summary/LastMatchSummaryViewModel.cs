﻿using HeroesParserData.DataQueries;
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
                        return;

                    CurrentReplay = lastRecord[0];
                    ClearSummaryDetails();
                    QuerySummaryDetails();

                    Task.Run(async () =>
                    {
                        while (App.IsProcessingReplays)
                        {
                            await Task.Delay(5000);
                            await Application.Current.Dispatcher.InvokeAsync(delegate
                            {
                                CurrentReplay = Query.Replay.ReadLastRecords(1)[0];
                                ClearSummaryDetails();
                                QuerySummaryDetails();
                            });
                        }

                        IsRefreshLastMatchDataOn = false;
                    });
                }
            }
        }
    }
}
