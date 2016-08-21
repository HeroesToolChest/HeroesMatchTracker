using System;
using System.Reflection;

namespace HeroesParserData.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        public string AppVersion
        {
            get
            {
                Version version = Assembly.GetEntryAssembly().GetName().Version;
            #if DEBUG
                return $"{version.Major}.{version.Minor}.{version.Build}.{version.Revision}";
            #endif
            #if !DEBUG
                return $"{version.Major}.{version.Minor}.{version.Build}";
            #endif
            }
        }

        public MainWindowViewModel()
        {

        }
    }
}
