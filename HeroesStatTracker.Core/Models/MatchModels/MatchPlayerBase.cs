using GalaSoft.MvvmLight.CommandWpf;
using Heroes.Helpers;
using Heroes.Icons;
using HeroesStatTracker.Core.ViewServices;
using HeroesStatTracker.Data;
using HeroesStatTracker.Data.Models.Replays;
using Microsoft.Practices.ServiceLocation;
using System;
using System.Collections.Generic;
using System.Windows.Media.Imaging;

namespace HeroesStatTracker.Core.Models.MatchModels
{
    public class MatchPlayerBase
    {
        public MatchPlayerBase(IDatabaseService database, IHeroesIconsService heroesIcons, ReplayMatchPlayer player)
        {
            Database = database;
            HeroesIcons = heroesIcons;
            Player = player;
        }

        protected MatchPlayerBase(MatchPlayerBase matchPlayerBase)
        {
            Database = matchPlayerBase.Database;
            HeroesIcons = matchPlayerBase.HeroesIcons;
            Player = matchPlayerBase.Player;

            LeaderboardPortrait = matchPlayerBase.LeaderboardPortrait;
            MvpAward = matchPlayerBase.MvpAward;
            PartyIcon = matchPlayerBase.PartyIcon;
            PlayerName = matchPlayerBase.PlayerName;
            CharacterName = matchPlayerBase.CharacterName;
            CharacterTooltip = matchPlayerBase.CharacterTooltip;
            CharacterLevel = matchPlayerBase.CharacterLevel;
            MvpAwardDescription = matchPlayerBase.MvpAwardDescription;
            Silenced = matchPlayerBase.Silenced;
            IsUserPlayer = matchPlayerBase.IsUserPlayer;
        }

        public RelayCommand ShowHotsLogsPlayerProfileCommand => new RelayCommand(ShowHotsLogsPlayerProfile);

        public BitmapImage LeaderboardPortrait { get; private set; }
        public BitmapImage MvpAward { get; private set; }
        public BitmapImage PartyIcon { get; private set; }
        public string PlayerName { get; private set; }
        public string CharacterName { get; private set; }
        public string CharacterTooltip { get; private set; }
        public string CharacterLevel { get; private set; }
        public string MvpAwardDescription { get; private set; }
        public bool Silenced { get; private set; }
        public bool IsUserPlayer { get; private set; }

        public IBrowserWindowService BrowserWindow
        {
            get { return ServiceLocator.Current.GetInstance<IBrowserWindowService>(); }
        }

        protected IDatabaseService Database { get; }
        protected IHeroesIconsService HeroesIcons { get; }
        protected ReplayMatchPlayer Player { get; }

        public void SetPlayerInfo(bool isAutoSelect, Dictionary<int, PartyIconColor> playerPartyIcons, Dictionary<long, string> matchAwardDictionary)
        {
            var playerInfo = Database.ReplaysDb().HotsPlayer.ReadRecordFromPlayerId(Player.PlayerId);

            LeaderboardPortrait = Player.Character != "None" ? HeroesIcons.Heroes().GetHeroLeaderboardPortrait(Player.Character) : null;
            CharacterTooltip = $"{Player.Character}{Environment.NewLine}{HeroesIcons.Heroes().GetHeroRoleList(Player.Character)[0]}";
            Silenced = Player.IsSilenced;
            CharacterName = Player.Character;

            PlayerName = Database.SettingsDb().UserSettings.IsBattleTagHidden ? HeroesHelpers.BattleTags.GetNameFromBattleTagName(playerInfo.BattleTagName) : playerInfo.BattleTagName;
            IsUserPlayer = (playerInfo.BattleTagName == Database.SettingsDb().UserSettings.UserBattleTagName && playerInfo.BattleNetRegionId == Database.SettingsDb().UserSettings.UserRegion) ? true : false;
            CharacterLevel = isAutoSelect ? "Auto Select" : Player.CharacterLevel.ToString();

            if (playerPartyIcons.ContainsKey(Player.PlayerNumber))
                SetPartyIcon(playerPartyIcons[Player.PlayerNumber]);

            if (matchAwardDictionary.ContainsKey(Player.PlayerId))
                SetMVPAward(matchAwardDictionary[Player.PlayerId]);
        }

        public virtual void Dispose()
        {
            LeaderboardPortrait = null;
            PartyIcon = null;
            MvpAward = null;
            MvpAwardDescription = null;
        }

        private void SetPartyIcon(PartyIconColor icon)
        {
            PartyIcon = HeroesIcons.GetPartyIcon(icon);
        }

        private void SetMVPAward(string awardType)
        {
            string mvpAwardName = null;
            MVPScoreScreenColor teamColor;

            if (Player.Team == 0)
                teamColor = MVPScoreScreenColor.Blue;
            else
                teamColor = MVPScoreScreenColor.Red;

            MvpAward = HeroesIcons.MatchAwards().GetMVPScoreScreenAward(awardType, teamColor, out mvpAwardName);
            MvpAwardDescription = $"{mvpAwardName}{Environment.NewLine}{HeroesIcons.MatchAwards().GetMatchAwardDescription(awardType)}";
        }

        private void ShowHotsLogsPlayerProfile()
        {
            BrowserWindow.CreateBrowserWindow();
        }
    }
}
