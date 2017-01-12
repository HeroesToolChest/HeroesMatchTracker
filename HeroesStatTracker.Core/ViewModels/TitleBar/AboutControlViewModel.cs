using GalaSoft.MvvmLight;

namespace HeroesStatTracker.Core.ViewModels.TitleBar
{
    public class AboutControlViewModel : ViewModelBase
    {
        public string HeroesStatTrackerCoreVersion { get { return AssemblyVersions.HeroesStatTrackerCoreVersion().ToString(); } }
        public string HeroesStatTrackerDataVersion { get { return AssemblyVersions.HeroesStatTrackerDataVersion().ToString(); } }
        //public string HeroesStatTrackerHelpersVersion { get { return AssemblyVersions.HeroesHelpersVersion().ToString(); } }
        public string HeroesIconsVersion { get { return AssemblyVersions.HeroesIconsVersion().ToString(); } }
        //public string HeroesReplayParserVersion { get { return AssemblyVersions.HeroesReplayParserVersion().ToString(); } }
    }
}
