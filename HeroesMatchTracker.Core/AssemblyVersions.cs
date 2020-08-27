using System.Reflection;

namespace HeroesMatchTracker.Core
{
    public static class AssemblyVersions
    {
        /// <summary>
        /// Returns the version of Heroes Match Tracker.
        /// </summary>
        /// <returns>Returns the version in the form of x.x.x-yyy.</returns>
        public static string HeroesMatchTrackerVersion()
        {
            return Assembly.GetEntryAssembly()!.GetCustomAttribute<AssemblyInformationalVersionAttribute>()!.InformationalVersion;
        }
    }
}
