using NuGet;
using System;
using System.Linq;
using System.Reflection;

namespace HeroesMatchData.Core
{
    public static class AssemblyVersions
    {
        /// <summary>
        /// Returns the version of HeroesMatchData
        /// </summary>
        /// <returns></returns>
        public static SemanticVersion HeroesMatchDataVersion()
        {
            Version version = Assembly.GetEntryAssembly().GetName().Version;
            return SemanticVersion.Parse(new Version(version.Major, version.Minor, version.Build).ToString());
        }

        /// <summary>
        /// Returns the version of HeroesMatchData.Core
        /// </summary>
        /// <returns></returns>
        public static SemanticVersion HeroesMatchDataCoreVersion()
        {
            Version version = Assembly.GetExecutingAssembly().GetName().Version;
            return SemanticVersion.Parse(new Version(version.Major, version.Minor, version.Build).ToString());
        }

        /// <summary>
        /// Returns the version of HeroesMatchData.Data
        /// </summary>
        /// <returns></returns>
        public static SemanticVersion HeroesMatchDataDataVersion()
        {
            Version version = Assembly.GetExecutingAssembly().GetReferencedAssemblies().Where(x => x.Name == "HeroesMatchData.Data").ToList()[0].Version;
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
