using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using Heroes.Helpers;
using Heroes.Models;
using HeroesMatchTracker.Core.Messaging;
using HeroesMatchTracker.Core.Models.MatchModels;
using HeroesMatchTracker.Core.Services;
using HeroesMatchTracker.Core.ViewServices;
using HeroesMatchTracker.Data.Models.Replays;
using HeroesMatchTracker.Data.Queries.Replays;
using Microsoft.Practices.ServiceLocation;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace HeroesMatchTracker.Core.ViewModels.Matches
{
    public abstract class MatchesBase : HmtViewModel
    {
        private readonly MatchesTab CurrentTab;
        private readonly IWebsiteService Website;
        private readonly GameMode MatchGameMode;

        private int _selectedAccountLevel;
        private bool _isGivenBattleTagOnlyChecked;
        private bool _isPartyGivenBattleTagOnlyChecked;
        private bool _isPartyHeroOnlyChecked;
        private bool _isAccountyGivenBattleTagOnlyChecked;
        private bool _isAccountHeroOnlyChecked;
        private bool _showMatchSummaryButtonEnabled;
        private long _selectedReplayIdValue;
        private string _selectedSeasonOption;
        private string _selectedMapOption;
        private string _selectedBuildOption;
        private string _selectedGameTimeOption;
        private string _selectedGameDateOption;
        private string _selectedPlayerBattleTag;
        private string _selectedCharacter;
        private string _team1OverviewHeader;
        private string _team2OverviewHeader;
        private string _selectedParty;
        private ReplayMatch _selectedReplay;

        private ObservableCollection<ReplayMatch> _matchListCollection = new ObservableCollection<ReplayMatch>();
        private ObservableCollection<MatchPlayerBase> _matchOverviewTeam1Collection = new ObservableCollection<MatchPlayerBase>();
        private ObservableCollection<MatchPlayerBase> _matchOverviewTeam2Collection = new ObservableCollection<MatchPlayerBase>();

        public MatchesBase(IInternalService internalService, IWebsiteService website, IMatchesTabService matchesTab, GameMode matchGameMode, MatchesTab currentTab)
            : base(internalService)
        {
            Website = website;
            MatchesTabService = matchesTab;
            MatchGameMode = matchGameMode;
            CurrentTab = currentTab;

            ShowMatchSummaryButtonEnabled = true;

            SeasonList.AddRange(HeroesHelpers.Seasons.GetSeasonList());
            SelectedSeasonOption = SeasonList[0];

            GameTimeList = HeroesHelpers.GameDateFilters.GameTimeList;
            SelectedGameTimeOption = FilterGameTimeOption.Any.GetFriendlyName();

            GameDateList = HeroesHelpers.GameDateFilters.GameDateList;
            SelectedGameDateOption = FilterGameDateOption.Any.GetFriendlyName();

            MapsList.Add("Any");
            MapsList.AddRange(HeroesIcons.Battlegrounds().Battlegrounds(true).Select(x => x.Name).ToList());
            SelectedMapOption = MapsList[0];

            ReplayBuildsList.Add("Any");
            ReplayBuildsList.AddRange(HeroesIcons.HeroBuilds().Builds);
            SelectedBuildOption = ReplayBuildsList[0];

            HeroesList.Add("Any");
            HeroesList.AddRange(HeroesIcons.HeroData().HeroNames().ToList());
            SelectedCharacter = HeroesList[0];

            PartyCountList.Add("Any");
            PartyCountList.AddRange(HeroesHelpers.Parties.GetPartyList());
            SelectedParty = PartyCountList[0];

            SelectedAccountLevel = 0;

            Messenger.Default.Register<MatchesDataMessage>(this, (message) => ReceivedMatchSearchData(message));
            Messenger.Default.Register<NotificationMessage>(this, async (message) => await ReceivedMessageAsync(message));
        }

        public List<string> SeasonList { get; private set; } = new List<string>();
        public List<string> MapsList { get; private set; } = new List<string>();
        public List<string> ReplayBuildsList { get; private set; } = new List<string>();
        public List<string> GameTimeList { get; private set; } = new List<string>();
        public List<string> GameDateList { get; private set; } = new List<string>();
        public List<string> HeroesList { get; private set; } = new List<string>();
        public List<string> PartyCountList { get; private set; } = new List<string>();

        public int SelectedAccountLevel
        {
            get => _selectedAccountLevel;
            set
            {
                _selectedAccountLevel = value;
                RaisePropertyChanged();
            }
        }

        public bool IsGivenBattleTagOnlyChecked
        {
            get => _isGivenBattleTagOnlyChecked;
            set
            {
                _isGivenBattleTagOnlyChecked = value;
                RaisePropertyChanged();
            }
        }

        public bool IsPartyGivenBattleTagOnlyChecked
        {
            get => _isPartyGivenBattleTagOnlyChecked;
            set
            {
                _isPartyGivenBattleTagOnlyChecked = value;
                RaisePropertyChanged();
            }
        }

        public bool IsPartyHeroOnlyChecked
        {
            get => _isPartyHeroOnlyChecked;
            set
            {
                _isPartyHeroOnlyChecked = value;
                RaisePropertyChanged();
            }
        }

        public bool IsAccountyGivenBattleTagOnlyChecked
        {
            get => _isAccountyGivenBattleTagOnlyChecked;
            set
            {
                _isAccountyGivenBattleTagOnlyChecked = value;
                RaisePropertyChanged();
            }
        }

        public bool IsAccountHeroOnlyChecked
        {
            get => _isAccountHeroOnlyChecked;
            set
            {
                _isAccountHeroOnlyChecked = value;
                RaisePropertyChanged();
            }
        }

        public bool ShowMatchSummaryButtonEnabled
        {
            get => _showMatchSummaryButtonEnabled;
            set
            {
                _showMatchSummaryButtonEnabled = value;
                RaisePropertyChanged();
            }
        }

        public long SelectedReplayIdValue
        {
            get => _selectedReplayIdValue;
            set
            {
                _selectedReplayIdValue = value;
                RaisePropertyChanged();
            }
        }

        public string SelectedSeasonOption
        {
            get => _selectedSeasonOption;
            set
            {
                _selectedSeasonOption = value;
                RaisePropertyChanged();
            }
        }

        public string SelectedMapOption
        {
            get => _selectedMapOption;
            set
            {
                _selectedMapOption = value;
                RaisePropertyChanged();
            }
        }

        public string SelectedBuildOption
        {
            get => _selectedBuildOption;
            set
            {
                _selectedBuildOption = value;
                RaisePropertyChanged();
            }
        }

        public string SelectedGameTimeOption
        {
            get => _selectedGameTimeOption;
            set
            {
                _selectedGameTimeOption = value;
                RaisePropertyChanged();
            }
        }

        public string SelectedGameDateOption
        {
            get => _selectedGameDateOption;
            set
            {
                _selectedGameDateOption = value;
                RaisePropertyChanged();
            }
        }

        public string SelectedPlayerBattleTag
        {
            get => _selectedPlayerBattleTag;
            set
            {
                _selectedPlayerBattleTag = value;
                RaisePropertyChanged();
            }
        }

        public string SelectedCharacter
        {
            get => _selectedCharacter;
            set
            {
                _selectedCharacter = value;
                RaisePropertyChanged();
            }
        }

        public string Team1OverviewHeader
        {
            get => _team1OverviewHeader;
            set
            {
                _team1OverviewHeader = value;
                RaisePropertyChanged();
            }
        }

        public string Team2OverviewHeader
        {
            get => _team2OverviewHeader;
            set
            {
                _team2OverviewHeader = value;
                RaisePropertyChanged();
            }
        }

        public string SelectedParty
        {
            get => _selectedParty;
            set
            {
                _selectedParty = value;
                RaisePropertyChanged();
            }
        }

        public ReplayMatch SelectedReplay
        {
            get => _selectedReplay;
            set
            {
                _selectedReplay = value;
                RaisePropertyChanged();
            }
        }

        public ObservableCollection<ReplayMatch> MatchListCollection
        {
            get => _matchListCollection;
            set
            {
                _matchListCollection = value;
                RaisePropertyChanged();
            }
        }

        public ObservableCollection<MatchPlayerBase> MatchOverviewTeam1Collection
        {
            get => _matchOverviewTeam1Collection;
            set
            {
                _matchOverviewTeam1Collection = value;
                RaisePropertyChanged();
            }
        }

        public ObservableCollection<MatchPlayerBase> MatchOverviewTeam2Collection
        {
            get => _matchOverviewTeam2Collection;
            set
            {
                _matchOverviewTeam2Collection = value;
                RaisePropertyChanged();
            }
        }

        public IMatchSummaryFlyoutService MatchSummaryFlyout => ServiceLocator.Current.GetInstance<IMatchSummaryFlyoutService>();
        public IMatchSummaryReplayService MatchSummaryReplay => ServiceLocator.Current.GetInstance<IMatchSummaryReplayService>();

        public RelayCommand ClearSearchCommand => new RelayCommand(ClearSearch);
        public RelayCommand LoadMatchListCommand => new RelayCommand(LoadMatchList);
        public RelayCommand ShowMatchOverviewCommand => new RelayCommand(LoadMatchOverview);
        public RelayCommand ShowMatchSummaryCommand => new RelayCommand(async () => await ShowMatchSummaryAsync());

        protected IMatchesTabService MatchesTabService { get; private set; }

        protected virtual void ReceivedMatchSearchData(MatchesDataMessage message)
        {
            if (!string.IsNullOrEmpty(message.SelectedCharacter))
                SelectedCharacter = message.SelectedCharacter;

            if (!string.IsNullOrEmpty(message.SelectedBattleTagName))
                SelectedPlayerBattleTag = message.SelectedBattleTagName;

            if (!string.IsNullOrEmpty(message.SelectedCharacter) && !string.IsNullOrEmpty(message.SelectedBattleTagName))
                IsGivenBattleTagOnlyChecked = true;
        }

        private void LoadMatchList()
        {
            ReplayFilter filter = new ReplayFilter
            {
                IsGivenBattleTagOnlyChecked = IsGivenBattleTagOnlyChecked,
                IsPartyBattleTagOnlyChecked = IsPartyGivenBattleTagOnlyChecked,
                IsPartyHeroOnlyChecked = IsPartyHeroOnlyChecked,
                IsAccountBattleTagOnlyChecked = IsAccountyGivenBattleTagOnlyChecked,
                IsAccountHeroOnlyChecked = IsAccountHeroOnlyChecked,
                SelectedBattleTag = SelectedPlayerBattleTag,
                SelectedBuildOption = SelectedBuildOption,
                SelectedCharacter = SelectedCharacter,
                SelectedGameDateOption = SelectedGameDateOption.ConvertToEnum<FilterGameDateOption>(),
                SelectedGameTimeOption = SelectedGameTimeOption.ConvertToEnum<FilterGameTimeOption>(),
                SelectedMapOption = SelectedMapOption,
                SelectedReplayId = SelectedReplayIdValue,
                SelectedSeason = SelectedSeasonOption.ConvertToEnum<Season>(),
                SelectedPartyCount = SelectedParty,
                SelectedAccountLevel = SelectedAccountLevel,
                BuildOptionsList = ReplayBuildsList,
                HeroesList = HeroesList,
                MapOptionsList = MapsList,
                PartyCountList = PartyCountList,
            };

            MatchListCollection = new ObservableCollection<ReplayMatch>(Database.ReplaysDb().MatchReplay.ReadGameModeRecords(MatchGameMode, filter));
        }

        private void LoadMatchOverview()
        {
            ClearMatchOverview();

            if (SelectedReplay == null)
                return;

            ReplayMatch replayMatch = SelectedReplay;

            // get info
            replayMatch = Database.ReplaysDb().MatchReplay.ReadReplayIncludeAssociatedRecords(replayMatch.ReplayId);
            var playersList = replayMatch.ReplayMatchPlayers;
            var matchAwardDictionary = replayMatch.ReplayMatchAward.ToDictionary(x => x.PlayerId, x => x.Award);

            var playerParties = PlayerParties.FindPlayerParties(playersList);

            foreach (var player in playersList)
            {
                if (player.Team == 4)
                    continue;

                MatchPlayerBase matchPlayerBase = new MatchPlayerBase(InternalService, Website, player, replayMatch.ReplayBuild.Value);
                matchPlayerBase.SetPlayerInfo(player.IsAutoSelect, playerParties, matchAwardDictionary);

                // add to collection
                if (player.Team == 0)
                    MatchOverviewTeam1Collection.Add(matchPlayerBase);
                else if (player.Team == 1)
                    MatchOverviewTeam2Collection.Add(matchPlayerBase);
            }

            if (playersList.First().IsWinner)
            {
                Team1OverviewHeader = "TEAM 1 (WINNER)";
                Team2OverviewHeader = "TEAM 2";
            }
            else
            {
                Team1OverviewHeader = "TEAM 1";
                Team2OverviewHeader = "TEAM 2 (WINNER)";
            }
        }

        private void ClearSearch()
        {
            SelectedSeasonOption = SeasonList[0];
            SelectedReplayIdValue = 0;
            SelectedMapOption = MapsList[0];
            SelectedBuildOption = ReplayBuildsList[0];
            SelectedGameTimeOption = FilterGameTimeOption.Any.GetFriendlyName();
            SelectedGameDateOption = FilterGameDateOption.Any.GetFriendlyName();
            SelectedPlayerBattleTag = string.Empty;
            SelectedCharacter = HeroesList[0];
            SelectedParty = PartyCountList[0];
            SelectedAccountLevel = 0;

            IsGivenBattleTagOnlyChecked = false;
            IsPartyGivenBattleTagOnlyChecked = false;
            IsPartyHeroOnlyChecked = false;
            IsAccountyGivenBattleTagOnlyChecked = false;
            IsAccountHeroOnlyChecked = false;
        }

        private void ClearMatchOverview()
        {
            foreach (var player in MatchOverviewTeam1Collection)
                player.Dispose();

            foreach (var player in MatchOverviewTeam2Collection)
                player.Dispose();

            MatchOverviewTeam1Collection.Clear();
            MatchOverviewTeam2Collection.Clear();
        }

        private async Task ReceivedMessageAsync(NotificationMessage message)
        {
            if (message.Notification == StaticMessage.ReEnableMatchSummaryButton)
                ShowMatchSummaryButtonEnabled = true;

            if (CurrentTab == MatchesTabService.GetCurrentlySelectedTab() && (message.Notification == StaticMessage.ChangeCurrentSelectedReplayMatchLeft || message.Notification == StaticMessage.ChangeCurrentSelectedReplayMatchRight))
            {
                if (MatchListCollection.Count < 1 || SelectedReplay == null)
                    return;

                int index = MatchListCollection.ToList().FindIndex(x => x.ReplayId == SelectedReplay.ReplayId);

                if (message.Notification == StaticMessage.ChangeCurrentSelectedReplayMatchLeft)
                    SelectedReplay = MatchListCollection[index - 1];
                else if (message.Notification == StaticMessage.ChangeCurrentSelectedReplayMatchRight)
                    SelectedReplay = MatchListCollection[index + 1];

                await ShowMatchSummaryAsync();
            }
        }

        private async Task ShowMatchSummaryAsync()
        {
            if (SelectedReplay == null)
                return;

            ShowMatchSummaryButtonEnabled = false;
            await MatchSummaryReplay.LoadMatchSummaryAsync(SelectedReplay, MatchListCollection.ToList());

            MatchSummaryFlyout.SetMatchSummaryHeader($"Match Summary [Id:{SelectedReplay.ReplayId}] [Build:{SelectedReplay.ReplayBuild}]");
            MatchSummaryFlyout.OpenMatchSummaryFlyout();
        }
    }
}
