using GalaSoft.MvvmLight.CommandWpf;
using Heroes.Helpers;
using Heroes.ReplayParser;
using HeroesMatchTracker.Core.Services;
using HeroesMatchTracker.Core.ViewServices;
using NLog;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Markup;
using System.Windows.Media.Imaging;

namespace HeroesMatchTracker.Core.ViewModels.Statistics
{
    public class StatsAllHeroesViewModel : HmtViewModel, IDisposable
    {
        private readonly string InitialSeasonListOption = "- Select Season -";

        private bool _isQuickMatchSelected;
        private bool _isUnrankedDraftSelected;
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
            IsHeroLeagueSelected = false;
            IsTeamLeagueSelected = false;
            IsCustomGameSelected = false;
            IsBrawlSelected = false;

            SeasonList.Add(InitialSeasonListOption);
            SeasonList.Add("Lifetime");
            SeasonList.AddRange(HeroesHelpers.Seasons.GetSeasonList());
            SelectedSeason = SeasonList[0];
        }

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
            LoadingOverlayWindow.ShowLoadingOverlay();
            DataGridTextColumnsList = new List<DataGridColumn>();

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
            if (SelectedSeason == InitialSeasonListOption || string.IsNullOrEmpty(SelectedSeason) ||
               (!IsQuickMatchSelected && !IsUnrankedDraftSelected && !IsHeroLeagueSelected && !IsTeamLeagueSelected && !IsCustomGameSelected && !IsBrawlSelected && IsTotalSelected) ||
               (!IsQuickMatchSelected && !IsUnrankedDraftSelected && !IsHeroLeagueSelected && !IsTeamLeagueSelected && !IsCustomGameSelected && !IsBrawlSelected && !IsTotalSelected))
                return;

            Season selectedSeason = HeroesHelpers.EnumParser.ConvertSeasonStringToEnum(SelectedSeason);

            var heroesList = HeroesIcons.Heroes().GetListOfHeroes();
            foreach (var hero in heroesList)
            {
                List<object> rowStats = new List<object>();

                BitmapImage leaderboardPortrait = HeroesIcons.Heroes().GetHeroLeaderboardPortrait(hero);
                int heroLevel = Database.ReplaysDb().MatchPlayer.ReadHighestLevelOfHero(hero, selectedSeason);

                rowStats.Add(leaderboardPortrait);
                rowStats.Add(hero);
                rowStats.Add(heroLevel);

                if (IsQuickMatchSelected)
                    rowStats.AddRange(QueryGameModeStats(hero, selectedSeason, GameMode.QuickMatch));
                if (IsUnrankedDraftSelected)
                    rowStats.AddRange(QueryGameModeStats(hero, selectedSeason, GameMode.UnrankedDraft));
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

            StatsAllHeroesDataTable.Columns.Add("LeaderboardPortrait", typeof(string));
            StatsAllHeroesDataTable.Columns.Add("CharacterName", typeof(string));
            StatsAllHeroesDataTable.Columns.Add("CharacterLevel", typeof(int));
        }

        private void CreateGameModeColumns(GameMode gameMode)
        {
            string gm = gameMode.ToString();
            string gmShort = string.Empty;

            switch (gm)
            {
                case "QuickMatch":
                    gmShort = "QM";
                    break;
                case "UnrankedDraft":
                    gmShort = "UD";
                    break;
                case "HeroLeague":
                    gmShort = "HL";
                    break;
                case "TeamLeague":
                    gmShort = "TL";
                    break;
                case "Custom":
                    gmShort = "C";
                    break;
                case "Brawl":
                    gmShort = "B";
                    break;
                case "TryMe":
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
                Width = width,
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
                StringFormat = "{0}%",
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
            StatsAllHeroesDataTable.Columns.Add($"{gm}WinPercentage", typeof(string));
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
                CreateGameModeColumns(GameMode.TryMe);
            }
        }

        private List<object> QueryGameModeStats(string hero, Season season, GameMode gameMode)
        {
            int wins = Database.ReplaysDb().Statistics.ReadGameResults(hero, season, gameMode, true);
            int losses = Database.ReplaysDb().Statistics.ReadGameResults(hero, season, gameMode, false);
            int total = wins + losses;
            int percentage = Utilities.CalculateWinPercentage(wins, total);

            return new List<object>() { wins, losses, total, percentage };
        }

        private List<object> SetRowTotals(List<object> rowStats)
        {
            int totalWins = 0;
            int totalLosses = 0;
            int totalTotal = 0;

            int modes = (rowStats.Count - 3) / 4;

            for (int i = 0; i < modes; i++)
            {
                totalWins += (int)rowStats[(i * 4) + 3];
                totalLosses += (int)rowStats[(i * 4) + 4];
            }

            totalTotal = totalWins + totalLosses;
            int percentage = Utilities.CalculateWinPercentage(totalWins, totalTotal);

            return new List<object>() { totalWins, totalLosses, totalTotal, percentage };
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
