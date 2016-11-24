using GalaSoft.MvvmLight.Messaging;
using HeroesIcons;
using HeroesParserData.DataQueries;
using HeroesParserData.Messages;
using HeroesParserData.Models.DbModels;
using HeroesParserData.Models.MatchModels;
using NLog;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace HeroesParserData.ViewModels.Match.Summary
{
    public abstract class MatchSummaryContext : MatchContextBase
    {
        #region properties

        private string _matchTitle;
        private bool _hasBans;
        private bool _hasObservers;
        private bool _hasChat;
        private bool _isLeftChangeButtonVisible;
        private bool _isRightChangeButtonVisible;
        private bool _isLeftChangeButtonEnabled;
        private bool _isRightChangeButtonEnabled;
        private Color _mapNameGlowColor;

        private ObservableCollection<MatchTalents> _matchTalentsTeam1Collection = new ObservableCollection<MatchTalents>();
        private ObservableCollection<MatchTalents> _matchTalentsTeam2Collection = new ObservableCollection<MatchTalents>();
        private ObservableCollection<MatchTalents> _matchObserversCollection = new ObservableCollection<MatchTalents>();
        private ObservableCollection<MatchScores> _matchScoreTeam1Collection = new ObservableCollection<MatchScores>();
        private ObservableCollection<MatchScores> _matchScoreTeam2Collection = new ObservableCollection<MatchScores>();
        private ObservableCollection<MatchScores> _matchScoreTeam1TotalCollection = new ObservableCollection<MatchScores>();
        private ObservableCollection<MatchScores> _matchScoreTeam2TotalCollection = new ObservableCollection<MatchScores>();
        private ObservableCollection<MatchChat> _matchChatMessagesCollection = new ObservableCollection<MatchChat>();
        #endregion properties

        protected Replay CurrentReplay { get; set; }
        protected List<Replay> MatchList { get; set; }

        #region public properties
        public MatchHeroBans MatchHeroBans { get; private set; } = new MatchHeroBans();

        public ObservableCollection<MatchTalents> MatchTalentsTeam1Collection
        {
            get { return _matchTalentsTeam1Collection; }
            set
            {
                _matchTalentsTeam1Collection = value;
                RaisePropertyChangedEvent(nameof(MatchTalentsTeam1Collection));
            }
        }

        public ObservableCollection<MatchTalents> MatchTalentsTeam2Collection
        {
            get { return _matchTalentsTeam2Collection; }
            set
            {
                _matchTalentsTeam2Collection = value;
                RaisePropertyChangedEvent(nameof(MatchTalentsTeam2Collection));
            }
        }

        public ObservableCollection<MatchTalents> MatchObserversCollection
        {
            get { return _matchObserversCollection; }
            set
            {
                _matchObserversCollection = value;
                RaisePropertyChangedEvent(nameof(MatchObserversCollection));
            }
        }

        public ObservableCollection<MatchScores> MatchScoreTeam1Collection
        {
            get { return _matchScoreTeam1Collection; }
            set
            {
                _matchScoreTeam1Collection = value;
                RaisePropertyChangedEvent(nameof(MatchScoreTeam1Collection));
            }
        }

        public ObservableCollection<MatchScores> MatchScoreTeam2Collection
        {
            get { return _matchScoreTeam2Collection; }
            set
            {
                _matchScoreTeam2Collection = value;
                RaisePropertyChangedEvent(nameof(MatchScoreTeam2Collection));
            }
        }

        public ObservableCollection<MatchScores> MatchScoreTeam1TotalCollection
        {
            get { return _matchScoreTeam1TotalCollection; }
            set
            {
                _matchScoreTeam1TotalCollection = value;
                RaisePropertyChangedEvent(nameof(MatchScoreTeam1TotalCollection));
            }
        }

        public ObservableCollection<MatchScores> MatchScoreTeam2TotalCollection
        {
            get { return _matchScoreTeam2TotalCollection; }
            set
            {
                _matchScoreTeam2TotalCollection = value;
                RaisePropertyChangedEvent(nameof(MatchScoreTeam2TotalCollection));
            }
        }

        public ObservableCollection<MatchChat> MatchChatMessagesCollection
        {
            get { return _matchChatMessagesCollection; }
            set
            {
                _matchChatMessagesCollection = value;
                RaisePropertyChangedEvent(nameof(MatchChatMessagesCollection));
            }
        }

        public string MatchTitle
        {
            get { return _matchTitle; }
            set
            {
                _matchTitle = value;
                RaisePropertyChangedEvent(nameof(MatchTitle));
            }
        }

        public Color MapNameGlowColor
        {
            get { return _mapNameGlowColor; }
            set
            {
                _mapNameGlowColor = value;
                RaisePropertyChangedEvent(nameof(MapNameGlowColor));
            }
        }

        // shows the expander that shows the bans
        public bool HasBans
        {
            get { return _hasBans; }
            set
            {
                _hasBans = value;
                RaisePropertyChangedEvent(nameof(HasBans));
            }
        }

        // shows the expander that shows the Observers
        public bool HasObservers
        {
            get { return _hasObservers; }
            set
            {
                _hasObservers = value;
                RaisePropertyChangedEvent(nameof(HasObservers));
            }
        }

        // shows the expander that shows the observers
        public bool HasChat
        {
            get { return _hasChat; }
            set
            {
                _hasChat = value;
                RaisePropertyChangedEvent(nameof(HasChat));
            }
        }

        public bool IsLeftChangeButtonVisible
        {
            get { return _isLeftChangeButtonVisible; }
            set
            {
                _isLeftChangeButtonVisible = value;
                RaisePropertyChangedEvent(nameof(IsLeftChangeButtonVisible));
            }
        }

        public bool IsRightChangeButtonVisible
        {
            get { return _isRightChangeButtonVisible; }
            set
            {
                _isRightChangeButtonVisible = value;
                RaisePropertyChangedEvent(nameof(IsRightChangeButtonVisible));
            }
        }

        public bool IsLeftChangeButtonEnabled
        {
            get { return _isLeftChangeButtonEnabled; }
            set
            {
                _isLeftChangeButtonEnabled = value;
                RaisePropertyChangedEvent(nameof(IsLeftChangeButtonEnabled));
            }
        }

        public bool IsRightChangeButtonEnabled
        {
            get { return _isRightChangeButtonEnabled; }
            set
            {
                _isRightChangeButtonEnabled = value;
                RaisePropertyChangedEvent(nameof(IsRightChangeButtonEnabled));
            }
        }

        public ICommand LeftChangeButtonCommand
        {
            get { return new DelegateCommand(() => ChangeCurrentMatchSummary(-1)); }
        }

        public ICommand RightChangeButtonCommand
        {
            get { return new DelegateCommand(() => ChangeCurrentMatchSummary(1)); }
        }
        #endregion public properties

        /// <summary>
        /// Constructor
        /// </summary>
        protected MatchSummaryContext()
            : base()
        {
            Messenger.Default.Register<MatchSummaryMessage>(this, (action) => ReceiveMessage(action));

            HasChat = true;
            IsLeftChangeButtonEnabled = true;
            IsRightChangeButtonEnabled = true;
            IsLeftChangeButtonVisible = true;
            IsRightChangeButtonVisible = true;
        }

        protected abstract void ReceiveMessage(MatchSummaryMessage action);

        protected virtual void ExecuteSelectedReplay(MatchSummaryMessage action)
        {
            MatchList = action.MatchList;
            CurrentReplay = action.Replay;
            QuerySummaryDetails();

            if (CurrentReplay.ReplayId == MatchList[0].ReplayId)
                IsLeftChangeButtonEnabled = false;
            else
                IsLeftChangeButtonEnabled = true;

            if (CurrentReplay.ReplayId == MatchList[MatchList.Count - 1].ReplayId)
                IsRightChangeButtonEnabled = false;
            else
                IsRightChangeButtonEnabled = true;
        }

        protected void QuerySummaryDetails()
        {
            if (CurrentReplay == null)
                return;

            try
            {
                // get replay info
                Models.DbModels.Replay replay = Query.Replay.ReadReplayIncludeRecord(CurrentReplay.ReplayId);

                // load up correct build information
                HeroesInfo.ReInitializeSpecificHeroesXml(replay.ReplayBuild);

                // get players info 
                var playersList = replay.ReplayMatchPlayers.ToList();
                var playerTalentsList = replay.ReplayMatchPlayerTalents.ToList();
                var playerScoresList = replay.ReplayMatchPlayerScoreResults.ToList();
                var matchMessagesList = replay.ReplayMatchMessage.ToList();
                var matchAwardDictionary = replay.ReplayMatchAward.ToDictionary(x => x.PlayerId, x => x.Award);
                var matchTeamLevelsList = replay.ReplayMatchTeamLevels.ToList();

                int highestSiegeTeam1Index = 0, highestSiegeTeam1Count = 0, highestSiegeTeam2Index = 0, highestSiegeTeam2Count = 0;
                int highestHeroDamageTeam1Index = 0, highestHeroDamageTeam1Count = 0, highestHeroDamageTeam2Index = 0, highestHeroDamageTeam2Count = 0;
                int highestExpTeam1Index = 0, highestExpTeam1Count = 0, highestExpTeam2Index = 0, highestExpTeam2Count = 0;

                FindPlayerParties(playersList);

                foreach (var player in playersList)
                {
                    // fill in common infomation for player
                    string mvpAwardName = null;

                    MatchPlayerInfoBase matchPlayerInfoBase = new MatchPlayerInfoBase();

                    var playerInfo = Query.HotsPlayer.ReadRecordFromPlayerId(player.PlayerId);
                    matchPlayerInfoBase.LeaderboardPortrait = player.Character != "None" ? HeroesInfo.GetHeroLeaderboardPortrait(player.Character) : null;
                    matchPlayerInfoBase.CharacterName = player.Character;
                    matchPlayerInfoBase.PlayerName = Utilities.GetNameFromBattleTagName(playerInfo.BattleTagName);
                    matchPlayerInfoBase.PlayerTag = Utilities.GetTagFromBattleTagName(playerInfo.BattleTagName).ToString();
                    matchPlayerInfoBase.PlayerNumber = player.PlayerNumber;
                    matchPlayerInfoBase.PlayerSilenced = player.IsSilenced;
                    matchPlayerInfoBase.MvpAward = matchAwardDictionary.ContainsKey(player.PlayerId) == true ? SetPlayerMVPAward(player.Team, matchAwardDictionary[player.PlayerId], out mvpAwardName) : null;
                    matchPlayerInfoBase.MvpAwardName = mvpAwardName;

                    if (!player.IsAutoSelect)
                        matchPlayerInfoBase.CharacterLevel = player.CharacterLevel.ToString();
                    else
                        matchPlayerInfoBase.CharacterLevel = "Auto Select";

                    if (PlayerPartyIcons.ContainsKey(player.PlayerNumber))
                    {
                        matchPlayerInfoBase.PartyIcon = HeroesInfo.GetPartyIcon(PlayerPartyIcons[player.PlayerNumber]);
                    }

                    MatchTalents matchTalents = new MatchTalents(matchPlayerInfoBase);
                    MatchScores matchScores = new MatchScores(matchPlayerInfoBase);

                    if (player.Character != "None")
                    {
                        matchTalents.Talent1 = HeroesInfo.GetTalentIcon(playerTalentsList[player.PlayerNumber].TalentName1);
                        matchTalents.Talent4 = HeroesInfo.GetTalentIcon(playerTalentsList[player.PlayerNumber].TalentName4);
                        matchTalents.Talent7 = HeroesInfo.GetTalentIcon(playerTalentsList[player.PlayerNumber].TalentName7);
                        matchTalents.Talent10 = HeroesInfo.GetTalentIcon(playerTalentsList[player.PlayerNumber].TalentName10);
                        matchTalents.Talent13 = HeroesInfo.GetTalentIcon(playerTalentsList[player.PlayerNumber].TalentName13);
                        matchTalents.Talent16 = HeroesInfo.GetTalentIcon(playerTalentsList[player.PlayerNumber].TalentName16);
                        matchTalents.Talent20 = HeroesInfo.GetTalentIcon(playerTalentsList[player.PlayerNumber].TalentName20);

                        matchTalents.TalentName1 = HeroesInfo.GetTrueTalentName(playerTalentsList[player.PlayerNumber].TalentName1);
                        matchTalents.TalentName4 = HeroesInfo.GetTrueTalentName(playerTalentsList[player.PlayerNumber].TalentName4);
                        matchTalents.TalentName7 = HeroesInfo.GetTrueTalentName(playerTalentsList[player.PlayerNumber].TalentName7);
                        matchTalents.TalentName10 = HeroesInfo.GetTrueTalentName(playerTalentsList[player.PlayerNumber].TalentName10);
                        matchTalents.TalentName13 = HeroesInfo.GetTrueTalentName(playerTalentsList[player.PlayerNumber].TalentName13);
                        matchTalents.TalentName16 = HeroesInfo.GetTrueTalentName(playerTalentsList[player.PlayerNumber].TalentName16);
                        matchTalents.TalentName20 = HeroesInfo.GetTrueTalentName(playerTalentsList[player.PlayerNumber].TalentName20);

                        TalentDescription talent1 = HeroesInfo.GetTalentDescriptions(playerTalentsList[player.PlayerNumber].TalentName1);
                        TalentDescription talent4 = HeroesInfo.GetTalentDescriptions(playerTalentsList[player.PlayerNumber].TalentName4);
                        TalentDescription talent7 = HeroesInfo.GetTalentDescriptions(playerTalentsList[player.PlayerNumber].TalentName7);
                        TalentDescription talent10 = HeroesInfo.GetTalentDescriptions(playerTalentsList[player.PlayerNumber].TalentName10);
                        TalentDescription talent13 = HeroesInfo.GetTalentDescriptions(playerTalentsList[player.PlayerNumber].TalentName13);
                        TalentDescription talent16 = HeroesInfo.GetTalentDescriptions(playerTalentsList[player.PlayerNumber].TalentName16);
                        TalentDescription talent20 = HeroesInfo.GetTalentDescriptions(playerTalentsList[player.PlayerNumber].TalentName20);

                        matchTalents.TalentShortDescription1 = $"<c val=\"FFFFFF\">{matchTalents.TalentName1}:</c> {talent1.Short}";
                        matchTalents.TalentShortDescription4 = $"<c val=\"FFFFFF\">{matchTalents.TalentName4}:</c> {talent4.Short}";
                        matchTalents.TalentShortDescription7 = $"<c val=\"FFFFFF\">{matchTalents.TalentName7}:</c> {talent7.Short}";
                        matchTalents.TalentShortDescription10 = $"<c val=\"FFFFFF\">{matchTalents.TalentName10}:</c> {talent10.Short}";
                        matchTalents.TalentShortDescription13 = $"<c val=\"FFFFFF\">{matchTalents.TalentName13}:</c> {talent13.Short}";
                        matchTalents.TalentShortDescription16 = $"<c val=\"FFFFFF\">{matchTalents.TalentName16}:</c> {talent16.Short}";
                        matchTalents.TalentShortDescription20 = $"<c val=\"FFFFFF\">{matchTalents.TalentName20}:</c> {talent20.Short}";

                        matchTalents.TalentFullDescription1 = talent1.Full;
                        matchTalents.TalentFullDescription4 = talent4.Full;
                        matchTalents.TalentFullDescription7 = talent7.Full;
                        matchTalents.TalentFullDescription10 = talent10.Full;
                        matchTalents.TalentFullDescription13 = talent13.Full;
                        matchTalents.TalentFullDescription16 = talent16.Full;
                        matchTalents.TalentFullDescription20 = talent20.Full;

                        matchScores.SoloKills = playerScoresList[player.PlayerNumber].SoloKills;
                        matchScores.Assists = playerScoresList[player.PlayerNumber].Assists;
                        matchScores.Deaths = playerScoresList[player.PlayerNumber].Deaths;
                        matchScores.SiegeDamage = playerScoresList[player.PlayerNumber].SiegeDamage;
                        matchScores.HeroDamage = playerScoresList[player.PlayerNumber].HeroDamage;
                        matchScores.ExperienceContribution = playerScoresList[player.PlayerNumber].ExperienceContribution;
                        if (playerScoresList[player.PlayerNumber].DamageTaken != null)
                            matchScores.Role = playerScoresList[player.PlayerNumber].DamageTaken;
                        else if (IsHealingStatCharacter(player.Character))
                            matchScores.Role = playerScoresList[player.PlayerNumber].Healing;
                    }

                    if (player.IsWinner)
                    {
                        matchTalents.RowBackColor = WinningTeamBackColor;
                        matchScores.RowBackColor = WinningTeamBackColor;
                    }
                    else
                    {
                        matchTalents.RowBackColor = LosingTeamBackColor;
                        matchScores.RowBackColor = LosingTeamBackColor;
                    }

                    if (player.Team == 0 || player.Team == 1)
                    {
                        if (player.Team == 0)
                        {
                            matchTalents.PortraitBackColor = Team1BackColor;
                            matchScores.PortraitBackColor = Team1BackColor;

                            HighestSiegeDamage(MatchScoreTeam1Collection, matchScores, ref highestSiegeTeam1Index, ref highestSiegeTeam1Count);
                            HighestHeroDamage(MatchScoreTeam1Collection, matchScores, ref highestHeroDamageTeam1Index, ref highestHeroDamageTeam1Count);
                            HighestExpContribution(MatchScoreTeam1Collection, matchScores, ref highestExpTeam1Index, ref highestExpTeam1Count);

                            // add to collection
                            MatchTalentsTeam1Collection.Add(matchTalents);
                            MatchScoreTeam1Collection.Add(matchScores);
                        }
                        else
                        {
                            matchTalents.PortraitBackColor = Team2BackColor;
                            matchScores.PortraitBackColor = Team2BackColor;

                            HighestSiegeDamage(MatchScoreTeam2Collection, matchScores, ref highestSiegeTeam2Index, ref highestSiegeTeam2Count);
                            HighestHeroDamage(MatchScoreTeam2Collection, matchScores, ref highestHeroDamageTeam2Index, ref highestHeroDamageTeam2Count);
                            HighestExpContribution(MatchScoreTeam2Collection, matchScores, ref highestExpTeam2Index, ref highestExpTeam2Count);

                            // add to collection
                            MatchTalentsTeam2Collection.Add(matchTalents);
                            MatchScoreTeam2Collection.Add(matchScores);
                        }
                    }
                    else if (player.Team == 4) // observers
                    {
                        HasObservers = true;
                        matchTalents.CharacterLevel = "Observer";
                        MatchObserversCollection.Add(matchTalents);
                    }
                } // end foreach players

                // Total for score summaries
                SetScoreSummaryTotals(MatchScoreTeam1Collection, MatchScoreTeam1TotalCollection, matchTeamLevelsList.Max(x => x.Team0Level));
                SetScoreSummaryTotals(MatchScoreTeam2Collection, MatchScoreTeam2TotalCollection, matchTeamLevelsList.Max(x => x.Team1Level));

                MatchTitle = $"{replay.MapName} - {replay.GameMode} [{replay.TimeStamp}] [{replay.ReplayLength}] [Id: {replay.ReplayId}] [Build: {replay.ReplayBuild}]";

                Color mapNameGlowColor;
                BackgroundMapImage = SetMapImage(replay.MapName, out mapNameGlowColor);
                MapNameGlowColor = mapNameGlowColor;

                // hero bans
                if (replay.ReplayMatchTeamBan != null)
                {
                    HasBans = true;
                    MatchHeroBans.Team0Ban0HeroName = HeroesInfo.GetRealHeroNameFromAttId(replay.ReplayMatchTeamBan.Team0Ban0);
                    MatchHeroBans.Team0Ban1HeroName = HeroesInfo.GetRealHeroNameFromAttId(replay.ReplayMatchTeamBan.Team0Ban1);
                    MatchHeroBans.Team1Ban0HeroName = HeroesInfo.GetRealHeroNameFromAttId(replay.ReplayMatchTeamBan.Team1Ban0);
                    MatchHeroBans.Team1Ban1HeroName = HeroesInfo.GetRealHeroNameFromAttId(replay.ReplayMatchTeamBan.Team1Ban1);
                    MatchHeroBans.Team0Ban0 = HeroesInfo.GetHeroPortrait(MatchHeroBans.Team0Ban0HeroName);
                    MatchHeroBans.Team0Ban1 = HeroesInfo.GetHeroPortrait(MatchHeroBans.Team0Ban1HeroName);
                    MatchHeroBans.Team1Ban0 = HeroesInfo.GetHeroPortrait(MatchHeroBans.Team1Ban0HeroName);
                    MatchHeroBans.Team1Ban1 = HeroesInfo.GetHeroPortrait(MatchHeroBans.Team1Ban1HeroName);
                }

                // match chat
                if (matchMessagesList != null)
                {
                    foreach (var message in matchMessagesList)
                    {
                        if (message.MessageEventType == "SChatMessage")
                        {
                            MatchChat matchChat = new MatchChat();

                            matchChat.TimeStamp = message.TimeStamp;
                            matchChat.Target = message.MessageTarget;

                            if (string.IsNullOrEmpty(message.PlayerName))
                                matchChat.ChatMessage = $"((Unknown)): {message.Message}";
                            else if (!string.IsNullOrEmpty(message.CharacterName))
                                matchChat.ChatMessage = $"{message.PlayerName} ({message.CharacterName}): {message.Message}";
                            else
                                matchChat.ChatMessage = $"{message.PlayerName}: {message.Message}";

                            MatchChatMessagesCollection.Add(matchChat);
                        }
                    }
                }

                HasChat = MatchChatMessagesCollection.Count < 1 ? false : true;
            }
            catch (Exception ex)
            {
                WarningLog.Log(LogLevel.Warn, ex.Message);
                ExceptionLog.Log(LogLevel.Error, ex);
            }
        }

        // clear everything and free up resources
        protected void ClearSummaryDetails()
        {
            // talents
            foreach (var matchTalent in MatchTalentsTeam1Collection)
            {
                matchTalent.LeaderboardPortrait = null;
                matchTalent.PartyIcon = null;
                matchTalent.Talent1 = null;
                matchTalent.Talent4 = null;
                matchTalent.Talent7 = null;
                matchTalent.Talent10 = null;
                matchTalent.Talent13 = null;
                matchTalent.Talent16 = null;
                matchTalent.Talent20 = null;
                matchTalent.MvpAward = null;
            }
            MatchTalentsTeam1Collection = null;

            foreach (var matchTalent in MatchTalentsTeam2Collection)
            {
                matchTalent.LeaderboardPortrait = null;
                matchTalent.PartyIcon = null;
                matchTalent.Talent1 = null;
                matchTalent.Talent4 = null;
                matchTalent.Talent7 = null;
                matchTalent.Talent10 = null;
                matchTalent.Talent13 = null;
                matchTalent.Talent16 = null;
                matchTalent.Talent20 = null;
                matchTalent.MvpAward = null;
            }
            MatchTalentsTeam2Collection = null;

            // score summary
            foreach (var matchScore in MatchScoreTeam1Collection)
            {
                matchScore.LeaderboardPortrait = null;
                matchScore.PartyIcon = null;
                matchScore.MvpAward = null;
            }
            MatchScoreTeam1Collection = null;

            foreach (var matchScore in MatchScoreTeam2Collection)
            {
                matchScore.LeaderboardPortrait = null;
                matchScore.PartyIcon = null;
                matchScore.MvpAward = null;
            }
            MatchScoreTeam2Collection = null;
            MatchScoreTeam1TotalCollection = null;
            MatchScoreTeam2TotalCollection = null;

            // observers
            HasObservers = false;
            MatchObserversCollection = null;

            // bans
            MatchHeroBans.Team0Ban0 = null;
            MatchHeroBans.Team0Ban1 = null;
            MatchHeroBans.Team1Ban0 = null;
            MatchHeroBans.Team1Ban1 = null;
            MatchHeroBans.Team0Ban0HeroName = null;
            MatchHeroBans.Team0Ban1HeroName = null;
            MatchHeroBans.Team1Ban0HeroName = null;
            MatchHeroBans.Team1Ban1HeroName = null;
            HasBans = false;

            // chat
            MatchChatMessagesCollection = null;
            HasChat = true;

            BackgroundMapImage = null;

            MatchTalentsTeam1Collection = new ObservableCollection<MatchTalents>();
            MatchTalentsTeam2Collection = new ObservableCollection<MatchTalents>();
            MatchScoreTeam1Collection = new ObservableCollection<MatchScores>();
            MatchScoreTeam2Collection = new ObservableCollection<MatchScores>();
            MatchScoreTeam1TotalCollection = new ObservableCollection<MatchScores>();
            MatchScoreTeam2TotalCollection = new ObservableCollection<MatchScores>();
            MatchObserversCollection = new ObservableCollection<MatchTalents>();
            MatchChatMessagesCollection = new ObservableCollection<MatchChat>();
        }

        private bool IsHealingStatCharacter(string realHeroName)
        {
            if (HeroesInfo.GetHeroRole(realHeroName) == HeroRole.Support || HeroesInfo.IsNonSupportHeroWithHealingStat(realHeroName))
                return true;
            else
                return false;
        }

        private void HighestSiegeDamage(ObservableCollection<MatchScores> MatchScoreTeamList, MatchScores matchScores, ref int highestTeam1Index, ref int highestTeam1Count)
        {
            if (MatchScoreTeamList.Count == 0)
            {
                highestTeam1Index = 0;
                matchScores.HighestSiegeFont = FontWeights.ExtraBold;
            }
            else if (matchScores.SiegeDamage > MatchScoreTeamList[highestTeam1Index].SiegeDamage)
            {
                MatchScoreTeamList[highestTeam1Index].HighestSiegeFont = FontWeights.Normal; // clear previous
                matchScores.HighestSiegeFont = FontWeights.ExtraBold; // set current
                highestTeam1Index = highestTeam1Count;
            }
            highestTeam1Count++;
        }

        private void HighestHeroDamage(ObservableCollection<MatchScores> MatchScoreTeamList, MatchScores matchScores, ref int highestIndex, ref int highestCount)
        {
            if (MatchScoreTeamList.Count == 0)
            {
                highestIndex = 0;
                matchScores.HighestHeroDamageFont = FontWeights.ExtraBold;
            }
            else if (matchScores.HeroDamage > MatchScoreTeamList[highestIndex].HeroDamage)
            {
                MatchScoreTeamList[highestIndex].HighestHeroDamageFont = FontWeights.Normal; // clear previous
                matchScores.HighestHeroDamageFont = FontWeights.ExtraBold; // set current
                highestIndex = highestCount;
            }
            highestCount++;
        }

        private void HighestExpContribution(ObservableCollection<MatchScores> MatchScoreTeamList, MatchScores matchScores, ref int highestIndex, ref int highestCount)
        {
            if (MatchScoreTeamList.Count == 0)
            {
                highestIndex = 0;
                matchScores.HighestExpFont = FontWeights.ExtraBold;
            }
            else if (matchScores.ExperienceContribution > MatchScoreTeamList[highestIndex].ExperienceContribution)
            {
                MatchScoreTeamList[highestIndex].HighestExpFont = FontWeights.Normal; // clear previous
                matchScores.HighestExpFont = FontWeights.ExtraBold; // set current
                highestIndex = highestCount;
            }
            highestCount++;
        }

        private BitmapImage SetMapImage(string mapName, out Color glowColor)
        {
            glowColor = HeroesInfo.GetMapBackgroundFontGlowColor(mapName);
            return HeroesInfo.GetMapBackground(mapName);
        }

        private BitmapImage SetPlayerMVPAward(int? team, string awardType, out string awardName)
        {
            MVPScoreScreenColor teamColor;
            if (team == 0)
                teamColor = MVPScoreScreenColor.Blue;
            else if (team == 1)
                teamColor = MVPScoreScreenColor.Red;
            else
            {
                ExceptionLog.Log(LogLevel.Info, $"[MatchContext.cs]({nameof(SetPlayerMVPAward)}) Team is not a 0 or 1");
                awardName = null;
                return null;
            }

            return HeroesInfo.GetMVPScoreScreenAward(awardType, teamColor, out awardName);
        }

        private void SetScoreSummaryTotals(ObservableCollection<MatchScores> matchScoreTeam, ObservableCollection<MatchScores> collection, int? highestLevel)
        {
            int? killsTotal = matchScoreTeam.Sum(x => x.SoloKills);
            int? assistsTotal = matchScoreTeam.Sum(x => x.Assists);
            int? deathsTotal = matchScoreTeam.Sum(x => x.Deaths);
            int? siegeDamageTotal = matchScoreTeam.Sum(x => x.SiegeDamage);
            int? heroDamageTotal = matchScoreTeam.Sum(x => x.HeroDamage);
            int? experienceTotal = matchScoreTeam.Sum(x => x.ExperienceContribution);

            Color currentColor = matchScoreTeam[0].RowBackColor;
            Color newColor = Color.FromArgb(currentColor.A, (byte)(currentColor.R * 0.9), (byte)(currentColor.G * 0.9), (byte)(currentColor.B * 0.9));

            MatchScores matchScoresTotal = new MatchScores
            {
                PlayerName = "Total",
                CharacterLevel = $"Team: {highestLevel.ToString()}",
                SoloKills = killsTotal,
                Assists = assistsTotal,
                Deaths = deathsTotal,
                SiegeDamage = siegeDamageTotal,
                HeroDamage = heroDamageTotal,
                ExperienceContribution = experienceTotal,
                RowBackColor = matchScoreTeam[0].PortraitBackColor
            };

            collection.Add(matchScoresTotal);
        }

        private void ChangeCurrentMatchSummary(int value)
        {
            int index = MatchList.FindIndex(x => x.TimeStamp == CurrentReplay.TimeStamp);

            if (value == -1)
                CurrentReplay = MatchList[index - 1];
            else if (value == 1)
                CurrentReplay = MatchList[index + 1];

            ClearSummaryDetails();
            QuerySummaryDetails();

            if (CurrentReplay.ReplayId == MatchList[0].ReplayId)
                IsLeftChangeButtonEnabled = false;
            else
                IsLeftChangeButtonEnabled = true;

            if (CurrentReplay.ReplayId == MatchList[MatchList.Count - 1].ReplayId)
                IsRightChangeButtonEnabled = false;
            else
                IsRightChangeButtonEnabled = true;

            Messenger.Default.Send(new MatchOverviewMessage { Replay = CurrentReplay });
        }
    }
}
