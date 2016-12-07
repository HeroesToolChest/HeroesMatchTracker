using Amazon.S3;
using Heroes.ReplayParser;
using HeroesParserData.DataQueries;
using HeroesParserData.HotsLogs;
using HeroesParserData.Models;
using HeroesParserData.Properties;
using Microsoft.Win32;
using Microsoft.WindowsAPICodePack.Dialogs;
using NLog;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity.Validation;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using static Heroes.ReplayParser.DataParser;

namespace HeroesParserData.ViewModels
{
    public class ReplaysViewModel : ViewModelBase, IDisposable
    {
        private string _currentStatus;
        private string _hotsLogsUploaderUploadStatus;
        private string _hotsLogsUploaderStatus;
        private string _hotsLogsStopButtonText;
        private bool _isProcessSelected;
        private bool _areProcessButtonsEnabled;
        private int _totalParsedGrid;
        private int _totalReplaysGrid;
        private long _totalSavedInDatabase;

        private ObservableCollection<ReplayFile> _replayFiles = new ObservableCollection<ReplayFile>();

        private bool IsHotsLogsUploaderQueueOn = false;

        private Queue<Tuple<Replay, ReplayFile>> ReplayDataQueue = new Queue<Tuple<Replay, ReplayFile>>();
        private Queue<ReplayFile> ReplayHotsLogsUploadQueue = new Queue<ReplayFile>();
        private Dictionary<string, int> ReplayFileLocations = new Dictionary<string, int>();
        private FileSystemWatcher FileWatcher;

        #region public properties
        public string CurrentStatus
        {
            get { return _currentStatus; }
            set
            {
                _currentStatus = value;
                RaisePropertyChangedEvent(nameof(CurrentStatus));
            }
        }

        public string HotsLogsUploaderUploadStatus
        {
            get { return _hotsLogsUploaderUploadStatus; }
            set
            {
                _hotsLogsUploaderUploadStatus = value;
                RaisePropertyChangedEvent(nameof(HotsLogsUploaderUploadStatus));
            }
        }

        public string HotsLogsUploaderStatus
        {
            get { return _hotsLogsUploaderStatus; }
            set
            {
                _hotsLogsUploaderStatus = value;
                RaisePropertyChangedEvent(nameof(HotsLogsUploaderStatus));
            }
        }

        public string HotsLogsStopButtonText
        {
            get { return _hotsLogsStopButtonText; }
            set
            {
                _hotsLogsStopButtonText = value;
                RaisePropertyChangedEvent(nameof(HotsLogsStopButtonText));
            }
        }

        public bool IsProcessSelected
        {
            get { return _isProcessSelected; }
            set
            {
                _isProcessSelected = value;
                App.IsProcessingReplays = value;
                RaisePropertyChangedEvent(nameof(IsProcessSelected));
            }
        }

        public bool IsProcessWatchChecked
        {
            get { return UserSettings.Default.ReplayWatchCheckBox; }
            set
            {
                UserSettings.Default.ReplayWatchCheckBox = value;
                RaisePropertyChangedEvent(nameof(IsProcessWatchChecked));
            }
        }

        public bool IsAutoScanChecked
        {
            get { return UserSettings.Default.ReplayAutoScanCheckBox; }
            set
            {
                UserSettings.Default.ReplayAutoScanCheckBox = value;
                RaisePropertyChangedEvent(nameof(IsAutoScanChecked));
            }
        }

        public bool AreProcessButtonsEnabled
        {
            get { return _areProcessButtonsEnabled; }
            set
            {
                _areProcessButtonsEnabled = value;
                RaisePropertyChangedEvent(nameof(AreProcessButtonsEnabled));
            }
        }

        public int TotalParsedGrid
        {
            get { return _totalParsedGrid; }
            set
            {
                _totalParsedGrid = value;
                RaisePropertyChangedEvent(nameof(TotalParsedGrid));
            }
        }

        public int TotalReplaysGrid
        {
            get { return _totalReplaysGrid; }
            set
            {
                _totalReplaysGrid = value;
                RaisePropertyChangedEvent(nameof(TotalReplaysGrid));
            }
        }

        public long TotalSavedInDatabase
        {
            get { return _totalSavedInDatabase; }
            set
            {
                _totalSavedInDatabase = value;
                RaisePropertyChangedEvent(nameof(TotalSavedInDatabase));
            }
        }

        public DateTime ReplaysLatestSaved
        {
            get { return UserSettings.Default.ReplaysLatestSaved != DateTime.MinValue? UserSettings.Default.ReplaysLatestSaved : Query.Replay.ReadLatestReplayByDateTime(); }
            set
            {
                UserSettings.Default.ReplaysLatestSaved = value;
                RaisePropertyChangedEvent(nameof(ReplaysLatestSaved));
            }
        }

        public DateTime ReplaysLastSaved
        {
            get { return UserSettings.Default.ReplaysLastSaved != DateTime.MinValue ? UserSettings.Default.ReplaysLastSaved : Query.Replay.ReadLastReplayByDateTime(); }
            set
            {
                UserSettings.Default.ReplaysLastSaved = value;
                RaisePropertyChangedEvent(nameof(ReplaysLastSaved));
            }
        }

        public ObservableCollection<ReplayFile> ReplayFiles
        {
            get { return _replayFiles; }
            set
            {
                _replayFiles = value;
                RaisePropertyChangedEvent(nameof(ReplayFiles));
            }
        }

        public string ReplaysLocation
        {
            get { return UserSettings.Default.ReplaysLocation; }
            set
            {
                UserSettings.Default.ReplaysLocation = value;
                RaisePropertyChangedEvent(nameof(ReplaysLocation));
            }
        }

        public bool LatestParsedChecked
        {
            get { return UserSettings.Default.ParsedDateTimeCheckBox; }
            set
            {
                UserSettings.Default.ParsedDateTimeCheckBox = value;
                RaisePropertyChangedEvent(nameof(LatestParsedChecked));
                RaisePropertyChangedEvent(nameof(LastParsedChecked));
            }
        }

        public bool LastParsedChecked
        {
            get { return !UserSettings.Default.ParsedDateTimeCheckBox; }
            set
            {
                UserSettings.Default.ParsedDateTimeCheckBox = !value;
                RaisePropertyChangedEvent(nameof(LastParsedChecked));
                RaisePropertyChangedEvent(nameof(LatestParsedChecked));
            }
        }

        public bool IsIncludeSubDirectories
        {
            get { return UserSettings.Default.IsIncludeSubDirectories; }
            set
            {
                UserSettings.Default.IsIncludeSubDirectories = value;
                RaisePropertyChangedEvent(nameof(IsIncludeSubDirectories));
            }
        }

        public bool IsHotsLogsUploaderEnabled
        {
            get { return UserSettings.Default.IsHotsLogsUploaderEnabled; }
            set
            {
                UserSettings.Default.IsHotsLogsUploaderEnabled = value;
                RaisePropertyChangedEvent(nameof(IsHotsLogsUploaderEnabled));
            }
        }
        #endregion

        #region Button Commands
        public ICommand Scan
        {
            get { return new DelegateCommand(StartScan); }
        }

        public ICommand Start
        {
            get { return new DelegateCommand(StartProcessing); }
        }

        public ICommand Stop
        {
            get { return new DelegateCommand(StopProcessing); }
        }

        public ICommand Browse
        {
            get { return new DelegateCommand(BrowseClick); }
        }

        public ICommand DateTimeSet
        {
            get { return new DelegateCommand(ReplaysDateTimeSet); }
        }

        public ICommand DateTimeDefault
        {
            get { return new DelegateCommand(ReplaysDateTimeDefault); }
        }

        public ICommand DateTimeClear
        {
            get { return new DelegateCommand(ReplaysDateTimeClear); }
        }

        public ICommand LastDateTimeSet
        {
            get { return new DelegateCommand(LastReplaysDateTimeSet); }
        }

        public ICommand LastDateTimeDefault
        {
            get { return new DelegateCommand(LastReplaysDateTimeDefault); }
        }

        public ICommand LastDateTimeClear
        {
            get { return new DelegateCommand(LastReplaysDateTimeClear); }
        }

        public ICommand ManualSelectFiles
        {
            get { return new DelegateCommand(RetrieveUserSelectFiles); }
        }
        #endregion

        /// <summary>
        /// Constructor
        /// </summary>
        public ReplaysViewModel()
        {
            AreProcessButtonsEnabled = true;
            LatestParsedChecked = true;
            HotsLogsStopButtonText = "Stop";
            InitReplaySaveDataQueue();
        }

        #region start processing/init
        private void StartScan()
        {
            AreProcessButtonsEnabled = false;
            Task.Run(async () =>
            {
                await LoadAccountDirectory();
                AreProcessButtonsEnabled = true;
            });           
        }

        private void StartProcessing()
        {
            IsProcessSelected = true;
            AreProcessButtonsEnabled = false;

            if (IsProcessWatchChecked)
                InitReplayWatcher();

            InitProcessing(IsAutoScanChecked);

            if (IsHotsLogsUploaderEnabled)
            {
                if (!IsHotsLogsUploaderQueueOn)
                    InitReplayHotsLogsUploadQueue();
            }
            else
                IsHotsLogsUploaderQueueOn = false;
        }

        private void StopProcessing()
        {
            IsProcessSelected = false;
            if (FileWatcher != null && IsProcessWatchChecked)
            {
                FileWatcher.EnableRaisingEvents = false;
                FileWatcher = null;
            }

            if (!string.IsNullOrEmpty(CurrentStatus))
                CurrentStatus += " (Stopping, awaiting completion of current task)";
        }

        /// <summary>
        /// Start the parsing. Runs on a separate thread.
        /// </summary>
        private void InitProcessing(bool isAutoScan)
        {
            Task.Run(async () =>
            {
                if (isAutoScan)
                    await LoadAccountDirectory();

                await ParseReplays();
            });           
        }
        #endregion start processing/init

        /// <summary>
        /// Opens up a dialog to change the location of the replays location folder.
        /// Checks all subdirectories
        /// </summary>
        private void BrowseClick()
        {
            var dialog = new CommonOpenFileDialog();
            dialog.IsFolderPicker = true;
            dialog.InitialDirectory = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), @"Heroes of the Storm");
            CommonFileDialogResult result = dialog.ShowDialog();

            if (result == CommonFileDialogResult.Ok)
            {
                ReplaysLocation = dialog.FileName;
            }
        }

        #region date/time command methods
        private void ReplaysDateTimeSet()
        {
            ReplaysLatestSaved = ReplaysLatestSaved;
        }

        private void ReplaysDateTimeDefault()
        {
            ReplaysLatestSaved = Query.Replay.ReadLatestReplayByDateTime();
        }

        private void ReplaysDateTimeClear()
        {
            ReplaysLatestSaved = new DateTime(2014, 1, 1);
        }

        private void LastReplaysDateTimeSet()
        {
            ReplaysLastSaved = ReplaysLastSaved;
        }

        private void LastReplaysDateTimeDefault()
        {
            ReplaysLastSaved = Query.Replay.ReadLastReplayByDateTime();
        }

        private void LastReplaysDateTimeClear()
        {
            ReplaysLastSaved = new DateTime(2014, 1, 1);
        }
        #endregion date/time buttons

        /// <summary>
        /// Manual retrieval of the replay files, does not parse
        /// </summary>
        private void RetrieveUserSelectFiles()
        {
            var dialog = new OpenFileDialog();
            dialog.InitialDirectory = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), @"Heroes of the Storm");
            dialog.DefaultExt = ".StormReplay";
            dialog.Filter = $"Heroes Replay Files (*.{Resources.HeroesReplayFileType})|*.{Resources.HeroesReplayFileType}|All Files (*.*)|*.*";
            dialog.Multiselect = true;

            bool? result = dialog.ShowDialog();
            if (result == true)
            {
                ReplayFiles.Clear();
                ReplayFileLocations.Clear();
                IsAutoScanChecked = false;

                CurrentStatus = "Retrieving selected replay file(s)...";
                var files = dialog.FileNames;

                foreach (var file in files)
                {
                    var replayFile = new FileInfo(file);
                    if (replayFile.Extension == $".{Resources.HeroesReplayFileType}")
                    {
                        ReplayFiles.Add(new ReplayFile
                        {
                            FileName = replayFile.Name,
                            LastWriteTime = replayFile.LastWriteTime,
                            FilePath = replayFile.FullName,
                            Status = null
                        });
                        ReplayFileLocations.Add(replayFile.FullName, ReplayFiles.Count - 1);
                    }
                }

                TotalSavedInDatabase = GetTotalReplayDbCount();
                TotalReplaysGrid = ReplayFiles.Count;
                TotalParsedGrid = 0;
                CurrentStatus = $"{ReplayFiles.Count} replay file(s) retrieved";
            }
        }

        #region File Watcher
        private void InitReplayWatcher()
        {
            FileWatcher = new FileSystemWatcher();

            FileWatcher.Path = UserSettings.Default.ReplaysLocation;
            FileWatcher.IncludeSubdirectories = true;
            FileWatcher.NotifyFilter = NotifyFilters.LastWrite | NotifyFilters.FileName | NotifyFilters.Attributes;
            FileWatcher.Filter = $"*.{Resources.HeroesReplayFileType}";

            FileWatcher.Changed += new FileSystemEventHandler(OnChanged);
            FileWatcher.Deleted += new FileSystemEventHandler(OnDeleted);

            FileWatcher.EnableRaisingEvents = true;
        }

        private void OnChanged(object sender, FileSystemEventArgs e)
        {
            var filePath = e.FullPath;

            Application.Current.Dispatcher.Invoke(delegate
            {
                if (ReplayFiles.FirstOrDefault(x => x.FilePath == filePath) == null)
                {
                    ReplayFiles.Add(new ReplayFile
                    {
                        FileName = Path.GetFileName(filePath),
                        LastWriteTime = File.GetLastWriteTime(filePath),
                        FilePath = filePath,
                        Status = null
                    });
                    ReplayFileLocations.Add(filePath, ReplayFiles.Count - 1);
                }
            });

            TotalReplaysGrid = ReplayFiles.Count;
        }

        private void OnDeleted(object sender, FileSystemEventArgs e)
        {
            var filePath = e.FullPath;

            Application.Current.Dispatcher.Invoke(delegate
            {
                var file = ReplayFiles.FirstOrDefault(x => x.FilePath == filePath);
                if (file != null)
                {
                    ReplayFiles.Remove(file);
                    ReplayFileLocations.Remove(filePath);
                }
            });

            TotalReplaysGrid = ReplayFiles.Count;
        }
        #endregion File Watcher

        private long GetTotalReplayDbCount()
        {
            return Query.Replay.GetTotalReplayCount();
        }

        /// <summary>
        /// Scan the default replays location and get all the replay files
        /// </summary>
        private async Task LoadAccountDirectory()
        {
            CurrentStatus = "Scanning replay folder(s)...";

            SearchOption searchOption;
            if (IsIncludeSubDirectories)
                searchOption = SearchOption.AllDirectories;
            else
                searchOption = SearchOption.TopDirectoryOnly;

            try
            {
                var dateTime = LatestParsedChecked == true ? ReplaysLatestSaved : ReplaysLastSaved;
                List<FileInfo> listFiles = new DirectoryInfo(UserSettings.Default.ReplaysLocation)
                    .GetFiles($"*.{Resources.HeroesReplayFileType}", searchOption)
                    .OrderBy(x => x.LastWriteTime)
                    .Where(x => x.LastWriteTime > dateTime)
                    .ToList();

                TotalReplaysGrid = listFiles.Count;

                await Application.Current.Dispatcher.InvokeAsync(delegate
                {
                    ReplayFiles = new ObservableCollection<ReplayFile>();
                    ReplayFileLocations = new Dictionary<string, int>();
                    TotalParsedGrid = 0;

                    int index = 0;
                    foreach (var file in listFiles)
                    {
                        ReplayFiles.Add(new ReplayFile
                        {
                            FileName = file.Name,
                            LastWriteTime = file.LastWriteTime,
                            FilePath = file.FullName,
                            Status = null
                        });

                        ReplayFileLocations.Add(file.FullName, index);
                        index++;
                    }

                    TotalSavedInDatabase = GetTotalReplayDbCount();
                });

                CurrentStatus = "Scan completed";
            }
            catch (Exception ex) when (ex is SqlException || ex is DbEntityValidationException)
            {
                CurrentStatus = "Database error";
                SqlExceptionReplaysLog.Log(LogLevel.Error, ex);
            }
            catch (Exception ex)
            {
                CurrentStatus = "Error scanning folder";
                ExceptionLog.Log(LogLevel.Error, ex);
            }
        }

        /// <summary>
        /// Parse all the replay files in the ReplayFiles list
        /// </summary> 
        private async Task ParseReplays()
        {
            int currentCount = 0;

            // check if continuing parsing while all replays have been parsed
            while (IsProcessSelected)
            {
                for (; currentCount < ReplayFiles.Count(); currentCount++)
                {
                    // check if continuing parsing while still having non-parsed replays
                    if (!IsProcessSelected)
                        break;

                    #region parse replay and queue data to be saved
                    string tempReplayFile = Path.GetTempFileName();
                    ReplayFile originalfile = ReplayFiles[currentCount];

                    CurrentStatus = $"Parsing file {originalfile.FileName}";

                    try
                    {
                        if (!File.Exists(originalfile.FilePath))
                        {
                            originalfile.Status = ReplayParseResult.FileNotFound;
                            FailedReplaysLog.Log(LogLevel.Info, $"{originalfile.FileName}: {originalfile.Status}");
                            TotalParsedGrid++;
                            CurrentStatus = $"Failed to find file {originalfile.FileName}";
                            continue;
                        }
                        else if (originalfile.Status == ReplayParseResult.Saved)
                            continue;

                        // copy the contents of the replay file to the tempReplayFile file
                        File.Copy(originalfile.FilePath, tempReplayFile, overwrite: true);

                        var replayParsed = ParseReplay(tempReplayFile, ignoreErrors: false, deleteFile: false);
                        originalfile.Build = replayParsed.Item2.ReplayBuild;

                        if (replayParsed.Item1 == ReplayParseResult.Success)
                        {
                            originalfile.Status = ReplayParseResult.Success;

                            // give it a chance to dequeue some replays as the replay object takes quite a bit of memory
                            if (ReplayDataQueue.Count >= 5) 
                                await Task.Delay(1500);
                            else if (ReplayDataQueue.Count >= 7)
                                await Task.Delay(2500);
                            else if (ReplayDataQueue.Count >= 9)
                                await Task.Delay(4000);

                            ReplayDataQueue.Enqueue(new Tuple<Replay, ReplayFile>(replayParsed.Item2, originalfile));

                            if (IsHotsLogsUploaderEnabled)
                                ReplayHotsLogsUploadQueue.Enqueue(originalfile);
                        }
                        else if (replayParsed.Item1 == ReplayParseResult.ParserException)
                        {
                            if (replayParsed.Item2.ReplayBuild > HPDVersion.GetHeroesReplayParserSupportedBuild())
                                originalfile.Status = ReplayParseResult.NotYetSupported;
                            else
                                originalfile.Status = ReplayParseResult.ParserException;

                            WarningLog.Log(LogLevel.Warn, $"Could not parse replay {originalfile.FilePath}: {originalfile.Status}");
                        }
                        else
                        {
                            originalfile.Status = replayParsed.Item1;
                            FailedReplaysLog.Log(LogLevel.Info, $"{originalfile.FileName}: {originalfile.Status}");
                        }
                    }
                    catch (Exception ex)
                    {
                        originalfile.Status = ReplayParseResult.Exception;
                        ExceptionLog.Log(LogLevel.Error, ex);
                        FailedReplaysLog.Log(LogLevel.Info, $"{originalfile.FileName}: {originalfile.Status}");
                    }
                    finally
                    {
                        TotalParsedGrid++;
                        CurrentStatus = $"Parsed {originalfile.FileName}";

                        if (File.Exists(tempReplayFile))
                            File.Delete(tempReplayFile);
                    }
                    #endregion parse replay and save data
                } // end for

                // if no watch is selected and if all replays got parsed then automatically end
                if (!IsProcessWatchChecked && currentCount == ReplayFiles.Count)
                {
                    CurrentStatus = "Processing completed";
                    IsProcessSelected = false;
                    return;
                }
                else if (IsProcessWatchChecked && currentCount == ReplayFiles.Count)
                {
                    CurrentStatus = "Watching for new replays...";
                    await Task.Delay(2000);
                }
            } // end while

            CurrentStatus = "Processing stopped";
        }

        private void InitReplaySaveDataQueue()
        {
            Task.Run(async () =>
            {
                DateTime parsedDateTime;
                ReplayFile currentReplayFile;
                Tuple<Replay, ReplayFile> dequeuedItem;

                while (true)
                {
                    currentReplayFile = null;
                    dequeuedItem = null;

                    if (ReplayDataQueue.Count < 1)
                    {
                        if (!IsProcessSelected)
                        {
                            CurrentStatus = "Processing stopped";

                            if (ReplayHotsLogsUploadQueue.Count < 1)
                                AreProcessButtonsEnabled = true;
                        }
                        await Task.Delay(1000);
                        continue;
                    }
                    else if (!IsProcessSelected)
                    {
                        if (ReplayDataQueue.Count > 0)
                            CurrentStatus = "Processing stopped, waiting for parsed replays to be saved to database...";
                    }

                    dequeuedItem = ReplayDataQueue.Dequeue();

                    try
                    {
                        currentReplayFile = ReplayFiles[ReplayFileLocations[dequeuedItem.Item2.FilePath]];
                        currentReplayFile.Status = SaveAllReplayData.SaveAllData(dequeuedItem.Item1, dequeuedItem.Item2.FileName, out parsedDateTime);
                        if (currentReplayFile.Status == ReplayParseResult.Saved)
                        {
                            TotalSavedInDatabase++;
                            ReplaysLatestSaved = Query.Replay.ReadLatestReplayByDateTime();
                            ReplaysLastSaved = parsedDateTime.ToLocalTime();
                        }
                    }
                    catch (Exception ex) when (ex is SqlException || ex is DbEntityValidationException)
                    {
                        currentReplayFile.Status = ReplayParseResult.SqlException;
                        SqlExceptionReplaysLog.Log(LogLevel.Error, ex);
                        FailedReplaysLog.Log(LogLevel.Info, $"{currentReplayFile.FileName}: {currentReplayFile.Status}");
                    }
                    catch (Exception ex)
                    {
                        currentReplayFile.Status = ReplayParseResult.Exception;
                        ExceptionLog.Log(LogLevel.Error, ex);
                        FailedReplaysLog.Log(LogLevel.Info, $"{currentReplayFile.FileName}: {currentReplayFile.Status}");
                    }
                }
            });
        }

        private void InitReplayHotsLogsUploadQueue()
        {
            IsHotsLogsUploaderQueueOn = true;

            Task.Run(async () =>
            {
                ReplayFile currentReplayFile;
                ReplayFile dequeuedReplayFile;

                while (IsHotsLogsUploaderQueueOn)
                {
                    currentReplayFile = null;
                    dequeuedReplayFile = null;
                    HotsLogsUploaderUploadStatus = string.Empty;

                    if (ReplayHotsLogsUploadQueue.Count < 1)
                    {
                        if (!IsProcessSelected)
                        {
                            HotsLogsUploaderStatus = "Uploader stopped";

                            if (ReplayDataQueue.Count < 1)
                                AreProcessButtonsEnabled = true;

                        }
                        await Task.Delay(1000);
                        continue;
                    }
                    else if (!IsProcessSelected)
                    {
                        if (ReplayHotsLogsUploadQueue.Count > 0)
                            HotsLogsUploaderStatus = "Processing stopped, waiting for parsed replays to be uploaded...";
                    }

                    dequeuedReplayFile = ReplayHotsLogsUploadQueue.Dequeue();

                    try
                    {
                        currentReplayFile = ReplayFiles[ReplayFileLocations[dequeuedReplayFile.FilePath]];

                        if (!File.Exists(currentReplayFile.FilePath))
                        {
                            currentReplayFile.HotsLogsStatus = ReplayHotsLogStatus.FileNotFound;
                            HotsLogsLog.Log(LogLevel.Info, $"File does not exists: {currentReplayFile.FilePath}");
                            continue;
                        }

                        HotsLogsUploaderUploadStatus = $"Uploading {currentReplayFile.FileName} ...";
                        currentReplayFile.HotsLogsStatus = ReplayHotsLogStatus.Uploading;
                        var status = await HotsLogsUploader.UploadReplay(currentReplayFile.FilePath);

                        if (status == ReplayParseResult.Success)
                            currentReplayFile.HotsLogsStatus = ReplayHotsLogStatus.Success;
                        else if (status == ReplayParseResult.Duplicate)
                            currentReplayFile.HotsLogsStatus = ReplayHotsLogStatus.Duplicate;
                        else
                            currentReplayFile.HotsLogsStatus = ReplayHotsLogStatus.Failed;
                    }
                    catch (MaintenanceException)
                    {
                        currentReplayFile.HotsLogsStatus = ReplayHotsLogStatus.Maintenance;
                        break;
                    }
                    catch (AmazonS3Exception ex)
                    {
                        currentReplayFile.HotsLogsStatus = ReplayHotsLogStatus.UploadError;
                        HotsLogsLog.Log(LogLevel.Error, ex);
                        break;
                    }
                    catch (Exception ex)
                    {
                        currentReplayFile.HotsLogsStatus = ReplayHotsLogStatus.UploadError;
                        ExceptionLog.Log(LogLevel.Error, ex);
                        break;
                    }
                }
            });
        }

        #region IDisposable Support
        private bool disposedValue = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    ((IDisposable)FileWatcher).Dispose();
                }
                disposedValue = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        #endregion
    }
}
