using GalaSoft.MvvmLight.Command;
using Heroes.Helpers;
using Heroes.ReplayParser;
using HeroesStatTracker.Core.Models.MatchModels;
using HeroesStatTracker.Data;
using HeroesStatTracker.Data.Models.Replays;
using HeroesStatTracker.Data.Queries.Replays;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using static Heroes.Helpers.HeroesHelpers;

namespace HeroesStatTracker.Core.ViewModels.Matches
{
    public class MatchesBase : HstViewModel
    {
        private bool _isGivenBattleTagOnlyChecked;
        private long _selectedReplayIdValue;
        private string _selectedSeasonOption;
        private string _selectedMapOption;
        private string _selectedBuildOption;
        private string _selectedGameTimeOption;
        private string _selectedGameDateOption;
        private string _selectedPlayerBattleTag;
        private string _selectedCharacter;

        private IDatabaseService IDatabaseService;
        private ReplayMatch _selectedReplay;

        private ObservableCollection<ReplayMatch> _matchListCollection = new ObservableCollection<ReplayMatch>();
        private ObservableCollection<MatchPlayerBase> _matchOverviewTeam1Collection = new ObservableCollection<MatchPlayerBase>();
        private ObservableCollection<MatchPlayerBase> _matchOverviewTeam2Collection = new ObservableCollection<MatchPlayerBase>();

        private GameMode MatchGameMode;

        public MatchesBase(IDatabaseService iDatabaseService, GameMode matchGameMode)
        {
            IDatabaseService = iDatabaseService;

            MatchGameMode = matchGameMode;

            SeasonList.Add("Lifetime");
            SeasonList.AddRange(HeroesHelpers.Seasons.GetSeasonList());
            SelectedSeasonOption = SeasonList[0];

            GameTimeList = HeroesHelpers.GameDates.GameTimeList;
            SelectedGameTimeOption = GameTimeList[0];

            GameDateList = HeroesHelpers.GameDates.GameDateList;
            SelectedGameDateOption = GameDateList[0];

            MapsList.Add("Any");
            MapsList.AddRange(HeroesHelpers.HeroesInfo.HeroesIcons.GetMapsList());
            SelectedMapOption = MapsList[0];

            ReplayBuildsList.Add("Any");
            ReplayBuildsList.AddRange(HeroesHelpers.Builds.GetBuildsList());
            SelectedBuildOption = ReplayBuildsList[0];

            HeroesList.Add("Any");
            HeroesList.AddRange(HeroesHelpers.HeroesInfo.HeroesIcons.GetListOfHeroes());
            SelectedCharacter = HeroesList[0];
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

        public RelayCommand ClearSearchCommand => new RelayCommand(ClearSearch);
        public RelayCommand LoadMatchListCommand => new RelayCommand(LoadMatchList);
        public RelayCommand ShowMatchOverviewCommand => new RelayCommand(() => LoadMatchOverview(SelectedReplay));

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

            MatchListCollection = new ObservableCollection<ReplayMatch>(IDatabaseService.ReplaysDb().MatchReplay.ReadGameModeRecords(MatchGameMode, filter));
        }

        private void LoadMatchOverview(ReplayMatch replayMatch)
        {
            ClearMatchOverview();

            if (replayMatch == null)
                return;

            // get info
            replayMatch = IDatabaseService.ReplaysDb().MatchReplay.ReadReplayIncludeAssociatedRecords(replayMatch.ReplayId);
            var playersList = replayMatch.ReplayMatchPlayers.ToList();

            // load up correct build information
            HeroesInfo.HeroesIcons.LoadHeroesBuild(replayMatch.ReplayBuild);

            //FindPlayerParties(playersList);

            foreach (var player in playersList)
            {
                if (player.Team == 4)
                    continue;

                MatchPlayerBase matchPlayerBase = new MatchPlayerBase();
                var playerInfo = IDatabaseService.ReplaysDb().HotsPlayer.ReadRecordFromPlayerId(player.PlayerId);

                matchPlayerBase.LeaderboardPortrait = player.Character != "None" ? HeroesInfo.HeroesIcons.GetHeroLeaderboardPortrait(player.Character) : null;
                matchPlayerBase.CharacterName = player.Character;
                matchPlayerBase.Silenced = player.IsSilenced;

                //if (ShowPlayerTagNumber)
                //    matchPlayerInfoBase.PlayerName = playerInfo.BattleTagName;
                //else
                //    matchPlayerInfoBase.PlayerName = Utilities.GetNameFromBattleTagName(playerInfo.BattleTagName);

                //if (player.IsWinner)
                //    matchPlayerInfoBase.PortraitBackColor = WinningTeamBackColor;
                //else
                //    matchPlayerInfoBase.PortraitBackColor = LosingTeamBackColor;

                //if (PlayerPartyIcons.ContainsKey(player.PlayerNumber))
                //{
                //    matchPlayerInfoBase.PartyIcon = HeroesInfo.GetPartyIcon(PlayerPartyIcons[player.PlayerNumber]);
                //}

                //SetContextMenuCommands(matchPlayerInfoBase);

                // add to collection
                if (player.Team == 0)
                    MatchOverviewTeam1Collection.Add(matchPlayerBase);
                else if (player.Team == 1)
                    MatchOverviewTeam2Collection.Add(matchPlayerBase);
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
    }
}
