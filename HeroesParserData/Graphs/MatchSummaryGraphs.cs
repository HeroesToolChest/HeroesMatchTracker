using HeroesParserData.Models.DbModels;
using LiveCharts;
using LiveCharts.Defaults;
using LiveCharts.Wpf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Media;

namespace HeroesParserData.Graphs
{
    public class MatchSummaryGraphs : ObservableObject
    {
        private string _graphTeam0Title;
        private string _graphTeam1Title;
        private bool _isTeamExperiencePieChartVisible;
        private bool _isTeamExperienceRowChartVisible;

        private SeriesCollection _matchTeamLevelsLineChartCollection;
        private SeriesCollection _matchTeam0ExperienceStackedGraphCollection;
        private SeriesCollection _matchTeam1ExperienceStackedGraphCollection;
        private SeriesCollection _matchTeam0ExperiencePieChartCollection;
        private SeriesCollection _matchTeam1ExperiencePieChartCollection;
        private SeriesCollection _matchTeamExperienceRowChartCollection;

        public Func<double, string> MatchTeamLevelsFormatter { get; set; }
        public Func<double, string> MatchTeamExperienceFormatter { get; set; }
        public Func<ChartPoint, string> MatchTeamExperiencePiePointLabel { get; set; }
        public string[] ExperienceTypesLabels { get; set; }
        public double MatchTeamExperienceMaxYValue { get; set; }


        public string GraphTeam0Title
        {
            get { return _graphTeam0Title; }
            set
            {
                _graphTeam0Title = value;
                RaisePropertyChangedEvent(nameof(GraphTeam0Title));
            }
        }

        public string GraphTeam1Title
        {
            get { return _graphTeam1Title; }
            set
            {
                _graphTeam1Title = value;
                RaisePropertyChangedEvent(nameof(GraphTeam1Title));
            }
        }

        public bool IsTeamExperiencePieChartVisible
        {
            get { return _isTeamExperiencePieChartVisible; }
            set
            {
                _isTeamExperiencePieChartVisible = value;
                RaisePropertyChangedEvent(nameof(IsTeamExperiencePieChartVisible));
            }
        }

        public bool IsTeamExperienceRowChartVisible
        {
            get { return _isTeamExperienceRowChartVisible; }
            set
            {
                _isTeamExperienceRowChartVisible = value;
                RaisePropertyChangedEvent(nameof(IsTeamExperienceRowChartVisible));
            }
        }

        public bool ToggleSwitchRowOrPie
        {
            get { return UserSettings.Default.IsTeamExperienceRowChartEnabled; }
            set
            {
                UserSettings.Default.IsTeamExperienceRowChartEnabled = value;
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
                RaisePropertyChangedEvent(nameof(ToggleSwitchRowOrPie));
            }
        }

        public SeriesCollection MatchTeamLevelsLineChartCollection
        {
            get { return _matchTeamLevelsLineChartCollection; }
            set
            {
                _matchTeamLevelsLineChartCollection = value;
                RaisePropertyChangedEvent(nameof(MatchTeamLevelsLineChartCollection));
            }
        }

        public SeriesCollection MatchTeam0ExperienceStackedGraphCollection
        {
            get { return _matchTeam0ExperienceStackedGraphCollection; }
            set
            {
                _matchTeam0ExperienceStackedGraphCollection = value;
                RaisePropertyChangedEvent(nameof(MatchTeam0ExperienceStackedGraphCollection));
            }
        }

        public SeriesCollection MatchTeam1ExperienceStackedGraphCollection
        {
            get { return _matchTeam1ExperienceStackedGraphCollection; }
            set
            {
                _matchTeam1ExperienceStackedGraphCollection = value;
                RaisePropertyChangedEvent(nameof(MatchTeam1ExperienceStackedGraphCollection));
            }
        }
        public SeriesCollection MatchTeam0ExperiencePieChartCollection
        {
            get { return _matchTeam0ExperiencePieChartCollection; }
            set
            {
                _matchTeam0ExperiencePieChartCollection = value;
                RaisePropertyChangedEvent(nameof(MatchTeam0ExperiencePieChartCollection));
            }
        }

        public SeriesCollection MatchTeam1ExperiencePieChartCollection
        {
            get { return _matchTeam1ExperiencePieChartCollection; }
            set
            {
                _matchTeam1ExperiencePieChartCollection = value;
                RaisePropertyChangedEvent(nameof(MatchTeam1ExperiencePieChartCollection));
            }
        }

        public SeriesCollection MatchTeamExperienceRowChartCollection
        {
            get { return _matchTeamExperienceRowChartCollection; }
            set
            {
                _matchTeamExperienceRowChartCollection = value;
                RaisePropertyChangedEvent(nameof(MatchTeamExperienceRowChartCollection));
            }
        }

        public void SetTeamLevelSeriesCollection(List<ReplayMatchTeamLevel> matchTeamLevels)
        {
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
                        DateTime = DateTime.MinValue + time.TeamTime0.Value
                    });
                }

                if (time.Team1Level.HasValue)
                {
                    chartValuesTeam1.Add(new DateTimePoint
                    {
                        Value = time.Team1Level.Value,
                        DateTime = DateTime.MinValue + time.TeamTime1.Value
                    });
                }
            }

            MatchTeamLevelsLineChartCollection = new SeriesCollection()
            {
                new LineSeries
                {
                    Title = GraphTeam0Title,
                    Values = chartValuesTeam0,
                    Fill = Brushes.Transparent
                },
                new LineSeries
                {
                    Title = GraphTeam1Title,
                    Values = chartValuesTeam1,
                    Fill = Brushes.Transparent
                }
            };
        }

        public MatchSummaryGraphs()
        {
            ToggleSwitchRowOrPie = UserSettings.Default.IsTeamExperienceRowChartEnabled;

            GraphTeam0Title = "Team 1";
            GraphTeam1Title = "Team 2";
        }

        public void SetTeamExperienceSeriesCollection(List<ReplayMatchTeamExperience> matchTeamExperience)
        {
            MatchTeamExperienceFormatter = value => new DateTime((long)value).ToString("mm:ss");
            MatchTeamExperiencePiePointLabel = value => string.Format("{0} ({1:P})", value.Y, value.Participation);
            ExperienceTypesLabels = new[] { "Structures", "Passive", "Minions", "Mercenaries", "Heroes" };

            var chartValuesTeam0Mercs = new ChartValues<DateTimePoint>();
            var chartValuesTeam0Minions = new ChartValues<DateTimePoint>();
            var chartValuesTeam0Heroes = new ChartValues<DateTimePoint>();
            var chartValuesTeam0Structures = new ChartValues<DateTimePoint>();
            var chartValuesTeam0Passive = new ChartValues<DateTimePoint>();

            var chartValuesTeam1Mercs = new ChartValues<DateTimePoint>();
            var chartValuesTeam1Minions = new ChartValues<DateTimePoint>();
            var chartValuesTeam1Heroes = new ChartValues<DateTimePoint>();
            var chartValuesTeam1Structures = new ChartValues<DateTimePoint>();
            var chartValuesTeam1Passive = new ChartValues<DateTimePoint>(); ;

            double totalTeam0 = 0;
            double totalTeam1 = 0;

            foreach (var item in matchTeamExperience)
            {
                if (item.Time.HasValue)
                {
                    DateTime time = DateTime.MinValue + item.Time.Value;
                    chartValuesTeam0Heroes.Add(new DateTimePoint(time, (double)item.Team0HeroXP));
                    chartValuesTeam0Mercs.Add(new DateTimePoint(time, (double)item.Team0CreepXP));
                    chartValuesTeam0Minions.Add(new DateTimePoint(time, (double)item.Team0MinionXP));
                    chartValuesTeam0Passive.Add(new DateTimePoint(time, (double)item.Team0TrickleXP));
                    chartValuesTeam0Structures.Add(new DateTimePoint(time, (double)item.Team0StructureXP));
                    double total0 = (double)(item.Team0HeroXP + item.Team0CreepXP + item.Team0MinionXP + item.Team0TrickleXP + item.Team0StructureXP);

                    chartValuesTeam1Heroes.Add(new DateTimePoint(time, (double)item.Team1HeroXP));
                    chartValuesTeam1Mercs.Add(new DateTimePoint(time, (double)item.Team1CreepXP));
                    chartValuesTeam1Minions.Add(new DateTimePoint(time, (double)item.Team1MinionXP));
                    chartValuesTeam1Passive.Add(new DateTimePoint(time, (double)item.Team1TrickleXP));
                    chartValuesTeam1Structures.Add(new DateTimePoint(time, (double)item.Team1StructureXP));
                    double total1 = (double)(item.Team1HeroXP + item.Team1CreepXP + item.Team1MinionXP + item.Team1TrickleXP + item.Team1StructureXP);
                }
            }

            var lastExpTime = matchTeamExperience.Last();
            totalTeam0 += (double)(lastExpTime.Team0HeroXP + lastExpTime.Team0CreepXP + lastExpTime.Team0MinionXP + lastExpTime.Team0TrickleXP + lastExpTime.Team0StructureXP);
            totalTeam1 += (double)(lastExpTime.Team1HeroXP + lastExpTime.Team1CreepXP + lastExpTime.Team1MinionXP + lastExpTime.Team1TrickleXP + lastExpTime.Team1StructureXP);

            #region stacked graph
            MatchTeam0ExperienceStackedGraphCollection = new SeriesCollection()
            {
                new StackedAreaSeries
                {
                    Title = "Heroes",
                    Values = chartValuesTeam0Heroes,
                    LineSmoothness = 0,
                },
                new StackedAreaSeries
                {
                    Title = "Mercenaries",
                    Values = chartValuesTeam0Mercs,
                    LineSmoothness = 0,
                },
                new StackedAreaSeries
                {
                    Title = "Minions",
                    Values = chartValuesTeam0Minions,
                    LineSmoothness = 0,
                },
                new StackedAreaSeries
                {
                    Title = "Passive",
                    Values = chartValuesTeam0Passive,
                    LineSmoothness = 0,
                },
                new StackedAreaSeries
                {
                    Title = "Structures",
                    Values = chartValuesTeam0Structures,
                    LineSmoothness = 0,
                },
            };

            MatchTeam1ExperienceStackedGraphCollection = new SeriesCollection()
            {
                new StackedAreaSeries
                {
                    Title = "Heroes",
                    Values = chartValuesTeam1Heroes,
                    LineSmoothness = 0
                },
                new StackedAreaSeries
                {
                    Title = "Mercenaries",
                    Values = chartValuesTeam1Mercs,
                    LineSmoothness = 0
                },
                new StackedAreaSeries
                {
                    Title = "Minions",
                    Values = chartValuesTeam1Minions,
                    LineSmoothness = 0
                },
                new StackedAreaSeries
                {
                    Title = "Passive",
                    Values = chartValuesTeam1Passive,
                    LineSmoothness = 0
                },
                new StackedAreaSeries
                {
                    Title = "Structures",
                    Values = chartValuesTeam1Structures,
                    LineSmoothness = 0
                },
            };

            MatchTeamExperienceMaxYValue = totalTeam0 > totalTeam1 ? totalTeam0 : totalTeam1;
            #endregion stacked graph

            #region pie chart
            MatchTeam0ExperiencePieChartCollection = new SeriesCollection()
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
            MatchTeam1ExperiencePieChartCollection = new SeriesCollection()
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

            #endregion pie chart

            #region row chart
            MatchTeamExperienceRowChartCollection = new SeriesCollection()
            {
                new RowSeries
                {
                    Title = GraphTeam0Title,
                    Values = new ChartValues<double> { (double)lastExpTime.Team0StructureXP, (double)lastExpTime.Team0TrickleXP, (double)lastExpTime.Team0MinionXP, (double)lastExpTime.Team0CreepXP, (double)lastExpTime.Team0HeroXP },
                    DataLabels = true,
                },
                new RowSeries
                {
                    Title = GraphTeam1Title,
                    Values = new ChartValues<double> { (double)lastExpTime.Team1StructureXP, (double)lastExpTime.Team1TrickleXP, (double)lastExpTime.Team1MinionXP, (double)lastExpTime.Team1CreepXP, (double)lastExpTime.Team1HeroXP },
                    DataLabels = true,
                },
            };
            #endregion row chart
        }
    }
}
