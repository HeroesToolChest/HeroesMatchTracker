using GalaSoft.MvvmLight.Messaging;
using HeroesParserData.Messages;

namespace HeroesParserData.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        private int _selectedTab;

        public string AppVersion
        {
            get
            {
                return HPDVersion.GetVersion();
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
