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
    public class FailedReplaysWindowViewModel : ViewModelBase
    {
        private int _totalFailedReplays;

        private IDatabaseService Database;

        private ObservableCollection<FailedReplay> _failedReplaysCollection = new ObservableCollection<FailedReplay>();

        public FailedReplaysWindowViewModel(IDatabaseService database)
        {
            Database = database;

            IsAutoRequeueOnUpdate = Database.SettingsDb().UserSettings.IsAutoRequeueOnUpdate;
            FailedReplaysCollection = new ObservableCollection<FailedReplay>(Database.SettingsDb().FailedReplays.ReadAllReplays());
            TotalFailedReplays = FailedReplaysCollection.Count;
        }

        public List<FailedReplay> SelectedReplays { get; private set; } = new List<FailedReplay>();

        public RelayCommand<object> SelectedFailedReplaysCommand => new RelayCommand<object>((list) => SetSelectedFailedReplays(list));
        public RelayCommand RefreshCommand => new RelayCommand(Refresh);
        public RelayCommand RequeueCommand => new RelayCommand(Requeue);
        public RelayCommand RemoveCommand => new RelayCommand(Remove);
        public RelayCommand RequeueAllCommand => new RelayCommand(RequeueAll);
        public RelayCommand RemoveAllCommand => new RelayCommand(RemoveAll);

        public int TotalFailedReplays
        {
            get => _totalFailedReplays;
            set
            {
                _totalFailedReplays = value;
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

        public ObservableCollection<FailedReplay> FailedReplaysCollection
        {
            get => _failedReplaysCollection;
            set
            {
                _failedReplaysCollection = value;
                RaisePropertyChanged();
            }
        }

        private void SetSelectedFailedReplays(object list)
        {
            SelectedReplays = ((IEnumerable)list).Cast<FailedReplay>().ToList();
        }

        private void Refresh()
        {
            FailedReplaysCollection = new ObservableCollection<FailedReplay>(Database.SettingsDb().FailedReplays.ReadAllReplays());
            TotalFailedReplays = FailedReplaysCollection.Count;
        }

        private void Requeue()
        {
            var selectedReplays = new List<FailedReplay>(SelectedReplays);

            foreach (var replay in SelectedReplays)
            {
                Database.SettingsDb().FailedReplays.DeleteFailedReplay(replay.FailedReplayId);
                FailedReplaysCollection.Remove(replay);
            }

            TotalFailedReplays = FailedReplaysCollection.Count;

            Messenger.Default.Send(selectedReplays);
        }

        private void Remove()
        {
            foreach (var replay in SelectedReplays)
            {
                Database.SettingsDb().FailedReplays.DeleteFailedReplay(replay.FailedReplayId);
                FailedReplaysCollection.Remove(replay);
            }

            TotalFailedReplays = FailedReplaysCollection.Count;

            Messenger.Default.Send<List<FailedReplay>>(null);
        }

        private void RequeueAll()
        {
            var unparsedReplaysList = Database.SettingsDb().FailedReplays.ReadAllReplays();
            Database.SettingsDb().FailedReplays.DeleteAllFailedReplays();
            FailedReplaysCollection = null;
            TotalFailedReplays = 0;

            Messenger.Default.Send(unparsedReplaysList);
        }

        private void RemoveAll()
        {
            Database.SettingsDb().FailedReplays.DeleteAllFailedReplays();
            FailedReplaysCollection = null;
            TotalFailedReplays = 0;

            Messenger.Default.Send<List<FailedReplay>>(null);
        }
    }
}
