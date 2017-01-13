using GalaSoft.MvvmLight.Ioc;
using HeroesStatTracker.Core.ViewServices;
using System.Windows.Controls;

namespace HeroesStatTracker.Views.TitleBar
{
    /// <summary>
    /// Interaction logic for SettingsControl.xaml
    /// </summary>
    public partial class SettingsControl : UserControl, IPaletteSelectorWindowService
    {
        public SettingsControl()
        {
            InitializeComponent();

            SimpleIoc.Default.Register<IPaletteSelectorWindowService>(() => this);
        }

        public void CreatePaletteWindow()
        {
            PaletteSelectorWindow window = new PaletteSelectorWindow();
            window.ShowDialog();
        }
    }
}
