using GalaSoft.MvvmLight.CommandWpf;
using Heroes.Helpers;
using Heroes.Icons;
using HeroesStatTracker.Core.Models.MatchModels;
using HeroesStatTracker.Core.User;
using HeroesStatTracker.Core.ViewServices;
using HeroesStatTracker.Data;
using HeroesStatTracker.Data.Models.Replays;
using Microsoft.Practices.ServiceLocation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Media.Imaging;

namespace HeroesStatTracker.Core.Models.MatchHistoryModels
{
    public class MatchHistoryMatch
    {
        private IDatabaseService Database;
        private IHeroesIconsService HeroesIcons;
        private IUserProfileService UserProfile;
        private ReplayMatch ReplayMatch;

        public MatchHistoryMatch(IDatabaseService database, IHeroesIconsService heroesIcons, IUserProfileService userProfile, ReplayMatch replayMatch)
        {
            Database = database;
            HeroesIcons = heroesIcons;
            UserProfile = userProfile;
            ReplayMatch = replayMatch;

            SetMatch();
        }

        public List<MatchPlayerBase> MatchOverviewTeam1List { get; private set; } = new List<MatchPlayerBase>();
        public List<MatchPlayerBase> MatchOverviewTeam2List { get; private set; } = new List<MatchPlayerBase>();

        public bool IsExpanded { get; set; }
        public string GameMode { get; private set; }
        public string MapName { get; private set; }
        public BitmapImage UserHeroImage { get; private set; }
        public string UserHero { get; private set; }
        public string WinnerResult { get; private set; }
        public DateTime TimeStamp { get; private set; }
        public TimeSpan GameLength { get; private set; }
        public int? Build { get; private set; }
        public long ReplayId { get; private set; }

        public IMatchSummaryFlyoutService MatchSummaryFlyout
        {
            get { return ServiceLocator.Current.GetInstance<IMatchSummaryFlyoutService>(); }
        }

        public IMatchSummaryReplayService MatchSummaryReplay
        {
            get { return ServiceLocator.Current.GetInstance<IMatchSummaryReplayService>(); }
        }

        public RelayCommand ShowMatchSummaryCommand => new RelayCommand(ShowMatchSummary);

        private void ShowMatchSummary()
        {
            if (ReplayMatch == null)
                return;

            //ShowMatchSummaryButtonEnabled = false;
            MatchSummaryReplay.LoadMatchSummary(ReplayMatch, null);

            MatchSummaryFlyout.SetMatchSummaryHeader($"Match Summary [Id:{ReplayMatch.ReplayId}] [Build:{ReplayMatch.ReplayBuild}]");
            MatchSummaryFlyout.OpenMatchSummaryFlyout();
        }

        private void SetMatch()
        {
            var playersList = ReplayMatch.ReplayMatchPlayers.ToList();
            var matchAwardDictionary = ReplayMatch.ReplayMatchAward.ToDictionary(x => x.PlayerId, x => x.Award);

            // load up correct build information
            HeroesIcons.LoadHeroesBuild(ReplayMatch.ReplayBuild);

            var playerParties = PlayerParties.FindPlayerParties(playersList);

            foreach (var player in playersList)
            {
                if (player.Team == 4)
                    continue;

                MatchPlayerBase matchPlayerBase = new MatchPlayerBase(Database, HeroesIcons, UserProfile, player);
                matchPlayerBase.SetPlayerInfo(player.IsAutoSelect, playerParties, matchAwardDictionary);

                // add to collection
                if (player.Team == 0)
                    MatchOverviewTeam1List.Add(matchPlayerBase);
                else if (player.Team == 1)
                    MatchOverviewTeam2List.Add(matchPlayerBase);

                if (player.PlayerId == UserProfile.PlayerId)
                {
                    UserHero = player.Character;
                    UserHeroImage = HeroesIcons.Heroes().GetHeroPortrait(player.Character);
                    WinnerResult = player.IsWinner ? "Win" : "Loss";
                }
            }

            GameMode = HeroesHelpers.GameModes.GetStringFromGameMode(ReplayMatch.GameMode);
            MapName = ReplayMatch.MapName;
            TimeStamp = ReplayMatch.TimeStamp.Value;
            GameLength = ReplayMatch.ReplayLength;
            Build = ReplayMatch.ReplayBuild;
            ReplayId = ReplayMatch.ReplayId;
        }
    }
}
