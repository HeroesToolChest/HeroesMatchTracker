using System.Net.Http.Headers;

namespace HeroesMatchTracker.Core.ReplayUploads
{
    public static class Uploader
    {
        public static readonly ProductInfoHeaderValue UserAgentHeader = new ProductInfoHeaderValue("HeroesMatchTracker", AssemblyVersions.HeroesMatchTrackerVersion().ToString());
        public static readonly string AppUserAgent = $"HeroesMatchTracker/{AssemblyVersions.HeroesMatchTrackerVersion()}";
    }
}
