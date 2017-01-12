using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using HeroesStatTracker.Core.ViewServices;
using Microsoft.Practices.ServiceLocation;

namespace HeroesStatTracker.Core.ViewModels.TitleBar
{
    public class SettingsControlViewModel : ViewModelBase
    {
        public RelayCommand OpenPaletteSelectorWindowCommand => new RelayCommand(ExecutePaletteSelectorWindowCommand);

        public IPaletteSelectorWindowService PaletteSelectorWindowService
        {
            get { return ServiceLocator.Current.GetInstance<IPaletteSelectorWindowService>(); }
        }

        private void ExecutePaletteSelectorWindowCommand()
        {
            PaletteSelectorWindowService.CreatePaletteWindow();
        }
    }
}
