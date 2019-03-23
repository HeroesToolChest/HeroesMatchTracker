using GalaSoft.MvvmLight.CommandWpf;
using Heroes.Helpers;
using Heroes.Icons;
using Heroes.Models;
using HeroesMatchTracker.Core.Services;
using HeroesMatchTracker.Core.ViewServices;
using Microsoft.Practices.ServiceLocation;
using NLog;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.IO;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Markup;

namespace HeroesMatchTracker.Core.ViewModels.Statistics
{
    public class StatsAllHeroesViewModel : HmtViewModel, IDisposable
    {
        private readonly string InitialSeasonListOption = "- Select Season -";

        private bool _isQuickMatchSelected;
        private bool _isUnrankedDraftSelected;
        private bool _isStormLeagueSelected;
        private bool _isHeroLeagueSelected;
        private bool _isTeamLeagueSelected;
        private bool _isCustomGameSelected;
        private bool _isBrawlSelected;
        private bool _isTotalSelected;
        private string _selectedSeason;

        private DataTable _statsAllHeroesDataTable = new DataTable();
        private ObservableCollection<DataGridColumn> _gameModesColumnCollection = new ObservableCollection<DataGridColumn>();

        private List<DataGridColumn> DataGridTextColumnsList;
        private ILoadingOverlayWindowService LoadingOverlayWindow;

        public StatsAllHeroesViewModel(IInternalService internalService, ILoadingOverlayWindowService loadingOverlayWindow)
            : base(internalService)
        {
            LoadingOverlayWindow = loadingOverlayWindow;

            IsQuickMatchSelected = false;
            IsUnrankedDraftSelected = false;
            IsStormLeagueSelected = false;
            IsHeroLeagueSelected = false;
            IsTeamLeagueSelected = false;
            IsCustomGameSelected = false;
            IsBrawlSelected = false;

            SeasonList.Add(InitialSeasonListOption);
            SeasonList.AddRange(HeroesHelpers.Seasons.GetSeasonList());
            SelectedSeason = SeasonList[0];
        }

        public IMainWindowDialogsService MainWindowDialog => ServiceLocator.Current.GetInstance<IMainWindowDialogsService>();

        public RelayCommand QueryAllHeroesGameModeCommand => new RelayCommand(async () => await QueryAllHeroesGameModeAsyncCommand());

        public List<string> SeasonList { get; private set; } = new List<string>();

        public string SelectedSeason
        {
            get => _selectedSeason;
            set
            {
                _selectedSeason = value;
                RaisePropertyChanged();
            }
        }

        public bool IsQuickMatchSelected
        {
            get => _isQuickMatchSelected;
            set
            {
                _isQuickMatchSelected = value;
                RaisePropertyChanged();
            }
        }

        public bool IsUnrankedDraftSelected
        {
            get => _isUnrankedDraftSelected;
            set
            {
                _isUnrankedDraftSelected = value;
                RaisePropertyChanged();
            }
        }

        public bool IsStormLeagueSelected
        {
            get => _isStormLeagueSelected;
            set
            {
                _isStormLeagueSelected = value;
                RaisePropertyChanged();
            }
        }

        public bool IsHeroLeagueSelected
        {
            get => _isHeroLeagueSelected;
            set
            {
                _isHeroLeagueSelected = value;
                RaisePropertyChanged();
            }
        }

        public bool IsTeamLeagueSelected
        {
            get => _isTeamLeagueSelected;
            set
            {
                _isTeamLeagueSelected = value;
                RaisePropertyChanged();
            }
        }

        public bool IsCustomGameSelected
        {
            get => _isCustomGameSelected;
            set
            {
                _isCustomGameSelected = value;
                RaisePropertyChanged();
            }
        }

        public bool IsBrawlSelected
        {
            get => _isBrawlSelected;
            set
            {
                _isBrawlSelected = value;
                RaisePropertyChanged();
            }
        }

        public DataTable StatsAllHeroesDataTable
        {
            get => _statsAllHeroesDataTable;
            set
            {
                _statsAllHeroesDataTable = value;
                RaisePropertyChanged();
            }
        }

        public bool IsTotalSelected
        {
            get => _isTotalSelected;
            set
            {
                _isTotalSelected = value;
                RaisePropertyChanged();
            }
        }

        public ObservableCollection<DataGridColumn> GameModesColumnCollection
        {
            get => _gameModesColumnCollection;
            set
            {
                _gameModesColumnCollection = value;
                RaisePropertyChanged();
            }
        }

        private async Task QueryAllHeroesGameModeAsyncCommand()
        {
            if (await MainWindowDialog.CheckBattleTagSetDialog())
                return;

            LoadingOverlayWindow.ShowLoadingOverlay();
            DataGridTextColumnsList = new List<DataGridColumn>();

            if ((!IsQuickMatchSelected && !IsUnrankedDraftSelected && !IsStormLeagueSelected && !IsHeroLeagueSelected && !IsTeamLeagueSelected && !IsCustomGameSelected && !IsBrawlSelected && IsTotalSelected) ||
                (!IsQuickMatchSelected && !IsUnrankedDraftSelected && !IsStormLeagueSelected && !IsHeroLeagueSelected && !IsTeamLeagueSelected && !IsCustomGameSelected && !IsBrawlSelected && !IsTotalSelected))
            {
                IsQuickMatchSelected = true;
                IsUnrankedDraftSelected = true;
                IsStormLeagueSelected = true;
                IsHeroLeagueSelected = true;
                IsTeamLeagueSelected = true;
            }

            // these CANNOT be in a background thread
            EmptyTable();
            CreateHeroBasicColumns();
            SetNumberOfColumns();

            await Task.Run(async () =>
            {
                try
                {
                    await QueryAllHeroesGameModeAsync();
                }
                catch (Exception ex)
                {
                    ExceptionLog.Log(LogLevel.Error, ex);
                    throw;
                }
            });

            LoadingOverlayWindow.CloseLoadingOverlay();
        }

        private async Task QueryAllHeroesGameModeAsync()
        {
            if (SelectedSeason == InitialSeasonListOption || string.IsNullOrEmpty(SelectedSeason))
                return;

            Season selectedSeason = SelectedSeason.ConvertToEnum<Season>();

            var heroesList = HeroesIcons.HeroesData().HeroNames();
            foreach (var hero in heroesList)
            {
                List<object> rowStats = new List<object>();

                Stream leaderboardPortrait = HeroesIcons.HeroesData().HeroData(hero).HeroPortrait.LeaderboardImage();
                int heroLevel = Database.ReplaysDb().MatchPlayer.ReadHighestLevelOfHero(hero, selectedSeason);

                rowStats.Add(leaderboardPortrait);
                rowStats.Add(hero);
                rowStats.Add(heroLevel);

                if (IsQuickMatchSelected)
                    rowStats.AddRange(QueryGameModeStats(hero, selectedSeason, GameMode.QuickMatch));
                if (IsUnrankedDraftSelected)
                    rowStats.AddRange(QueryGameModeStats(hero, selectedSeason, GameMode.UnrankedDraft));
                if (IsStormLeagueSelected)
                    rowStats.AddRange(QueryGameModeStats(hero, selectedSeason, GameMode.StormLeague));
                if (IsHeroLeagueSelected)
                    rowStats.AddRange(QueryGameModeStats(hero, selectedSeason, GameMode.HeroLeague));
                if (IsTeamLeagueSelected)
                    rowStats.AddRange(QueryGameModeStats(hero, selectedSeason, GameMode.TeamLeague));
                if (IsCustomGameSelected)
                    rowStats.AddRange(QueryGameModeStats(hero, selectedSeason, GameMode.Custom));
                if (IsBrawlSelected)
                    rowStats.AddRange(QueryGameModeStats(hero, selectedSeason, GameMode.Brawl));
                if (IsTotalSelected)
                    rowStats.AddRange(SetRowTotals(rowStats));

                await Application.Current.Dispatcher.InvokeAsync(() => StatsAllHeroesDataTable.Rows.Add(rowStats.ToArray()));
            }
        }

        private void CreateHeroBasicColumns()
        {
            if (StatsAllHeroesDataTable.Columns.Contains("LeaderboardPortrait"))
                return;

            DataTemplate dataTemplate = null;
            string markup = string.Empty;

            markup = "<DataTemplate xmlns=\"http://schemas.microsoft.com/winfx/2006/xaml/presentation\" xmlns:x=\"http://schemas.microsoft.com/winfx/2006/xaml\">";
            markup += "<Grid>";
            markup += "<Image Source=\"{Binding LeaderboardPortrait}\" Height=\"40\" Width=\"71\" HorizontalAlignment=\"Center\" Margin=\"10 0 0 0\" />";
            markup += "</Grid>";
            markup += "</DataTemplate>";

            dataTemplate = (DataTemplate)XamlReader.Parse(markup);

            DataGridTemplateColumn heroDataTemplateColumn = new DataGridTemplateColumn()
            {
                Header = "Hero",
                MinWidth = 90,
                MaxWidth = 90,
            };

            // add to cell template
            heroDataTemplateColumn.CellTemplate = dataTemplate;

            GameModesColumnCollection.Add(heroDataTemplateColumn);

            DataGridTextColumn characterNameColumn = new DataGridTextColumn()
            {
                Header = "Name",
                Binding = new Binding("CharacterName"),
                Width = 100,
            };
            GameModesColumnCollection.Add(characterNameColumn);

            DataGridTextColumn levelColumn = new DataGridTextColumn()
            {
                Header = "Level",
                Binding = new Binding("CharacterLevel"),
            };
            GameModesColumnCollection.Add(levelColumn);

            StatsAllHeroesDataTable.Columns.Add("LeaderboardPortrait", typeof(Stream));
            StatsAllHeroesDataTable.Columns.Add("CharacterName", typeof(string));
            StatsAllHeroesDataTable.Columns.Add("CharacterLevel", typeof(int));
        }

        private void CreateGameModeColumns(GameMode gameMode)
        {
            string gm = gameMode.ToString();
            string gmShort = string.Empty;

            switch (gameMode)
            {
                case GameMode.QuickMatch:
                    gmShort = "QM";
                    break;
                case GameMode.UnrankedDraft:
                    gmShort = "UD";
                    break;
                case GameMode.HeroLeague:
                    gmShort = "HL";
                    break;
                case GameMode.TeamLeague:
                    gmShort = "TL";
                    break;
                case GameMode.Custom:
                    gmShort = "C";
                    break;
                case GameMode.Brawl:
                    gmShort = "B";
                    break;
                case GameMode.AllGameModes:
                    gmShort = "Total";
                    break;
            }

            if (StatsAllHeroesDataTable.Columns.Contains($"{gm}Wins"))
                return;

            int width = 100;
            DataGridTextColumn winsColumn = new DataGridTextColumn()
            {
                Header = $"{gmShort} Wins",
                Binding = new Binding($"{gm}Wins"),
                Width = width,
            };
            GameModesColumnCollection.Add(winsColumn);

            DataGridTextColumn lossesColumn = new DataGridTextColumn()
            {
                Header = $"{gmShort} Losses",
                Binding = new Binding($"{gm}Losses"),
                Width = width + 6,
            };
            GameModesColumnCollection.Add(lossesColumn);

            DataGridTextColumn gamesColumn = new DataGridTextColumn()
            {
                Header = $"{gmShort} Games",
                Binding = new Binding($"{gm}Games"),
                Width = width,
            };
            GameModesColumnCollection.Add(gamesColumn);

            Binding winPercentage = new Binding($"{gm}WinPercentage")
            {
                StringFormat = "{0:P1}",
            };
            DataGridTextColumn winPercentageColumn = new DataGridTextColumn()
            {
                Header = "Win %",
                Binding = winPercentage,
                Width = 60,
                FontWeight = FontWeights.Bold,
            };
            GameModesColumnCollection.Add(winPercentageColumn);

            StatsAllHeroesDataTable.Columns.Add($"{gm}Wins", typeof(int));
            StatsAllHeroesDataTable.Columns.Add($"{gm}Losses", typeof(int));
            StatsAllHeroesDataTable.Columns.Add($"{gm}Games", typeof(int));
            StatsAllHeroesDataTable.Columns.Add($"{gm}WinPercentage", typeof(double));
        }

        private void SetNumberOfColumns()
        {
            if (IsQuickMatchSelected)
            {
                CreateGameModeColumns(GameMode.QuickMatch);
            }

            if (IsUnrankedDraftSelected)
            {
                CreateGameModeColumns(GameMode.UnrankedDraft);
            }

            if (IsStormLeagueSelected)
            {
                CreateGameModeColumns(GameMode.StormLeague);
            }

            if (IsHeroLeagueSelected)
            {
                CreateGameModeColumns(GameMode.HeroLeague);
            }

            if (IsTeamLeagueSelected)
            {
                CreateGameModeColumns(GameMode.TeamLeague);
            }

            if (IsCustomGameSelected)
            {
                CreateGameModeColumns(GameMode.Custom);
            }

            if (IsBrawlSelected)
            {
                CreateGameModeColumns(GameMode.Brawl);
            }

            if (IsTotalSelected)
            {
                CreateGameModeColumns(GameMode.AllGameModes);
            }
        }

        private List<object> QueryGameModeStats(string hero, Season season, GameMode gameMode)
        {
            int wins = Database.ReplaysDb().Statistics.ReadGameResults(hero, season, gameMode, true);
            int losses = Database.ReplaysDb().Statistics.ReadGameResults(hero, season, gameMode, false);
            int total = wins + losses;
            double? percentage = Utilities.CalculateWinValue(wins, total);

            if (total != 0)
                return new List<object>() { wins, losses, total, percentage };
            else
                return new List<object>() { null, null, null, null };
        }

        private List<object> SetRowTotals(List<object> rowStats)
        {
            int totalWins = 0;
            int totalLosses = 0;
            int totalTotal = 0;

            int modes = (rowStats.Count - 3) / 4;

            for (int i = 0; i < modes; i++)
            {
                totalWins += (int?)rowStats[(i * 4) + 3] ?? 0;
                totalLosses += (int?)rowStats[(i * 4) + 4] ?? 0;
            }

            totalTotal = totalWins + totalLosses;

            if (totalTotal > 0)
            {
                double percentage = Utilities.CalculateWinValue(totalWins, totalTotal);
                return new List<object>() { totalWins, totalLosses, totalTotal, percentage };
            }
            else
            {
                return new List<object>() { null, null, null, null };
            }
        }

        private void AddDataGridColumns()
        {
            foreach (var column in DataGridTextColumnsList)
                GameModesColumnCollection.Add(column);
        }

        private void EmptyTable()
        {
            StatsAllHeroesDataTable.Columns.Clear();
            StatsAllHeroesDataTable.Rows.Clear();

            GameModesColumnCollection.Clear();
        }

        #region IDisposable Support
#pragma warning disable SA1201 // Elements must appear in the correct order
        private bool disposedValue = false; // To detect redundant calls
#pragma warning restore SA1201 // Elements must appear in the correct order

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    ((IDisposable)_statsAllHeroesDataTable).Dispose();
                }

                _statsAllHeroesDataTable = null;
                disposedValue = true;
            }
        }

#pragma warning disable SA1202 // Elements must be ordered by access
        public void Dispose()
#pragma warning restore SA1202 // Elements must be ordered by access
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        #endregion
    }
}
