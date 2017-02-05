using GalaSoft.MvvmLight.Ioc;
using Heroes.Helpers;
using Heroes.Icons;
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
        private bool _isLeftChangeButtonVisible;
        private bool _isRightChangeButtonVisible;
        private bool _isLeftChangeButtonEnabled;
        private bool _isRightChangeButtonEnabled;
        private string _matchTitle;
        private Color _matchTitleGlowColor;

        private ObservableCollection<MatchPlayerTalents> _matchTalentsTeam1Collection = new ObservableCollection<MatchPlayerTalents>();
        private ObservableCollection<MatchPlayerTalents> _matchTalentsTeam2Collection = new ObservableCollection<MatchPlayerTalents>();

        private IDatabaseService Database;

        public MatchSummaryViewModel(IDatabaseService database, IHeroesIconsService heroesIcons)
            : base(heroesIcons)
        {
            Database = database;

            IsLeftChangeButtonVisible = true;
            IsRightChangeButtonVisible = true;
            IsLeftChangeButtonEnabled = false;
            IsRightChangeButtonEnabled = false;

            SimpleIoc.Default.Register<IMatchSummaryReplayService>(() => this);
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

        public string MatchTitle
        {
            get { return _matchTitle; }
            set
            {
                _matchTitle = value;
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

        public void LoadMatchSummary(ReplayMatch replayMatch, List<ReplayMatch> matchList)
        {
            if (replayMatch == null || matchList == null || matchList.Count == 0)
                return;

            LoadMatchSummaryData(replayMatch);

            IsLeftChangeButtonEnabled = replayMatch.ReplayId == matchList[0].ReplayId ? false : true;
            IsRightChangeButtonEnabled = replayMatch.ReplayId == matchList[matchList.Count - 1].ReplayId ? true : false;
        }

        private void LoadMatchSummaryData(ReplayMatch replayMatch)
        {
            try
            {
                DisposeMatchSummary();

                replayMatch = Database.ReplaysDb().MatchReplay.ReadReplayIncludeAssociatedRecords(replayMatch.ReplayId);

                HeroesIcons.LoadHeroesBuild(replayMatch.ReplayBuild);
                SetBackgroundImage(replayMatch.MapName);
                MatchTitleGlowColor = HeroesIcons.MapBackgrounds().GetMapBackgroundFontGlowColor(replayMatch.MapName);
                MatchTitle = $"{replayMatch.MapName} - {HeroesHelpers.GameModes.GetStringFromGameMode(replayMatch.GameMode)} [{replayMatch.TimeStamp}] [{replayMatch.ReplayLength}]";

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

                    //SetContextMenuCommands(matchPlayerInfoBase);

                    MatchPlayerTalents matchPlayerTalents = new MatchPlayerTalents(matchPlayerBase);

                    if (player.Character != "None")
                    {
                        matchPlayerTalents.SetTalents(playerTalentsList, player.PlayerNumber);
                    }

                    if (player.Team == 0 || player.Team == 1)
                    {
                        if (player.Team == 0)
                        {
                            MatchTalentsTeam1Collection.Add(matchPlayerTalents);
                        }
                        else
                        {
                            MatchTalentsTeam2Collection.Add(matchPlayerTalents);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionLog.Log(NLog.LogLevel.Error, ex);
            }
        }

        private void DisposeMatchSummary()
        {
            foreach (var player in MatchTalentsTeam1Collection)
            {
                player.Dispose();
            }

            foreach (var player in MatchTalentsTeam2Collection)
            {
                player.Dispose();
            }

            MatchTalentsTeam1Collection.Clear();
            MatchTalentsTeam2Collection.Clear();
        }
    }
}
