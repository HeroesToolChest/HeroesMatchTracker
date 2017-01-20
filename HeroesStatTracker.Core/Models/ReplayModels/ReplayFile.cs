using GalaSoft.MvvmLight;
using HeroesStatTracker.Core.HotsLogs;
using HeroesStatTracker.Data;
using System;

namespace HeroesStatTracker.Core.Models.ReplayModels
{
    public class ReplayFile : ObservableObject
    {
        private int? _build;
        private ReplayResult? _status;
        private ReplayFileHotsLogsStatus? _replayFileHotsLogsStatus;

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

        public ReplayFileHotsLogsStatus? ReplayFileHotsLogsStatus
        {
            get { return _replayFileHotsLogsStatus; }
            set
            {
                _replayFileHotsLogsStatus = value;
                RaisePropertyChanged();
            }
        }
    }
}
