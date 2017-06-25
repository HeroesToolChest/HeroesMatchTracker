using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using HeroesMatchTracker.Data;

namespace HeroesMatchTracker.Core.ViewModels.Matches
{
    public class PlayerNotesWindowViewModel : ViewModelBase
    {
        private string _playerNotes;
        private string _playerNotesHeader;

        private IDatabaseService Database;

        public PlayerNotesWindowViewModel(IDatabaseService database)
        {
            Database = database;

            Database.ReplaysDb().HotsPlayer.ReadRecordFromPlayerId(0);
        }

        public IDatabaseService GetDatabaseService => Database;

        public string PlayerNotes
        {
            get => _playerNotes;
            set
            {
                _playerNotes = value;
                RaisePropertyChanged();
            }
        }

        public string PlayerNotesHeader
        {
            get => _playerNotesHeader;
            set
            {
                _playerNotesHeader = value;
                RaisePropertyChanged();
            }
        }

        public RelayCommand SaveCommand => new RelayCommand(Save);
        public RelayCommand SaveCloseCommand => new RelayCommand(SaveClose);
        public RelayCommand CloseCommand => new RelayCommand(Close);

        private void Save()
        {
            //Database.ReplaysDb().HotsPlayer.ReadRecordFromPlayerId
        }

        private void SaveClose()
        {

        }

        private void Close()
        {

        }
    }
}
