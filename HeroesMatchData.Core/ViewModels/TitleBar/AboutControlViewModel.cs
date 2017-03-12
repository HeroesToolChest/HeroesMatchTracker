using GalaSoft.MvvmLight;

namespace HeroesMatchData.Core.ViewModels.TitleBar
{
    public class AboutControlViewModel : ViewModelBase
    {
        public string HeroesMatchDataCoreVersion => AssemblyVersions.HeroesMatchDataCoreVersion().ToString();
        public string HeroesMatchDataDataVersion => AssemblyVersions.HeroesMatchDataDataVersion().ToString();
        public string HeroesMatchDataHelpersVersion => AssemblyVersions.HeroesHelpersVersion().ToString();
        public string HeroesIconsVersion => AssemblyVersions.HeroesIconsVersion().ToString();
        public string HeroesReplayParserVersion => AssemblyVersions.HeroesReplayParserVersion().ToString();
    }
}
