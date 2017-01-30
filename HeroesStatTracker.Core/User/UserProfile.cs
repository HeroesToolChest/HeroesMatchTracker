using GalaSoft.MvvmLight.Messaging;
using Heroes.Icons;
using HeroesStatTracker.Core.Messaging;
using HeroesStatTracker.Data;

namespace HeroesStatTracker.Core.User
{
    public class UserProfile : IUserProfileService
    {
        private IDatabaseService Database;
        private IHeroesIconsService HeroesIcons;

        public UserProfile(IDatabaseService database, IHeroesIconsService heroesIcons)
        {
            HeroesIcons = heroesIcons;
            Database = database;
            RetrieveUserProfile();
        }

        public string BattleTagName { get; set; }
        public long PlayerId { get; set; }
        public int RegionId { get; set; }

        public void SetProfile()
        {
            Database.SettingsDb().UserSettings.UserBattleTagName = BattleTagName;
            Database.SettingsDb().UserSettings.UserRegion = RegionId;

            Database.SettingsDb().UserSettings.UserPlayerId = Database.ReplaysDb().HotsPlayer.ReadPlayerIdFromBattleTagName(BattleTagName, RegionId);
            Messenger.Default.Send(new NotificationMessage(StaticMessage.UpdateUserBattleTag));
        }

        private void RetrieveUserProfile()
        {
            BattleTagName = Database.SettingsDb().UserSettings.UserBattleTagName;
            PlayerId = Database.SettingsDb().UserSettings.UserPlayerId;
            RegionId = Database.SettingsDb().UserSettings.UserRegion;
        }
    }
}
