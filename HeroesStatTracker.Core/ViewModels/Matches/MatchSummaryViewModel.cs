using GalaSoft.MvvmLight.CommandWpf;
using GalaSoft.MvvmLight.Ioc;
using GalaSoft.MvvmLight.Messaging;
using Heroes.Helpers;
using Heroes.Icons;
using HeroesStatTracker.Core.Messaging;
using HeroesStatTracker.Core.Models.MatchModels;
using HeroesStatTracker.Core.ViewServices;
using HeroesStatTracker.Data;
using HeroesStatTracker.Data.Models.Replays;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Media;

namespace HeroesStatTracker.Core.ViewModels.Matches
{
    public class MatchSummaryViewModel : HstViewModel, IMatchSummaryReplayService
    {
        private int _teamBlueKills;
        private int _teamRedKills;
        private int _teamBlueLevel;
        private int _teamRedLevel;
        private bool _isLeftChangeButtonVisible;
        private bool _isRightChangeButtonVisible;
        private bool _isLeftChangeButtonEnabled;
        private bool _isRightChangeButtonEnabled;
        private bool _hasBans;
        private bool _hasObservers;
        private bool _hasChat;
        private string _teamBlueIsWinner;
        private string _teamRedIsWinner;
        private string _matchTitle;
        private string _teamBlueName;
        private string _teamRedName;
        private string _matchLength;
        private Color _matchTitleGlowColor;

        private ObservableCollection<MatchPlayerTalents> _matchTalentsTeam1Collection = new ObservableCollection<MatchPlayerTalents>();
        private ObservableCollection<MatchPlayerTalents> _matchTalentsTeam2Collection = new ObservableCollection<MatchPlayerTalents>();
        private ObservableCollection<MatchPlayerStats> _matchStatsTeam1Collection = new ObservableCollection<MatchPlayerStats>();
        private ObservableCollection<MatchPlayerStats> _matchStatsTeam2Collection = new ObservableCollection<MatchPlayerStats>();
        private ObservableCollection<MatchChat> _matchChatCollection = new ObservableCollection<MatchChat>();
        private IDatabaseService Database;

        public MatchSummaryViewModel(IDatabaseService database, IHeroesIconsService heroesIcons)
            : base(heroesIcons)
        {
            Database = database;

            IsLeftChangeButtonVisible = true;
            IsRightChangeButtonVisible = true;
            IsLeftChangeButtonEnabled = false;
            IsRightChangeButtonEnabled = false;

            HasBans = false;
            HasObservers = false;
            HasChat = false;

            Messenger.Default.Register<NotificationMessage>(this, (message) => ReceivedMessage(message));

            SimpleIoc.Default.Register<IMatchSummaryReplayService>(() => this);
        }

        public int TeamBlueKills
        {
            get { return _teamBlueKills; }
            set
            {
                _teamBlueKills = value;
                RaisePropertyChanged();
            }
        }

        public int TeamRedKills
        {
            get { return _teamRedKills; }
            set
            {
                _teamRedKills = value;
                RaisePropertyChanged();
            }
        }

        public int TeamBlueLevel
        {
            get { return _teamBlueLevel; }
            set
            {
                _teamBlueLevel = value;
                RaisePropertyChanged();
            }
        }

        public int TeamRedLevel
        {
            get { return _teamRedLevel; }
            set
            {
                _teamRedLevel = value;
                RaisePropertyChanged();
            }
        }

        public bool IsLeftChangeButtonVisible
        {
            get { return _isLeftChangeButtonVisible; }
            set
            {
                _isLeftChangeButtonVisible = value;
                RaisePropertyChanged();
            }
        }

        public bool IsRightChangeButtonVisible
        {
            get { return _isRightChangeButtonVisible; }
            set
            {
                _isRightChangeButtonVisible = value;
                RaisePropertyChanged();
            }
        }

        public bool IsLeftChangeButtonEnabled
        {
            get { return _isLeftChangeButtonEnabled; }
            set
            {
                _isLeftChangeButtonEnabled = value;
                RaisePropertyChanged();
            }
        }

        public bool IsRightChangeButtonEnabled
        {
            get { return _isRightChangeButtonEnabled; }
            set
            {
                _isRightChangeButtonEnabled = value;
                RaisePropertyChanged();
            }
        }

        public bool HasBans
        {
            get { return _hasBans; }
            set
            {
                _hasBans = value;
                RaisePropertyChanged();
            }
        }

        public bool HasObservers
        {
            get { return _hasObservers; }
            set
            {
                _hasObservers = value;
                RaisePropertyChanged();
            }
        }

        public bool HasChat
        {
            get { return _hasChat; }
            set
            {
                _hasChat = value;
                RaisePropertyChanged();
            }
        }

        public string MatchTitle
        {
            get { return _matchTitle; }
            set
            {
                _matchTitle = value;
                RaisePropertyChanged();
            }
        }

        public string TeamBlueName
        {
            get { return _teamBlueName; }
            set
            {
                _teamBlueName = value;
                RaisePropertyChanged();
            }
        }

        public string TeamRedName
        {
            get { return _teamRedName; }
            set
            {
                _teamRedName = value;
                RaisePropertyChanged();
            }
        }

        public string TeamBlueIsWinner
        {
            get { return _teamBlueIsWinner; }
            set
            {
                _teamBlueIsWinner = value;
                RaisePropertyChanged();
            }
        }

        public string TeamRedIsWinner
        {
            get { return _teamRedIsWinner; }
            set
            {
                _teamRedIsWinner = value;
                RaisePropertyChanged();
            }
        }

        public string MatchLength
        {
            get { return _matchLength; }
            set
            {
                _matchLength = value;
                RaisePropertyChanged();
            }
        }

        public Color MatchTitleGlowColor
        {
            get { return _matchTitleGlowColor; }
            set
            {
                _matchTitleGlowColor = value;
                RaisePropertyChanged();
            }
        }

        public ObservableCollection<MatchPlayerTalents> MatchTalentsTeam1Collection
        {
            get { return _matchTalentsTeam1Collection; }
            set
            {
                _matchTalentsTeam1Collection = value;
                RaisePropertyChanged();
            }
        }

        public ObservableCollection<MatchPlayerTalents> MatchTalentsTeam2Collection
        {
            get { return _matchTalentsTeam2Collection; }
            set
            {
                _matchTalentsTeam2Collection = value;
                RaisePropertyChanged();
            }
        }

        public ObservableCollection<MatchPlayerStats> MatchStatsTeam1Collection
        {
            get { return _matchStatsTeam1Collection; }
            set
            {
                _matchStatsTeam1Collection = value;
                RaisePropertyChanged();
            }
        }

        public ObservableCollection<MatchPlayerStats> MatchStatsTeam2Collection
        {
            get { return _matchStatsTeam2Collection; }
            set
            {
                _matchStatsTeam2Collection = value;
                RaisePropertyChanged();
            }
        }

        public ObservableCollection<MatchChat> MatchChatCollection
        {
            get { return _matchChatCollection; }
            set
            {
                _matchChatCollection = value;
                RaisePropertyChanged();
            }
        }

        public MatchBans MatchHeroBans { get; private set; } = new MatchBans();

        public RelayCommand MatchSummaryLeftChangeButtonCommand => new RelayCommand(() => ChangeCurrentMatchSummary(-1));
        public RelayCommand MatchSummaryRightChangeButtonCommand => new RelayCommand(() => ChangeCurrentMatchSummary(1));

        public void LoadMatchSummary(ReplayMatch replayMatch, List<ReplayMatch> matchList)
        {
            if (replayMatch == null || matchList == null || matchList.Count == 0)
                return;

            LoadMatchSummaryData(replayMatch);

            IsLeftChangeButtonEnabled = replayMatch.ReplayId == matchList[0].ReplayId ? false : true;
            IsRightChangeButtonEnabled = replayMatch.ReplayId == matchList[matchList.Count - 1].ReplayId ? false : true;
        }

        private void LoadMatchSummaryData(ReplayMatch replayMatch)
        {
            try
            {
                if (BackgroundImage != null)
                    DisposeMatchSummary();

                replayMatch = Database.ReplaysDb().MatchReplay.ReadReplayIncludeAssociatedRecords(replayMatch.ReplayId);

                HeroesIcons.LoadHeroesBuild(replayMatch.ReplayBuild);
                SetBackgroundImage(replayMatch.MapName);
                MatchTitleGlowColor = HeroesIcons.MapBackgrounds().GetMapBackgroundFontGlowColor(replayMatch.MapName);
                MatchTitle = $"{replayMatch.MapName} - {HeroesHelpers.GameModes.GetStringFromGameMode(replayMatch.GameMode)} [{replayMatch.TimeStamp}] [{replayMatch.ReplayLength}]";
                MatchLength = $"{replayMatch.ReplayLength.ToString(@"mm\:ss")}";

                // get players info
                var playersList = replayMatch.ReplayMatchPlayers.ToList();
                var playerTalentsList = replayMatch.ReplayMatchPlayerTalents.ToList();
                var playerScoresList = replayMatch.ReplayMatchPlayerScoreResults.ToList();
                var matchMessagesList = replayMatch.ReplayMatchMessage.ToList();
                var matchAwardDictionary = replayMatch.ReplayMatchAward.ToDictionary(x => x.PlayerId, x => x.Award);
                var matchTeamLevelsList = replayMatch.ReplayMatchTeamLevels.ToList();
                var matchTeamExperienceList = replayMatch.ReplayMatchTeamExperiences.ToList();

                FindPlayerParties(playersList);

                foreach (var player in playersList)
                {
                    if (player.Team == 4)
                        continue;

                    MatchPlayerBase matchPlayerBase = new MatchPlayerBase(Database, HeroesIcons, player);
                    matchPlayerBase.SetPlayerInfo(player.IsAutoSelect, PlayerPartyIcons, matchAwardDictionary);

                    MatchPlayerTalents matchPlayerTalents = new MatchPlayerTalents(matchPlayerBase);
                    MatchPlayerStats matchPlayerStats = new MatchPlayerStats(matchPlayerBase);

                    if (player.Character != "None")
                    {
                        matchPlayerTalents.SetTalents(playerTalentsList[player.PlayerNumber]);
                        matchPlayerStats.SetStats(playerScoresList[player.PlayerNumber], player);
                    }

                    if (player.Team == 0 || player.Team == 1)
                    {
                        if (player.Team == 0)
                        {
                            MatchTalentsTeam1Collection.Add(matchPlayerTalents);
                            MatchStatsTeam1Collection.Add(matchPlayerStats);
                        }
                        else
                        {
                            MatchTalentsTeam2Collection.Add(matchPlayerTalents);
                            MatchStatsTeam2Collection.Add(matchPlayerStats);
                        }
                    }
                }

                SetHighestTeamStatValues();

                // match bans
                if (replayMatch.ReplayMatchTeamBan != null)
                {
                    MatchHeroBans.Team0Ban0HeroName = HeroesIcons.Heroes().GetRealHeroNameFromAttributeId(replayMatch.ReplayMatchTeamBan.Team0Ban0);
                    MatchHeroBans.Team0Ban1HeroName = HeroesIcons.Heroes().GetRealHeroNameFromAttributeId(replayMatch.ReplayMatchTeamBan.Team0Ban1);
                    MatchHeroBans.Team1Ban0HeroName = HeroesIcons.Heroes().GetRealHeroNameFromAttributeId(replayMatch.ReplayMatchTeamBan.Team1Ban0);
                    MatchHeroBans.Team1Ban1HeroName = HeroesIcons.Heroes().GetRealHeroNameFromAttributeId(replayMatch.ReplayMatchTeamBan.Team1Ban1);
                    MatchHeroBans.Team0Ban0 = HeroesIcons.Heroes().GetHeroPortrait(MatchHeroBans.Team0Ban0HeroName);
                    MatchHeroBans.Team0Ban1 = HeroesIcons.Heroes().GetHeroPortrait(MatchHeroBans.Team0Ban1HeroName);
                    MatchHeroBans.Team1Ban0 = HeroesIcons.Heroes().GetHeroPortrait(MatchHeroBans.Team1Ban0HeroName);
                    MatchHeroBans.Team1Ban1 = HeroesIcons.Heroes().GetHeroPortrait(MatchHeroBans.Team1Ban1HeroName);

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

                            MatchChatCollection.Add(matchChat);
                        }
                    }

                    if (MatchChatCollection.Count > 0)
                        HasChat = true;
                }

                // Set the match results: total kills, team levels, game time
                MatchResult matchResult = new MatchResult(Database);
                matchResult.SetResult(MatchStatsTeam1Collection.ToList(), MatchStatsTeam2Collection.ToList(), matchTeamLevelsList.ToList(), playersList.ToList());
                SetMatchResults(matchResult);
            }
            catch (Exception ex)
            {
                ExceptionLog.Log(NLog.LogLevel.Error, ex);
            }
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
            int? highestSiege1 = MatchStatsTeam1Collection.Max(x => x.SiegeDamage);
            int? highestSiege2 = MatchStatsTeam2Collection.Max(x => x.SiegeDamage);

            int? highestHero1 = MatchStatsTeam1Collection.Max(x => x.HeroDamage);
            int? highestHero2 = MatchStatsTeam2Collection.Max(x => x.HeroDamage);

            int? highestExp1 = MatchStatsTeam1Collection.Max(x => x.ExperienceContribution);
            int? highestExp2 = MatchStatsTeam2Collection.Max(x => x.ExperienceContribution);

            int? highestDamageTaken1 = MatchStatsTeam1Collection.Max(x => x.DamageTakenRole);
            int? highestDamageTaken2 = MatchStatsTeam2Collection.Max(x => x.DamageTakenRole);

            int? highestHealing1 = MatchStatsTeam1Collection.Max(x => x.HealingRole);
            int? highestHealing2 = MatchStatsTeam2Collection.Max(x => x.HealingRole);

            foreach (var item in MatchStatsTeam1Collection)
            {
                if (item.SiegeDamage == highestSiege1)
                    item.HighestSiegeDamage = true;

                if (item.HeroDamage == highestHero1)
                    item.HighestHeroDamage = true;

                if (item.ExperienceContribution == highestExp1)
                    item.HighestExperience = true;

                if (item.DamageTakenRole == highestDamageTaken1)
                    item.HighestDamageTaken = true;

                if (item.HealingRole == highestHealing1)
                    item.HighestHealing = true;
            }

            foreach (var item in MatchStatsTeam2Collection)
            {
                if (item.SiegeDamage == highestSiege2)
                    item.HighestSiegeDamage = true;

                if (item.HeroDamage == highestHero2)
                    item.HighestHeroDamage = true;

                if (item.ExperienceContribution == highestExp2)
                    item.HighestExperience = true;

                if (item.DamageTakenRole == highestDamageTaken2)
                    item.HighestDamageTaken = true;

                if (item.HealingRole == highestHealing2)
                    item.HighestHealing = true;
            }
        }

        private void ReceivedMessage(NotificationMessage message)
        {
            if (message.Notification == StaticMessage.MatchSummaryClosed)
            {
                Messenger.Default.Send(new NotificationMessage(StaticMessage.ReEnableMatchSummaryButton));
            }
        }

        private void ChangeCurrentMatchSummary(int value)
        {
            if (value < 0)
                Messenger.Default.Send(new NotificationMessage(StaticMessage.ChangeCurrentSelectedReplayMatchLeft));
            else
                Messenger.Default.Send(new NotificationMessage(StaticMessage.ChangeCurrentSelectedReplayMatchRight));
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

            // bans
            MatchHeroBans.Team0Ban0 = null;
            MatchHeroBans.Team0Ban1 = null;
            MatchHeroBans.Team1Ban0 = null;
            MatchHeroBans.Team1Ban1 = null;
            MatchHeroBans.Team0Ban0HeroName = null;
            MatchHeroBans.Team0Ban1HeroName = null;
            MatchHeroBans.Team1Ban0HeroName = null;
            MatchHeroBans.Team1Ban1HeroName = null;

            // chat
            MatchChatCollection = null;

            BackgroundImage = null;

            HasBans = false;
            HasChat = false;
            HasObservers = false;

            MatchTalentsTeam1Collection = new ObservableCollection<MatchPlayerTalents>();
            MatchTalentsTeam2Collection = new ObservableCollection<MatchPlayerTalents>();
            MatchStatsTeam1Collection = new ObservableCollection<MatchPlayerStats>();
            MatchStatsTeam2Collection = new ObservableCollection<MatchPlayerStats>();
            MatchChatCollection = new ObservableCollection<MatchChat>();
        }
    }
}
