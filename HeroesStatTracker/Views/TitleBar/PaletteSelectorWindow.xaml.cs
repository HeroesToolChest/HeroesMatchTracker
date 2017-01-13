using GalaSoft.MvvmLight.Messaging;
using HeroesStatTracker.Core.Messaging;
using MahApps.Metro.Controls;

namespace HeroesStatTracker.Views.TitleBar
{
    /// <summary>
    /// Interaction logic for PaletteSelectorWindow.xaml
    /// </summary>
    public partial class PaletteSelectorWindow : MetroWindow
    {
        public PaletteSelectorWindow()
        {
            InitializeComponent();
        }

        private void PaletteSelectorWindow_Closed(object sender, System.EventArgs e)
        {
            Messenger.Default.Send(new NotificationMessage(StaticMessage.IsNightModeToggle));
        }
    }
}
