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
        public ReplaysControl()
        {
            InitializeComponent();

            if (QueryDb.SettingsDb.UserSettings.ReplayAutoStartStartUp)
                ((IInvokeProvider)new ButtonAutomationPeer(StartButton).GetPattern(PatternInterface‌​.Invoke)).Invoke();
        }
    }
}
