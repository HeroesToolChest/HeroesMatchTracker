using GalaSoft.MvvmLight;

namespace HeroesMatchData.Core.ViewModels.TitleBar
{
    public class AboutControlViewModel : ViewModelBase
    {
        public string HeroesMatchDataCoreVersion { get { return AssemblyVersions.HeroesMatchDataCoreVersion().ToString(); } }
        public string HeroesMatchDataDataVersion { get { return AssemblyVersions.HeroesMatchDataDataVersion().ToString(); } }
        public string HeroesMatchDataHelpersVersion { get { return AssemblyVersions.HeroesHelpersVersion().ToString(); } }
        public string HeroesIconsVersion { get { return AssemblyVersions.HeroesIconsVersion().ToString(); } }
        public string HeroesReplayParserVersion { get { return AssemblyVersions.HeroesReplayParserVersion().ToString(); } }
    }
}
