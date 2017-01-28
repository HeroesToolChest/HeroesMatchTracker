using HeroesStatTracker.Core.ViewModels.Replays;
using HeroesStatTracker.Data;
using System.Windows.Automation.Peers;
using System.Windows.Automation.Provider;
using System.Windows.Controls;

namespace HeroesStatTracker.Views.Replays
{
    /// <summary>
    /// Interaction logic for ReplaysControl.xaml
    /// </summary>
    public partial class ReplaysControl : UserControl
    {
        private IDatabaseService IDatabaseService;

        public ReplaysControl()
        {
            InitializeComponent();

            IDatabaseService = ReplaysControlViewModel.GetDatabaseService;

            if (IDatabaseService.SettingsDb().UserSettings.ReplayAutoStartStartUp)
                ((IInvokeProvider)new ButtonAutomationPeer(StartButton).GetPattern(PatternInterface‌​.Invoke)).Invoke();
        }

        public ReplaysControlViewModel ReplaysControlViewModel
        {
            get { return (ReplaysControlViewModel)DataContext; }
        }
    }
}
