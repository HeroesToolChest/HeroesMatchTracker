using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using HeroesMatchTracker.Core.Services;
using HeroesMatchTracker.Core.ViewServices;
using NLog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace HeroesMatchTracker.Core.Models.ReplayModels.Uploaders
{
    public abstract class UploaderBaseModel : ObservableObject
    {
        private bool _buttonsEnabled;
        private bool _startButtonEnabled;
        private string _uploaderStatus;
        private string _fileStatus;
        private StartButtonState _startButtonText;

        private string HostName;

        public UploaderBaseModel(IInternalService internalService, IMainTabService mainTab, string hostName)
        {
            HostName = hostName;
            InternalService = internalService;
            MainTab = mainTab;

            IsParsingReplaysOn = false;
            IsIdleMode = true;
            StartButtonText = StartButtonState.START;
            UploaderStatus = "Disabled";
        }

        public RelayCommand StartButtonCommand => new RelayCommand(StartButton);
        public RelayCommand LatestDateTimeSetCommand => new RelayCommand(LatestDateTimeSet);
        public RelayCommand LatestDateTimeDefaultCommand => new RelayCommand(LatestDateTimeDefault);
        public RelayCommand LastDateTimeSetCommand => new RelayCommand(LastDateTimeSet);
        public RelayCommand LastDateTimeDefaultCommand => new RelayCommand(LastDateTimeDefault);

        /// <summary>
        /// Indicates whether the replay parser is in use (parsing only)
        /// </summary>
        public bool IsParsingReplaysOn { get; set; }

        public bool IsIdleMode { get; set; }
        public ParserCheckboxes ParserCheckboxes { get; set; }
        public Queue<ReplayFile> ReplayUploadQueue { get; } = new Queue<ReplayFile>();

        public string UploaderStatus
        {
            get => _uploaderStatus;
            set
            {
                _uploaderStatus = value;
                RaisePropertyChanged();
            }
        }

        public string UploadFileStatus
        {
            get => _fileStatus;
            set
            {
                _fileStatus = value;
                RaisePropertyChanged();
            }
        }

        public bool ButtonsEnabled
        {
            get => _buttonsEnabled;
            set
            {
                _buttonsEnabled = value;
                RaisePropertyChanged();
            }
        }

        public bool StartButtonEnabled
        {
            get => _startButtonEnabled;
            set
            {
                _startButtonEnabled = value;
                RaisePropertyChanged();
            }
        }

        public StartButtonState StartButtonText
        {
            get => _startButtonText;
            set
            {
                _startButtonText = value;
                RaisePropertyChanged();
            }
        }

        public abstract bool IsUploaderEnabled { get; set; }
        public abstract DateTime ReplaysLatestUploaded { get; set; }
        public abstract DateTime ReplaysLastUploaded { get; set; }

        protected static Logger UploaderLog => LogManager.GetLogger(LogFileNames.ReplayUploaderLogFileName);

        protected bool RequestUploaderToStop { get; set; }
        protected bool IsHostUnderMaintenance { get; set; }
        protected ReplayFile CurrentReplayFile { get; set; }
        protected IInternalService InternalService { get; set; }
        protected IMainTabService MainTab { get; set; }

        /// <summary>
        /// Request that the uploader start
        /// </summary>
        public void RequestStart()
        {
            if (IsUploaderEnabled)
            {
                if (IsParsingReplaysOn)
                    StartButtonText = StartButtonState.STOP;

                StartButtonEnabled = true;

                ButtonsEnabled = false;
                RequestUploaderToStop = false;

                if (!IsParsingReplaysOn)
                    ReplayUploadQueue.Clear();
            }
        }

        /// <summary>
        /// Request that the uploader stop
        /// </summary>
        public void RequestStop()
        {
            if (IsUploaderEnabled)
            {
                StartButtonText = StartButtonState.STOPPING;

                if (!IsParsingReplaysOn)
                    StartButtonEnabled = false;

                RequestUploaderToStop = true;
            }
        }

        protected async Task InitiateUploadQueueAsync()
        {
            while (IsUploaderEnabled)
            {
                CurrentReplayFile = null;
                UploadFileStatus = string.Empty;

                if (await HostMaintenanceMode())
                    continue;

                if (await IdleMode())
                    continue;

                UploaderStatus = "Uploading";
                ButtonsEnabled = false;
                StartButtonText = StartButtonState.STOP;

                await Uploader();
            }

            UploaderStatus = "Disabled";
            await Task.CompletedTask;
        }

        protected abstract void LatestDateTimeDefault();
        protected abstract void LastDateTimeDefault();
        protected abstract void UpdateUploadStatus(ReplayFileUploaderStatus status);
        protected abstract Task Uploader();

        protected bool ReplayExists()
        {
            if (!File.Exists(CurrentReplayFile.FilePath))
            {
                UploaderLog.Log(LogLevel.Info, $"File does not exists: {CurrentReplayFile.FilePath}");

                // remove it from queue
                ReplayUploadQueue.Dequeue();

                return false;
            }

            return true;
        }

        protected bool InvalidReplayCheck()
        {
            if (CurrentReplayFile.ReplayId == 0 || CurrentReplayFile.TimeStamp == DateTime.MinValue)
            {
                if (CurrentReplayFile.ReplayId == 0) UploaderLog.Log(LogLevel.Info, "A ReplayId of 0 was detected");
                if (CurrentReplayFile.TimeStamp == DateTime.MinValue) UploaderLog.Log(LogLevel.Info, "A TimeStamp of 1/1/0001 was detected");

                // remove it before we continue
                ReplayUploadQueue.Dequeue();
                return true;
            }

            return false;
        }

        private async Task<bool> HostMaintenanceMode()
        {
            if (IsHostUnderMaintenance)
            {
                UploaderStatus = $"{HostName} is currently down for maintenance";

                // wait 30 mintues before retrying again
                for (int i = 1800000; i >= 0; i = i - 1000)
                {
                    UploadFileStatus = $"({HostName} maintenance) Retrying in {TimeSpan.FromMilliseconds(i).ToString("mm':'ss")} minutes";
                    await Task.Delay(1000);

                    if (!IsUploaderEnabled)
                        break;
                }

                IsHostUnderMaintenance = false;
                UploadFileStatus = string.Empty;

                return true;
            }
            else
            {
                return false;
            }
        }

        private async Task<bool> IdleMode()
        {
            if (ReplayUploadQueue.Count < 1 || RequestUploaderToStop)
            {
                if (IsParsingReplaysOn)
                {
                    ButtonsEnabled = false;
                    StartButtonEnabled = true;
                }
                else
                {
                    ButtonsEnabled = true;
                }

                StartButtonText = StartButtonState.START;
                IsIdleMode = true;
                UploaderStatus = "Idle";

                await Task.Delay(1000);
                return true;
            }
            else
            {
                IsIdleMode = false;
                return false;
            }
        }

        private void LatestDateTimeSet()
        {
            ReplaysLatestUploaded = ReplaysLatestUploaded;
        }

        private void LastDateTimeSet()
        {
            ReplaysLastUploaded = ReplaysLastUploaded;
        }

        private void StartButton()
        {
            if (StartButtonText == StartButtonState.STOP)
            {
                StartButtonEnabled = false;
                RequestStop();
            }
            else if (StartButtonText == StartButtonState.START)
            {
                RequestStart();
            }
        }
    }
}
