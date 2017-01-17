using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using HeroesStatTracker.Core.ViewServices;
using Microsoft.Practices.ServiceLocation;

namespace HeroesStatTracker.Core.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        public MainWindowViewModel() { }

        public string AppVersion { get { return AssemblyVersions.HeroesStatTrackerVersion().ToString(); } }

        public RelayCommand OpenWhatsNewWindowCommand => new RelayCommand(ExecuteOpenWhatsNewWindowCommand);

        public IWhatsNewWindowService WhatsNewWindowService
        {
            get { return ServiceLocator.Current.GetInstance<IWhatsNewWindowService>(); }
        }

        private void ExecuteOpenWhatsNewWindowCommand()
        {
            WhatsNewWindowService.CreateWhatsNewWindow();
        }
    }
}