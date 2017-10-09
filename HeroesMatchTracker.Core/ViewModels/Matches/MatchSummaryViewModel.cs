using GalaSoft.MvvmLight.CommandWpf;
using GalaSoft.MvvmLight.Ioc;
using GalaSoft.MvvmLight.Messaging;
using Heroes.Helpers;
using Heroes.Icons.Models;
using HeroesMatchTracker.Core.Messaging;
using HeroesMatchTracker.Core.Models.GraphSummaryModels;
using HeroesMatchTracker.Core.Models.MatchModels;
using HeroesMatchTracker.Core.Services;
using HeroesMatchTracker.Core.ViewServices;
using HeroesMatchTracker.Data.Models.Replays;
using Microsoft.Practices.ServiceLocation;
using NLog;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace HeroesMatchTracker.Core.ViewModels.Matches
{
    public class MatchSummaryViewModel : HmtViewModel, IMatchSummaryReplayService
    {
        private int? _teamBlueKills;
        private int? _teamRedKills;
        private int? _teamBlueLevel;
        private int? _teamRedLevel;
        private bool _isLeftChangeButtonVisible;
        private bool _isRightChangeButtonVisible;
        private bool _isLeftChangeButtonEnabled;
        private bool _isRightChangeButtonEnabled;
        private bool _hasBans;
        private bool _hasObservers;
        private bool _hasChat;
        private bool _isFlyoutLoadingOverlayVisible;
        private string _teamBlueIsWinner;
        private string _teamRedIsWinner;
        private string _matchTitle;
        private string _teamBlueName;
        private string _teamRedName;
        private string _matchLength;
        private Color _matchTitleGlowColor;
        private Stream _leftArrowNormalIcon;
        private Stream _leftArrowHoverIcon;
        private Stream _leftArrowDownIcon;
        private Stream _leftArrowDisabledIcon;
        private Stream _rightArrowNormalIcon;
        private Stream _rightArrowHoverIcon;
        private Stream _rightArrowDownIcon;
        private Stream _rightArrowDisabledIcon;

        private IWebsiteService Website;
        private ILoadingOverlayWindowService LoadingOverlayWindow;
        private List<MatchPlayerTalents> MatchPlayerTalentsTeam1List;
        private List<MatchPlayerTalents> MatchPlayerTalentsTeam2List;
        private List<MatchPlayerStats> MatchPlayerStatsTeam1List;
        private List<MatchPlayerStats> MatchPlayerStatsTeam2List;
        private List<MatchPlayerAdvancedStats> MatchPlayerAdvancedStatsTeam1List;
        private List<MatchPlayerAdvancedStats> MatchPlayerAdvancedStatsTeam2List;
        private List<MatchChat> MatchPlayerChatList;
        private List<MatchObserver> MatchPlayerObserversList;

        private ObservableCollection<MatchPlayerTalents> _matchTalentsTeam1Collection = new ObservableCollection<MatchPlayerTalents>();
        private ObservableCollection<MatchPlayerTalents> _matchTalentsTeam2Collection = new ObservableCollection<MatchPlayerTalents>();
        private ObservableCollection<MatchPlayerStats> _matchStatsTeam1Collection = new ObservableCollection<MatchPlayerStats>();
        private ObservableCollection<MatchPlayerStats> _matchStatsTeam2Collection = new ObservableCollection<MatchPlayerStats>();
        private ObservableCollection<MatchPlayerAdvancedStats> _matchAdvancedStatsTeam1Collection = new ObservableCollection<MatchPlayerAdvancedStats>();
        private ObservableCollection<MatchPlayerAdvancedStats> _matchAdvancedStatsTeam2Collection = new ObservableCollection<MatchPlayerAdvancedStats>();
        private ObservableCollection<MatchChat> _matchChatCollection = new ObservableCollection<MatchChat>();
        private ObservableCollection<MatchObserver> _matchObserversCollection = new ObservableCollection<MatchObserver>();

        public MatchSummaryViewModel(IInternalService internalService, IWebsiteService website, ILoadingOverlayWindowService loadingOverlayWindow)
            : base(internalService)
        {
            Website = website;
            LoadingOverlayWindow = loadingOverlayWindow;

            IsFlyoutLoadingOverlayVisible = false;
            IsLeftChangeButtonVisible = true;
            IsRightChangeButtonVisible = true;
            IsLeftChangeButtonEnabled = false;
            IsRightChangeButtonEnabled = false;

            ScoreKillIcon = HeroesIcons.GetOtherIcon(OtherIcon.Kills);
            ScoreAssistIcon = HeroesIcons.GetOtherIcon(OtherIcon.Assist);
            ScoreDeathIcon = HeroesIcons.GetOtherIcon(OtherIcon.Death);
            BlueKillsIcons = HeroesIcons.GetOtherIcon(OtherIcon.KillsBlue);
            RedKillsIcons = HeroesIcons.GetOtherIcon(OtherIcon.KillsRed);

            LeftArrowDisabledIcon = HeroesIcons.GetOtherIcon(OtherIcon.LongarrowLeftDisabled);
            LeftArrowDownIcon = HeroesIcons.GetOtherIcon(OtherIcon.LongarrowLeftDown);
            LeftArrowHoverIcon = HeroesIcons.GetOtherIcon(OtherIcon.LongarrowLeftHover);
            LeftArrowNormalIcon = HeroesIcons.GetOtherIcon(OtherIcon.LongarrowLeftNormal);
            RightArrowDisabledIcon = HeroesIcons.GetOtherIcon(OtherIcon.LongarrowRightDisabled);
            RightArrowDownIcon = HeroesIcons.GetOtherIcon(OtherIcon.LongarrowRightDown);
            RightArrowHoverIcon = HeroesIcons.GetOtherIcon(OtherIcon.LongarrowRightHover);
            RightArrowNormalIcon = HeroesIcons.GetOtherIcon(OtherIcon.LongarrowRightNormal);

            HasBans = false;
            HasObservers = false;
            HasChat = false;

            TeamLevelTimeGraph = new TeamLevelTimeGraph();
            TeamExperienceGraph = new TeamExperienceGraph(Database);
            StatGraphs = new StatGraphs(Database);

            Messenger.Default.Register<NotificationMessage>(this, (message) => ReceivedMessage(message));

            SimpleIoc.Default.Register<IMatchSummaryReplayService>(() => this);
        }

        public IMatchSummaryFlyoutService MatchSummaryFlyout => ServiceLocator.Current.GetInstance<IMatchSummaryFlyoutService>();

        public MatchBans MatchHeroBans { get; private set; } = new MatchBans();

        public TeamLevelTimeGraph TeamLevelTimeGraph { get; private set; }
        public TeamExperienceGraph TeamExperienceGraph { get; private set; }
        public StatGraphs StatGraphs { get; private set; }
        public Stream ScoreKillIcon { get; private set; }
        public Stream ScoreAssistIcon { get; private set; }
        public Stream ScoreDeathIcon { get; private set; }
        public Stream BlueKillsIcons { get; private set; }
        public Stream RedKillsIcons { get; private set; }

        public int? TeamBlueKills
        {
            get => _teamBlueKills;
            set
            {
                _teamBlueKills = value;
                RaisePropertyChanged();
            }
        }

        public int? TeamRedKills
        {
            get => _teamRedKills;
            set
            {
                _teamRedKills = value;
                RaisePropertyChanged();
            }
        }

        public int? TeamBlueLevel
        {
            get => _teamBlueLevel;
            set
            {
                _teamBlueLevel = value;
                RaisePropertyChanged();
            }
        }

        public int? TeamRedLevel
        {
            get => _teamRedLevel;
            set
            {
                _teamRedLevel = value;
                RaisePropertyChanged();
            }
        }

        public bool IsLeftChangeButtonVisible
        {
            get => _isLeftChangeButtonVisible;
            set
            {
                _isLeftChangeButtonVisible = value;
                RaisePropertyChanged();
            }
        }

        public bool IsRightChangeButtonVisible
        {
            get => _isRightChangeButtonVisible;
            set
            {
                _isRightChangeButtonVisible = value;
                RaisePropertyChanged();
            }
        }

        public bool IsLeftChangeButtonEnabled
        {
            get => _isLeftChangeButtonEnabled;
            set
            {
                _isLeftChangeButtonEnabled = value;
                RaisePropertyChanged();
            }
        }

        public bool IsRightChangeButtonEnabled
        {
            get => _isRightChangeButtonEnabled;
            set
            {
                _isRightChangeButtonEnabled = value;
                RaisePropertyChanged();
            }
        }

        public bool HasBans
        {
            get => _hasBans;
            set
            {
                _hasBans = value;
                RaisePropertyChanged();
            }
        }

        public bool HasObservers
        {
            get => _hasObservers;
            set
            {
                _hasObservers = value;
                RaisePropertyChanged();
            }
        }

        public bool HasChat
        {
            get => _hasChat;
            set
            {
                _hasChat = value;
                RaisePropertyChanged();
            }
        }

        public bool IsFlyoutLoadingOverlayVisible
        {
            get => _isFlyoutLoadingOverlayVisible;
            set
            {
                _isFlyoutLoadingOverlayVisible = value;
                RaisePropertyChanged();
            }
        }

        public string MatchTitle
        {
            get => _matchTitle;
            set
            {
                _matchTitle = value;
                RaisePropertyChanged();
            }
        }

        public string TeamBlueName
        {
            get => _teamBlueName;
            set
            {
                _teamBlueName = value;
                RaisePropertyChanged();
            }
        }

        public string TeamRedName
        {
            get => _teamRedName;
            set
            {
                _teamRedName = value;
                RaisePropertyChanged();
            }
        }

        public string TeamBlueIsWinner
        {
            get => _teamBlueIsWinner;
            set
            {
                _teamBlueIsWinner = value;
                RaisePropertyChanged();
            }
        }

        public string TeamRedIsWinner
        {
            get => _teamRedIsWinner;
            set
            {
                _teamRedIsWinner = value;
                RaisePropertyChanged();
            }
        }

        public string MatchLength
        {
            get => _matchLength;
            set
            {
                _matchLength = value;
                RaisePropertyChanged();
            }
        }

        public Color MatchTitleGlowColor
        {
            get => _matchTitleGlowColor;
            set
            {
                _matchTitleGlowColor = value;
                RaisePropertyChanged();
            }
        }

        public Stream LeftArrowNormalIcon
        {
            get => _leftArrowNormalIcon;
            set
            {
                _leftArrowNormalIcon = value;
                RaisePropertyChanged();
            }
        }

        public Stream LeftArrowHoverIcon
        {
            get => _leftArrowHoverIcon;
            set
            {
                _leftArrowHoverIcon = value;
                RaisePropertyChanged();
            }
        }

        public Stream LeftArrowDownIcon
        {
            get => _leftArrowDownIcon;
            set
            {
                _leftArrowDownIcon = value;
                RaisePropertyChanged();
            }
        }

        public Stream LeftArrowDisabledIcon
        {
            get => _leftArrowDisabledIcon;
            set
            {
                _leftArrowDisabledIcon = value;
                RaisePropertyChanged();
            }
        }

        public Stream RightArrowNormalIcon
        {
            get => _rightArrowNormalIcon;
            set
            {
                _rightArrowNormalIcon = value;
                RaisePropertyChanged();
            }
        }

        public Stream RightArrowHoverIcon
        {
            get => _rightArrowHoverIcon;
            set
            {
                _rightArrowHoverIcon = value;
                RaisePropertyChanged();
            }
        }

        public Stream RightArrowDownIcon
        {
            get => _rightArrowDownIcon;
            set
            {
                _rightArrowDownIcon = value;
                RaisePropertyChanged();
            }
        }

        public Stream RightArrowDisabledIcon
        {
            get => _rightArrowDisabledIcon;
            set
            {
                _rightArrowDisabledIcon = value;
                RaisePropertyChanged();
            }
        }

        public ObservableCollection<MatchPlayerTalents> MatchTalentsTeam1Collection
        {
            get => _matchTalentsTeam1Collection;
            set
            {
                _matchTalentsTeam1Collection = value;
                RaisePropertyChanged();
            }
        }

        public ObservableCollection<MatchPlayerTalents> MatchTalentsTeam2Collection
        {
            get => _matchTalentsTeam2Collection;
            set
            {
                _matchTalentsTeam2Collection = value;
                RaisePropertyChanged();
            }
        }

        public ObservableCollection<MatchPlayerStats> MatchStatsTeam1Collection
        {
            get => _matchStatsTeam1Collection;
            set
            {
                _matchStatsTeam1Collection = value;
                RaisePropertyChanged();
            }
        }

        public ObservableCollection<MatchPlayerStats> MatchStatsTeam2Collection
        {
            get => _matchStatsTeam2Collection;
            set
            {
                _matchStatsTeam2Collection = value;
                RaisePropertyChanged();
            }
        }

        public ObservableCollection<MatchPlayerAdvancedStats> MatchAdvancedStatsTeam1Collection
        {
            get => _matchAdvancedStatsTeam1Collection;
            set
            {
                _matchAdvancedStatsTeam1Collection = value;
                RaisePropertyChanged();
            }
        }

        public ObservableCollection<MatchPlayerAdvancedStats> MatchAdvancedStatsTeam2Collection
        {
            get => _matchAdvancedStatsTeam2Collection;
            set
            {
                _matchAdvancedStatsTeam2Collection = value;
                RaisePropertyChanged();
            }
        }

        public ObservableCollection<MatchChat> MatchChatCollection
        {
            get => _matchChatCollection;
            set
            {
                _matchChatCollection = value;
                RaisePropertyChanged();
            }
        }

        public ObservableCollection<MatchObserver> MatchObserversCollection
        {
            get => _matchObserversCollection;

            set
            {
                _matchObserversCollection = value;
                RaisePropertyChanged();
            }
        }

        public RelayCommand MatchSummaryLeftChangeButtonCommand => new RelayCommand(async () => await ChangeCurrentMatchSummaryAsync(-1));
        public RelayCommand MatchSummaryRightChangeButtonCommand => new RelayCommand(async () => await ChangeCurrentMatchSummaryAsync(1));
        public RelayCommand KeyEscCommand => new RelayCommand(KeyEscPressed);

        public async Task LoadMatchSummaryAsync(ReplayMatch replayMatch, List<ReplayMatch> matchList)
        {
            if (replayMatch == null)
                return;

            await Task.Run(async () =>
            {
                try
                {
                    LoadingOverlayWindow.ShowLoadingOverlay();
                    await LoadMatchSummaryDataAsync(replayMatch);

                    if (matchList == null)
                    {
                        IsLeftChangeButtonEnabled = false;
                        IsLeftChangeButtonVisible = false;
                        IsRightChangeButtonEnabled = false;
                        IsRightChangeButtonVisible = false;
                    }
                    else if (matchList.Count <= 0)
                    {
                        IsLeftChangeButtonEnabled = false;
                        IsLeftChangeButtonVisible = true;
                        IsRightChangeButtonEnabled = false;
                        IsRightChangeButtonVisible = true;
                    }
                    else
                    {
                        IsLeftChangeButtonVisible = true;
                        IsLeftChangeButtonEnabled = replayMatch.ReplayId == matchList[0].ReplayId ? false : true;

                        IsRightChangeButtonVisible = true;
                        IsRightChangeButtonEnabled = replayMatch.ReplayId == matchList[matchList.Count - 1].ReplayId ? false : true;
                    }
                }
                catch (Exception ex)
                {
                    ExceptionLog.Log(LogLevel.Error, ex);
                    throw;
                }
            });

            IsFlyoutLoadingOverlayVisible = false;
            LoadingOverlayWindow.CloseLoadingOverlay();
        }

        private async Task LoadMatchSummaryDataAsync(ReplayMatch replayMatch)
        {
            DisposeMatchSummary();

            replayMatch = Database.ReplaysDb().MatchReplay.ReadReplayIncludeAssociatedRecords(replayMatch.ReplayId);

            HeroesIcons.LoadHeroesBuild(replayMatch.ReplayBuild);
            SetBackgroundImage(replayMatch.MapName);
            MatchTitleGlowColor = HeroesIcons.MapBackgrounds().GetMapBackgroundFontGlowColor(replayMatch.MapName);
            MatchTitle = $"{replayMatch.MapName} - {replayMatch.GameMode.GetFriendlyName()} [{replayMatch.TimeStamp}] [{replayMatch.ReplayLength}]";
            MatchLength = $"{replayMatch.ReplayLength.ToString(@"mm\:ss")}";

            // get players info
            var playersList = replayMatch.ReplayMatchPlayers.ToList();
            var playerTalentsList = replayMatch.ReplayMatchPlayerTalents.ToList();
            var playerScoresList = replayMatch.ReplayMatchPlayerScoreResults.ToList();
            var matchMessagesList = replayMatch.ReplayMatchMessage.ToList();
            var matchAwardDictionary = replayMatch.ReplayMatchAward.ToDictionary(x => x.PlayerId, x => x.Award);
            var matchTeamLevelsList = replayMatch.ReplayMatchTeamLevels.ToList();
            var matchTeamExperienceList = replayMatch.ReplayMatchTeamExperiences.ToList();

            var playerParties = PlayerParties.FindPlayerParties(playersList);
            var playerHeroes = CreateListOfCharacterHeroes(playersList);

            foreach (var player in playersList)
            {
                MatchPlayerBase matchPlayerBase;
                MatchPlayerTalents matchPlayerTalents;
                MatchPlayerStats matchPlayerStats;
                MatchPlayerAdvancedStats matchPlayerAdvancedStats;

                matchPlayerBase = new MatchPlayerBase(InternalService, Website, player);
                matchPlayerBase.SetPlayerInfo(player.IsAutoSelect, playerParties, matchAwardDictionary);

                if (player.Character != "None")
                {
                    matchPlayerTalents = new MatchPlayerTalents(matchPlayerBase);
                    matchPlayerTalents.SetTalents(playerTalentsList[player.PlayerNumber]);

                    matchPlayerStats = new MatchPlayerStats(matchPlayerBase);
                    matchPlayerStats.SetStats(playerScoresList[player.PlayerNumber], player);

                    matchPlayerAdvancedStats = new MatchPlayerAdvancedStats(matchPlayerStats);
                    matchPlayerAdvancedStats.SetAdvancedStats(playerScoresList[player.PlayerNumber]);

                    if (player.Team == 0 || player.Team == 1)
                    {
                        if (player.Team == 0)
                        {
                            MatchPlayerTalentsTeam1List.Add(matchPlayerTalents);
                            MatchPlayerStatsTeam1List.Add(matchPlayerStats);
                            MatchPlayerAdvancedStatsTeam1List.Add(matchPlayerAdvancedStats);
                        }
                        else
                        {
                            MatchPlayerTalentsTeam2List.Add(matchPlayerTalents);
                            MatchPlayerStatsTeam2List.Add(matchPlayerStats);
                            MatchPlayerAdvancedStatsTeam2List.Add(matchPlayerAdvancedStats);
                        }
                    }
                }

                if (player.Team == 4)
                    MatchPlayerObserversList.Add(new MatchObserver(matchPlayerBase));
            }

            if (MatchPlayerObserversList.Count > 0)
                HasObservers = true;

            // set the highest stat values
            SetHighestTeamStatValues();
            SetHighestTeamAdvancedStatValues();

            // match bans
            if (replayMatch.ReplayMatchTeamBan != null)
            {
                string ban1 = HeroesIcons.HeroBuilds().GetRealHeroNameFromAttributeId(replayMatch.ReplayMatchTeamBan.Team0Ban0);
                string ban2 = HeroesIcons.HeroBuilds().GetRealHeroNameFromAttributeId(replayMatch.ReplayMatchTeamBan.Team0Ban1);
                string ban3 = HeroesIcons.HeroBuilds().GetRealHeroNameFromAttributeId(replayMatch.ReplayMatchTeamBan.Team1Ban0);
                string ban4 = HeroesIcons.HeroBuilds().GetRealHeroNameFromAttributeId(replayMatch.ReplayMatchTeamBan.Team1Ban1);

                if (string.IsNullOrEmpty(ban1)) ban1 = null;
                if (string.IsNullOrEmpty(ban2)) ban2 = null;
                if (string.IsNullOrEmpty(ban3)) ban3 = null;
                if (string.IsNullOrEmpty(ban4)) ban4 = null;

                var bannedHero1 = HeroesIcons.HeroBuilds().GetHeroInfo(ban1);
                var bannedHero2 = HeroesIcons.HeroBuilds().GetHeroInfo(ban2);
                var bannedHero3 = HeroesIcons.HeroBuilds().GetHeroInfo(ban3);
                var bannedHero4 = HeroesIcons.HeroBuilds().GetHeroInfo(ban4);

                await Application.Current.Dispatcher.InvokeAsync(
                    () =>
                {
                    MatchHeroBans.Team0Ban0 = bannedHero1.GetHeroPortrait();
                    MatchHeroBans.Team0Ban1 = bannedHero2.GetHeroPortrait();
                    MatchHeroBans.Team1Ban0 = bannedHero3.GetHeroPortrait();
                    MatchHeroBans.Team1Ban1 = bannedHero4.GetHeroPortrait();
                });

                MatchHeroBans.Team0Ban0HeroName = $"{ban1}{Environment.NewLine}{bannedHero1.Roles.FirstOrDefault()}";
                MatchHeroBans.Team0Ban1HeroName = $"{ban2}{Environment.NewLine}{bannedHero2.Roles.FirstOrDefault()}";
                MatchHeroBans.Team1Ban0HeroName = $"{ban3}{Environment.NewLine}{bannedHero3.Roles.FirstOrDefault()}";
                MatchHeroBans.Team1Ban1HeroName = $"{ban4}{Environment.NewLine}{bannedHero4.Roles.FirstOrDefault()}";

                HasBans = true;
            }

            // match chat
            if (matchMessagesList != null && matchMessagesList.Count > 0)
            {
                foreach (var message in matchMessagesList)
                {
                    if (message.MessageEventType == "SChatMessage")
                    {
                        MatchChat matchChat = new MatchChat();
                        matchChat.SetChatMessages(message);

                        MatchPlayerChatList.Add(matchChat);
                    }
                }

                if (MatchPlayerChatList.Count > 0)
                    HasChat = true;
            }

            // Set the match results: total kills, team levels, game time
            MatchResult matchResult = new MatchResult(Database);
            matchResult.SetResult(MatchPlayerStatsTeam1List.ToList(), MatchPlayerStatsTeam2List.ToList(), matchTeamLevelsList.ToList(), playersList.ToList());
            SetMatchResults(matchResult);

            // graphs
            await TeamLevelTimeGraph.SetTeamLevelGraphsAsync(matchTeamLevelsList, playersList[0].IsWinner);
            await TeamExperienceGraph.SetTeamExperienceGraphsAsync(matchTeamExperienceList, playersList[0].IsWinner);
            await StatGraphs.SetStatGraphsAsync(playerHeroes, playerScoresList);

            // add to collections
            await Application.Current.Dispatcher.InvokeAsync(
                () =>
            {
                MatchTalentsTeam1Collection = new ObservableCollection<MatchPlayerTalents>(MatchPlayerTalentsTeam1List);
                MatchTalentsTeam2Collection = new ObservableCollection<MatchPlayerTalents>(MatchPlayerTalentsTeam2List);
                MatchStatsTeam1Collection = new ObservableCollection<MatchPlayerStats>(MatchPlayerStatsTeam1List);
                MatchStatsTeam2Collection = new ObservableCollection<MatchPlayerStats>(MatchPlayerStatsTeam2List);
                MatchAdvancedStatsTeam1Collection = new ObservableCollection<MatchPlayerAdvancedStats>(MatchPlayerAdvancedStatsTeam1List);
                MatchAdvancedStatsTeam2Collection = new ObservableCollection<MatchPlayerAdvancedStats>(MatchPlayerAdvancedStatsTeam2List);
                MatchChatCollection = new ObservableCollection<MatchChat>(MatchPlayerChatList);
                MatchObserversCollection = new ObservableCollection<MatchObserver>(MatchPlayerObserversList);
            });
        }

        private void SetMatchResults(MatchResult matchResult)
        {
            TeamBlueKills = matchResult.TeamBlueKills;
            TeamRedKills = matchResult.TeamRedKills;

            TeamBlueLevel = matchResult.TeamBlueLevel;
            TeamRedLevel = matchResult.TeamRedLevel;

            TeamBlueName = matchResult.TeamBlue;
            TeamRedName = matchResult.TeamRed;

            TeamBlueIsWinner = matchResult.TeamBlueIsWinner;
            TeamRedIsWinner = matchResult.TeamRedIsWinner;
        }

        private void SetHighestTeamStatValues()
        {
            int? highestSiege1 = MatchPlayerStatsTeam1List.Max(x => x.SiegeDamage);
            int? highestSiege2 = MatchPlayerStatsTeam2List.Max(x => x.SiegeDamage);

            int? highestHero1 = MatchPlayerStatsTeam1List.Max(x => x.HeroDamage);
            int? highestHero2 = MatchPlayerStatsTeam2List.Max(x => x.HeroDamage);

            int? highestExp1 = MatchPlayerStatsTeam1List.Max(x => x.ExperienceContribution);
            int? highestExp2 = MatchPlayerStatsTeam2List.Max(x => x.ExperienceContribution);

            int? highestDamageTaken1 = MatchPlayerStatsTeam1List.Max(x => x.DamageTaken);
            int? highestDamageTaken2 = MatchPlayerStatsTeam2List.Max(x => x.DamageTaken);

            int? highestHealing1 = MatchPlayerStatsTeam1List.Max(x => x.HealingRole);
            int? highestHealing2 = MatchPlayerStatsTeam2List.Max(x => x.HealingRole);

            foreach (var item in MatchPlayerStatsTeam1List)
            {
                if (item.SiegeDamage == highestSiege1)
                    item.HighestSiegeDamage = true;

                if (item.HeroDamage == highestHero1)
                    item.HighestHeroDamage = true;

                if (item.ExperienceContribution == highestExp1)
                    item.HighestExperience = true;

                if (item.DamageTaken == highestDamageTaken1)
                    item.HighestDamageTaken = true;

                if (item.HealingRole == highestHealing1)
                    item.HighestHealing = true;
            }

            foreach (var item in MatchPlayerStatsTeam2List)
            {
                if (item.SiegeDamage == highestSiege2)
                    item.HighestSiegeDamage = true;

                if (item.HeroDamage == highestHero2)
                    item.HighestHeroDamage = true;

                if (item.ExperienceContribution == highestExp2)
                    item.HighestExperience = true;

                if (item.DamageTaken == highestDamageTaken2)
                    item.HighestDamageTaken = true;

                if (item.HealingRole == highestHealing2)
                    item.HighestHealing = true;
            }
        }

        private void SetHighestTeamAdvancedStatValues()
        {
            int? highestSiege1 = MatchPlayerAdvancedStatsTeam1List.Max(x => x.SiegeDamage);
            int? highestSiege2 = MatchPlayerAdvancedStatsTeam2List.Max(x => x.SiegeDamage);

            int? highestHero1 = MatchPlayerAdvancedStatsTeam1List.Max(x => x.HeroDamage);
            int? highestHero2 = MatchPlayerAdvancedStatsTeam2List.Max(x => x.HeroDamage);

            int? highestExp1 = MatchPlayerAdvancedStatsTeam1List.Max(x => x.ExperienceContribution);
            int? highestExp2 = MatchPlayerAdvancedStatsTeam2List.Max(x => x.ExperienceContribution);

            int? highestDamageTaken1 = MatchPlayerAdvancedStatsTeam1List.Max(x => x.DamageTaken);
            int? highestDamageTaken2 = MatchPlayerAdvancedStatsTeam2List.Max(x => x.DamageTaken);

            int? highestHealing1 = MatchPlayerAdvancedStatsTeam1List.Max(x => x.HealingRole);
            int? highestHealing2 = MatchPlayerAdvancedStatsTeam2List.Max(x => x.HealingRole);

            int? highestMinion1 = MatchPlayerAdvancedStatsTeam1List.Max(x => x.MinionDamage);
            int? highestMinion2 = MatchPlayerAdvancedStatsTeam2List.Max(x => x.MinionDamage);

            int? highestSummon1 = MatchPlayerAdvancedStatsTeam1List.Max(x => x.SummonDamage);
            int? highestSummon2 = MatchPlayerAdvancedStatsTeam2List.Max(x => x.SummonDamage);

            int? highestStructure1 = MatchPlayerAdvancedStatsTeam1List.Max(x => x.StrutureDamage);
            int? highestStructure2 = MatchPlayerAdvancedStatsTeam2List.Max(x => x.StrutureDamage);

            TimeSpan? highestLive1 = MatchPlayerAdvancedStatsTeam1List.Min(x => x.TimeSpentDead);
            TimeSpan? highestLive2 = MatchPlayerAdvancedStatsTeam2List.Min(x => x.TimeSpentDead);

            int? highestSelfHealing1 = MatchPlayerAdvancedStatsTeam1List.Max(x => x.SelfHealing);
            int? highestSelfHealing2 = MatchPlayerAdvancedStatsTeam2List.Max(x => x.SelfHealing);

            int? highestMercDamage1 = MatchPlayerAdvancedStatsTeam1List.Max(x => x.CreepDamage);
            int? highestMercDamage2 = MatchPlayerAdvancedStatsTeam2List.Max(x => x.CreepDamage);

            int? highestMercCaptures1 = MatchPlayerAdvancedStatsTeam1List.Max(x => x.MercCampCaptures);
            int? highestMercCaptures2 = MatchPlayerAdvancedStatsTeam2List.Max(x => x.MercCampCaptures);

            int? highestWatchTowers1 = MatchPlayerAdvancedStatsTeam1List.Max(x => x.WatchTowerCaptures);
            int? highestWatchTowers2 = MatchPlayerAdvancedStatsTeam2List.Max(x => x.WatchTowerCaptures);

            int? highestKills1 = MatchPlayerAdvancedStatsTeam1List.Max(x => x.SoloKills);
            int? highestKills2 = MatchPlayerAdvancedStatsTeam2List.Max(x => x.SoloKills);

            int? highestTakedowns1 = MatchPlayerAdvancedStatsTeam1List.Max(x => x.TakeDowns);
            int? highestTakedowns2 = MatchPlayerAdvancedStatsTeam2List.Max(x => x.TakeDowns);

            int? highestAssists1 = MatchPlayerAdvancedStatsTeam1List.Max(x => x.Assists);
            int? highestAssists2 = MatchPlayerAdvancedStatsTeam2List.Max(x => x.Assists);

            int? highestNonDeaths1 = MatchPlayerAdvancedStatsTeam1List.Min(x => x.Deaths);
            int? highestNonDeaths2 = MatchPlayerAdvancedStatsTeam2List.Min(x => x.Deaths);

            foreach (var item in MatchPlayerAdvancedStatsTeam1List)
            {
                if (item.SiegeDamage == highestSiege1)
                    item.HighestSiegeDamage = true;

                if (item.HeroDamage == highestHero1)
                    item.HighestHeroDamage = true;

                if (item.ExperienceContribution == highestExp1)
                    item.HighestExperience = true;

                if (item.DamageTaken == highestDamageTaken1)
                    item.HighestDamageTaken = true;

                if (item.HealingRole == highestHealing1)
                    item.HighestHealing = true;

                if (item.MinionDamage == highestMinion1)
                    item.HighestMinionDamage = true;

                if (item.SummonDamage == highestSummon1)
                    item.HighestSummonDamage = true;

                if (item.StrutureDamage == highestStructure1)
                    item.HighestStructureDamage = true;

                if (item.TimeSpentDead == highestLive1)
                    item.HighestLiveTime = true;

                if (item.SelfHealing == highestSelfHealing1)
                    item.HighestSelfHealing = true;

                if (item.CreepDamage == highestMercDamage1)
                    item.HighestMercDamage = true;

                if (item.MercCampCaptures == highestMercCaptures1)
                    item.HighestMercCaptures = true;

                if (item.WatchTowerCaptures == highestWatchTowers1)
                    item.HighestWatchTowerCaptures = true;

                if (item.SoloKills == highestKills1)
                    item.HighestKills = true;

                if (item.TakeDowns == highestTakedowns1)
                    item.HighestTakedowns = true;

                if (item.Assists == highestAssists1)
                    item.HighestAssists = true;

                if (item.Deaths == highestNonDeaths1)
                    item.HighestNonDeaths = true;
            }

            foreach (var item in MatchPlayerAdvancedStatsTeam2List)
            {
                if (item.SiegeDamage == highestSiege2)
                    item.HighestSiegeDamage = true;

                if (item.HeroDamage == highestHero2)
                    item.HighestHeroDamage = true;

                if (item.ExperienceContribution == highestExp2)
                    item.HighestExperience = true;

                if (item.DamageTaken == highestDamageTaken2)
                    item.HighestDamageTaken = true;

                if (item.HealingRole == highestHealing2)
                    item.HighestHealing = true;

                if (item.MinionDamage == highestMinion2)
                    item.HighestMinionDamage = true;

                if (item.SummonDamage == highestSummon2)
                    item.HighestSummonDamage = true;

                if (item.StrutureDamage == highestStructure2)
                    item.HighestStructureDamage = true;

                if (item.TimeSpentDead == highestLive2)
                    item.HighestLiveTime = true;

                if (item.SelfHealing == highestSelfHealing2)
                    item.HighestSelfHealing = true;

                if (item.CreepDamage == highestMercDamage2)
                    item.HighestMercDamage = true;

                if (item.MercCampCaptures == highestMercCaptures2)
                    item.HighestMercCaptures = true;

                if (item.WatchTowerCaptures == highestWatchTowers2)
                    item.HighestWatchTowerCaptures = true;

                if (item.SoloKills == highestKills2)
                    item.HighestKills = true;

                if (item.TakeDowns == highestTakedowns2)
                    item.HighestTakedowns = true;

                if (item.Assists == highestAssists2)
                    item.HighestAssists = true;

                if (item.Deaths == highestNonDeaths2)
                    item.HighestNonDeaths = true;
            }
        }

        private void ReceivedMessage(NotificationMessage message)
        {
            if (message.Notification == StaticMessage.MatchSummaryClosed)
            {
                Messenger.Default.Send(new NotificationMessage(StaticMessage.ReEnableMatchSummaryButton));
            }
        }

        private async Task ChangeCurrentMatchSummaryAsync(int value)
        {
            await Application.Current.Dispatcher.InvokeAsync(() => { IsFlyoutLoadingOverlayVisible = true; });

            if (value < 0)
                Messenger.Default.Send(new NotificationMessage(StaticMessage.ChangeCurrentSelectedReplayMatchLeft));
            else
                Messenger.Default.Send(new NotificationMessage(StaticMessage.ChangeCurrentSelectedReplayMatchRight));
        }

        private List<Tuple<string, string>> CreateListOfCharacterHeroes(List<ReplayMatchPlayer> players)
        {
            var list = new List<Tuple<string, string>>();
            foreach (var player in players)
            {
                var playerInfo = Database.ReplaysDb().HotsPlayer.ReadRecordFromPlayerId(player.PlayerId);
                var playerName = Database.SettingsDb().UserSettings.IsBattleTagHidden ? HeroesHelpers.BattleTags.GetNameFromBattleTagName(playerInfo.BattleTagName) : playerInfo.BattleTagName;

                list.Add(new Tuple<string, string>(player.Character, playerName));
            }

            return list;
        }

        private void KeyEscPressed()
        {
            MatchSummaryFlyout.CloseMatchSummaryFlyout();
        }

        private void DisposeMatchSummary()
        {
            foreach (var player in MatchTalentsTeam1Collection)
                player.Dispose();

            foreach (var player in MatchTalentsTeam2Collection)
                player.Dispose();

            foreach (var player in MatchStatsTeam1Collection)
                player.Dispose();

            foreach (var player in MatchStatsTeam2Collection)
                player.Dispose();

            foreach (var player in MatchAdvancedStatsTeam1Collection)
                player.Dispose();

            foreach (var player in MatchAdvancedStatsTeam2Collection)
                player.Dispose();

            foreach (var player in MatchObserversCollection)
                player.Dispose();

            // bans
            MatchHeroBans.Team0Ban0 = null;
            MatchHeroBans.Team0Ban1 = null;
            MatchHeroBans.Team1Ban0 = null;
            MatchHeroBans.Team1Ban1 = null;
            MatchHeroBans.Team0Ban0HeroName = null;
            MatchHeroBans.Team0Ban1HeroName = null;
            MatchHeroBans.Team1Ban0HeroName = null;
            MatchHeroBans.Team1Ban1HeroName = null;

            BackgroundImage = null;

            HasBans = false;
            HasChat = false;
            HasObservers = false;

            // graphs
            TeamLevelTimeGraph.Dispose();
            TeamExperienceGraph.Dispose();
            StatGraphs.Dispose();

            MatchPlayerTalentsTeam1List = new List<MatchPlayerTalents>();
            MatchPlayerTalentsTeam2List = new List<MatchPlayerTalents>();
            MatchPlayerStatsTeam1List = new List<MatchPlayerStats>();
            MatchPlayerStatsTeam2List = new List<MatchPlayerStats>();
            MatchPlayerAdvancedStatsTeam1List = new List<MatchPlayerAdvancedStats>();
            MatchPlayerAdvancedStatsTeam2List = new List<MatchPlayerAdvancedStats>();
            MatchPlayerChatList = new List<MatchChat>();
            MatchPlayerObserversList = new List<MatchObserver>();
        }
    }
}