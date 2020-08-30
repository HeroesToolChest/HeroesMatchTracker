using GalaSoft.MvvmLight.CommandWpf;
using Heroes.Helpers;
using Heroes.Models;
using HeroesMatchTracker.Core.Services;
using HeroesMatchTracker.Core.ViewServices;
using Microsoft.Practices.ServiceLocation;
using NLog;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using static Heroes.Helpers.HeroesHelpers.Regions;

namespace HeroesMatchTracker.Core.ViewModels.Statistics
{
    public class StatsPartiesViewModel : HmtViewModel
    {
        private readonly string InitialSeasonListOption = "- Select Season -";
        private readonly string InitialHeroListOption = "Any";

        private int _partyWins;
        private int _partyLosses;
        private int _partyTotal;
        private int _selectedRegion;
        private bool _isQuickMatchSelected;
        private bool _isUnrankedDraftSelected;
        private bool _isStormLeagueSelected;
        private bool _isHeroLeagueSelected;
        private bool _isTeamLeagueSelected;
        private bool _isCustomGameSelected;
        private bool _isBrawlSelected;
        private bool _isPlayersInParty;
        private string _selectedSeason;

        private double _partyWinrate;

        private bool[] _isPlayerChecked = new bool[5];
        private string[] _playerBattleTag = new string[5];
        private string[] _selectedCharacter = new string[5];

        private ILoadingOverlayWindowService LoadingOverlayWindow;

        private ObservableCollection<string> _errorMessages = new ObservableCollection<string>();

        public StatsPartiesViewModel(IInternalService internalService, ILoadingOverlayWindowService loadingOverlayWindow)
            : base(internalService)
        {
            LoadingOverlayWindow = loadingOverlayWindow;

            SeasonList.Add(InitialSeasonListOption);
            SeasonList.AddRange(HeroesHelpers.Seasons.GetSeasonList());
            SelectedSeason = SeasonList[0];

            HeroesList.Add(InitialHeroListOption);
            HeroesList.AddRange(HeroesIcons.HeroesData().HeroNames().OrderBy(x => x).ToList());

            SelectedCharacter = Enumerable.Repeat(InitialHeroListOption, SelectedCharacter.Length).ToArray();

            ClearOptions();
        }

        public IMainWindowDialogsService MainWindowDialog => ServiceLocator.Current.GetInstance<IMainWindowDialogsService>();

        public RelayCommand QueryPartyStatsCommand => new RelayCommand(async () => await QueryPartyStatsAsyncCommand());
        public RelayCommand ClearPartyStatsCommand => new RelayCommand(ClearOptions);

        public List<string> SeasonList { get; private set; } = new List<string>();
        public List<string> HeroesList { get; private set; } = new List<string>();
        public List<string> RegionsList
        {
            get { return GetRegionsList(); }
        }

        public string SelectedSeason
        {
            get => _selectedSeason;
            set
            {
                _selectedSeason = value;
                RaisePropertyChanged();
            }
        }

        public string SelectedRegion
        {
            get => ((Region)_selectedRegion).ToString();
            set
            {
                if (Enum.TryParse(value, out Region region))
                    _selectedRegion = (int)region;
                else
                    _selectedRegion = 99;
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

        public bool IsPlayersInParty
        {
            get => _isPlayersInParty;
            set
            {
                _isPlayersInParty = value;
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

        public bool[] IsPlayerChecked
        {
            get => _isPlayerChecked;
            set
            {
                _isPlayerChecked = value;
                RaisePropertyChanged();
            }
        }

        public string[] PlayerBattleTag
        {
            get => _playerBattleTag;
            set
            {
                _playerBattleTag = value;
                RaisePropertyChanged();
            }
        }

        public string[] SelectedCharacter
        {
            get => _selectedCharacter;
            set
            {
                _selectedCharacter = value;
                RaisePropertyChanged();
            }
        }

        public int PartyWins
        {
            get => _partyWins;
            set
            {
                _partyWins = value;
                RaisePropertyChanged();
            }
        }

        public int PartyLosses
        {
            get => _partyLosses;
            set
            {
                _partyLosses = value;
                RaisePropertyChanged();
            }
        }

        public int PartyTotal
        {
            get => _partyTotal;
            set
            {
                _partyTotal = value;
                RaisePropertyChanged();
            }
        }

        public double PartyWinrate
        {
            get => _partyWinrate;
            set
            {
                _partyWinrate = value;
                RaisePropertyChanged();
            }
        }

        public ObservableCollection<string> ErrorMessages
        {
            get => _errorMessages;
            set
            {
                _errorMessages = value;
                RaisePropertyChanged();
            }
        }

        private async Task QueryPartyStatsAsyncCommand()
        {
            if (await MainWindowDialog.CheckBattleTagSetDialog())
                return;

            await Task.Run(async () =>
            {
                try
                {
                    LoadingOverlayWindow.ShowLoadingOverlay();
                    await Task.Delay(1);
                    await QueryPartyStats();
                }
                catch (Exception ex)
                {
                    ExceptionLog.Log(LogLevel.Error, ex);
                    throw;
                }
            });

            LoadingOverlayWindow.CloseLoadingOverlay();
        }

        private async Task QueryPartyStats()
        {
            ClearPartyStats();

            if (SelectedSeason == InitialSeasonListOption || string.IsNullOrEmpty(SelectedSeason))
                return;

            Season selectedSeason = SelectedSeason.ConvertToEnum<Season>();
            GameMode selectedGameModes = SetSelectedGameModes();

            if (!Enum.TryParse(SelectedRegion, out Region region))
                region = Region.XX;

            List<long> playerIds = new List<long>();
            List<string> characters = new List<string>();

            for (int i = 0; i < PlayerBattleTag.Length; i++)
            {
                if (IsPlayerChecked[i])
                {
                    long playerId = Database.ReplaysDb().HotsPlayer.ReadPlayerIdFromBattleTagName(PlayerBattleTag[i], (int)region);
                    if (playerId <= 0 && !string.IsNullOrEmpty(PlayerBattleTag[i]))
                        ErrorMessages.Add("BattleTag not found");
                    else
                        ErrorMessages.Add(string.Empty);

                    playerIds.Add(playerId);
                    characters.Add(SelectedCharacter[i]);
                }
            }

            PartyWins = Database.ReplaysDb().Statistics.ReadPartyGameResult(selectedSeason, selectedGameModes, playerIds, characters, IsPlayersInParty, true);
            PartyLosses = Database.ReplaysDb().Statistics.ReadPartyGameResult(selectedSeason, selectedGameModes, playerIds, characters, IsPlayersInParty, false);
            PartyTotal = PartyWins + PartyLosses;
            PartyWinrate = Utilities.CalculateWinPercentage(PartyWins, PartyTotal) / 100;

            await Task.CompletedTask;
        }

        private GameMode SetSelectedGameModes()
        {
            GameMode gameModes = GameMode.Unknown;

            if (!IsQuickMatchSelected && !IsUnrankedDraftSelected && !IsStormLeagueSelected && !IsHeroLeagueSelected && !IsTeamLeagueSelected && !IsCustomGameSelected && !IsBrawlSelected)
            {
                gameModes = GameMode.QuickMatch | GameMode.UnrankedDraft | GameMode.StormLeague | GameMode.HeroLeague | GameMode.TeamLeague;
                IsQuickMatchSelected = true;
                IsUnrankedDraftSelected = true;
                IsStormLeagueSelected = true;
                IsHeroLeagueSelected = true;
                IsTeamLeagueSelected = true;
            }
            else
            {
                if (IsQuickMatchSelected)
                    gameModes |= GameMode.QuickMatch;
                if (IsUnrankedDraftSelected)
                    gameModes |= GameMode.UnrankedDraft;
                if (IsStormLeagueSelected)
                    gameModes |= GameMode.StormLeague;
                if (IsHeroLeagueSelected)
                    gameModes |= GameMode.HeroLeague;
                if (IsTeamLeagueSelected)
                    gameModes |= GameMode.TeamLeague;
                if (IsCustomGameSelected)
                    gameModes |= GameMode.Custom;
                if (IsBrawlSelected)
                    gameModes |= GameMode.Brawl;
            }

            return gameModes;
        }

        private void ClearOptions()
        {
            IsPlayersInParty = false;
            IsPlayerChecked = Enumerable.Repeat(false, IsPlayerChecked.Length).ToArray();
            PlayerBattleTag = Enumerable.Repeat(string.Empty, PlayerBattleTag.Length).ToArray();
            SelectedCharacter = Enumerable.Repeat(InitialHeroListOption, SelectedCharacter.Length).ToArray();
            ErrorMessages = new ObservableCollection<string>();
            ClearPartyStats();
        }

        private void ClearPartyStats()
        {
            PartyWins = 0;
            PartyLosses = 0;
            PartyTotal = 0;
            PartyWinrate = 0;

            ErrorMessages = new ObservableCollection<string>();
        }
    }
}
