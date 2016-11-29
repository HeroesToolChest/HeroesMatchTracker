using NuGet;
using System;
using System.Linq;
using System.Reflection;

namespace HeroesParserData
{
    public class HPDVersion
    {
        /// <summary>
        /// Returns the version of Heroes Parser Data. x.x.x.x if revision is available otherwise x.x.x
        /// </summary>
        /// <returns></returns>
        public static string GetVersionAsString()
        {
            Version version = Assembly.GetEntryAssembly().GetName().Version;
            if (version.Revision == 0)
                return $"{version.Major}.{version.Minor}.{version.Build}";
            else
                return $"{version.Major}.{version.Minor}.{version.Build}.{version.Revision}";
        }

        /// <summary>
        /// Returns the Version fo Heroes Parser Data
        /// </summary>
        /// <returns></returns>
        public static Version GetVersion()
        {
            return Assembly.GetEntryAssembly().GetName().Version; 
        }

        /// <summary>
        /// Returnss x.x.x.x of Heroes Icons
        /// </summary>
        /// <returns></returns>
        public static string GetHeroesIconsVersionAsString()
        {
            Version version = Assembly.GetExecutingAssembly().GetReferencedAssemblies().Where(x => x.Name == "HeroesIcons").ToList()[0].Version;
            return $"{version.Major}.{version.Minor}.{version.Build}.{version.Revision}";
        }

        /// <summary>
        /// Returns x.x.x.x of Heroes.ReplayParser
        /// </summary>
        /// <returns></returns>
        public static string GetHeroesReplayParserVersionAsString()
        {
            Version version = Assembly.GetExecutingAssembly().GetReferencedAssemblies().Where(x => x.Name == "Heroes.ReplayParser").ToList()[0].Version;
            return $"{version.Major}.{version.Minor}.{version.Build}.{version.Revision}";
        }

        /// <summary>
        /// Returns the revision number of Heroes.ReplayParser
        /// </summary>
        /// <returns></returns>
        public static int GetHeroesReplayParserSupportedBuild()
        {
            Version version = Assembly.GetExecutingAssembly().GetReferencedAssemblies().Where(x => x.Name == "Heroes.ReplayParser").ToList()[0].Version;
            return version.Revision;
        }
    }
}
