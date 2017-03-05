using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using Heroes.Helpers;
using Heroes.Icons;
using Heroes.ReplayParser;
using HeroesStatTracker.Core.Messaging;
using HeroesStatTracker.Core.Models.MatchModels;
using HeroesStatTracker.Core.User;
using HeroesStatTracker.Core.ViewServices;
using HeroesStatTracker.Data;
using HeroesStatTracker.Data.Models.Replays;
using HeroesStatTracker.Data.Queries.Replays;
using Microsoft.Practices.ServiceLocation;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace HeroesStatTracker.Core.ViewModels.Matches
{
    public abstract class MatchesBase : HstViewModel
    {
        private bool _isGivenBattleTagOnlyChecked;
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
        private ReplayMatch _selectedReplay;

        private IDatabaseService Database;
        private IUserProfileService UserProfile;

        private ObservableCollection<ReplayMatch> _matchListCollection = new ObservableCollection<ReplayMatch>();
        private ObservableCollection<MatchPlayerBase> _matchOverviewTeam1Collection = new ObservableCollection<MatchPlayerBase>();
        private ObservableCollection<MatchPlayerBase> _matchOverviewTeam2Collection = new ObservableCollection<MatchPlayerBase>();

        private GameMode MatchGameMode;

        public MatchesBase(IDatabaseService database, IHeroesIconsService heroesIcons, IUserProfileService userProfile, GameMode matchGameMode)
            : base(heroesIcons)
        {
            Database = database;
            UserProfile = userProfile;

            MatchGameMode = matchGameMode;

            ShowMatchSummaryButtonEnabled = true;

            SeasonList.Add("Lifetime");
            SeasonList.AddRange(HeroesHelpers.Seasons.GetSeasonList());
            SelectedSeasonOption = SeasonList[0];

            GameTimeList = HeroesHelpers.GameDates.GameTimeList;
            SelectedGameTimeOption = GameTimeList[0];

            GameDateList = HeroesHelpers.GameDates.GameDateList;
            SelectedGameDateOption = GameDateList[0];

            MapsList.Add("Any");
            MapsList.AddRange(HeroesIcons.MapBackgrounds().GetMapsList());
            SelectedMapOption = MapsList[0];

            ReplayBuildsList.Add("Any");
            ReplayBuildsList.AddRange(HeroesHelpers.Builds.GetBuildsList(HeroesIcons));
            SelectedBuildOption = ReplayBuildsList[0];

            HeroesList.Add("Any");
            HeroesList.AddRange(HeroesIcons.Heroes().GetListOfHeroes());
            SelectedCharacter = HeroesList[0];

            Messenger.Default.Register<MatchesDataMessage>(this, (message) => ReceivedMatchSearchData(message));
            Messenger.Default.Register<NotificationMessage>(this, (message) => ReceivedMessage(message));
        }

        public List<string> SeasonList { get; private set; } = new List<string>();
        public List<string> MapsList { get; private set; } = new List<string>();
        public List<string> ReplayBuildsList { get; private set; } = new List<string>();
        public List<string> GameTimeList { get; private set; } = new List<string>();
        public List<string> GameDateList { get; private set; } = new List<string>();
        public List<string> HeroesList { get; private set; } = new List<string>();

        public bool IsGivenBattleTagOnlyChecked
        {
            get { return _isGivenBattleTagOnlyChecked; }
            set
            {
                _isGivenBattleTagOnlyChecked = value;
                RaisePropertyChanged();
            }
        }

        public bool ShowMatchSummaryButtonEnabled
        {
            get { return _showMatchSummaryButtonEnabled; }
            set
            {
                _showMatchSummaryButtonEnabled = value;
                RaisePropertyChanged();
            }
        }

        public long SelectedReplayIdValue
        {
            get { return _selectedReplayIdValue; }
            set
            {
                _selectedReplayIdValue = value;
                RaisePropertyChanged();
            }
        }

        public string SelectedSeasonOption
        {
            get { return _selectedSeasonOption; }
            set
            {
                _selectedSeasonOption = value;
                RaisePropertyChanged();
            }
        }

        public string SelectedMapOption
        {
            get { return _selectedMapOption; }
            set
            {
                _selectedMapOption = value;
                RaisePropertyChanged();
            }
        }

        public string SelectedBuildOption
        {
            get { return _selectedBuildOption; }
            set
            {
                _selectedBuildOption = value;
                RaisePropertyChanged();
            }
        }

        public string SelectedGameTimeOption
        {
            get { return _selectedGameTimeOption; }
            set
            {
                _selectedGameTimeOption = value;
                RaisePropertyChanged();
            }
        }

        public string SelectedGameDateOption
        {
            get { return _selectedGameDateOption; }
            set
            {
                _selectedGameDateOption = value;
                RaisePropertyChanged();
            }
        }

        public string SelectedPlayerBattleTag
        {
            get { return _selectedPlayerBattleTag; }
            set
            {
                _selectedPlayerBattleTag = value;
                RaisePropertyChanged();
            }
        }

        public string SelectedCharacter
        {
            get { return _selectedCharacter; }
            set
            {
                _selectedCharacter = value;
                RaisePropertyChanged();
            }
        }

        public string Team1OverviewHeader
        {
            get { return _team1OverviewHeader; }
            set
            {
                _team1OverviewHeader = value;
                RaisePropertyChanged();
            }
        }

        public string Team2OverviewHeader
        {
            get { return _team2OverviewHeader; }
            set
            {
                _team2OverviewHeader = value;
                RaisePropertyChanged();
            }
        }

        public ReplayMatch SelectedReplay
        {
            get { return _selectedReplay; }
            set
            {
                _selectedReplay = value;
                RaisePropertyChanged();
            }
        }

        public ObservableCollection<ReplayMatch> MatchListCollection
        {
            get { return _matchListCollection; }
            set
            {
                _matchListCollection = value;
                RaisePropertyChanged();
            }
        }

        public ObservableCollection<MatchPlayerBase> MatchOverviewTeam1Collection
        {
            get { return _matchOverviewTeam1Collection; }
            set
            {
                _matchOverviewTeam1Collection = value;
                RaisePropertyChanged();
            }
        }

        public ObservableCollection<MatchPlayerBase> MatchOverviewTeam2Collection
        {
            get { return _matchOverviewTeam2Collection; }
            set
            {
                _matchOverviewTeam2Collection = value;
                RaisePropertyChanged();
            }
        }

        public IMatchSummaryFlyoutService MatchSummaryFlyout
        {
            get { return ServiceLocator.Current.GetInstance<IMatchSummaryFlyoutService>(); }
        }

        public IMatchSummaryReplayService MatchSummaryReplay
        {
            get { return ServiceLocator.Current.GetInstance<IMatchSummaryReplayService>(); }
        }

        public RelayCommand ClearSearchCommand => new RelayCommand(ClearSearch);
        public RelayCommand LoadMatchListCommand => new RelayCommand(LoadMatchList);
        public RelayCommand ShowMatchOverviewCommand => new RelayCommand(LoadMatchOverview);
        public RelayCommand ShowMatchSummaryCommand => new RelayCommand(ShowMatchSummary);

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
                SelectedBattleTag = SelectedPlayerBattleTag,
                SelectedBuildOption = SelectedBuildOption,
                SelectedCharacter = SelectedCharacter,
                SelectedGameDateOption = SelectedGameDateOption,
                SelectedGameTimeOption = SelectedGameTimeOption,
                SelectedMapOption = SelectedMapOption,
                SelectedReplayId = SelectedReplayIdValue,
                SelectedSeason = HeroesHelpers.EnumParser.ConvertSeasonStringToEnum(SelectedSeasonOption),
                BuildOptionsList = ReplayBuildsList,
                GameDateOptionList = GameDateList,
                GameTimeOptionList = GameTimeList,
                HeroesList = HeroesList,
                MapOptionsList = MapsList,
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

            // load up correct build information
            HeroesIcons.LoadHeroesBuild(replayMatch.ReplayBuild);

            var playerParties = PlayerParties.FindPlayerParties(playersList);

            foreach (var player in playersList)
            {
                if (player.Team == 4)
                    continue;

                MatchPlayerBase matchPlayerBase = new MatchPlayerBase(Database, HeroesIcons, UserProfile, player);
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
            SelectedGameTimeOption = GameTimeList[0];
            SelectedGameDateOption = GameDateList[0];
            SelectedPlayerBattleTag = string.Empty;
            SelectedCharacter = HeroesList[0];
            IsGivenBattleTagOnlyChecked = false;
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

        private void ShowMatchSummary()
        {
            if (SelectedReplay == null)
                return;

            ShowMatchSummaryButtonEnabled = false;
            MatchSummaryReplay.LoadMatchSummary(SelectedReplay, MatchListCollection.ToList());

            MatchSummaryFlyout.SetMatchSummaryHeader($"Match Summary [Id:{SelectedReplay.ReplayId}] [Build:{SelectedReplay.ReplayBuild}]");
            MatchSummaryFlyout.OpenMatchSummaryFlyout();
        }

        private void ReceivedMessage(NotificationMessage message)
        {
            if (message.Notification == StaticMessage.ReEnableMatchSummaryButton)
                ShowMatchSummaryButtonEnabled = true;

            if (message.Notification == StaticMessage.ChangeCurrentSelectedReplayMatchLeft || message.Notification == StaticMessage.ChangeCurrentSelectedReplayMatchRight)
            {
                int index = MatchListCollection.ToList().FindIndex(x => x.ReplayId == SelectedReplay.ReplayId);

                if (message.Notification == StaticMessage.ChangeCurrentSelectedReplayMatchLeft)
                    SelectedReplay = MatchListCollection[index - 1];
                else if (message.Notification == StaticMessage.ChangeCurrentSelectedReplayMatchRight)
                    SelectedReplay = MatchListCollection[index + 1];

                ShowMatchSummary();
            }
        }
    }
}
