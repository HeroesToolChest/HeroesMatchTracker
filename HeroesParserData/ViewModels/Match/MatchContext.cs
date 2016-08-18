using Heroes.ReplayParser;
using HeroesIcons;
using HeroesParserData.DataQueries.ReplayData;
using HeroesParserData.Models;
using HeroesParserData.Models.DbModels;
using NLog;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity.Validation;
using System.Data.SqlClient;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media;

namespace HeroesParserData.ViewModels.Match
{
    public abstract class MatchContext : ViewModelBase
    {
        #region properties
        private ObservableCollection<Models.DbModels.Replay> _matchList = new ObservableCollection<Models.DbModels.Replay>();
        private ObservableCollection<MatchTalents> _matchTalentsTeam1 = new ObservableCollection<MatchTalents>();
        private ObservableCollection<MatchTalents> _matchTalentsTeam2 = new ObservableCollection<MatchTalents>();
        private ObservableCollection<MatchTalents> _matchObservers = new ObservableCollection<MatchTalents>();
        private long _replayId;
        private GameMode _gameMode;
        private string _mapName;
        private DateTime? _gameDate;
        private TimeSpan _gameTime;
        private Models.DbModels.Replay _selectedReplay;
        private int _rowsReturned;
        private bool _hasBans;
        private bool _hasObservers;
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

        public ObservableCollection<Models.DbModels.Replay> MatchList
        {
            get { return _matchList; }
            set
            {
                _matchList = value;
                RaisePropertyChangedEvent(nameof(MatchList));
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

        public string MapName
        {
            get { return _mapName; }
            set
            {
                _mapName = value;
                RaisePropertyChangedEvent(nameof(MapName));
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
        #endregion public properties

        protected HeroesInfo HeroesInfo = new HeroesInfo();

        public ICommand Refresh
        {
            get { return new DelegateCommand(async () => await RefreshExecute()); }
        }

        public ICommand DisplayReplayDetails
        {
            get { return new DelegateCommand(async () => await LoadReplayDetails(SelectedReplay)); }
        }

        /// <summary>
        /// Constructor
        /// </summary>
        protected MatchContext()
            :base()
        {

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
                MatchObservers = new ObservableCollection<MatchTalents>();

                // get replay
                Models.DbModels.Replay replay = await Query.Replay.ReadReplayIncludeRecord(replayId);

                // get player info
                List<ReplayMatchPlayer> players = await Query.MatchPlayer.ReadRecordsByReplayId(replay.ReplayId);
                // get player talents
                List<ReplayMatchPlayerTalent> playerTalents = await Query.MatchPlayerTalent.ReadRecordsByReplayId(replay.ReplayId);

                foreach (var player in players)
                {
                    MatchTalents matchTalents = new MatchTalents();

                    var playerInfo = await Query.HotsPlayer.ReadRecordFromPlayerId(player.PlayerId);
                    matchTalents.LeaderboardPortrait = player.Character != "None" ? HeroesInfo.GetHeroLeaderboardPortrait(player.Character) : null;
                    matchTalents.CharacterName = player.Character;
                    matchTalents.PlayerName = Utilities.GetNameFromBattleTagName(playerInfo.BattleTagName);
                    matchTalents.PlayerTag = Utilities.GetTagFromBattleTagName(playerInfo.BattleTagName).ToString();

                    if (!player.IsAutoSelect)
                        matchTalents.CharacterLevel = player.CharacterLevel.ToString();
                    else
                        matchTalents.CharacterLevel = "Auto Select";

                    if (player.Character != "None")
                    {
                        matchTalents.Talent1 = HeroesInfo.GetTalentIcon(playerTalents[player.PlayerNumber].TalentName1);
                        matchTalents.Talent4 = HeroesInfo.GetTalentIcon(playerTalents[player.PlayerNumber].TalentName4);
                        matchTalents.Talent7 = HeroesInfo.GetTalentIcon(playerTalents[player.PlayerNumber].TalentName7);
                        matchTalents.Talent10 = HeroesInfo.GetTalentIcon(playerTalents[player.PlayerNumber].TalentName10);
                        matchTalents.Talent13 = HeroesInfo.GetTalentIcon(playerTalents[player.PlayerNumber].TalentName13);
                        matchTalents.Talent16 = HeroesInfo.GetTalentIcon(playerTalents[player.PlayerNumber].TalentName16);
                        matchTalents.Talent20 = HeroesInfo.GetTalentIcon(playerTalents[player.PlayerNumber].TalentName20);

                        matchTalents.TalentName1 = HeroesInfo.GetTrueTalentName(playerTalents[player.PlayerNumber].TalentName1);
                        matchTalents.TalentName4 = HeroesInfo.GetTrueTalentName(playerTalents[player.PlayerNumber].TalentName4);
                        matchTalents.TalentName7 = HeroesInfo.GetTrueTalentName(playerTalents[player.PlayerNumber].TalentName7);
                        matchTalents.TalentName10 = HeroesInfo.GetTrueTalentName(playerTalents[player.PlayerNumber].TalentName10);
                        matchTalents.TalentName13 = HeroesInfo.GetTrueTalentName(playerTalents[player.PlayerNumber].TalentName13);
                        matchTalents.TalentName16 = HeroesInfo.GetTrueTalentName(playerTalents[player.PlayerNumber].TalentName16);
                        matchTalents.TalentName20 = HeroesInfo.GetTrueTalentName(playerTalents[player.PlayerNumber].TalentName20);
                    }

                    if (player.IsWinner)
                        matchTalents.TalentsBackColor = Color.FromRgb(233, 252, 233);

                    if (player.Team == 0 || player.Team == 1)
                    {
                        if (player.Team == 0)
                        {
                            matchTalents.TalentsPortraitBackColor = Color.FromRgb(179, 179, 255);
                            MatchTalentsTeam1.Add(matchTalents);
                        }
                        else
                        {
                            matchTalents.TalentsPortraitBackColor = Color.FromRgb(255, 179, 179);
                            MatchTalentsTeam2.Add(matchTalents);
                        }
                        
                    }
                    else if (player.Team == 4) // observers
                    {
                        HasObservers = true;
                        MatchObservers.Add(matchTalents);
                    }             
                }

                ReplayId = replay.ReplayId;
                GameMode = replay.GameMode;
                MapName = replay.MapName;
                GameDate = replay.TimeStamp;
                GameTime = replay.ReplayLength;

                // hero bans
                if (replay.ReplayMatchTeamBan != null)
                {
                    HasBans = true;
                    MatchHeroBans.Team0Ban0 = HeroesInfo.GetHeroPortrait(replay.ReplayMatchTeamBan.Team0Ban0);
                    MatchHeroBans.Team0Ban1 = HeroesInfo.GetHeroPortrait(replay.ReplayMatchTeamBan.Team0Ban1);
                    MatchHeroBans.Team1Ban0 = HeroesInfo.GetHeroPortrait(replay.ReplayMatchTeamBan.Team1Ban0);
                    MatchHeroBans.Team1Ban1 = HeroesInfo.GetHeroPortrait(replay.ReplayMatchTeamBan.Team1Ban1);
                    MatchHeroBans.Team0Ban0HeroName = HeroesInfo.GeRealHeroNameFromAttId(replay.ReplayMatchTeamBan.Team0Ban0);
                    MatchHeroBans.Team0Ban1HeroName = HeroesInfo.GeRealHeroNameFromAttId(replay.ReplayMatchTeamBan.Team0Ban1);
                    MatchHeroBans.Team1Ban0HeroName = HeroesInfo.GeRealHeroNameFromAttId(replay.ReplayMatchTeamBan.Team1Ban0);
                    MatchHeroBans.Team1Ban1HeroName = HeroesInfo.GeRealHeroNameFromAttId(replay.ReplayMatchTeamBan.Team1Ban1);
                }
            }
            catch (Exception ex) when (ex is SqlException || ex is DbEntityValidationException)
            {
                SqlExceptionReplaysLog.Log(LogLevel.Error, ex);
            }
            catch (Exception ex)
            {
                ExceptionLog.Log(LogLevel.Warn, ex);
            }
        }

        private void ClearSummaryDetails()
        {
            // talents
            foreach (var matchTalent in MatchTalentsTeam1)
            {
                // free up resources
                matchTalent.LeaderboardPortrait = null;
                matchTalent.Talent1 = null;
                matchTalent.Talent4 = null;
                matchTalent.Talent7 = null;
                matchTalent.Talent10 = null;
                matchTalent.Talent13 = null;
                matchTalent.Talent16 = null;
                matchTalent.Talent20 = null;
            }
            MatchTalentsTeam1 = null;

            foreach (var matchTalent in MatchTalentsTeam2)
            {
                // free up resources
                matchTalent.LeaderboardPortrait = null;
                matchTalent.Talent1 = null;
                matchTalent.Talent4 = null;
                matchTalent.Talent7 = null;
                matchTalent.Talent10 = null;
                matchTalent.Talent13 = null;
                matchTalent.Talent16 = null;
                matchTalent.Talent20 = null;
            }
            MatchTalentsTeam2 = null;

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
        }
    }
}
