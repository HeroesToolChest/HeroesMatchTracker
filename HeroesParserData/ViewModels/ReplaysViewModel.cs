using Amazon.S3;
using Heroes.ReplayParser;
using HeroesIcons;
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
        private bool _areHotsLogsUploaderButtonsEnabled;
        private bool _isHotsLogsStopButtonEnabled;
        private int _totalParsedGrid;
        private int _totalReplaysGrid;
        private long _totalSavedInDatabase;

        private ObservableCollection<ReplayFile> _replayFiles = new ObservableCollection<ReplayFile>();

        private bool IsSaveDataQueueSaving = false;
        private bool IsHotsLogsQueueUploading = false;
        private bool IsHotsLogsUploaderQueueOn = false;
        private bool HotsLogsMaintenance = false;

        private bool[] ScanDateTimeCheckboxes = new bool[4] { false, false, false, false };
        private Dictionary<string, int> ReplayFileLocations = new Dictionary<string, int>();
        private FileSystemWatcher FileWatcher;

        private Queue<Tuple<Replay, ReplayFile>> ReplayDataQueue = new Queue<Tuple<Replay, ReplayFile>>();
        private Queue<ReplayFile> ReplayHotsLogsUploadQueue = new Queue<ReplayFile>();

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
                if (value && IsHotsLogsUploaderEnabled)
                    AreHotsLogsUploaderButtonsEnabled = true;
                else if (!value)
                    AreHotsLogsUploaderButtonsEnabled = false;
                RaisePropertyChangedEvent(nameof(AreProcessButtonsEnabled));
            }
        }

        public bool IsHotsLogsStopButtonEnabled
        {
            get
            {
                return _isHotsLogsStopButtonEnabled;
            }
            set
            {
                _isHotsLogsStopButtonEnabled = value;
                RaisePropertyChangedEvent(nameof(IsHotsLogsStopButtonEnabled));
            }
        }

        public bool AreHotsLogsUploaderButtonsEnabled
        {
            get { return _areHotsLogsUploaderButtonsEnabled; }
            set
            {
                _areHotsLogsUploaderButtonsEnabled = value;
                RaisePropertyChangedEvent(nameof(AreHotsLogsUploaderButtonsEnabled));
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
            get { return ScanDateTimeCheckboxes[0]; }
            set
            {
                ScanDateTimeCheckboxes[0] = value;
                if (value)
                {
                    UserSettings.Default.SelectedScanDateTimeIndex = 0;
                    LastParsedChecked = false;
                    LatestHotsLogsChecked = false;
                    LastHotsLogsChecked = false;
                }
                RaisePropertyChangedEvent(nameof(LatestParsedChecked));
            }
        }

        public bool LastParsedChecked
        {
            get { return ScanDateTimeCheckboxes[1]; }
            set
            {
                ScanDateTimeCheckboxes[1] = value;
                if (value)
                {
                    UserSettings.Default.SelectedScanDateTimeIndex = 1;
                    LatestParsedChecked = false;
                    LatestHotsLogsChecked = false;
                    LastHotsLogsChecked = false;
                }
                RaisePropertyChangedEvent(nameof(LastParsedChecked));
            }
        }

        public bool LatestHotsLogsChecked
        {
            get { return ScanDateTimeCheckboxes[2]; }
            set
            {
                ScanDateTimeCheckboxes[2] = value;
                if (value)
                {
                    UserSettings.Default.SelectedScanDateTimeIndex = 2;
                    LatestParsedChecked = false;
                    LastParsedChecked = false;
                    LastHotsLogsChecked = false;
                }
                RaisePropertyChangedEvent(nameof(LatestHotsLogsChecked));
            }
        }

        public bool LastHotsLogsChecked
        {
            get { return ScanDateTimeCheckboxes[3]; }
            set
            {
                ScanDateTimeCheckboxes[3] = value;
                if (value)
                {
                    UserSettings.Default.SelectedScanDateTimeIndex = 3;
                    LatestParsedChecked = false;
                    LastParsedChecked = false;
                    LatestHotsLogsChecked = false;
                }
                RaisePropertyChangedEvent(nameof(LastHotsLogsChecked));
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
                if (value)
                {
                    AreHotsLogsUploaderButtonsEnabled = true;
                    IsHotsLogsStopButtonEnabled = false;
                    InitReplayHotsLogsUploadQueue();
                }
                else
                {
                    AreHotsLogsUploaderButtonsEnabled = false;
                    if (LatestHotsLogsChecked || LastHotsLogsChecked)
                        LatestParsedChecked = true;
                    LatestHotsLogsChecked = false;
                    LastHotsLogsChecked = false;
                }
                RaisePropertyChangedEvent(nameof(IsHotsLogsUploaderEnabled));
            }
        }

        public DateTime ReplaysLatestHotsLogs
        {
            get { return UserSettings.Default.ReplaysLatestHotsLogs != DateTime.MinValue? UserSettings.Default.ReplaysLatestHotsLogs : Query.HotsLogsUpload.ReadLatestReplayHotsLogsUploadedByDateTime(); }
            set
            {
                UserSettings.Default.ReplaysLatestHotsLogs = value;
                RaisePropertyChangedEvent(nameof(ReplaysLatestHotsLogs));
            }
        }

        public DateTime ReplaysLastHotsLogs
        {
            get { return UserSettings.Default.ReplaysLastHotsLogs != DateTime.MinValue? UserSettings.Default.ReplaysLastHotsLogs : Query.HotsLogsUpload.ReadLastReplayHotsLogsUploadedByDateTime(); }
            set
            {
                UserSettings.Default.ReplaysLastHotsLogs = value;
                RaisePropertyChangedEvent(nameof(ReplaysLastHotsLogs));
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

        public ICommand HotsLogsStopButtonCommand
        {
            get { return new DelegateCommand(ExecuteHotsLogsStopButtonCommand); }
        }

        public ICommand DateTimeHotsLogsSetCommand
        {
            get { return new DelegateCommand(ReplaysDateTimeHotsLogsSet); }
        }

        public ICommand DateTimeHotsLogsDefaultCommand
        {
            get { return new DelegateCommand(ReplaysDateTimeHotsLogsDefault); }
        }

        public ICommand DateTimeHotsLogsClearCommand
        {
            get { return new DelegateCommand(ReplaysDateTimeHotsLogsClear); }
        }

        public ICommand LastDateTimeHotsLogsSetCommand
        {
            get { return new DelegateCommand(LastReplaysDateTimeHotsLogsSet); }
        }

        public ICommand LastDateTimeHotsLogsDefaultCommand
        {
            get { return new DelegateCommand(LastReplaysDateTimeHotsLogsDefault); }
        }

        public ICommand LastDateTimeHotsLogsClearCommand
        {
            get { return new DelegateCommand(LastReplaysDateTimeHotsLogsClear); }
        }
        #endregion

        /// <summary>
        /// Constructor
        /// </summary>
        public ReplaysViewModel()
        {
            ScanDateTimeCheckboxes[UserSettings.Default.SelectedScanDateTimeIndex] = true;

            AreProcessButtonsEnabled = true;
            HotsLogsStopButtonText = "STOP";
            InitReplaySaveDataQueue();
            InitReplayHotsLogsUploadQueue();
        }

        #region start processing/init
        private void StartScan()
        {
            AreProcessButtonsEnabled = false;
            ReplayHotsLogsUploadQueue.Clear();

            Task.Run(async () =>
            {
                await LoadAccountDirectory();
                AreProcessButtonsEnabled = true;
            });           
        }

        private void StartProcessing()
        {
            IsProcessSelected = true;
            if (IsHotsLogsUploaderEnabled) IsHotsLogsStopButtonEnabled = true;
            IsHotsLogsUploaderQueueOn = true;
            AreProcessButtonsEnabled = false;

            if (IsProcessWatchChecked)
                InitReplayWatcher();

            if (!IsHotsLogsUploaderEnabled)
                ReplayHotsLogsUploadQueue.Clear();

            InitProcessing(IsAutoScanChecked);
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
            ReplaysLatestSaved = Settings.Default.ClearedStartDate;
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
            ReplaysLastSaved = Settings.Default.ClearedStartDate;
        }

        // HotsLogs uploader options
        private void ReplaysDateTimeHotsLogsSet()
        {
            ReplaysLatestHotsLogs = ReplaysLatestHotsLogs;
        }

        private void ReplaysDateTimeHotsLogsDefault()
        {
            ReplaysLatestHotsLogs = Query.Replay.ReadLatestReplayByDateTime();
        }

        private void ReplaysDateTimeHotsLogsClear()
        {
            ReplaysLatestHotsLogs = Settings.Default.ClearedStartDate;
        }

        private void LastReplaysDateTimeHotsLogsSet()
        {
            ReplaysLastHotsLogs = ReplaysLastSaved;
        }

        private void LastReplaysDateTimeHotsLogsDefault()
        {
            ReplaysLastHotsLogs = Query.Replay.ReadLastReplayByDateTime();
        }

        private void LastReplaysDateTimeHotsLogsClear()
        {
            ReplaysLastHotsLogs = Settings.Default.ClearedStartDate;
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
                DateTime dateTime;
                int selectedScanDateTime = UserSettings.Default.SelectedScanDateTimeIndex;

                if (selectedScanDateTime == 1)
                    dateTime = ReplaysLastSaved;
                else if (selectedScanDateTime == 2)
                    dateTime = ReplaysLatestHotsLogs;
                else if (selectedScanDateTime == 3)
                    dateTime = ReplaysLastHotsLogs;
                else // default
                    dateTime = ReplaysLatestSaved;

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
            HeroesInfo heroesInfo = HeroesInfo.Initialize();

            Task.Run(async () =>
            {
                long replayId;
                DateTime replayTimeStamp;
                ReplayFile currentReplayFile;
                Tuple<Replay, ReplayFile> dequeuedItem;

                while (true)
                {
                    replayId = 0;
                    currentReplayFile = null;
                    dequeuedItem = null;

                    if (ReplayDataQueue.Count < 1)
                    {
                        IsSaveDataQueueSaving = false;

                        if (!IsProcessSelected)
                        {
                            CurrentStatus = "Processing stopped";

                            if (!IsHotsLogsQueueUploading || HotsLogsMaintenance)
                                AreProcessButtonsEnabled = true;
                        }
                        await Task.Delay(1500);
                        continue;
                    }
                    else if (!IsProcessSelected)
                    {
                        if (ReplayDataQueue.Count > 0)
                            CurrentStatus = "Processing stopped, waiting for parsed replays to be saved to database...";
                    }

                    IsSaveDataQueueSaving = true;
                    dequeuedItem = ReplayDataQueue.Dequeue();

                    try
                    {
                        currentReplayFile = ReplayFiles[ReplayFileLocations[dequeuedItem.Item2.FilePath]];
                        currentReplayFile.Status = SaveAllReplayData.SaveAllData(dequeuedItem.Item1, dequeuedItem.Item2.FileName, heroesInfo, out replayTimeStamp, out replayId);

                        currentReplayFile.ReplayId = replayId;
                        currentReplayFile.TimeStamp = replayTimeStamp;

                        if (currentReplayFile.Status == ReplayParseResult.Saved)
                        {
                            TotalSavedInDatabase++;
                            ReplaysLatestSaved = Query.Replay.ReadLatestReplayByDateTime();
                            ReplaysLastSaved = replayTimeStamp.ToLocalTime();
                        }

                        if (IsHotsLogsUploaderEnabled && (currentReplayFile.Status == ReplayParseResult.Saved || currentReplayFile.Status == ReplayParseResult.Duplicate))
                            ReplayHotsLogsUploadQueue.Enqueue(currentReplayFile);
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
            Task.Run(async () =>
            {
                ReplayFile currentReplayFile;
                ReplayFile onQueuedReplayFile;

                while (IsHotsLogsUploaderEnabled)
                {
                    currentReplayFile = null;
                    onQueuedReplayFile = null;
                    HotsLogsUploaderUploadStatus = string.Empty;

                    if (HotsLogsMaintenance)
                    {
                        HotsLogsUploaderStatus = "HOTSLogs.com is currently down for maintenance";

                        // wait 30 mintues before retrying again
                        for (int i = 1800000; i >= 0; i = i - 1000)
                        {
                            HotsLogsUploaderUploadStatus = $"(HOTSLogs.com maintenance) Retrying in {TimeSpan.FromMilliseconds(i).ToString("mm':'ss")} minutes";
                            await Task.Delay(1000);

                            if (AreProcessButtonsEnabled || !IsHotsLogsUploaderQueueOn || !IsProcessSelected)
                            {
                                ReplayHotsLogsUploadQueue.Clear();
                                break;
                            }
                        }

                        HotsLogsMaintenance = false;
                        HotsLogsUploaderUploadStatus = string.Empty;
                    }

                    if (IsProcessSelected) HotsLogsUploaderStatus = "Uploading";

                    if (ReplayHotsLogsUploadQueue.Count < 1 || !IsHotsLogsUploaderQueueOn)
                    {
                        IsHotsLogsQueueUploading = false;

                        if (!IsProcessSelected)
                        {
                            if (!IsSaveDataQueueSaving)
                            {
                                HotsLogsStopButtonText = "STOP";
                                AreProcessButtonsEnabled = true;
                                IsHotsLogsStopButtonEnabled = false;
                            }
                        }

                        HotsLogsUploaderStatus = "Idle";
                        await Task.Delay(1500);
                        continue;
                    }
                    else if (!IsProcessSelected)
                    {
                        if (ReplayHotsLogsUploadQueue.Count > 0)
                            HotsLogsUploaderStatus = "Processing stopped, waiting for parsed replays to be uploaded...";
                    }

                    IsHotsLogsQueueUploading = true;
                    onQueuedReplayFile = ReplayHotsLogsUploadQueue.Peek(); // just grab the replay, will remove it later

                    try
                    {
                        currentReplayFile = ReplayFiles[ReplayFileLocations[onQueuedReplayFile.FilePath]];
                        HotsLogsUploaderUploadStatus = $"Uploading {currentReplayFile.FileName}";
                        currentReplayFile.HotsLogsStatus = ReplayHotsLogStatus.Uploading;

                        // check if file exists
                        if (!File.Exists(currentReplayFile.FilePath))
                        {
                            currentReplayFile.HotsLogsStatus = ReplayHotsLogStatus.FileNotFound;
                            HotsLogsLog.Log(LogLevel.Info, $"File does not exists: {currentReplayFile.FilePath}");

                            // remove it before we continue
                            ReplayHotsLogsUploadQueue.Dequeue();
                            continue;
                        }

                        if (currentReplayFile.ReplayId == 0 || currentReplayFile.TimeStamp == DateTime.Now)
                        {
                            if (currentReplayFile.ReplayId == 0) WarningLog.Log(LogLevel.Info, "HOTS Logs Queue: A ReplayId of 0 was detected");
                            if (currentReplayFile.TimeStamp == DateTime.Now) WarningLog.Log(LogLevel.Info, "HOTS Logs Queue: A TimeStamp of 1/1/0001 was detected");
                            // remove it before we continue
                            ReplayHotsLogsUploadQueue.Dequeue();
                            continue;
                        }

                        Models.DbModels.ReplayHotsLogsUpload replayHotsLogsUpload = new Models.DbModels.ReplayHotsLogsUpload
                        {
                            ReplayId = currentReplayFile.ReplayId,
                            Status = ReplayHotsLogStatus.Uploading
                        };

                        // check if an upload record exists for the replay
                        if (Query.HotsLogsUpload.IsExistingRecord(replayHotsLogsUpload))
                        {
                            var existingStatus = Query.HotsLogsUpload.ReadUploadStatus(replayHotsLogsUpload);
                            if (existingStatus == ReplayHotsLogStatus.Success || existingStatus == ReplayHotsLogStatus.Duplicate)
                            {
                                // already added, so its a duplicate
                                currentReplayFile.HotsLogsStatus = ReplayHotsLogStatus.Duplicate;

                                // remove it before we continue
                                ReplayHotsLogsUploadQueue.Dequeue(); 
                                continue;
                            }
                        }
                        else
                        {
                            Query.HotsLogsUpload.CreateRecord(replayHotsLogsUpload);
                        }

                        // upload it to the amazon bucket
                        // this will throw MaintenanceException if there is ongoing maintenance
                        var status = await HotsLogsUploader.UploadReplay(currentReplayFile.FilePath);

                        if (status == ReplayParseResult.Success || status == ReplayParseResult.Duplicate)
                        {
                            replayHotsLogsUpload.ReplayFileTimeStamp = currentReplayFile.TimeStamp; // the date/time of the replay itself

                            if (status == ReplayParseResult.Success)
                            {
                                replayHotsLogsUpload.Status = ReplayHotsLogStatus.Success;
                                currentReplayFile.HotsLogsStatus = ReplayHotsLogStatus.Success;
                            }
                            else if (status == ReplayParseResult.Duplicate)
                            {
                                replayHotsLogsUpload.Status = ReplayHotsLogStatus.Duplicate;
                                currentReplayFile.HotsLogsStatus = ReplayHotsLogStatus.Duplicate;
                            }

                            Query.HotsLogsUpload.UpdateHotsLogsUploadedDateTime(replayHotsLogsUpload);

                            ReplaysLatestHotsLogs = Query.HotsLogsUpload.ReadLatestReplayHotsLogsUploadedByDateTime();
                            ReplaysLastHotsLogs = replayHotsLogsUpload.ReplayFileTimeStamp.Value.ToLocalTime();
                        }
                        else
                            currentReplayFile.HotsLogsStatus = ReplayHotsLogStatus.Failed;

                        ReplayHotsLogsUploadQueue.Dequeue(); // we're done with the replay so remove it from the queue
                    }
                    catch (MaintenanceException)
                    {
                        currentReplayFile.HotsLogsStatus = ReplayHotsLogStatus.Maintenance;
                        HotsLogsMaintenance = true;
                    }
                    catch (AmazonS3Exception ex) // note: the replay is still in front of the queue
                    {
                        currentReplayFile.HotsLogsStatus = ReplayHotsLogStatus.UploadError;
                        HotsLogsLog.Log(LogLevel.Error, ex);
                    }
                    catch (Exception ex) // note: the replay is still in front of the queue
                    {
                        currentReplayFile.HotsLogsStatus = ReplayHotsLogStatus.UploadError;
                        ExceptionLog.Log(LogLevel.Error, ex);
                    }
                }

                HotsLogsUploaderStatus = "Off";
            });
        }

        private void ExecuteHotsLogsStopButtonCommand()
        {
            if (IsProcessSelected) // still parsing replays
            {
                if (IsHotsLogsUploaderQueueOn)
                {
                    HotsLogsUploaderStatus = "Uploader stopped";
                    IsHotsLogsUploaderQueueOn = false;
                    HotsLogsStopButtonText = "--START--";
                }
                else
                {
                    HotsLogsUploaderStatus = "Uploading";
                    IsHotsLogsUploaderQueueOn = true;
                    HotsLogsStopButtonText = "STOP";
                }
            }
            else
            {
                HotsLogsUploaderStatus = "Uploader stopped";
                IsHotsLogsUploaderQueueOn = false;
                HotsLogsStopButtonText = "--START--";
            }
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
