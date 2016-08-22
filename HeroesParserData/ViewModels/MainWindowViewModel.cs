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
                if (version.Revision == 0)
                    return $"{version.Major}.{version.Minor}.{version.Build}";
                else
                    return $"{version.Major}.{version.Minor}.{version.Build}.{version.Revision}";
            }
        }

        public MainWindowViewModel()
        {

        }
    }
}
