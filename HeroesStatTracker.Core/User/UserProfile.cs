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
        }

        public string BattleTagName { get { return Database.SettingsDb().UserSettings.UserBattleTagName; } }
        public long PlayerId { get { return Database.SettingsDb().UserSettings.UserPlayerId; } }
        public int RegionId { get { return Database.SettingsDb().UserSettings.UserRegion; } }

        public void SetProfile(string battleTag, int regionId)
        {
            Database.SettingsDb().UserSettings.UserBattleTagName = battleTag;
            Database.SettingsDb().UserSettings.UserRegion = regionId;

            Database.SettingsDb().UserSettings.UserPlayerId = Database.ReplaysDb().HotsPlayer.ReadPlayerIdFromBattleTagName(battleTag, regionId);
            Messenger.Default.Send(new NotificationMessage(StaticMessage.UpdateUserBattleTag));
        }
    }
}
