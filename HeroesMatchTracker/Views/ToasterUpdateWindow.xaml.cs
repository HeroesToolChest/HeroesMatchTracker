using HeroesMatchTracker.Core;
using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Navigation;
using System.Windows.Threading;

namespace HeroesMatchTracker.Views
{
    /// <summary>
    /// Interaction logic for ToasterWindow.xaml
    /// </summary>
    public partial class ToasterUpdateWindow
    {
        public ToasterUpdateWindow(string currentVersion, string newVersion)
        {
            InitializeComponent();
            ShowInTaskbar = false;

            CurrentVersion.Text = currentVersion;
            NewVersion.Text = newVersion;

            Dispatcher.BeginInvoke(DispatcherPriority.ApplicationIdle, new Action(() =>
            {
                var workingArea = SystemParameters.WorkArea;
                var transform = PresentationSource.FromVisual(this).CompositionTarget.TransformFromDevice;
                var corner = transform.Transform(new Point(workingArea.Right, workingArea.Bottom));

                Left = corner.X - ActualWidth - 20;
                Top = corner.Y - ActualHeight - 20;
            }));
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
