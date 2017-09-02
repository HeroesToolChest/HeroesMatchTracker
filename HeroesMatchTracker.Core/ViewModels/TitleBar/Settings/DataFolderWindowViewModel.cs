using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using GalaSoft.MvvmLight.Messaging;
using HeroesMatchTracker.Core.Messaging;
using HeroesMatchTracker.Core.ViewServices;
using HeroesMatchTracker.Data;
using Microsoft.WindowsAPICodePack.Dialogs;
using System.Diagnostics;
using System.IO;

namespace HeroesMatchTracker.Core.ViewModels.TitleBar.Settings
{
    public class DataFolderWindowViewModel : ViewModelBase
    {
        private bool _isSaveButtonEnabled;
        private string _message;
        private string _dataFolderLocation;

        private IDatabaseService Database;
        private IMainWindowDialogService WindowDialog;
        private IMainTabService MainTab;
        private string CurrentDataFolderLocation;

        public DataFolderWindowViewModel(IDatabaseService database, IMainWindowDialogService windowDialog, IMainTabService mainTab)
        {
            Database = database;
            WindowDialog = windowDialog;
            MainTab = mainTab;

            IsSaveButtonEnabled = false;
            CurrentDataFolderLocation = DataFolderLocation = Database.SettingsDb().UserSettings.DataFolderLocation;
        }

        public bool IsSaveButtonEnabled
        {
            get => _isSaveButtonEnabled;
            set
            {
                _isSaveButtonEnabled = value;
                RaisePropertyChanged();
            }
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

            if (CurrentDataFolderLocation != DataFolderLocation)
                IsSaveButtonEnabled = true;
            else
                IsSaveButtonEnabled = false;
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

                if (CurrentDataFolderLocation != dialog.FileName)
                    IsSaveButtonEnabled = true;
                else
                    IsSaveButtonEnabled = false;
            }
        }

        private void SaveLocation(ICloseable window)
        {
            if (!Directory.Exists(DataFolderLocation))
            {
                Message = "Could not save, folder does not exist.";
                return;
            }

            if (CurrentDataFolderLocation != DataFolderLocation)
            {
                Database.SettingsDb().UserSettings.DataFolderLocation = DataFolderLocation;
                CurrentDataFolderLocation = DataFolderLocation;
                Messenger.Default.Send(new NotificationMessage(StaticMessage.UpdateDataFolderLocation));
                MainTab.SetExtendedSettingsText("(Restart Required)");

                IsSaveButtonEnabled = false;
                CancelLocation(window);

                WindowDialog.ShowSimpleMessageAsync("Restart Required", "In order for the new data path location to be applied, the application needs to be restarted.");
            }
        }

        private void CancelLocation(ICloseable window)
        {
            DataFolderLocation = CurrentDataFolderLocation;

            if (window != null)
            {
                window.Close();
            }
        }
    }
}
