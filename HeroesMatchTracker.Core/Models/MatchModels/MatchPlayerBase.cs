using GalaSoft.MvvmLight.CommandWpf;
using GalaSoft.MvvmLight.Messaging;
using Heroes.Helpers;
using Heroes.Icons;
using Heroes.Icons.Models;
using Heroes.Models;
using HeroesMatchTracker.Core.Messaging;
using HeroesMatchTracker.Core.Models.HeroModels;
using HeroesMatchTracker.Core.Services;
using HeroesMatchTracker.Core.User;
using HeroesMatchTracker.Core.ViewServices;
using HeroesMatchTracker.Data;
using HeroesMatchTracker.Data.Models.Replays;
using Microsoft.Practices.ServiceLocation;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using static Heroes.Helpers.HeroesHelpers.Regions;

namespace HeroesMatchTracker.Core.Models.MatchModels
{
    public class MatchPlayerBase
    {
        public MatchPlayerBase(IInternalService internalService, IWebsiteService website, ReplayMatchPlayer player, int build)
        {
            Database = internalService.Database;
            HeroesIcons = internalService.HeroesIcons;
            UserProfile = internalService.UserProfile;
            Website = website;
            Player = player;
            Build = build;

            SilenceIcon = ImageStreams.OtherIconImage(OtherIcon.Silence);
            VoiceSilenceIcon = ImageStreams.OtherIconImage(OtherIcon.VoiceSilence);
            TalentBorderScoreScreenIcon = ImageStreams.OtherIconImage(OtherIcon.TalentAvailable);
        }

        protected MatchPlayerBase(MatchPlayerBase matchPlayerBase)
        {
            Build = matchPlayerBase.Build;

            Database = matchPlayerBase.Database;
            HeroesIcons = matchPlayerBase.HeroesIcons;
            Player = matchPlayerBase.Player;

            LeaderboardPortrait = matchPlayerBase.LeaderboardPortrait;
            MvpAward = matchPlayerBase.MvpAward;
            PartyIcon = matchPlayerBase.PartyIcon;
            PlayerName = matchPlayerBase.PlayerName;
            PlayerBattleTagName = matchPlayerBase.PlayerBattleTagName;
            CharacterName = matchPlayerBase.CharacterName;
            CharacterLevel = matchPlayerBase.CharacterLevel;
            MvpAwardDescription = matchPlayerBase.MvpAwardDescription;
            AccountLevel = matchPlayerBase.AccountLevel;
            Silenced = matchPlayerBase.Silenced;
            VoiceSilenced = matchPlayerBase.VoiceSilenced;
            IsUserPlayer = matchPlayerBase.IsUserPlayer;
            PlayerRegion = matchPlayerBase.PlayerRegion;
            HeroDescriptionSubInfo = matchPlayerBase.HeroDescriptionSubInfo;
            HeroDescription = matchPlayerBase.HeroDescription;
            PlayerTag = matchPlayerBase.PlayerTag;
            SilenceIcon = matchPlayerBase.SilenceIcon;
            VoiceSilenceIcon = matchPlayerBase.VoiceSilenceIcon;
            TalentBorderScoreScreenIcon = matchPlayerBase.TalentBorderScoreScreenIcon;
        }

        public bool Silenced { get; private set; }
        public bool VoiceSilenced { get; private set; }
        public bool IsUserPlayer { get; private set; }
        public string PlayerName { get; private set; }
        public string PlayerBattleTagName { get; private set; }
        public string CharacterName { get; private set; }
        public string CharacterLevel { get; private set; }
        public string MvpAwardDescription { get; private set; }
        public string AccountLevel { get; private set; }
        public string HeroDescriptionSubInfo { get; private set; }
        public Region PlayerRegion { get; private set; }
        public Stream LeaderboardPortrait { get; private set; }
        public Stream MvpAward { get; private set; }
        public Stream PartyIcon { get; private set; }
        public Stream SilenceIcon { get; private set; }
        public Stream VoiceSilenceIcon { get; private set; }
        public Stream TalentBorderScoreScreenIcon { get; private set; }
        public PlayerTag PlayerTag { get; private set; }
        public HeroDescription HeroDescription { get; private set; }

        public RelayCommand HeroSearchAllMatchCommand => new RelayCommand(HeroSearchAllMatch);
        public RelayCommand HeroSearchQuickMatchCommand => new RelayCommand(HeroSearchQuickMatch);
        public RelayCommand HeroSearchUnrankedDraftCommand => new RelayCommand(HeroSearchUnrankedDraft);
        public RelayCommand HeroSearchHeroLeagueCommand => new RelayCommand(HeroSearchHeroLeague);
        public RelayCommand HeroSearchTeamLeagueCommand => new RelayCommand(HeroSearchTeamLeague);
        public RelayCommand HeroSearchBrawlCommand => new RelayCommand(HeroSearchBrawl);
        public RelayCommand HeroSearchCustomGameCommand => new RelayCommand(HeroSearchCustomGame);
        public RelayCommand PlayerSearchAllMatchCommand => new RelayCommand(PlayerSearchAllMatch);
        public RelayCommand PlayerSearchQuickMatchCommand => new RelayCommand(PlayerSearchQuickMatch);
        public RelayCommand PlayerSearchUnrankedDraftCommand => new RelayCommand(PlayerSearchUnrankedDraft);
        public RelayCommand PlayerSearchHeroLeagueCommand => new RelayCommand(PlayerSearchHeroLeague);
        public RelayCommand PlayerSearchTeamLeagueCommand => new RelayCommand(PlayerSearchTeamLeague);
        public RelayCommand PlayerSearchBrawlCommand => new RelayCommand(PlayerSearchBrawl);
        public RelayCommand PlayerSearchCustomGameCommand => new RelayCommand(PlayerSearchCustomGame);
        public RelayCommand PlayerAndHeroSearchAllMatchCommand => new RelayCommand(PlayerAndHeroSearchAllMatch);
        public RelayCommand PlayerAndHeroSearchQuickMatchCommand => new RelayCommand(PlayerAndHeroSearchQuickMatch);
        public RelayCommand PlayerAndHeroSearchUnrankedDraftCommand => new RelayCommand(PlayerAndHeroSearchUnrankedDraft);
        public RelayCommand PlayerAndHeroSearchHeroLeagueCommand => new RelayCommand(PlayerAndHeroSearchHeroLeague);
        public RelayCommand PlayerAndHeroSearchTeamLeagueCommand => new RelayCommand(PlayerAndHeroSearchTeamLeague);
        public RelayCommand PlayerAndHeroSearchBrawlCommand => new RelayCommand(PlayerAndHeroSearchBrawl);
        public RelayCommand PlayerAndHeroSearchCustomGameCommand => new RelayCommand(PlayerAndHeroSearchCustomGame);
        public RelayCommand CopyHeroNameToClipboardCommand => new RelayCommand(CopyHeroNameToClipboard);
        public RelayCommand CopyPlayerNameToClipboardCommand => new RelayCommand(CopyPlayerNameToClipboard);
        public RelayCommand CopyHeroAndPlayerNameToClipboardCommand => new RelayCommand(CopyHeroAndPlayerNameToClipboard);
        public RelayCommand PlayerNotesCommand => new RelayCommand(PlayerNotes);

        public IMainTabService MainTabs => ServiceLocator.Current.GetInstance<IMainTabService>();
        public IMatchesTabService MatchesTab => ServiceLocator.Current.GetInstance<IMatchesTabService>();
        public IMatchSummaryFlyoutService MatchSummaryFlyout => ServiceLocator.Current.GetInstance<IMatchSummaryFlyoutService>();
        public ICreateWindowService CreateWindow => ServiceLocator.Current.GetInstance<ICreateWindowService>();

        protected IDatabaseService Database { get; }
        protected IHeroesIcons HeroesIcons { get; }
        protected ISelectedUserProfileService UserProfile { get; }
        protected IWebsiteService Website { get; }
        protected ReplayMatchPlayer Player { get; }

        protected int Build { get; }

        public void SetPlayerInfo(bool isAutoSelect, Dictionary<int, PartyIconColor> playerPartyIcons, Dictionary<long, string> matchAwardDictionary)
        {
            var playerInfo = Database.ReplaysDb().HotsPlayer.ReadRecordFromPlayerId(Player.PlayerId);

            Hero hero = HeroesIcons.HeroesData(Build).HeroData(Player.Character, includeAbilities: false, additionalUnits: false);

            LeaderboardPortrait = Player.Character != "None" ? hero.HeroPortrait.LeaderboardImage() : null;
            Silenced = Player.IsSilenced;
            VoiceSilenced = Player.IsVoiceSilenced;
            CharacterName = hero.Name;
            PlayerName = Database.SettingsDb().UserSettings.IsBattleTagHidden ? HeroesHelpers.BattleTags.GetNameFromBattleTagName(playerInfo.BattleTagName) : playerInfo.BattleTagName;
            PlayerBattleTagName = playerInfo.BattleTagName;
            PlayerRegion = (Region)playerInfo.BattleNetRegionId;
            IsUserPlayer = (playerInfo.PlayerId == UserProfile.PlayerId && playerInfo.BattleNetRegionId == UserProfile.RegionId) ? true : false;

            if (Player.Team == 4)
                CharacterLevel = "Observer";
            else
                CharacterLevel = isAutoSelect ? "Auto Select" : Player.CharacterLevel.ToString();

            PlayerTag = new PlayerTag
            {
                PlayerName = PlayerName,
                AccountLevel = Player.AccountLevel > 0 ? Player.AccountLevel.ToString() : "N/A",
                TotalSeen = playerInfo.Seen,
                LastSeenBefore = playerInfo.LastSeenBefore.HasValue ? playerInfo.LastSeenBefore.Value.ToString() : "Never",
                FormerPlayerNames = Database.ReplaysDb().RenamedPlayer.ReadPlayersFromPlayerId(playerInfo.PlayerId),
                Notes = playerInfo.Notes ?? string.Empty,
            };

            HeroDescription = new HeroDescription
            {
                HeroName = hero.Name,
                Description = hero.Description.ColoredText,
                Franchise = hero.HeroFranchiseImage(),
                Type = hero.Type,
                Difficulty = hero.Difficulty,
                Roles = hero.Roles.ToList(),
            };

            if (playerPartyIcons.ContainsKey(Player.PlayerNumber))
                SetPartyIcon(playerPartyIcons[Player.PlayerNumber]);

            if (matchAwardDictionary.ContainsKey(Player.PlayerId))
                SetMVPAward(matchAwardDictionary[Player.PlayerId]);
        }

        public virtual void Dispose()
        {
            if (LeaderboardPortrait != null) LeaderboardPortrait.Dispose();
            if (PartyIcon != null) PartyIcon.Dispose();
            if (MvpAward != null) MvpAward.Dispose();
            MvpAwardDescription = null;
            if (HeroDescription != null) HeroDescription.Franchise.Dispose();
        }

        private void SetPartyIcon(PartyIconColor icon)
        {
            PartyIcon = ImageStreams.PartyIconImage(icon);
        }

        private void SetMVPAward(string awardType)
        {
            ScoreScreenAwardColor teamColor;

            if (Player.Team == 0)
                teamColor = ScoreScreenAwardColor.Blue;
            else
                teamColor = ScoreScreenAwardColor.Red;

            MatchAward matchAward = HeroesIcons.MatchAwards(Build).MatchAward(awardType);

            MvpAward = matchAward.MatchAwardScoreScreenImage(teamColor);
            MvpAwardDescription = $"{matchAward.Name}{Environment.NewLine}{matchAward.Description}";
        }

        private void HeroSearchAllMatch()
        {
            HeroSearch(Core.MatchesTab.AllMatches);
        }

        private void HeroSearchQuickMatch()
        {
            HeroSearch(Core.MatchesTab.QuickMatch);
        }

        private void HeroSearchUnrankedDraft()
        {
            HeroSearch(Core.MatchesTab.UnrankedDraft);
        }

        private void HeroSearchHeroLeague()
        {
            HeroSearch(Core.MatchesTab.HeroLeague);
        }

        private void HeroSearchTeamLeague()
        {
            HeroSearch(Core.MatchesTab.TeamLeague);
        }

        private void HeroSearchBrawl()
        {
            HeroSearch(Core.MatchesTab.Brawl);
        }

        private void HeroSearchCustomGame()
        {
            HeroSearch(Core.MatchesTab.Custom);
        }

        private void PlayerSearchAllMatch()
        {
            PlayerSearch(Core.MatchesTab.AllMatches);
        }

        private void PlayerSearchQuickMatch()
        {
            PlayerSearch(Core.MatchesTab.QuickMatch);
        }

        private void PlayerSearchUnrankedDraft()
        {
            PlayerSearch(Core.MatchesTab.UnrankedDraft);
        }

        private void PlayerSearchHeroLeague()
        {
            PlayerSearch(Core.MatchesTab.HeroLeague);
        }

        private void PlayerSearchTeamLeague()
        {
            PlayerSearch(Core.MatchesTab.TeamLeague);
        }

        private void PlayerSearchBrawl()
        {
            PlayerSearch(Core.MatchesTab.Brawl);
        }

        private void PlayerSearchCustomGame()
        {
            PlayerSearch(Core.MatchesTab.Custom);
        }

        private void PlayerAndHeroSearchAllMatch()
        {
            PlayerAndHeroSearch(Core.MatchesTab.AllMatches);
        }

        private void PlayerAndHeroSearchQuickMatch()
        {
            PlayerAndHeroSearch(Core.MatchesTab.QuickMatch);
        }

        private void PlayerAndHeroSearchUnrankedDraft()
        {
            PlayerAndHeroSearch(Core.MatchesTab.UnrankedDraft);
        }

        private void PlayerAndHeroSearchHeroLeague()
        {
            PlayerAndHeroSearch(Core.MatchesTab.HeroLeague);
        }

        private void PlayerAndHeroSearchTeamLeague()
        {
            PlayerAndHeroSearch(Core.MatchesTab.TeamLeague);
        }

        private void PlayerAndHeroSearchBrawl()
        {
            PlayerAndHeroSearch(Core.MatchesTab.Brawl);
        }

        private void PlayerAndHeroSearchCustomGame()
        {
            PlayerAndHeroSearch(Core.MatchesTab.Custom);
        }

        private void CopyHeroNameToClipboard()
        {
            Clipboard.SetText(CharacterName);
        }

        private void CopyPlayerNameToClipboard()
        {
            Clipboard.SetText(PlayerName);
        }

        private void CopyHeroAndPlayerNameToClipboard()
        {
            Clipboard.SetText($"{CharacterName} - {PlayerName}");
        }

        private void PlayerNotes()
        {
            CreateWindow.ShowPlayerNotesWindow(Player);
        }

        private void HeroSearch(MatchesTab matchTab)
        {
            MainTabs.SwitchToTab(MainPage.Matches);
            MatchesTab.SwitchToTab(matchTab);
            Messenger.Default.Send(new MatchesDataMessage { MatchTab = matchTab, SelectedCharacter = CharacterName });
            MatchSummaryFlyout.CloseMatchSummaryFlyout();
        }

        private void PlayerSearch(MatchesTab matchTab)
        {
            MainTabs.SwitchToTab(MainPage.Matches);
            MatchesTab.SwitchToTab(matchTab);
            Messenger.Default.Send(new MatchesDataMessage { MatchTab = matchTab, SelectedBattleTagName = PlayerName });
            MatchSummaryFlyout.CloseMatchSummaryFlyout();
        }

        private void PlayerAndHeroSearch(MatchesTab matchTab)
        {
            MainTabs.SwitchToTab(MainPage.Matches);
            MatchesTab.SwitchToTab(matchTab);
            Messenger.Default.Send(new MatchesDataMessage { MatchTab = matchTab, SelectedBattleTagName = PlayerName, SelectedCharacter = CharacterName });
            MatchSummaryFlyout.CloseMatchSummaryFlyout();
        }
    }
}
