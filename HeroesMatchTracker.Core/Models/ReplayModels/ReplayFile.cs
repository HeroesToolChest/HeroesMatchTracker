using GalaSoft.MvvmLight;
using HeroesMatchTracker.Core.Models.ReplayModels.Uploaders;
using HeroesMatchTracker.Data;
using System;

namespace HeroesMatchTracker.Core.Models.ReplayModels
{
    public class ReplayFile : ObservableObject
    {
        private int? _build;
        private ReplayResult? _status;
        private ReplayFileUploaderStatus? _hotsLogsUploadStatus;
        private ReplayFileUploaderStatus? _hotsApiUploadStatus;
        private ReplayFileUploaderStatus? _heroesProfileUploadStatus;

        public string FileName { get; set; }
        public DateTime LastWriteTime { get; set; }
        public string FilePath { get; set; }
        public long ReplayId { get; set; }
        public DateTime TimeStamp { get; set; }
        public int? Build
        {
            get { return _build; }
            set
            {
                _build = value;
                RaisePropertyChanged();
            }
        }

        public ReplayResult? Status
        {
            get { return _status; }
            set
            {
                _status = value;
                RaisePropertyChanged();
            }
        }

        public ReplayFileUploaderStatus? HotsLogsUploadStatus
        {
            get { return _hotsLogsUploadStatus; }
            set
            {
                _hotsLogsUploadStatus = value;
                RaisePropertyChanged();
            }
        }

        public ReplayFileUploaderStatus? HotsApiUploadStatus
        {
            get => _hotsApiUploadStatus;
            set
            {
                _hotsApiUploadStatus = value;
                RaisePropertyChanged();
            }
        }

        public ReplayFileUploaderStatus? HeroesProfileUploadStatus
        {
            get => _heroesProfileUploadStatus;
            set
            {
                _heroesProfileUploadStatus = value;
                RaisePropertyChanged();
            }
        }
    }
}
