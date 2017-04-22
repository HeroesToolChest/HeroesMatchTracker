using HeroesMatchTracker.Data.Databases;
using HeroesMatchTracker.Data.Properties;

namespace HeroesMatchTracker.Data
{
    public static class ConnectionString
    {
        public static string GetConnectionStringByType<T>()
        {
            if (typeof(T) == typeof(ReleaseNotesContext))
                return Settings.Default.ReleaseNotesConnNameDb;
            else if (typeof(T) == typeof(ReplaysContext))
                return Settings.Default.ReplaysConnNameDb;
            else if (typeof(T) == typeof(SettingsContext))
                return Settings.Default.SettingsConnNameDb;
            else
                return null;
        }
    }
}
