using HeroesMatchTracker.Data.Databases;
using HeroesMatchTracker.Data.Models.Settings;
using HeroesMatchTracker.Data.Queries.Settings;

namespace HeroesMatchTracker.Data.Migrations.Settings
{
    internal class MigrationAddon4_v2_2_0_1 : MigrationMethods<SettingsContext>, IMigrationAddon
    {
        public void Execute()
        {
            SettingsDb settingsDb = new SettingsDb();

            if (settingsDb.UserSettings.UserPlayerId > 0)
            {
                UserProfile profile = new UserProfile()
                {
                    UserBattleTagName = settingsDb.UserSettings.UserBattleTagName,
                    UserRegion = settingsDb.UserSettings.UserRegion,
                };

                settingsDb.UserProfiles.CreateUserProfile(profile);
            }
            else
            {
                settingsDb.UserSettings.UserBattleTagName = string.Empty;
                settingsDb.UserSettings.UserRegion = 0;
                settingsDb.UserSettings.UserPlayerId = 0;
            }
        }
    }
}
