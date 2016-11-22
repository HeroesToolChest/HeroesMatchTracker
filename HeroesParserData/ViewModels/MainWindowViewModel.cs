using GalaSoft.MvvmLight.Messaging;
using HeroesParserData.Messages;

namespace HeroesParserData.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        private int _selectedMainTab;
        private int _selectedStatisticsTab;

        public string AppVersion
        {
            get
            {
                return HPDVersion.GetVersion();
            }
        }

        public int SelectedMainTab
        {
            get { return _selectedMainTab; }
            set
            {
                _selectedMainTab = value;
                if (value == 0)
                    Messenger.Default.Send(new HomeTabMessage() { Trigger = Trigger.Update });

                if (value == 1)
                    Messenger.Default.Send(new MatchSummaryMessage() { MatchSummary = MatchSummary.LastMatch });

                if (value == 4) // stats tab
                {
                    SelectedStatisticsTab = SelectedStatisticsTab;
                }
                RaisePropertyChangedEvent(nameof(SelectedMainTab));
            }
        }

        public int SelectedStatisticsTab
        {
            get { return _selectedStatisticsTab; }
            set
            {
                _selectedStatisticsTab = value;
                if (value == 0)
                    Messenger.Default.Send(new StatisticsTabMessage { StatisticsTab = StatisticsTab.Overview });
                else if (value == 1)
                    Messenger.Default.Send(new StatisticsTabMessage { StatisticsTab = StatisticsTab.Heroes });
                else if (value == 2)
                    Messenger.Default.Send(new StatisticsTabMessage { StatisticsTab = StatisticsTab.GameModes });
                RaisePropertyChangedEvent(nameof(SelectedStatisticsTab));
            }
        }

        public MainWindowViewModel() 
            : base()
        {

        }
    }
}
