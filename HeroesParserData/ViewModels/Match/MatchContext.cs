using Heroes.ReplayParser;
using HeroesIcons;
using HeroesParserData.DataQueries.ReplayData;
using HeroesParserData.Models;
using HeroesParserData.Models.DbModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media;

namespace HeroesParserData.ViewModels.Match
{
    public abstract class MatchContext : ViewModelBase
    {
        #region properties
        private ObservableCollection<MatchInfo> _matchInfoTeam1 = new ObservableCollection<MatchInfo>();
        private ObservableCollection<MatchInfo> _matchInfoTeam2 = new ObservableCollection<MatchInfo>();
        private ObservableCollection<Models.DbModels.Replay> _matchList = new ObservableCollection<Models.DbModels.Replay>();
        private long _replayId;
        private GameMode _gameMode;
        private string _mapName;
        private DateTime? _gameDate;
        private TimeSpan _gameTime;
        private Models.DbModels.Replay _selectedReplay;
        private int _rowsReturned;
        #endregion properties

        #region public properties
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
        #endregion public properties

        protected TalentIcons TalentIcons = new TalentIcons();

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

            await QueryGameDetails(replay.ReplayId);
        }

        protected async Task QueryGameDetails(long replayId)
        {
            ClearGameDetails();

            MatchInfoTeam1 = new ObservableCollection<MatchInfo>();
            MatchInfoTeam2 = new ObservableCollection<MatchInfo>();

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
                matchinfo.Talent1 = TalentIcons.GetTalentIcon(playerTalents[player.PlayerNumber].TalentName1);
                matchinfo.Talent4 = TalentIcons.GetTalentIcon(playerTalents[player.PlayerNumber].TalentName2);
                matchinfo.Talent7 = TalentIcons.GetTalentIcon(playerTalents[player.PlayerNumber].TalentName3);
                matchinfo.Talent10 = TalentIcons.GetTalentIcon(playerTalents[player.PlayerNumber].TalentName4);
                matchinfo.Talent13 = TalentIcons.GetTalentIcon(playerTalents[player.PlayerNumber].TalentName5);
                matchinfo.Talent16 = TalentIcons.GetTalentIcon(playerTalents[player.PlayerNumber].TalentName6);
                matchinfo.Talent20 = TalentIcons.GetTalentIcon(playerTalents[player.PlayerNumber].TalentName7);

                matchinfo.TalentName1 = TalentIcons.GetTrueTalentName(playerTalents[player.PlayerNumber].TalentName1);
                matchinfo.TalentName4 = TalentIcons.GetTrueTalentName(playerTalents[player.PlayerNumber].TalentName2);
                matchinfo.TalentName7 = TalentIcons.GetTrueTalentName(playerTalents[player.PlayerNumber].TalentName3);
                matchinfo.TalentName10 = TalentIcons.GetTrueTalentName(playerTalents[player.PlayerNumber].TalentName4);
                matchinfo.TalentName13 = TalentIcons.GetTrueTalentName(playerTalents[player.PlayerNumber].TalentName5);
                matchinfo.TalentName16 = TalentIcons.GetTrueTalentName(playerTalents[player.PlayerNumber].TalentName6);
                matchinfo.TalentName20 = TalentIcons.GetTrueTalentName(playerTalents[player.PlayerNumber].TalentName7);

                if (player.IsWinner)
                    matchinfo.TalentsBackColor = Color.FromRgb(233, 252, 233);

                if (player.Team == 0)
                    MatchInfoTeam1.Add(matchinfo);
                else if (player.Team == 1)
                    MatchInfoTeam2.Add(matchinfo);
                //else 
                // observer
            }

            ReplayId = replay.ReplayId;
            GameMode = replay.GameMode;
            MapName = replay.MapName;
            GameDate = replay.TimeStamp;
            GameTime = replay.ReplayLength;

        }

        private void ClearGameDetails()
        {
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
        }
    }
}
