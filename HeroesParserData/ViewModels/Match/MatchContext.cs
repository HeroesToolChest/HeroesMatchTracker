using Heroes.ReplayParser;
using HeroesIcons;
using HeroesParserData.DataQueries;
using HeroesParserData.Models.DbModels;
using HeroesParserData.Models.MatchModels;
using HeroesParserData.Properties;
using NLog;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace HeroesParserData.ViewModels.Match
{
    public abstract class MatchContext : ViewModelBase
    {
        #region properties
        private string _matchTitle;
        private string _queryStatus;
        private string _selectedSeasonOption;
        private int _rowsReturned;
        private long _replayId;
        private GameMode _gameMode;
        private DateTime? _gameDate;
        private TimeSpan _gameTime;
        private Models.DbModels.Replay _selectedReplay;
        private BitmapImage _backgroundMapImage;
        private Color _mapNameGlowColor;
        private bool _hasBans;
        private bool _hasObservers;
        private bool _hasChat;

        private List<string> _seasonList = new List<string>();

        private ObservableCollection<Models.DbModels.Replay> _matchList = new ObservableCollection<Models.DbModels.Replay>();
        private ObservableCollection<MatchTalents> _matchTalentsTeam1 = new ObservableCollection<MatchTalents>();
        private ObservableCollection<MatchTalents> _matchTalentsTeam2 = new ObservableCollection<MatchTalents>();
        private ObservableCollection<MatchTalents> _matchObservers = new ObservableCollection<MatchTalents>();
        private ObservableCollection<MatchScores> _matchScoreTeam1 = new ObservableCollection<MatchScores>();
        private ObservableCollection<MatchScores> _matchScoreTeam2 = new ObservableCollection<MatchScores>();
        private ObservableCollection<MatchChat> _matchChatMessages = new ObservableCollection<MatchChat>();

        private Dictionary<int, PartyIconColor> PlayerPartyIcons = new Dictionary<int, PartyIconColor>();
        #endregion properties

        #region public properties
        public MatchHeroBans MatchHeroBans { get; set; } = new MatchHeroBans();

        public int RowsReturned
        {
            get { return _rowsReturned; }
            set
            {
                _rowsReturned = value;
                RaisePropertyChangedEvent(nameof(RowsReturned));
            }
        }

        public ObservableCollection<MatchTalents> MatchTalentsTeam1
        {
            get { return _matchTalentsTeam1; }
            set
            {
                _matchTalentsTeam1 = value;
                RaisePropertyChangedEvent(nameof(MatchTalentsTeam1));
            }
        }

        public ObservableCollection<MatchTalents> MatchTalentsTeam2
        {
            get { return _matchTalentsTeam2; }
            set
            {
                _matchTalentsTeam2 = value;
                RaisePropertyChangedEvent(nameof(MatchTalentsTeam2));
            }
        }

        public ObservableCollection<MatchTalents> MatchObservers
        {
            get { return _matchObservers; }
            set
            {
                _matchObservers = value;
                RaisePropertyChangedEvent(nameof(MatchObservers));
            }
        }

        public ObservableCollection<MatchScores> MatchScoreTeam1
        {
            get { return _matchScoreTeam1; }
            set
            {
                _matchScoreTeam1 = value;
                RaisePropertyChangedEvent(nameof(MatchScoreTeam1));
            }
        }

        public ObservableCollection<MatchScores> MatchScoreTeam2
        {
            get { return _matchScoreTeam2; }
            set
            {
                _matchScoreTeam2 = value;
                RaisePropertyChangedEvent(nameof(MatchScoreTeam2));
            }
        }

        public ObservableCollection<Models.DbModels.Replay> MatchList
        {
            get { return _matchList; }
            set
            {
                _matchList = value;
                RaisePropertyChangedEvent(nameof(MatchList));
            }
        }

        public ObservableCollection<MatchChat> MatchChatMessages
        {
            get { return _matchChatMessages; }
            set
            {
                _matchChatMessages = value;
                RaisePropertyChangedEvent(nameof(MatchChatMessages));
            }
        }

        public long ReplayId
        {
            get { return _replayId; }
            set
            {
                _replayId = value;
                RaisePropertyChangedEvent(nameof(ReplayId));
            }
        }

        public GameMode GameMode
        {
            get { return _gameMode; }
            set
            {
                _gameMode = value;
                RaisePropertyChangedEvent(nameof(GameMode));
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

        public DateTime? GameDate
        {
            get { return _gameDate; }
            set
            {
                _gameDate = value;
                RaisePropertyChangedEvent(nameof(GameDate));
            }
        }

        public TimeSpan GameTime
        {
            get { return _gameTime; }
            set
            {
                _gameTime = value;
                RaisePropertyChangedEvent(nameof(GameTime));
            }
        }

        public Models.DbModels.Replay SelectedReplay
        {
            get { return _selectedReplay; }
            set
            {
                _selectedReplay = value;
                RaisePropertyChangedEvent(nameof(SelectedReplay));
            }
        }

        public BitmapImage BackgroundMapImage
        {
            get { return _backgroundMapImage; }
            set
            {
                _backgroundMapImage = value;
                RaisePropertyChangedEvent(nameof(BackgroundMapImage));
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

        public string QueryStatus
        {
            get { return _queryStatus; }
            set
            {
                _queryStatus = value;
                RaisePropertyChangedEvent(nameof(QueryStatus));
            }
        }

        public bool ShowPlayerTagColumn
        {
            get { return !Settings.Default.IsBattleTagHidden; }
            set
            {
                Settings.Default.IsBattleTagHidden = !value;
                RaisePropertyChangedEvent(nameof(ShowPlayerTagColumn));
            }
        }

        public string SelectedSeasonOption
        {
            get { return _selectedSeasonOption; }
            set
            {
                _selectedSeasonOption = value;
                RaisePropertyChangedEvent(nameof(SelectedSeasonOption));
            }
        }

        public List<string> SeasonList
        {
            get { return _seasonList; }
            set
            {
                _seasonList = value;
                RaisePropertyChangedEvent(nameof(SeasonList));
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

        public Season GetSelectedSeason
        {
            get { return Utilities.GetSeasonFromString(SelectedSeasonOption); }
        }
        #endregion public properties

        public ICommand Refresh
        {
            get
            {
                return new DelegateCommand(async () =>
                {
                    try
                    {
                        QueryStatus = "Waiting for query...";
                        await RefreshExecute();
                        QueryStatus = "Match list queried successfully";
                    }
                    catch (Exception)
                    {
                        QueryStatus = "Match list queried failed";
                    }
                });
            }
        }

        public ICommand DisplayReplayDetails
        {
            get
            {
                return new DelegateCommand(async () =>
                {
                    try
                    {
                        QueryStatus = "Waiting for query...";
                        await LoadReplayDetails(SelectedReplay);
                        QueryStatus = "Match details queried successfully";
                    }
                    catch (Exception)
                    {
                        QueryStatus = "Match details queried failed";
                    }
                });
            }
        }

        /// <summary>
        /// Constructor
        /// </summary>
        protected MatchContext()
            : base()
        {
            HasChat = true;

            SeasonList.Add("Lifetime");
            SeasonList.AddRange(AllSeasonsList);
            SelectedSeasonOption = SeasonList[0];
        }

        protected abstract Task RefreshExecute();

        private async Task LoadReplayDetails(Models.DbModels.Replay replay)
        {
            if (replay == null)
                return;

            await QuerySummaryDetails(replay.ReplayId);
        }

        protected async Task QuerySummaryDetails(long replayId)
        {
            try
            {
                ClearSummaryDetails();

                MatchTalentsTeam1 = new ObservableCollection<MatchTalents>();
                MatchTalentsTeam2 = new ObservableCollection<MatchTalents>();
                MatchScoreTeam1 = new ObservableCollection<MatchScores>();
                MatchScoreTeam2 = new ObservableCollection<MatchScores>();
                MatchObservers = new ObservableCollection<MatchTalents>();
                MatchChatMessages = new ObservableCollection<MatchChat>();

                // get replay
                Models.DbModels.Replay replay = await Query.Replay.ReadReplayIncludeRecord(replayId);

                // get players info 
                var playersList = replay.ReplayMatchPlayers.ToList();
                var playerTalentsList = replay.ReplayMatchPlayerTalents.ToList();
                var playerScoresList = replay.ReplayMatchPlayerScoreResults.ToList();
                var matchMessagesList = replay.ReplayMatchMessage.ToList();
                var matchAwardDictionary = replay.ReplayMatchAward.ToDictionary(x => x.PlayerId, x => x.Award);

                int highestSiegeTeam1Index = 0, highestSiegeTeam1Count = 0, highestSiegeTeam2Index = 0, highestSiegeTeam2Count = 0;
                int highestHeroDamageTeam1Index = 0, highestHeroDamageTeam1Count = 0, highestHeroDamageTeam2Index = 0, highestHeroDamageTeam2Count = 0;
                int highestExpTeam1Index = 0, highestExpTeam1Count = 0, highestExpTeam2Index = 0, highestExpTeam2Count = 0;

                FindPlayerParties(playersList);

                foreach (var player in playersList)
                {
                    // fill in common infomation for player
                    string mvpAwardName = null;

                    MatchPlayerInfoBase matchPlayerInfoBase = new MatchPlayerInfoBase();

                    var playerInfo = await Query.HotsPlayer.ReadRecordFromPlayerId(player.PlayerId);
                    matchPlayerInfoBase.LeaderboardPortrait = player.Character != "None" ? HeroesInfo.GetHeroLeaderboardPortrait(player.Character) : null;
                    matchPlayerInfoBase.CharacterName = player.Character;
                    matchPlayerInfoBase.PlayerName = Utilities.GetNameFromBattleTagName(playerInfo.BattleTagName);
                    matchPlayerInfoBase.PlayerTag = Utilities.GetTagFromBattleTagName(playerInfo.BattleTagName).ToString();
                    matchPlayerInfoBase.PlayerNumber = player.PlayerNumber;
                    matchPlayerInfoBase.PlayerSilenced = player.IsSilenced;
                    matchPlayerInfoBase.MvpAward = matchAwardDictionary.ContainsKey(player.PlayerId) == true? SetPlayerMVPAward(player.Team, matchAwardDictionary[player.PlayerId], out mvpAwardName) : null;
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
                        matchTalents.RowBackColor = Color.FromRgb(233, 252, 233);
                        matchScores.RowBackColor = Color.FromRgb(233, 252, 233);
                    }

                    if (player.Team == 0 || player.Team == 1)
                    {
                        if (player.Team == 0)
                        {
                            matchTalents.PortraitBackColor = Color.FromRgb(155, 155, 235);
                            matchScores.PortraitBackColor = Color.FromRgb(179, 179, 255);

                            HighestSiegeDamage(MatchScoreTeam1, matchScores, ref highestSiegeTeam1Index, ref highestSiegeTeam1Count);
                            HighestHeroDamage(MatchScoreTeam1, matchScores, ref highestHeroDamageTeam1Index, ref highestHeroDamageTeam1Count);
                            HighestExpContribution(MatchScoreTeam1, matchScores, ref highestExpTeam1Index, ref highestExpTeam1Count);

                            // add to collection
                            MatchTalentsTeam1.Add(matchTalents);
                            MatchScoreTeam1.Add(matchScores);                          
                        }
                        else
                        {
                            matchTalents.PortraitBackColor = Color.FromRgb(235, 155, 155);
                            matchScores.PortraitBackColor = Color.FromRgb(235, 159, 159);

                            HighestSiegeDamage(MatchScoreTeam2, matchScores, ref highestSiegeTeam2Index, ref highestSiegeTeam2Count);
                            HighestHeroDamage(MatchScoreTeam2, matchScores, ref highestHeroDamageTeam2Index, ref highestHeroDamageTeam2Count);
                            HighestExpContribution(MatchScoreTeam2, matchScores, ref highestExpTeam2Index, ref highestExpTeam2Count);

                            // add to collection
                            MatchTalentsTeam2.Add(matchTalents);
                            MatchScoreTeam2.Add(matchScores);
                        }
                    }
                    else if (player.Team == 4) // observers
                    {
                        HasObservers = true;
                        matchTalents.CharacterLevel = "Observer";
                        MatchObservers.Add(matchTalents);
                    }
                } // end foreach players

                MatchTitle = $"{replay.MapName} - {replay.GameMode} [{replay.TimeStamp}] [{replay.ReplayLength}]";

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

                            MatchChatMessages.Add(matchChat);
                        }
                    }
                }

                HasChat = MatchChatMessages.Count < 1 ? false : true;
            }
            catch (Exception ex)
            {
                ExceptionLog.Log(LogLevel.Warn, ex);
                throw;
            }
        }

        // clear everything and free up resources
        private void ClearSummaryDetails()
        {
            // talents
            foreach (var matchTalent in MatchTalentsTeam1)
            {
                matchTalent.LeaderboardPortrait = null;
                matchTalent.Talent1 = null;
                matchTalent.Talent4 = null;
                matchTalent.Talent7 = null;
                matchTalent.Talent10 = null;
                matchTalent.Talent13 = null;
                matchTalent.Talent16 = null;
                matchTalent.Talent20 = null;
                matchTalent.MvpAward = null;
            }
            MatchTalentsTeam1 = null;

            foreach (var matchTalent in MatchTalentsTeam2)
            {
                matchTalent.LeaderboardPortrait = null;
                matchTalent.Talent1 = null;
                matchTalent.Talent4 = null;
                matchTalent.Talent7 = null;
                matchTalent.Talent10 = null;
                matchTalent.Talent13 = null;
                matchTalent.Talent16 = null;
                matchTalent.Talent20 = null;
                matchTalent.MvpAward = null;
            }
            MatchTalentsTeam2 = null;

            // score summary
            foreach (var matchScore in MatchScoreTeam1)
            {
                matchScore.LeaderboardPortrait = null;
                matchScore.MvpAward = null;
            }
            MatchScoreTeam1 = null;

            foreach (var matchScore in MatchScoreTeam2)
            {
                matchScore.LeaderboardPortrait = null;
                matchScore.MvpAward = null;
            }
            MatchScoreTeam2 = null;

            // observers
            HasObservers = false;
            MatchObservers = null;

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
            MatchChatMessages = null;
            HasChat = true;

            BackgroundMapImage = null;
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
            switch (mapName)
            {
                case "Battlefield of Eternity":
                    glowColor = Colors.Red;
                    return HeroesInfo.GetMapBackground(MapName.BattlefieldofEternity);
                case "Blackheart's Bay":
                    glowColor = Colors.Green;
                    return HeroesInfo.GetMapBackground(MapName.BlackheartsBay);
                case "Cursed Hollow":
                    glowColor = Colors.Purple;
                    return HeroesInfo.GetMapBackground(MapName.CursedHollow);
                case "Dragon Shire":
                    glowColor = Colors.Red;
                    return HeroesInfo.GetMapBackground(MapName.DragonShire);
                case "Garden of Terror":
                    glowColor = Colors.LightBlue;
                    return HeroesInfo.GetMapBackground(MapName.GardenofTerror);
                case "Haunted Mines":
                    glowColor = Colors.Red;
                    return HeroesInfo.GetMapBackground(MapName.HauntedMines);
                case "Infernal Shrines":
                    glowColor = Colors.Red;
                    return HeroesInfo.GetMapBackground(MapName.InfernalShrines);
                case "Lost Cavern":
                    glowColor = Colors.LightBlue;
                    return HeroesInfo.GetMapBackground(MapName.LostCavern);
                case "Sky Temple":
                    glowColor = Colors.Gold;
                    return HeroesInfo.GetMapBackground(MapName.SkyTemple);
                case "Tomb of the Spider Queen":
                    glowColor = Colors.LightBlue;
                    return HeroesInfo.GetMapBackground(MapName.TomboftheSpiderQueen);
                case "Towers of Doom":
                    glowColor = Colors.Orange;
                    return HeroesInfo.GetMapBackground(MapName.TowersofDoom);
                case "Braxis Holdout":
                    glowColor = Colors.Blue;
                    return HeroesInfo.GetMapBackground(MapName.BraxisHoldout);
                case "Warhead Junction":
                    glowColor = Colors.Yellow;
                    return HeroesInfo.GetMapBackground(MapName.WarheadJunction);
                default:
                    glowColor = Colors.White;
                    return null;
            }
        }

        private BitmapImage SetPlayerMVPAward(int? team, string award, out string awardName)
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

            MVPAwardType mvpAwardType = HeroesInfo.GetMVPAwardTypeFromString(award);

            if (mvpAwardType == MVPAwardType.Unknown)
            {
                ExceptionLog.Log(LogLevel.Info, $"[MatchContext.cs]({nameof(SetPlayerMVPAward)}) Could not find {award} award");
                awardName = null;
                return null;
            }

            return HeroesInfo.GetMVPScoreScreenAward(mvpAwardType, teamColor, out awardName);
        }

        private void FindPlayerParties(List<ReplayMatchPlayer> playersList)
        {
            Dictionary<long, List<int>> parties = new Dictionary<long, List<int>>();

            foreach (var player in playersList)
            {
                if (player.PartyValue > 0)
                {
                    if (!parties.ContainsKey(player.PartyValue))
                    {
                        var listOfMembers = new List<int>();
                        listOfMembers.Add(player.PlayerNumber);
                        parties.Add(player.PartyValue, listOfMembers);
                    }
                    else
                    {
                        var listOfMembers = parties[player.PartyValue];
                        listOfMembers.Add(player.PlayerNumber);
                        parties[player.PartyValue] = listOfMembers;
                    }
                }
            }

            PlayerPartyIcons = new Dictionary<int, PartyIconColor>();
            PartyIconColor color = 0;

            foreach (var party in parties)
            {
                foreach (int playerNum in party.Value)
                {
                    PlayerPartyIcons.Add(playerNum, color);
                }
                color++;
            }
        }
    }
}
