using GalaSoft.MvvmLight.Messaging;
using HeroesParserData.DataQueries;
using HeroesParserData.Messages;
using HeroesParserData.Models.DbModels;
using HeroesParserData.Models.MatchModels;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;

namespace HeroesParserData.ViewModels.Match
{
    public abstract class MatchOverviewContext : MatchContextBase
    {
        private int _rowsReturned;

        private string _queryStatus;
        private string _selectedSeasonOption;

        private Replay _selectedReplay;

        private ObservableCollection<Replay> _matchListCollection = new ObservableCollection<Replay>();
        private ObservableCollection<MatchPlayerInfoBase> _matchOverviewTeam1Collection = new ObservableCollection<MatchPlayerInfoBase>();
        private ObservableCollection<MatchPlayerInfoBase> _matchOverviewTeam2Collection = new ObservableCollection<MatchPlayerInfoBase>();

        #region public properties
        public string QueryStatus
        {
            get { return _queryStatus; }
            set
            {
                _queryStatus = value;
                RaisePropertyChangedEvent(nameof(QueryStatus));
            }
        }

        public int RowsReturned
        {
            get { return _rowsReturned; }
            set
            {
                _rowsReturned = value;
                RaisePropertyChangedEvent(nameof(RowsReturned));
            }
        }

        public Season GetSelectedSeason
        {
            get { return Utilities.GetSeasonFromString(SelectedSeasonOption); }
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

        public Models.DbModels.Replay SelectedReplay
        {
            get { return _selectedReplay; }
            set
            {
                _selectedReplay = value;
                RaisePropertyChangedEvent(nameof(SelectedReplay));
            }
        }

        public ObservableCollection<Models.DbModels.Replay> MatchListCollection
        {
            get { return _matchListCollection; }
            set
            {
                _matchListCollection = value;
                RaisePropertyChangedEvent(nameof(MatchListCollection));
            }
        }

        public ObservableCollection<MatchPlayerInfoBase> MatchOverviewTeam1Collection
        {
            get { return _matchOverviewTeam1Collection; }
            set
            {
                _matchOverviewTeam1Collection = value;
                RaisePropertyChangedEvent(nameof(MatchOverviewTeam1Collection));
            }
        }

        public ObservableCollection<MatchPlayerInfoBase> MatchOverviewTeam2Collection
        {
            get { return _matchOverviewTeam2Collection; }
            set
            {
                _matchOverviewTeam2Collection = value;
                RaisePropertyChangedEvent(nameof(MatchOverviewTeam2Collection));
            }
        }
        #endregion public properties

        public ICommand LoadMatchListCommand
        {
            get
            {
                return new DelegateCommand(() =>
                {
                    try
                    {
                        QueryStatus = "Waiting for query...";
                        ExecuteLoadMatchListCommmand();
                        QueryStatus = "Match list queried successfully";
                    }
                    catch (Exception)
                    {
                        QueryStatus = "Match list queried failed";
                    }
                });
            }
        }

        public ICommand ShowMatchOverviewCommand
        {
            get { return new DelegateCommand(() => LoadMatchOverviewCommand(SelectedReplay)); }
        }

        public ICommand ShowMatchSummaryCommand
        {
            get { return new DelegateCommand(() => ExecuteShowMatchSummaryCommand()); }
        }

        protected MatchOverviewContext()
            :base()
        {
            SelectedSeasonOption = "Season 2";
        }

        protected abstract void ExecuteLoadMatchListCommmand();

        private void LoadMatchOverviewCommand(Models.DbModels.Replay replay)
        {
            if (replay == null)
                return;

            QueryMatchOverview(replay.ReplayId);
        }

        private void QueryMatchOverview(long replayId)
        {
            ClearMatchOverview();

            // get info
            Models.DbModels.Replay replay = Query.Replay.ReadReplayIncludeRecord(replayId);
            var playersList = replay.ReplayMatchPlayers.ToList();

            FindPlayerParties(playersList);

            foreach (var player in playersList)
            {
                if (player.Team == 4)
                    continue;

                MatchPlayerInfoBase matchPlayerInfoBase = new MatchPlayerInfoBase();
                var playerInfo = Query.HotsPlayer.ReadRecordFromPlayerId(player.PlayerId);

                matchPlayerInfoBase.LeaderboardPortrait = player.Character != "None" ? HeroesInfo.GetHeroLeaderboardPortrait(player.Character) : null;
                matchPlayerInfoBase.CharacterName = player.Character;
                matchPlayerInfoBase.PlayerSilenced = player.IsSilenced;

                if (ShowPlayerTagColumn)
                    matchPlayerInfoBase.PlayerName = playerInfo.BattleTagName;
                else
                    matchPlayerInfoBase.PlayerName = Utilities.GetNameFromBattleTagName(playerInfo.BattleTagName);

                if (player.IsWinner)
                    matchPlayerInfoBase.PortraitBackColor = WinningTeamBackColor;
                else
                    matchPlayerInfoBase.PortraitBackColor = LosingTeamBackColor;

                if (PlayerPartyIcons.ContainsKey(player.PlayerNumber))
                {
                    matchPlayerInfoBase.PartyIcon = HeroesInfo.GetPartyIcon(PlayerPartyIcons[player.PlayerNumber]);
                }

                // add to collection
                if (player.Team == 0)
                    MatchOverviewTeam1Collection.Add(matchPlayerInfoBase);
                else if (player.Team == 1)
                    MatchOverviewTeam2Collection.Add(matchPlayerInfoBase);
            }
        }

        private void ClearMatchOverview()
        {
            foreach (var player in MatchOverviewTeam1Collection)
                player.LeaderboardPortrait = null;

            foreach (var player in MatchOverviewTeam2Collection)
                player.LeaderboardPortrait = null;

            MatchOverviewTeam1Collection.Clear();
            MatchOverviewTeam2Collection.Clear();
        }

        private void ExecuteShowMatchSummaryCommand()
        {
            if (SelectedReplay == null)
                return;

            Messenger.Default.Send(new MatchSummaryMessage { ReplayId = SelectedReplay.ReplayId, MatchSummary = MatchSummary.QuickMatch });
        }
    }
}
