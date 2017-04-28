using GalaSoft.MvvmLight.Messaging;
using HeroesMatchTracker.Core.Messaging;
using HeroesMatchTracker.Data;

namespace HeroesMatchTracker.Core.User
{
    public class UserProfile : IUserProfileService
    {
        private IDatabaseService Database;

        public UserProfile(IDatabaseService database)
        {
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
