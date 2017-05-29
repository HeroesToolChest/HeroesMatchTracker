using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using HeroesMatchTracker.Data;

namespace HeroesMatchTracker.Core.ViewModels.Replays
{
    public class UnParsedReplaysWindowViewModel : ViewModelBase
    {
        private IDatabaseService DatabaseService;

        public UnParsedReplaysWindowViewModel(IDatabaseService databaseService)
        {
            DatabaseService = databaseService;
        }

        public RelayCommand RequeueCommand => new RelayCommand(Requeue);
        public RelayCommand RemoveCommand => new RelayCommand(Remove);
        public RelayCommand RequeueAllCommand => new RelayCommand(RequeueAll);
        public RelayCommand RemoveAllCommand => new RelayCommand(RemoveAll);

        private void Requeue()
        {

        }

        private void Remove()
        {

        }

        private void RequeueAll()
        {

        }

        private void RemoveAll()
        {

        }
    }
}
