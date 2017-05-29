using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using HeroesMatchTracker.Data;
using HeroesMatchTracker.Data.Models.Settings;
using System.Collections.ObjectModel;

namespace HeroesMatchTracker.Core.ViewModels.Replays
{
    public class UnParsedReplaysWindowViewModel : ViewModelBase
    {
        private IDatabaseService DatabaseService;

        private ObservableCollection<UnParsedReplay> _unParsedReplaysCollection = new ObservableCollection<UnParsedReplay>();

        public UnParsedReplaysWindowViewModel(IDatabaseService databaseService)
        {
            DatabaseService = databaseService;
        }

        public RelayCommand RequeueCommand => new RelayCommand(Requeue);
        public RelayCommand RemoveCommand => new RelayCommand(Remove);
        public RelayCommand RequeueAllCommand => new RelayCommand(RequeueAll);
        public RelayCommand RemoveAllCommand => new RelayCommand(RemoveAll);

        public ObservableCollection<UnParsedReplay> UnParsedReplaysCollection
        {
            get => _unParsedReplaysCollection;
            set
            {
                _unParsedReplaysCollection = value;
                RaisePropertyChanged();
            }
        }

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
