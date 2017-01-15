using NuGet;
using System;
using System.Linq;
using System.Reflection;

namespace HeroesStatTracker.Core
{
    public static class AssemblyVersions
    {
        /// <summary>
        /// Returns the version of HeroesStatTracker
        /// </summary>
        /// <returns></returns>
        public static SemanticVersion HeroesStatTrackerVersion()
        {
            Version version = Assembly.GetEntryAssembly().GetName().Version;
            return SemanticVersion.Parse(new Version(version.Major, version.Minor, version.Build).ToString());
        }

        /// <summary>
        /// Returns the version of HeroesStatTracker.Core
        /// </summary>
        /// <returns></returns>
        public static SemanticVersion HeroesStatTrackerCoreVersion()
        {
            Version version = Assembly.GetExecutingAssembly().GetName().Version;
            return SemanticVersion.Parse(new Version(version.Major, version.Minor, version.Build).ToString());
        }

        /// <summary>
        /// Returns the version of HeroesStatTracker.Data
        /// </summary>
        /// <returns></returns>
        public static SemanticVersion HeroesStatTrackerDataVersion()
        {
            Version version = Assembly.GetExecutingAssembly().GetReferencedAssemblies().Where(x => x.Name == "HeroesStatTracker.Data").ToList()[0].Version;
            return SemanticVersion.Parse(new Version(version.Major, version.Minor, version.Build).ToString());
        }

        /// <summary>
        /// Returns the version of Heroes.Icons
        /// </summary>
        /// <returns></returns>
        public static SemanticVersion HeroesIconsVersion()
        {
            return SemanticVersion.Parse(Assembly.GetExecutingAssembly().GetReferencedAssemblies().Where(x => x.Name == "Heroes.Icons").ToList()[0].Version.ToString());
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
            return SemanticVersion.Parse(Assembly.GetExecutingAssembly().GetReferencedAssemblies().Where(x => x.Name == "Heroes.ReplayParser").ToList()[0].Version.ToString());
        }
    }
}
