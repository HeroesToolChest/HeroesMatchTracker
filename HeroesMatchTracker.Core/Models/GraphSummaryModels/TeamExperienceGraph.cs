using HeroesMatchTracker.Data;
using HeroesMatchTracker.Data.Models.Replays;
using LiveCharts;
using LiveCharts.Defaults;
using LiveCharts.Wpf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace HeroesMatchTracker.Core.Models.GraphSummaryModels
{
    public class TeamExperienceGraph : GraphSummaryBase
    {
        private bool _isTeamExperiencePieChartVisible;
        private bool _isTeamExperienceRowChartVisible;
        private double _matchTeamExperienceMaxYValue;

        private SeriesCollection _matchTeam1ExperienceStackedGraphCollection;
        private SeriesCollection _matchTeam2ExperienceStackedGraphCollection;
        private SeriesCollection _matchTeam1ExperiencePieChartCollection;
        private SeriesCollection _matchTeam2ExperiencePieChartCollection;
        private SeriesCollection _matchTeamExperienceRowChartCollection;

        private IDatabaseService Database;

        public TeamExperienceGraph(IDatabaseService database)
        {
            Database = database;

            ToggleSwitchRowOrPie = Database.SettingsDb().UserSettings.IsTeamExperienceRowChartEnabled;
        }

        public Func<double, string> MatchTeamLevelsFormatter { get; set; }
        public Func<double, string> MatchTeamExperienceFormatter { get; set; }
        public Func<ChartPoint, string> MatchTeamExperiencePiePointLabel { get; set; }
        public string[] ExperienceTypesLabels { get; set; }

        public bool IsTeamExperiencePieChartVisible
        {
            get => _isTeamExperiencePieChartVisible;
            set
            {
                _isTeamExperiencePieChartVisible = value;
                RaisePropertyChanged();
            }
        }

        public bool IsTeamExperienceRowChartVisible
        {
            get => _isTeamExperienceRowChartVisible;
            set
            {
                _isTeamExperienceRowChartVisible = value;
                RaisePropertyChanged();
            }
        }

        public bool ToggleSwitchRowOrPie
        {
            get => Database.SettingsDb().UserSettings.IsTeamExperienceRowChartEnabled;
            set
            {
                Database.SettingsDb().UserSettings.IsTeamExperienceRowChartEnabled = value;
                if (value)
                {
                    IsTeamExperienceRowChartVisible = true;
                    IsTeamExperiencePieChartVisible = false;
                }
                else
                {
                    IsTeamExperienceRowChartVisible = false;
                    IsTeamExperiencePieChartVisible = true;
                }

                RaisePropertyChanged();
            }
        }

        public double MatchTeamExperienceMaxYValue
        {
            get => _matchTeamExperienceMaxYValue;
            set
            {
                _matchTeamExperienceMaxYValue = value;
                RaisePropertyChanged();
            }
        }

        public SeriesCollection MatchTeam1ExperienceStackedGraphCollection
        {
            get => _matchTeam1ExperienceStackedGraphCollection;
            set
            {
                _matchTeam1ExperienceStackedGraphCollection = value;
                RaisePropertyChanged();
            }
        }

        public SeriesCollection MatchTeam2ExperienceStackedGraphCollection
        {
            get => _matchTeam2ExperienceStackedGraphCollection;
            set
            {
                _matchTeam2ExperienceStackedGraphCollection = value;
                RaisePropertyChanged();
            }
        }

        public SeriesCollection MatchTeam1ExperiencePieChartCollection
        {
            get => _matchTeam1ExperiencePieChartCollection;
            set
            {
                _matchTeam1ExperiencePieChartCollection = value;
                RaisePropertyChanged();
            }
        }

        public SeriesCollection MatchTeam2ExperiencePieChartCollection
        {
            get => _matchTeam2ExperiencePieChartCollection;
            set
            {
                _matchTeam2ExperiencePieChartCollection = value;
                RaisePropertyChanged();
            }
        }

        public SeriesCollection MatchTeamExperienceRowChartCollection
        {
            get => _matchTeamExperienceRowChartCollection;
            set
            {
                _matchTeamExperienceRowChartCollection = value;
                RaisePropertyChanged();
            }
        }

        public async Task SetTeamExperienceGraphsAsync(List<ReplayMatchTeamExperience> matchTeamExperience, bool isTeam1Winner)
        {
            if (matchTeamExperience.Count < 1)
                return;

            SetWinner(isTeam1Winner);

            MatchTeamExperienceFormatter = value => new DateTime((long)value).ToString("mm:ss");
            MatchTeamExperiencePiePointLabel = value => string.Format("{0} ({1:P})", value.Y, value.Participation);
            ExperienceTypesLabels = new[] { "Structures", "Passive", "Minions", "Mercenaries", "Heroes" };

            var chartValuesTeam1Mercs = new ChartValues<DateTimePoint>();
            var chartValuesTeam1Minions = new ChartValues<DateTimePoint>();
            var chartValuesTeam1Heroes = new ChartValues<DateTimePoint>();
            var chartValuesTeam1Structures = new ChartValues<DateTimePoint>();
            var chartValuesTeam1Passive = new ChartValues<DateTimePoint>();

            var chartValuesTeam2Mercs = new ChartValues<DateTimePoint>();
            var chartValuesTeam2Minions = new ChartValues<DateTimePoint>();
            var chartValuesTeam2Heroes = new ChartValues<DateTimePoint>();
            var chartValuesTeam2Structures = new ChartValues<DateTimePoint>();
            var chartValuesTeam2Passive = new ChartValues<DateTimePoint>();

            double totalTeam1 = 0;
            double totalTeam2 = 0;

            foreach (var item in matchTeamExperience)
            {
                if (item.Time.HasValue)
                {
                    DateTime time = DateTime.MinValue + item.Time.Value;
                    chartValuesTeam1Heroes.Add(new DateTimePoint(time, (double)item.Team0HeroXP));
                    chartValuesTeam1Mercs.Add(new DateTimePoint(time, (double)item.Team0CreepXP));
                    chartValuesTeam1Minions.Add(new DateTimePoint(time, (double)item.Team0MinionXP));
                    chartValuesTeam1Passive.Add(new DateTimePoint(time, (double)item.Team0TrickleXP));
                    chartValuesTeam1Structures.Add(new DateTimePoint(time, (double)item.Team0StructureXP));
                    double total0 = (double)(item.Team0HeroXP + item.Team0CreepXP + item.Team0MinionXP + item.Team0TrickleXP + item.Team0StructureXP);

                    chartValuesTeam2Heroes.Add(new DateTimePoint(time, (double)item.Team1HeroXP));
                    chartValuesTeam2Mercs.Add(new DateTimePoint(time, (double)item.Team1CreepXP));
                    chartValuesTeam2Minions.Add(new DateTimePoint(time, (double)item.Team1MinionXP));
                    chartValuesTeam2Passive.Add(new DateTimePoint(time, (double)item.Team1TrickleXP));
                    chartValuesTeam2Structures.Add(new DateTimePoint(time, (double)item.Team1StructureXP));
                    double total1 = (double)(item.Team1HeroXP + item.Team1CreepXP + item.Team1MinionXP + item.Team1TrickleXP + item.Team1StructureXP);
                }
            }

            var lastExpTime = matchTeamExperience.Last();
            totalTeam1 += (double)(lastExpTime.Team0HeroXP + lastExpTime.Team0CreepXP + lastExpTime.Team0MinionXP + lastExpTime.Team0TrickleXP + lastExpTime.Team0StructureXP);
            totalTeam2 += (double)(lastExpTime.Team1HeroXP + lastExpTime.Team1CreepXP + lastExpTime.Team1MinionXP + lastExpTime.Team1TrickleXP + lastExpTime.Team1StructureXP);

            await Application.Current.Dispatcher.InvokeAsync(() =>
            {
                MatchTeam1ExperienceStackedGraphCollection = new SeriesCollection()
                {
                    new StackedAreaSeries
                    {
                        Title = "Heroes",
                        Values = chartValuesTeam1Heroes,
                        LineSmoothness = 0,
                    },
                    new StackedAreaSeries
                    {
                        Title = "Mercenaries",
                        Values = chartValuesTeam1Mercs,
                        LineSmoothness = 0,
                    },
                    new StackedAreaSeries
                    {
                        Title = "Minions",
                        Values = chartValuesTeam1Minions,
                        LineSmoothness = 0,
                    },
                    new StackedAreaSeries
                    {
                        Title = "Passive",
                        Values = chartValuesTeam1Passive,
                        LineSmoothness = 0,
                    },
                    new StackedAreaSeries
                    {
                        Title = "Structures",
                        Values = chartValuesTeam1Structures,
                        LineSmoothness = 0,
                    },
                };

                MatchTeam2ExperienceStackedGraphCollection = new SeriesCollection()
                {
                    new StackedAreaSeries
                    {
                        Title = "Heroes",
                        Values = chartValuesTeam2Heroes,
                        LineSmoothness = 0,
                    },
                    new StackedAreaSeries
                    {
                        Title = "Mercenaries",
                        Values = chartValuesTeam2Mercs,
                        LineSmoothness = 0,
                    },
                    new StackedAreaSeries
                    {
                        Title = "Minions",
                        Values = chartValuesTeam2Minions,
                        LineSmoothness = 0,
                    },
                    new StackedAreaSeries
                    {
                        Title = "Passive",
                        Values = chartValuesTeam2Passive,
                        LineSmoothness = 0,
                    },
                    new StackedAreaSeries
                    {
                        Title = "Structures",
                        Values = chartValuesTeam2Structures,
                        LineSmoothness = 0,
                    },
                };

                MatchTeamExperienceMaxYValue = totalTeam1 > totalTeam2 ? totalTeam1 : totalTeam2;

                MatchTeam1ExperiencePieChartCollection = new SeriesCollection()
                {
                    new PieSeries
                    {
                         Title = "Heroes",
                         Values = new ChartValues<ObservableValue> { new ObservableValue((double)lastExpTime.Team0HeroXP) },
                         DataLabels = true,
                         LabelPoint = MatchTeamExperiencePiePointLabel,
                    },
                    new PieSeries
                    {
                         Title = "Mercenaries",
                         Values = new ChartValues<ObservableValue> { new ObservableValue((double)lastExpTime.Team0CreepXP) },
                         DataLabels = true,
                         LabelPoint = MatchTeamExperiencePiePointLabel,
                    },
                    new PieSeries
                    {
                         Title = "Minions",
                         Values = new ChartValues<ObservableValue> { new ObservableValue((double)lastExpTime.Team0MinionXP) },
                         DataLabels = true,
                         LabelPoint = MatchTeamExperiencePiePointLabel,
                    },
                    new PieSeries
                    {
                         Title = "Passive",
                         Values = new ChartValues<ObservableValue> { new ObservableValue((double)lastExpTime.Team0TrickleXP) },
                         DataLabels = true,
                         LabelPoint = MatchTeamExperiencePiePointLabel,
                    },
                    new PieSeries
                    {
                         Title = "Structures",
                         Values = new ChartValues<ObservableValue> { new ObservableValue((double)lastExpTime.Team0StructureXP) },
                         DataLabels = true,
                         LabelPoint = MatchTeamExperiencePiePointLabel,
                    },
                };
                MatchTeam2ExperiencePieChartCollection = new SeriesCollection()
                {
                    new PieSeries
                    {
                         Title = "Heroes",
                         Values = new ChartValues<ObservableValue> { new ObservableValue((double)lastExpTime.Team1HeroXP) },
                         DataLabels = true,
                         LabelPoint = MatchTeamExperiencePiePointLabel,
                    },
                    new PieSeries
                    {
                         Title = "Mercenaries",
                         Values = new ChartValues<ObservableValue> { new ObservableValue((double)lastExpTime.Team1CreepXP) },
                         DataLabels = true,
                         LabelPoint = MatchTeamExperiencePiePointLabel,
                    },
                    new PieSeries
                    {
                         Title = "Minions",
                         Values = new ChartValues<ObservableValue> { new ObservableValue((double)lastExpTime.Team1MinionXP) },
                         DataLabels = true,
                         LabelPoint = MatchTeamExperiencePiePointLabel,
                    },
                    new PieSeries
                    {
                         Title = "Passive",
                         Values = new ChartValues<ObservableValue> { new ObservableValue((double)lastExpTime.Team1TrickleXP) },
                         DataLabels = true,
                         LabelPoint = MatchTeamExperiencePiePointLabel,
                    },
                    new PieSeries
                    {
                         Title = "Structures",
                         Values = new ChartValues<ObservableValue> { new ObservableValue((double)lastExpTime.Team1StructureXP) },
                         DataLabels = true,
                         LabelPoint = MatchTeamExperiencePiePointLabel,
                    },
                };

                MatchTeamExperienceRowChartCollection = new SeriesCollection()
                {
                    new RowSeries
                    {
                        Title = GraphTeam1Title,
                        Values = new ChartValues<double> { (double)lastExpTime.Team0StructureXP, (double)lastExpTime.Team0TrickleXP, (double)lastExpTime.Team0MinionXP, (double)lastExpTime.Team0CreepXP, (double)lastExpTime.Team0HeroXP },
                        DataLabels = false,
                    },
                    new RowSeries
                    {
                        Title = GraphTeam2Title,
                        Values = new ChartValues<double> { (double)lastExpTime.Team1StructureXP, (double)lastExpTime.Team1TrickleXP, (double)lastExpTime.Team1MinionXP, (double)lastExpTime.Team1CreepXP, (double)lastExpTime.Team1HeroXP },
                        DataLabels = false,
                    },
                };
            });
        }

        public override void Dispose()
        {
            MatchTeam1ExperienceStackedGraphCollection = null;
            MatchTeam2ExperienceStackedGraphCollection = null;
            MatchTeam1ExperiencePieChartCollection = null;
            MatchTeam2ExperiencePieChartCollection = null;
            MatchTeamExperienceRowChartCollection = null;
        }
    }
}
