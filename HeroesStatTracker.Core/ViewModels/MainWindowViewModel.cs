using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using HeroesStatTracker.Core.ViewServices;
using HeroesStatTracker.Data;
using Microsoft.Practices.ServiceLocation;

namespace HeroesStatTracker.Core.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        private IDatabaseService IDatabaseService;

        public MainWindowViewModel(IDatabaseService iDatabaseService)
        {
            IDatabaseService = iDatabaseService;
        }

        public IDatabaseService GetDatabaseService { get { return IDatabaseService; } }

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