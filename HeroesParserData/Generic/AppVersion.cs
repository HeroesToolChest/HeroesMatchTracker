using System;
using System.Reflection;

namespace HeroesParserData
{
    public class HPDVersion
    {
        public static string GetVersion()
        {
            Version version = Assembly.GetEntryAssembly().GetName().Version;
            if (version.Revision == 0)
                return $"{version.Major}.{version.Minor}.{version.Build}";
            else
                return $"{version.Major}.{version.Minor}.{version.Build}.{version.Revision}";
        }
    }
}
