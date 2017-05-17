using HeroesMatchTracker.Data;
using HeroesMatchTracker.Data.Models.Replays;
using LiveCharts;
using LiveCharts.Wpf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace HeroesMatchTracker.Core.Models.GraphSummaryModels
{
    public class StatGraphs : GraphSummaryBase
    {
        private string[] _playerLabels;
        private SeriesCollection _siegeDamageChartCollection;
        private SeriesCollection _heroDamageChartCollection;
        private SeriesCollection _experienceChartCollection;

        private IDatabaseService Database;

        public StatGraphs(IDatabaseService database)
        {
            Database = database;
        }

        public SeriesCollection SiegeDamageColumnCollection
        {
            get => _siegeDamageChartCollection;
            set
            {
                _siegeDamageChartCollection = value;
                RaisePropertyChanged();
            }
        }

        public SeriesCollection HeroDamageChartCollection
        {
            get => _heroDamageChartCollection;
            set
            {
                _heroDamageChartCollection = value;
                RaisePropertyChanged();
            }
        }

        public SeriesCollection ExperienceChartCollection
        {
            get => _experienceChartCollection;
            set
            {
                _experienceChartCollection = value;
                RaisePropertyChanged();
            }
        }

        public string[] PlayerLabels
        {
            get => _playerLabels;
            set
            {
                _playerLabels = value;
                RaisePropertyChanged();
            }
        }

        public Func<int, string> NormalValueFormatter { get; set; }

        public async Task SetStatGraphsAsync(List<Tuple<string, string>> players, List<ReplayMatchPlayerScoreResult> playerScoreResult)
        {
            if (playerScoreResult.Count < 1)
                return;

            NormalValueFormatter = value => value.ToString();
            SetPlayerLabels(players);

            var chartValuesSiegeDamage = new ChartValues<int>();
            var chartValuesHeroDamage = new ChartValues<int>();
            var chartValuesExperience = new ChartValues<int>();

            chartValuesSiegeDamage.AddRange(playerScoreResult.Select(x => x.SiegeDamage.Value));
            chartValuesHeroDamage.AddRange(playerScoreResult.Select(x => x.HeroDamage.Value));
            chartValuesExperience.AddRange(playerScoreResult.Select(x => x.ExperienceContribution.Value));

            await Application.Current.Dispatcher.InvokeAsync(() =>
            {
                SiegeDamageColumnCollection = new SeriesCollection()
                {
                    new RowSeries
                    {
                        Values = chartValuesSiegeDamage,
                        Fill = new SolidColorBrush(Color.FromArgb(255, 0, 171, 169)),
                        Stroke = new SolidColorBrush(Color.FromArgb(255, 0, 171, 169)),
                        DataLabels = true,
                        LabelsPosition = BarLabelPosition.Parallel,
                    },
                };

                HeroDamageChartCollection = new SeriesCollection()
                {
                    new RowSeries
                    {
                        Values = chartValuesHeroDamage,
                        Fill = new SolidColorBrush(Color.FromArgb(255, 0, 171, 169)),
                        Stroke = new SolidColorBrush(Color.FromArgb(255, 0, 171, 169)),
                        DataLabels = true,
                        LabelsPosition = BarLabelPosition.Parallel,
                    },
                };

                ExperienceChartCollection = new SeriesCollection()
                {
                    new RowSeries
                    {
                        Values = chartValuesExperience,
                        Fill = new SolidColorBrush(Color.FromArgb(255, 0, 171, 169)),
                        Stroke = new SolidColorBrush(Color.FromArgb(255, 0, 171, 169)),
                        DataLabels = true,
                        LabelsPosition = BarLabelPosition.Parallel,
                    },
                };
            });
        }

        public void SetPlayerLabels(List<Tuple<string, string>> players)
        {
            PlayerLabels = new string[players.Count];

            for (int i = 0; i < players.Count; i++)
            {
                PlayerLabels[i] = $"{players[i].Item1} ({players[i].Item2})";
            }
        }

        public override void Dispose()
        {
            SiegeDamageColumnCollection = null;
            HeroDamageChartCollection = null;
            ExperienceChartCollection = null;
            PlayerLabels = null;
        }
    }
}
