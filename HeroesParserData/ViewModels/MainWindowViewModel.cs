using GalaSoft.MvvmLight.Messaging;
using HeroesParserData.Messages;
using System;
using System.Reflection;

namespace HeroesParserData.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        private int _selectedTab;

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

        public int SelectedTab
        {
            get { return _selectedTab; }
            set
            {
                _selectedTab = value;
                if (value == 0)
                    Messenger.Default.Send(new HomeWindowMessage() { Trigger = Trigger.Update });
                else
                    Messenger.Default.Send(new HomeWindowMessage() { Trigger = Trigger.Stop });
                RaisePropertyChangedEvent(nameof(SelectedTab));
            }
        }

        public MainWindowViewModel() 
            : base()
        {

        }
    }
}
