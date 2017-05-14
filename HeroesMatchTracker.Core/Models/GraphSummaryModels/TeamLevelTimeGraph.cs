using HeroesMatchTracker.Data.Models.Replays;
using LiveCharts;
using LiveCharts.Defaults;
using LiveCharts.Wpf;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace HeroesMatchTracker.Core.Models.GraphSummaryModels
{
    public class TeamLevelTimeGraph : GraphSummaryBase
    {
        private SeriesCollection _matchTeamLevelsLineChartCollection;

        public Func<double, string> MatchTeamLevelsFormatter { get; set; }

        public SeriesCollection MatchTeamLevelsLineChartCollection
        {
            get { return _matchTeamLevelsLineChartCollection; }
            set
            {
                _matchTeamLevelsLineChartCollection = value;
                RaisePropertyChanged();
            }
        }

        public async Task SetTeamLevelGraphsAsync(List<ReplayMatchTeamLevel> matchTeamLevels, bool isTeam1Winner)
        {
            if (matchTeamLevels.Count < 1)
                return;

            SetWinner(isTeam1Winner);

            MatchTeamLevelsFormatter = value => new DateTime((long)value).ToString("mm:ss");

            var chartValuesTeam0 = new ChartValues<DateTimePoint>();
            var chartValuesTeam1 = new ChartValues<DateTimePoint>();

            foreach (var time in matchTeamLevels)
            {
                if (time.Team0Level.HasValue)
                {
                    chartValuesTeam0.Add(new DateTimePoint
                    {
                        Value = time.Team0Level.Value,
                        DateTime = DateTime.MinValue + time.TeamTime0.Value,
                    });
                }

                if (time.Team1Level.HasValue)
                {
                    chartValuesTeam1.Add(new DateTimePoint
                    {
                        Value = time.Team1Level.Value,
                        DateTime = DateTime.MinValue + time.TeamTime1.Value,
                    });
                }
            }

            await Application.Current.Dispatcher.InvokeAsync(() =>
            {
                MatchTeamLevelsLineChartCollection = new SeriesCollection()
                {
                    new LineSeries
                    {
                        Title = GraphTeam1Title,
                        Values = chartValuesTeam0,
                        Fill = Brushes.Transparent,
                    },
                    new LineSeries
                    {
                        Title = GraphTeam2Title,
                        Values = chartValuesTeam1,
                        Fill = Brushes.Transparent,
                    },
                };
            });
        }

        public override void Dispose()
        {
            MatchTeamLevelsLineChartCollection = null;
        }
    }
}
