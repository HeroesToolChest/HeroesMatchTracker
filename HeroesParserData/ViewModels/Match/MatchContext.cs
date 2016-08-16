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
        private ObservableCollection<MatchInfo> _matchInfoTeam1 = new ObservableCollection<MatchInfo>();
        private ObservableCollection<MatchInfo> _matchInfoTeam2 = new ObservableCollection<MatchInfo>();
        private ObservableCollection<MatchInfo> _matchObservers = new ObservableCollection<MatchInfo>();
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

        public ObservableCollection<MatchInfo> MatchInfoTeam1
        {
            get { return _matchInfoTeam1; }
            set
            {
                _matchInfoTeam1 = value;
                RaisePropertyChangedEvent(nameof(MatchInfoTeam1));
            }
        }

        public ObservableCollection<MatchInfo> MatchInfoTeam2
        {
            get { return _matchInfoTeam2; }
            set
            {
                _matchInfoTeam2 = value;
                RaisePropertyChangedEvent(nameof(MatchInfoTeam2));
            }
        }

        public ObservableCollection<MatchInfo> MatchObservers
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

                MatchInfoTeam1 = new ObservableCollection<MatchInfo>();
                MatchInfoTeam2 = new ObservableCollection<MatchInfo>();
                MatchObservers = new ObservableCollection<MatchInfo>();

                Models.DbModels.Replay replay = await Query.Replay.ReadReplayIncludeRecord(replayId);

                List<ReplayMatchPlayer> players = await Query.MatchPlayer.ReadRecordsByReplayId(replay.ReplayId);
                List<ReplayMatchPlayerTalent> playerTalents = await Query.MatchPlayerTalent.ReadRecordsByReplayId(replay.ReplayId);

                foreach (var player in players)
                {
                    MatchInfo matchinfo = new MatchInfo();

                    var playerInfo = await Query.HotsPlayer.ReadRecordFromPlayerId(player.PlayerId);
                    matchinfo.PlayerName = Utilities.GetNameFromBattleTagName(playerInfo.BattleTagName);
                    matchinfo.BattleNetTag = Utilities.GetTagFromBattleTagName(playerInfo.BattleTagName);
                    matchinfo.BattleNetId = playerInfo.BattleNetId;
                    matchinfo.CharacterName = player.Character;
                    matchinfo.CharacterLevel = player.CharacterLevel;

                    if (player.Character != "None")
                    {
                        matchinfo.Talent1 = HeroesInfo.GetTalentIcon(playerTalents[player.PlayerNumber].TalentName1);
                        matchinfo.Talent4 = HeroesInfo.GetTalentIcon(playerTalents[player.PlayerNumber].TalentName4);
                        matchinfo.Talent7 = HeroesInfo.GetTalentIcon(playerTalents[player.PlayerNumber].TalentName7);
                        matchinfo.Talent10 = HeroesInfo.GetTalentIcon(playerTalents[player.PlayerNumber].TalentName10);
                        matchinfo.Talent13 = HeroesInfo.GetTalentIcon(playerTalents[player.PlayerNumber].TalentName13);
                        matchinfo.Talent16 = HeroesInfo.GetTalentIcon(playerTalents[player.PlayerNumber].TalentName16);
                        matchinfo.Talent20 = HeroesInfo.GetTalentIcon(playerTalents[player.PlayerNumber].TalentName20);

                        matchinfo.TalentName1 = HeroesInfo.GetTrueTalentName(playerTalents[player.PlayerNumber].TalentName1);
                        matchinfo.TalentName4 = HeroesInfo.GetTrueTalentName(playerTalents[player.PlayerNumber].TalentName4);
                        matchinfo.TalentName7 = HeroesInfo.GetTrueTalentName(playerTalents[player.PlayerNumber].TalentName7);
                        matchinfo.TalentName10 = HeroesInfo.GetTrueTalentName(playerTalents[player.PlayerNumber].TalentName10);
                        matchinfo.TalentName13 = HeroesInfo.GetTrueTalentName(playerTalents[player.PlayerNumber].TalentName13);
                        matchinfo.TalentName16 = HeroesInfo.GetTrueTalentName(playerTalents[player.PlayerNumber].TalentName16);
                        matchinfo.TalentName20 = HeroesInfo.GetTrueTalentName(playerTalents[player.PlayerNumber].TalentName20);
                    }

                    if (player.IsWinner)
                        matchinfo.TalentsBackColor = Color.FromRgb(233, 252, 233);

                    if (player.Team == 0)
                        MatchInfoTeam1.Add(matchinfo);
                    else if (player.Team == 1)
                        MatchInfoTeam2.Add(matchinfo);
                    else if (player.Team == 4)
                    {
                        HasObservers = true;
                        MatchObservers.Add(matchinfo);
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
            foreach (var matchInfo in MatchInfoTeam1)
            {
                // free up resources
                matchInfo.Talent1 = null;
                matchInfo.Talent4 = null;
                matchInfo.Talent7 = null;
                matchInfo.Talent10 = null;
                matchInfo.Talent13 = null;
                matchInfo.Talent16 = null;
                matchInfo.Talent20 = null;
            }
            MatchInfoTeam1 = null;

            foreach (var matchInfo in MatchInfoTeam2)
            {
                // free up resources
                matchInfo.Talent1 = null;
                matchInfo.Talent4 = null;
                matchInfo.Talent7 = null;
                matchInfo.Talent10 = null;
                matchInfo.Talent13 = null;
                matchInfo.Talent16 = null;
                matchInfo.Talent20 = null;
            }
            MatchInfoTeam2 = null;

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
