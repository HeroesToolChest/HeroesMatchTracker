using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using HeroesStatTracker.Core.ViewServices;
using HeroesStatTracker.Data;
using Microsoft.Practices.ServiceLocation;

namespace HeroesStatTracker.Core.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        private IDatabaseService Database;

        public MainWindowViewModel(IDatabaseService database)
        {
            Database = database;
        }

        public IDatabaseService GetDatabaseService { get { return Database; } }

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