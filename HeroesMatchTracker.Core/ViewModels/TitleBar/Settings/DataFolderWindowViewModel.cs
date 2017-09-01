using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using HeroesMatchTracker.Core.ViewServices;
using HeroesMatchTracker.Data;
using Microsoft.WindowsAPICodePack.Dialogs;
using System.Diagnostics;
using System.IO;

namespace HeroesMatchTracker.Core.ViewModels.TitleBar.Settings
{
    public class DataFolderWindowViewModel : ViewModelBase
    {
        private string _message;
        private string _dataFolderLocation;

        private IDatabaseService Database;
        private IMainWindowDialogService WindowDialog;

        public DataFolderWindowViewModel(IDatabaseService database, IMainWindowDialogService windowDialog)
        {
            Database = database;
            WindowDialog = windowDialog;

            DataFolderLocation = Database.SettingsDb().UserSettings.DataFolderLocation;
        }

        public string Message
        {
            get => _message;
            set
            {
                _message = value;
                RaisePropertyChanged();
            }
        }

        public string DataFolderLocation
        {
            get => _dataFolderLocation;
            set
            {
                _dataFolderLocation = value;
                RaisePropertyChanged();
            }
        }

        public string DefaultLocationPath
        {
            get => Data.Database.DefaultDataLocation;
        }

        public RelayCommand OpenLocationCommand => new RelayCommand(OpenLocation);
        public RelayCommand DefaultLocationCommand => new RelayCommand(DefaultLocation);
        public RelayCommand BrowseLocationCommand => new RelayCommand(BrowseLocation);
        public RelayCommand<ICloseable> SaveLocationCommand => new RelayCommand<ICloseable>(SaveLocation);
        public RelayCommand<ICloseable> CancelLocationCommand => new RelayCommand<ICloseable>(CancelLocation);

        private void OpenLocation()
        {
            if (string.IsNullOrEmpty(DataFolderLocation) || !Directory.Exists(DataFolderLocation))
            {
                Message = "Could not open folder location, folder does not exist.";
                return;
            }

            Process.Start(DataFolderLocation);
        }

        private void DefaultLocation()
        {
            if (!Directory.Exists(Data.Database.DefaultDataLocation))
                Directory.CreateDirectory(Data.Database.DefaultDataLocation);

            DataFolderLocation = Data.Database.DefaultDataLocation;
        }

        private void BrowseLocation()
        {
            Message = string.Empty;

            var dialog = new CommonOpenFileDialog()
            {
                IsFolderPicker = true,
                InitialDirectory = DataFolderLocation,
            };

            CommonFileDialogResult result = dialog.ShowDialog();

            if (result == CommonFileDialogResult.Ok)
            {
                DataFolderLocation = dialog.FileName;
            }
        }

        private void SaveLocation(ICloseable window)
        {
            if (!Directory.Exists(DataFolderLocation))
            {
                Message = "Could not save, folder does not exist.";
                return;
            }

            Database.SettingsDb().UserSettings.DataFolderLocation = DataFolderLocation;
            CancelLocation(window);

            WindowDialog.ShowSimpleMessageAsync("Restart Required", "In order for the new data location path to be used, the application needs to be restarted.");
        }

        private void CancelLocation(ICloseable window)
        {
            if (window != null)
            {
                window.Close();
            }
        }
    }
}
