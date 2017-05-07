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
        private SeriesCollection _siegeDamageRowChartCollection;

        private IDatabaseService Database;

        public StatGraphs(IDatabaseService database)
        {
            Database = database;
        }

        public SeriesCollection SiegeDamageRowColumnCollection
        {
            get => _siegeDamageRowChartCollection;
            set
            {
                _siegeDamageRowChartCollection = value;
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

        public Func<int, string> SiegeDamageFormatter { get; set; }

        public async Task SetStatGraphsAsync(List<Tuple<string, string>> players, List<ReplayMatchPlayerScoreResult> playerScoreResult)
        {
            SiegeDamageRowColumnCollection = null;
            PlayerLabels = null;

            if (playerScoreResult.Count < 1)
                return;

            SiegeDamageFormatter = value => value.ToString();
            SetPlayerLabels(players);

            var chartValuesSiegeDamage = new ChartValues<int>();

            chartValuesSiegeDamage.AddRange(playerScoreResult.Select(x => x.SiegeDamage.Value));
            await Application.Current.Dispatcher.InvokeAsync(() =>
            {
                SiegeDamageRowColumnCollection = new SeriesCollection()
                {
                    new ColumnSeries
                    {
                        Values = chartValuesSiegeDamage,
                        Fill = new SolidColorBrush(Color.FromArgb(255, 0, 171, 169)),
                        Stroke = new SolidColorBrush(Color.FromArgb(255, 0, 171, 169)),
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
    }
}
