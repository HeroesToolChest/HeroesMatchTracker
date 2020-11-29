using HeroesMatchTracker.Core.Startup;
using Splat;
using System.Runtime.InteropServices;

namespace HeroesMatchTracker.Infrastructure
{
    public class LoadStartup : ILoadStartup, IEnableLogger
    {
        public LoadStartup()
        {
        }

        public void LogSystemInformation()
        {
            this.Log().Info($"      Operating System: {RuntimeInformation.OSDescription}");
            this.Log().Info($"Operating Architecture: {RuntimeInformation.OSArchitecture}");
            this.Log().Info($"  Process Architecture: {RuntimeInformation.ProcessArchitecture}");
            this.Log().Info($"             Framework: {RuntimeInformation.FrameworkDescription}");
        }
    }
}
