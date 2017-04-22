using NuGet;
using System;
using System.Linq;
using System.Reflection;

namespace HeroesMatchTracker.Core
{
    public static class AssemblyVersions
    {
        /// <summary>
        /// Returns the version of HeroesMatchTracker
        /// </summary>
        /// <returns></returns>
        public static SemanticVersion HeroesMatchTrackerVersion()
        {
            Version version = Assembly.GetEntryAssembly().GetName().Version;
            return SemanticVersion.Parse(new Version(version.Major, version.Minor, version.Build).ToString());
        }

        /// <summary>
        /// Returns the version of HeroesMatchTracker.Core
        /// </summary>
        /// <returns></returns>
        public static SemanticVersion HeroesMatchTrackerCoreVersion()
        {
            Version version = Assembly.GetExecutingAssembly().GetName().Version;
            return SemanticVersion.Parse(new Version(version.Major, version.Minor, version.Build).ToString());
        }

        /// <summary>
        /// Returns the version of HeroesMatchTracker.Data
        /// </summary>
        /// <returns></returns>
        public static SemanticVersion HeroesMatchTrackerDataVersion()
        {
            Version version = Assembly.GetExecutingAssembly().GetReferencedAssemblies().Where(x => x.Name == "HeroesMatchTracker.Data").ToList()[0].Version;
            return SemanticVersion.Parse(new Version(version.Major, version.Minor, version.Build).ToString());
        }

        /// <summary>
        /// Returns the version of Heroes.Icons
        /// </summary>
        /// <returns></returns>
        public static SemanticVersion HeroesIconsVersion()
        {
            Version version = Assembly.GetExecutingAssembly().GetReferencedAssemblies().Where(x => x.Name == "Heroes.Icons").ToList()[0].Version;
            return SemanticVersion.Parse(new Version(version.Major, version.Minor, version.Build).ToString());
        }

        /// <summary>
        /// Returns the version of Heroes.Helpers
        /// </summary>
        /// <returns></returns>
        public static SemanticVersion HeroesHelpersVersion()
        {
            Version version = Assembly.GetExecutingAssembly().GetReferencedAssemblies().Where(x => x.Name == "Heroes.Helpers").ToList()[0].Version;
            return SemanticVersion.Parse(new Version(version.Major, version.Minor, version.Build).ToString());
        }

        /// <summary>
        /// Returns the version of Heroes.ReplayParser
        /// </summary>
        /// <returns></returns>
        public static SemanticVersion HeroesReplayParserVersion()
        {
            Version version = Assembly.GetExecutingAssembly().GetReferencedAssemblies().Where(x => x.Name == "Heroes.ReplayParser").ToList()[0].Version;
            return SemanticVersion.Parse(new Version(version.Major, version.Minor, version.Build).ToString());
        }
    }
}
