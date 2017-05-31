using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using GalaSoft.MvvmLight.Messaging;
using HeroesMatchTracker.Data;
using HeroesMatchTracker.Data.Models.Settings;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace HeroesMatchTracker.Core.ViewModels.Replays
{
    public class UnparsedReplaysWindowViewModel : ViewModelBase
    {
        private int _totalUnparsedReplays;

        private IDatabaseService Database;

        private ObservableCollection<UnparsedReplay> _unParsedReplaysCollection = new ObservableCollection<UnparsedReplay>();

        public UnparsedReplaysWindowViewModel(IDatabaseService database)
        {
            Database = database;

            IsAutoRequeueOnUpdate = Database.SettingsDb().UserSettings.IsAutoRequeueOnUpdate;
            UnparsedReplaysCollection = new ObservableCollection<UnparsedReplay>(Database.SettingsDb().UnparsedReplays.ReadAllReplays());
            TotalUnparsedReplays = UnparsedReplaysCollection.Count;
        }

        public List<UnparsedReplay> SelectedReplays { get; private set; } = new List<UnparsedReplay>();

        public RelayCommand<object> SelectedUnparsedReplaysCommand => new RelayCommand<object>((list) => SetSelectedUnparsedReplays(list));
        public RelayCommand RefreshCommand => new RelayCommand(Refresh);
        public RelayCommand RequeueCommand => new RelayCommand(Requeue);
        public RelayCommand RemoveCommand => new RelayCommand(Remove);
        public RelayCommand RequeueAllCommand => new RelayCommand(RequeueAll);
        public RelayCommand RemoveAllCommand => new RelayCommand(RemoveAll);

        public int TotalUnparsedReplays
        {
            get => _totalUnparsedReplays;
            set
            {
                _totalUnparsedReplays = value;
                RaisePropertyChanged();
            }
        }

        public bool IsAutoRequeueOnUpdate
        {
            get => Database.SettingsDb().UserSettings.IsAutoRequeueOnUpdate;
            set
            {
                Database.SettingsDb().UserSettings.IsAutoRequeueOnUpdate = value;
                RaisePropertyChanged();
            }
        }

        public ObservableCollection<UnparsedReplay> UnparsedReplaysCollection
        {
            get => _unParsedReplaysCollection;
            set
            {
                _unParsedReplaysCollection = value;
                RaisePropertyChanged();
            }
        }

        private void SetSelectedUnparsedReplays(object list)
        {
            SelectedReplays = ((IEnumerable)list).Cast<UnparsedReplay>().ToList();
        }

        private void Refresh()
        {
            UnparsedReplaysCollection = new ObservableCollection<UnparsedReplay>(Database.SettingsDb().UnparsedReplays.ReadAllReplays());
            TotalUnparsedReplays = UnparsedReplaysCollection.Count;
        }

        private void Requeue()
        {
            var selectedReplays = new List<UnparsedReplay>(SelectedReplays);

            foreach (var replay in SelectedReplays)
            {
                Database.SettingsDb().UnparsedReplays.DeleteUnParsedReplay(replay.UnparsedReplaysId);
                UnparsedReplaysCollection.Remove(replay);
            }

            TotalUnparsedReplays = UnparsedReplaysCollection.Count;

            Messenger.Default.Send(selectedReplays);
        }

        private void Remove()
        {
            foreach (var replay in SelectedReplays)
            {
                Database.SettingsDb().UnparsedReplays.DeleteUnParsedReplay(replay.UnparsedReplaysId);
                UnparsedReplaysCollection.Remove(replay);
            }

            TotalUnparsedReplays = UnparsedReplaysCollection.Count;

            Messenger.Default.Send<List<UnparsedReplay>>(null);
        }

        private void RequeueAll()
        {
            var unparsedReplaysList = Database.SettingsDb().UnparsedReplays.ReadAllReplays();
            Database.SettingsDb().UnparsedReplays.DeleteAllUnParsedReplays();
            UnparsedReplaysCollection = null;
            TotalUnparsedReplays = 0;

            Messenger.Default.Send(unparsedReplaysList);
        }

        private void RemoveAll()
        {
            Database.SettingsDb().UnparsedReplays.DeleteAllUnParsedReplays();
            UnparsedReplaysCollection = null;
            TotalUnparsedReplays = 0;

            Messenger.Default.Send<List<UnparsedReplay>>(null);
        }
    }
}
