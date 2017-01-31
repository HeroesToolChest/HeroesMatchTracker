using GalaSoft.MvvmLight.CommandWpf;
using Heroes.Helpers;
using Heroes.Icons;
using HeroesStatTracker.Data;
using HeroesStatTracker.Data.Models.Replays;
using System;
using System.Windows.Media.Imaging;

namespace HeroesStatTracker.Core.Models.MatchModels
{
    public class MatchPlayerBase
    {
        private IDatabaseService Database;
        private IHeroesIconsService HeroesIcons;
        private ReplayMatchPlayer Player;

        public MatchPlayerBase(IDatabaseService database, IHeroesIconsService heroesIcons, ReplayMatchPlayer player)
        {
            Database = database;
            HeroesIcons = heroesIcons;
            Player = player;
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
        public int PlayerNumber { get; private set; }
        public bool Silenced { get; private set; }
        public bool IsUserPlayer { get; private set; }

        public void SetPlayerInfo()
        {
            var playerInfo = Database.ReplaysDb().HotsPlayer.ReadRecordFromPlayerId(Player.PlayerId);

            LeaderboardPortrait = Player.Character != "None" ? HeroesIcons.Heroes().GetHeroLeaderboardPortrait(Player.Character) : null;
            CharacterTooltip = $"{Player.Character}{Environment.NewLine}{HeroesIcons.Heroes().GetHeroRole(Player.Character)[0]}";
            Silenced = Player.IsSilenced;
            CharacterName = Player.Character;

            if (Database.SettingsDb().UserSettings.IsBattleTagHidden)
                PlayerName = HeroesHelpers.BattleTags.GetNameFromBattleTagName(playerInfo.BattleTagName);
            else
                PlayerName = playerInfo.BattleTagName;

            if (playerInfo.BattleTagName == Database.SettingsDb().UserSettings.UserBattleTagName && playerInfo.BattleNetRegionId == Database.SettingsDb().UserSettings.UserRegion)
                IsUserPlayer = true;
            else
                IsUserPlayer = false;
        }

        public void SetPartyIcon(PartyIconColor icon)
        {
            PartyIcon = HeroesIcons.GetPartyIcon(icon);
        }

        public void SetMVPAward(string awardType)
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

        public void ClearInfo()
        {
            LeaderboardPortrait = null;
            PartyIcon = null;
            MvpAward = null;
            MvpAwardDescription = null;
        }

        private void ShowHotsLogsPlayerProfile()
        {

        }
    }
}
