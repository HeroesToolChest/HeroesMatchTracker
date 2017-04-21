using HeroesMatchData.Core;
using System.Diagnostics;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace HeroesMatchData.Views.TitleBar
{
    /// <summary>
    /// Interaction logic for AboutControl.xaml
    /// </summary>
    public partial class AboutControl : UserControl
    {
        public AboutControl()
        {
            InitializeComponent();

            AppVersion.Text = $"Version: {App.VersionAsString()}";
        }

        private void Hyperlink_RequestNavigate(object sender, RequestNavigateEventArgs e)
        {
            if (ExternalLinkedSites.IsApprovedSite(e.Uri))
            {
                Process.Start(new ProcessStartInfo(e.Uri.AbsoluteUri));
                e.Handled = true;
            }
        }
    }
}
