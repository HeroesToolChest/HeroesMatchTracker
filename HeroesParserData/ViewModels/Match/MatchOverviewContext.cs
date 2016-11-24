using GalaSoft.MvvmLight.Messaging;
using HeroesParserData.DataQueries;
using HeroesParserData.Messages;
using HeroesParserData.Models.DbModels;
using HeroesParserData.Models.MatchModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;

namespace HeroesParserData.ViewModels.Match
{
    public abstract class MatchOverviewContext : MatchContextBase
    {
        private bool _isViewMatchSummaryEnabled;

        private int _rowsReturned;
        private int _selectedReplayBuildIdValue;

        private string _queryStatus;
        private string _selectedSeasonOption;
        private string _selectedMapOption;
        private string _selectedBuildOption;
        private string _selectedGameTimeOption;
        private string _selectedGameDateOption;

        private Replay _selectedReplay;

        private ObservableCollection<Replay> _matchListCollection = new ObservableCollection<Replay>();
        private ObservableCollection<MatchPlayerInfoBase> _matchOverviewTeam1Collection = new ObservableCollection<MatchPlayerInfoBase>();
        private ObservableCollection<MatchPlayerInfoBase> _matchOverviewTeam2Collection = new ObservableCollection<MatchPlayerInfoBase>();

        #region public properties
        public UniqueStringLists UniqueStringLists { get; private set; } = new UniqueStringLists();
        public List<string> SeasonList { get; private set; } = new List<string>();
        public List<string> MapsList { get; private set; } = new List<string>();
        public List<string> ReplayBuildsList { get; private set; } = new List<string>();
        public List<string> GameTimeList { get; private set; } = new List<string>();
        public List<string> GameDateList { get; private set; } = new List<string>();

        public bool IsViewMatchSummaryEnabled
        {
            get { return _isViewMatchSummaryEnabled; }
            set
            {
                _isViewMatchSummaryEnabled = value;
                RaisePropertyChangedEvent(nameof(IsViewMatchSummaryEnabled));
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

        public string SelectedSeasonOption
        {
            get { return _selectedSeasonOption; }
            set
            {
                _selectedSeasonOption = value;
                RaisePropertyChangedEvent(nameof(SelectedSeasonOption));
            }
        }

        public string SelectedMapOption
        {
            get { return _selectedMapOption; }
            set
            {
                _selectedMapOption = value;
                RaisePropertyChangedEvent(nameof(SelectedMapOption));
            }
        }

        public string SelectedBuildOption
        {
            get { return _selectedBuildOption; }
            set
            {
                _selectedBuildOption = value;
                RaisePropertyChangedEvent(nameof(SelectedBuildOption));
            }
        }

        public int SelectedReplayBuildIdValue
        {
            get { return _selectedReplayBuildIdValue; }
            set
            {
                _selectedReplayBuildIdValue = value;
                RaisePropertyChangedEvent(nameof(SelectedReplayBuildIdValue));
            }
        }

        public string SelectedGameTimeOption
        {
            get { return _selectedGameTimeOption; }
            set
            {

                _selectedGameTimeOption = value;
                RaisePropertyChangedEvent(nameof(SelectedGameTimeOption));
            }
        }

        public string SelectedGameDateOption
        {
            get { return _selectedGameDateOption; }
            set
            {
                _selectedGameDateOption = value;
                RaisePropertyChangedEvent(nameof(SelectedGameDateOption));
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

        // databinded
        public List<string> HeroesList
        {
            get { return HeroesInfo.GetListOfHeroes(); }
        }

        public Season GetSelectedSeason
        {
            get { return Utilities.GetSeasonFromString(SelectedSeasonOption); }
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

        public ObservableCollection<Replay> MatchListCollection
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
                    catch (Exception ex)
                    {
                        QueryStatus = "Match list queried failed";
                        WarningLog.Log(NLog.LogLevel.Warn, ex, "Failed to load match list");
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

        public ICommand ClearSearchCommand
        {
            get { return new DelegateCommand(() => ExecuteClearSearchCommand()); }
        }

        protected MatchOverviewContext()
            :base()
        {
            Messenger.Default.Register<MatchOverviewMessage>(this, (action) => ReceiveMessage(action));

            IsViewMatchSummaryEnabled = false;

            SelectedReplayBuildIdValue = 0;

            SeasonList.Add("Lifetime");
            SeasonList.AddRange(AllSeasonsList);
            SelectedSeasonOption = SeasonList[0];

            GameTimeList = UniqueStringLists.GameTimeList;
            SelectedGameTimeOption = GameTimeList[0];

            GameDateList = UniqueStringLists.GameDateList;
            SelectedGameDateOption = GameDateList[0];

            MapsList.Add("Any");
            MapsList.AddRange(HeroesInfo.GetMapsList());
            SelectedMapOption = MapsList[0];

            ReplayBuildsList.Add("Any");
            ReplayBuildsList.AddRange(AllReplayBuildsList);
            SelectedBuildOption = ReplayBuildsList[0];
        }

        protected abstract void ExecuteLoadMatchListCommmand();

        protected abstract void ExecuteShowMatchSummaryCommand();

        private void LoadMatchOverviewCommand(Replay replay)
        {
            IsViewMatchSummaryEnabled = false;
            ClearMatchOverview();

            if (replay == null)
                return;

            QueryMatchOverview(replay.ReplayId);
        }

        private void QueryMatchOverview(long replayId)
        {
            IsViewMatchSummaryEnabled = true;

            // get info
            Replay replay = Query.Replay.ReadReplayIncludeRecord(replayId);
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
            {
                player.LeaderboardPortrait = null;
                player.PartyIcon = null;
            }

            foreach (var player in MatchOverviewTeam2Collection)
            {
                player.LeaderboardPortrait = null;
                player.PartyIcon = null;
            }

            MatchOverviewTeam1Collection.Clear();
            MatchOverviewTeam2Collection.Clear();
        }

        private void ExecuteClearSearchCommand()
        {
            SelectedSeasonOption = SeasonList[0];
            SelectedReplayBuildIdValue = 0;
            SelectedMapOption = MapsList[0];
            SelectedBuildOption = ReplayBuildsList[0];
            SelectedGameTimeOption = GameTimeList[0];
            SelectedGameDateOption = GameDateList[0];
        }

        private void ReceiveMessage(MatchOverviewMessage action)
        {
            SelectedReplay = action.Replay;
        }
    }
}
