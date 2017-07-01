using Amazon.S3;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using Heroes.Icons;
using Heroes.ReplayParser;
using HeroesMatchTracker.Core.HotsLogs;
using HeroesMatchTracker.Core.Models.ReplayModels;
using HeroesMatchTracker.Core.Services;
using HeroesMatchTracker.Core.ViewServices;
using HeroesMatchTracker.Data;
using HeroesMatchTracker.Data.Models.Replays;
using HeroesMatchTracker.Data.Models.Settings;
using HeroesMatchTracker.Data.Queries.Replays;
using Microsoft.Practices.ServiceLocation;
using Microsoft.Win32;
using Microsoft.WindowsAPICodePack.Dialogs;
using NLog;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using static Heroes.ReplayParser.DataParser;

namespace HeroesMatchTracker.Core.ViewModels.Replays
{
    public class ReplaysControlViewModel : HmtViewModel, IDisposable
    {
        private bool _areProcessButtonsEnabled;
        private bool _areHotsLogsUploaderButtonsEnabled;
        private bool _isParsingReplaysOn;
        private bool _isHotsLogsStartButtonEnabled;
        private int _totalReplaysGrid;
        private int _totalParsedGrid;
        private int _totalFailedReplays;
        private long _totalSavedInDatabase;
        private string _currentStatus;
        private string _hotsLogsStartButtonText;
        private string _hotsLogsUploaderStatus;
        private string _hotsLogsUploaderUploadStatus;

        private FileSystemWatcher FileWatcher;
        private IMainTabService MainTab;

        private Dictionary<string, int> ReplayFileLocations = new Dictionary<string, int>();
        private bool[] ScanDateTimeCheckboxes = new bool[4] { false, false, false, false };
        private bool IsSaveDataQueueSaving = false;
        private bool IsHotsLogsQueueUploading = false;
        private bool IsHotsLogsUploaderQueueOn = false;
        private bool IsHotsLogsMaintenance = false;

        private Queue<Tuple<Replay, ReplayFile>> ReplayDataQueue = new Queue<Tuple<Replay, ReplayFile>>();
        private Queue<ReplayFile> ReplayHotsLogsUploadQueue = new Queue<ReplayFile>();

        private ObservableCollection<ReplayFile> _replayFileCollection = new ObservableCollection<ReplayFile>();

        /// <summary>
        /// Constructor
        /// </summary>
        public ReplaysControlViewModel(IInternalService internalService, IMainTabService mainTab)
            : base(internalService)
        {
            MainTab = mainTab;
            HotsLogsStartButtonText = "[Stop]";

            ScanDateTimeCheckboxes[Database.SettingsDb().UserSettings.SelectedScanDateTimeIndex] = true;
            AreProcessButtonsEnabled = true;
            IsParsingReplaysOn = false;

            IsReplayWatch = Database.SettingsDb().UserSettings.ReplayWatchCheckBox;
            IsHotsLogsUploaderEnabled = Database.SettingsDb().UserSettings.IsHotsLogsUploaderEnabled;
            TotalSavedInDatabase = Database.ReplaysDb().MatchReplay.GetTotalReplayCount();
            TotalFailedReplays = Database.SettingsDb().FailedReplays.GetTotalReplaysCount();

            Messenger.Default.Register<List<FailedReplay>>(this, (replays) => ReceiveFailedReplays(replays));

            InitializeReplaySaveDataQueue();
        }

        public RelayCommand ScanCommand => new RelayCommand(Scan);
        public RelayCommand StartCommand => new RelayCommand(Start);
        public RelayCommand StopCommand => new RelayCommand(Stop);
        public RelayCommand ManualSelectFilesCommand => new RelayCommand(ManualSelectFiles);
        public RelayCommand ReplaysLocationBrowseCommand => new RelayCommand(ReplaysLocationBrowse);
        public RelayCommand LatestDateTimeDefaultCommand => new RelayCommand(LatestDateTimeDefault);
        public RelayCommand LatestDateTimeSetCommand => new RelayCommand(LatestDateTimeSet);
        public RelayCommand LastDateTimeDefaultCommandCommand => new RelayCommand(LastDateTimeDefault);
        public RelayCommand LastDateTimeSetCommand => new RelayCommand(LastDateTimeSet);
        public RelayCommand LatestHotsLogsDateTimeDefaultCommand => new RelayCommand(LatestHotsLogsDateTimeDefault);
        public RelayCommand LatestHotsLogsDateTimeSetCommand => new RelayCommand(LatestHotsLogsDateTimeSet);
        public RelayCommand LastHotsLogsDateTimeDefaultCommand => new RelayCommand(LastHotsLogsDateTimeDefault);
        public RelayCommand LastHotsLogsDateTimeSetCommand => new RelayCommand(LastHotsLogsDateTimeSet);
        public RelayCommand HotsLogsStartButtonCommand => new RelayCommand(HotsLogsStartButton);
        public RelayCommand ViewFailedReplaysCommand => new RelayCommand(ViewFailedReplays);

        #region public properties
        public IDatabaseService GetDatabaseService => Database;
        public ICreateWindowService CreateWindow => ServiceLocator.Current.GetInstance<ICreateWindowService>();

        public bool AreProcessButtonsEnabled
        {
            get => _areProcessButtonsEnabled;
            set
            {
                _areProcessButtonsEnabled = value;
                RaisePropertyChanged();
            }
        }

        public bool AreHotsLogsUploaderButtonsEnabled
        {
            get => _areHotsLogsUploaderButtonsEnabled;
            set
            {
                _areHotsLogsUploaderButtonsEnabled = value;
                RaisePropertyChanged();
            }
        }

        public string CurrentStatus
        {
            get => _currentStatus;
            set
            {
                _currentStatus = value;
                RaisePropertyChanged();
            }
        }

        public string HotsLogsUploaderStatus
        {
            get => _hotsLogsUploaderStatus;
            set
            {
                _hotsLogsUploaderStatus = value;
                RaisePropertyChanged();
            }
        }

        public string HotsLogsUploaderUploadStatus
        {
            get => _hotsLogsUploaderUploadStatus;
            set
            {
                _hotsLogsUploaderUploadStatus = value;
                RaisePropertyChanged();
            }
        }

        public long TotalSavedInDatabase
        {
            get => _totalSavedInDatabase;
            set
            {
                _totalSavedInDatabase = value;
                MainTab.SetTotalParsedReplays(value);
                RaisePropertyChanged();
            }
        }

        public int TotalReplaysGrid
        {
            get => _totalReplaysGrid;
            set
            {
                _totalReplaysGrid = value;
                RaisePropertyChanged();
            }
        }

        public int TotalParsedGrid
        {
            get => _totalParsedGrid;
            set
            {
                _totalParsedGrid = value;
                RaisePropertyChanged();
            }
        }

        public bool IsReplayWatch
        {
            get => Database.SettingsDb().UserSettings.ReplayWatchCheckBox;
            set
            {
                if (value)
                    MainTab.SetReplayParserWatchStatus(ReplayParserWatchStatus.Enabled);
                else
                    MainTab.SetReplayParserWatchStatus(ReplayParserWatchStatus.Disabled);

                Database.SettingsDb().UserSettings.ReplayWatchCheckBox = value;
                RaisePropertyChanged();
            }
        }

        public bool IsAutoScanStart
        {
            get => Database.SettingsDb().UserSettings.ReplayAutoScanCheckBox;
            set
            {
                Database.SettingsDb().UserSettings.ReplayAutoScanCheckBox = value;
                RaisePropertyChanged();
            }
        }

        public bool IsAutoStartStartup
        {
            get => Database.SettingsDb().UserSettings.ReplayAutoStartStartUp;
            set
            {
                Database.SettingsDb().UserSettings.ReplayAutoStartStartUp = value;
                RaisePropertyChanged();
            }
        }

        public bool IsIncludeSubDirectories
        {
            get => Database.SettingsDb().UserSettings.IsIncludeSubDirectories;
            set
            {
                Database.SettingsDb().UserSettings.IsIncludeSubDirectories = value;
                RaisePropertyChanged();
            }
        }

        public bool IsHotsLogsUploaderEnabled
        {
            get => Database.SettingsDb().UserSettings.IsHotsLogsUploaderEnabled;
            set
            {
                Database.SettingsDb().UserSettings.IsHotsLogsUploaderEnabled = value;
                if (value)
                {
                    MainTab.SetReplayParserHotsLogsStatus(ReplayParserHotsLogsStatus.Enabled);

                    AreHotsLogsUploaderButtonsEnabled = true;
                    IsHotsLogsStartButtonEnabled = false;
                    InitializeReplayHotsLogsUploadQueue();
                }
                else
                {
                    MainTab.SetReplayParserHotsLogsStatus(ReplayParserHotsLogsStatus.Disabled);

                    AreHotsLogsUploaderButtonsEnabled = false;
                    if (LatestHotsLogsChecked || LastHotsLogsChecked)
                        LatestParsedChecked = true;
                    LatestHotsLogsChecked = false;
                    LastHotsLogsChecked = false;
                }

                RaisePropertyChanged();
            }
        }

        public bool LatestParsedChecked
        {
            get => ScanDateTimeCheckboxes[0];
            set
            {
                ScanDateTimeCheckboxes[0] = value;
                if (value)
                {
                    Database.SettingsDb().UserSettings.SelectedScanDateTimeIndex = 0;
                    LastParsedChecked = false;
                    LatestHotsLogsChecked = false;
                    LastHotsLogsChecked = false;
                }

                RaisePropertyChanged();
            }
        }

        public bool LastParsedChecked
        {
            get => ScanDateTimeCheckboxes[1];
            set
            {
                ScanDateTimeCheckboxes[1] = value;
                if (value)
                {
                    Database.SettingsDb().UserSettings.SelectedScanDateTimeIndex = 1;
                    LatestParsedChecked = false;
                    LatestHotsLogsChecked = false;
                    LastHotsLogsChecked = false;
                }

                RaisePropertyChanged();
            }
        }

        public bool LatestHotsLogsChecked
        {
            get => ScanDateTimeCheckboxes[2];
            set
            {
                ScanDateTimeCheckboxes[2] = value;
                if (value)
                {
                    Database.SettingsDb().UserSettings.SelectedScanDateTimeIndex = 2;
                    LatestParsedChecked = false;
                    LastParsedChecked = false;
                    LastHotsLogsChecked = false;
                }

                RaisePropertyChanged();
            }
        }

        public bool IsHotsLogsStartButtonEnabled
        {
            get => _isHotsLogsStartButtonEnabled;
            set
            {
                _isHotsLogsStartButtonEnabled = value;
                RaisePropertyChanged();
            }
        }

        public bool LastHotsLogsChecked
        {
            get => ScanDateTimeCheckboxes[3];
            set
            {
                ScanDateTimeCheckboxes[3] = value;
                if (value)
                {
                    Database.SettingsDb().UserSettings.SelectedScanDateTimeIndex = 3;
                    LatestParsedChecked = false;
                    LastParsedChecked = false;
                    LatestHotsLogsChecked = false;
                }

                RaisePropertyChanged();
            }
        }

        public DateTime ReplaysLatestSaved
        {
            get => Database.SettingsDb().UserSettings.ReplaysLatestSaved;
            set
            {
                Database.SettingsDb().UserSettings.ReplaysLatestSaved = value;
                RaisePropertyChanged();
            }
        }

        public DateTime ReplaysLastSaved
        {
            get => Database.SettingsDb().UserSettings.ReplaysLastSaved;
            set
            {
                Database.SettingsDb().UserSettings.ReplaysLastSaved = value;
                RaisePropertyChanged();
            }
        }

        public DateTime ReplaysLatestHotsLogs
        {
            get => Database.SettingsDb().UserSettings.ReplaysLatestHotsLogs;
            set
            {
                Database.SettingsDb().UserSettings.ReplaysLatestHotsLogs = value;
                RaisePropertyChanged();
            }
        }

        public DateTime ReplaysLastHotsLogs
        {
            get => Database.SettingsDb().UserSettings.ReplaysLastHotsLogs;
            set
            {
                Database.SettingsDb().UserSettings.ReplaysLastHotsLogs = value;
                RaisePropertyChanged();
            }
        }

        public string ReplaysFolderLocation
        {
            get => Database.SettingsDb().UserSettings.ReplaysLocation;
            set
            {
                Database.SettingsDb().UserSettings.ReplaysLocation = value;
                RaisePropertyChanged();
            }
        }

        public bool IsParsingReplaysOn
        {
            get => _isParsingReplaysOn;
            set
            {
                if (value)
                    MainTab.SetReplayParserStatus(ReplayParserStatus.Parsing);
                else
                    MainTab.SetReplayParserStatus(ReplayParserStatus.Stopped);

                _isParsingReplaysOn = value;

                RaisePropertyChanged();
            }
        }

        public string HotsLogsStartButtonText
        {
            get => _hotsLogsStartButtonText;
            set
            {
                _hotsLogsStartButtonText = value;
                RaisePropertyChanged();
            }
        }

        public ObservableCollection<ReplayFile> ReplayFileCollection
        {
            get => _replayFileCollection;
            set
            {
                _replayFileCollection = value;
                RaisePropertyChanged();
            }
        }

        public int TotalFailedReplays
        {
            get => _totalFailedReplays;
            set
            {
                _totalFailedReplays = value;
                RaisePropertyChanged();
            }
        }
        #endregion public properties

        private void ManualSelectFiles()
        {
            var dialog = new OpenFileDialog()
            {
                InitialDirectory = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), @"Heroes of the Storm"),
                DefaultExt = $".{Properties.Settings.Default.HeroesReplayFileType}",
                Filter = $"Heroes Replay Files (*.{Properties.Settings.Default.HeroesReplayFileType})|*.{Properties.Settings.Default.HeroesReplayFileType}|All Files (*.*)|*.*",
                Multiselect = true,
            };

            bool? result = dialog.ShowDialog();
            if (result == true)
            {
                ReplayFileCollection.Clear();
                ReplayFileLocations.Clear();
                IsAutoScanStart = false;

                CurrentStatus = "Retrieving selected replay file(s)...";
                var files = dialog.FileNames;

                foreach (var file in files)
                {
                    var replayFile = new FileInfo(file);
                    if (replayFile.Extension == $".{Properties.Settings.Default.HeroesReplayFileType}")
                    {
                        ReplayFileCollection.Add(new ReplayFile
                        {
                            FileName = replayFile.Name,
                            LastWriteTime = replayFile.LastWriteTime,
                            FilePath = replayFile.FullName,
                            Status = null,
                        });
                        ReplayFileLocations.Add(replayFile.FullName, ReplayFileCollection.Count - 1);
                    }
                }

                TotalSavedInDatabase = GetTotalReplayDbCount();
                TotalReplaysGrid = ReplayFileCollection.Count;
                TotalParsedGrid = 0;
                CurrentStatus = $"{ReplayFileCollection.Count} replay file(s) retrieved";
            }
        }

        private void ReplaysLocationBrowse()
        {
            var dialog = new CommonOpenFileDialog()
            {
                IsFolderPicker = true,
                InitialDirectory = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), @"Heroes of the Storm"),
            };

            CommonFileDialogResult result = dialog.ShowDialog();

            if (result == CommonFileDialogResult.Ok)
            {
                ReplaysFolderLocation = dialog.FileName;
            }
        }

        #region date/time options
        private void LatestDateTimeDefault()
        {
            ReplaysLatestSaved = Database.ReplaysDb().MatchReplay.ReadLatestReplayByDateTime();
        }

        private void LatestDateTimeSet()
        {
            ReplaysLatestSaved = ReplaysLatestSaved;
        }

        private void LastDateTimeDefault()
        {
            ReplaysLastSaved = Database.ReplaysDb().MatchReplay.ReadLastReplayByDateTime();
        }

        private void LastDateTimeSet()
        {
            ReplaysLastSaved = ReplaysLastSaved;
        }

        private void LatestHotsLogsDateTimeDefault()
        {
            ReplaysLatestHotsLogs = Database.ReplaysDb().MatchReplay.ReadLatestReplayByDateTime();
        }

        private void LatestHotsLogsDateTimeSet()
        {
            ReplaysLatestHotsLogs = ReplaysLatestHotsLogs;
        }

        private void LastHotsLogsDateTimeDefault()
        {
            ReplaysLastHotsLogs = Database.ReplaysDb().MatchReplay.ReadLastReplayByDateTime();
        }

        private void LastHotsLogsDateTimeSet()
        {
            ReplaysLastHotsLogs = ReplaysLastHotsLogs;
        }
        #endregion date/time options

        #region start processing/init
        private void Scan()
        {
            AreProcessButtonsEnabled = false;
            ReplayHotsLogsUploadQueue.Clear();

            Task.Run(async () =>
            {
                await LoadAccountDirectory();
                AreProcessButtonsEnabled = true;
            });
        }

        private void Start()
        {
            IsParsingReplaysOn = true;
            if (IsHotsLogsUploaderEnabled)
            {
                IsHotsLogsStartButtonEnabled = true;
                ReplayHotsLogsUploadQueue.Clear();
            }

            IsHotsLogsUploaderQueueOn = true;
            AreProcessButtonsEnabled = false;

            if (IsReplayWatch)
                InitializeReplayWatcher();

            ExecuteProcessing();
        }

        private void Stop()
        {
            IsParsingReplaysOn = false;
            if (FileWatcher != null && IsAutoScanStart)
            {
                FileWatcher.EnableRaisingEvents = false;
                FileWatcher = null;
            }

            if (!string.IsNullOrEmpty(CurrentStatus))
                CurrentStatus += " (Stopping, awaiting completion of current task)";
        }

        private void ExecuteProcessing()
        {
            Task.Run(async () =>
            {
                if (IsAutoScanStart)
                    await LoadAccountDirectory();

                await ParseReplays();
            });
        }

        #endregion start processing/init

        #region file watcher
        private void InitializeReplayWatcher()
        {
            FileWatcher = new FileSystemWatcher()
            {
                Path = Database.SettingsDb().UserSettings.ReplaysLocation,
                IncludeSubdirectories = true,
                NotifyFilter = NotifyFilters.LastWrite | NotifyFilters.FileName | NotifyFilters.Attributes,
                Filter = $"*.{Properties.Settings.Default.HeroesReplayFileType}",
            };

            FileWatcher.Changed += new FileSystemEventHandler(OnChanged);
            FileWatcher.Deleted += new FileSystemEventHandler(OnDeleted);

            FileWatcher.EnableRaisingEvents = true;
        }

        private void OnChanged(object sender, FileSystemEventArgs e)
        {
            var filePath = e.FullPath;

            Application.Current.Dispatcher.Invoke(() =>
            {
                if (ReplayFileCollection.FirstOrDefault(x => x.FilePath == filePath) == null)
                {
                    ReplayFileCollection.Add(new ReplayFile
                    {
                        FileName = Path.GetFileName(filePath),
                        LastWriteTime = File.GetLastWriteTime(filePath),
                        FilePath = filePath,
                        Status = null,
                    });
                    ReplayFileLocations.Add(filePath, ReplayFileCollection.Count - 1);
                }
            });

            TotalReplaysGrid = ReplayFileCollection.Count;
        }

        private void OnDeleted(object sender, FileSystemEventArgs e)
        {
            var filePath = e.FullPath;

            Application.Current.Dispatcher.Invoke(() =>
            {
                var file = ReplayFileCollection.FirstOrDefault(x => x.FilePath == filePath);
                if (file != null)
                {
                    ReplayFileCollection.Remove(file);
                    ReplayFileLocations.Remove(filePath);
                }
            });

            TotalReplaysGrid = ReplayFileCollection.Count;
        }
        #endregion file watcher

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

            DateTime dateTime;
            int selectedScanDateTime = Database.SettingsDb().UserSettings.SelectedScanDateTimeIndex;

            if (selectedScanDateTime == 1)
                dateTime = ReplaysLastSaved;
            else if (selectedScanDateTime == 2)
                dateTime = ReplaysLatestHotsLogs;
            else if (selectedScanDateTime == 3)
                dateTime = ReplaysLastHotsLogs;
            else // default
                dateTime = ReplaysLatestSaved;

            List<FileInfo> listFiles = new DirectoryInfo(Database.SettingsDb().UserSettings.ReplaysLocation)
                .GetFiles($"*.{Properties.Settings.Default.HeroesReplayFileType}", searchOption)
                .OrderBy(x => x.LastWriteTime)
                .Where(x => x.LastWriteTime > dateTime)
                .ToList();

            TotalReplaysGrid = listFiles.Count;

            await Application.Current.Dispatcher.InvokeAsync(() =>
            {
                ReplayFileCollection = new ObservableCollection<ReplayFile>();
                ReplayFileLocations = new Dictionary<string, int>();
                TotalParsedGrid = 0;

                int index = 0;
                foreach (var file in listFiles)
                {
                    ReplayFileCollection.Add(new ReplayFile
                    {
                        FileName = file.Name,
                        LastWriteTime = file.LastWriteTime,
                        FilePath = file.FullName,
                        Status = null,
                    });

                    ReplayFileLocations.Add(file.FullName, index);
                    index++;
                }

                // add failed replays
                if (Database.SettingsDb().UserSettings.RequeueAllFailedReplays && Database.SettingsDb().UserSettings.IsAutoRequeueOnUpdate)
                {
                    var failedReplaysList = Database.SettingsDb().FailedReplays.ReadAllReplays();
                    Database.SettingsDb().FailedReplays.DeleteAllFailedReplays();

                    TotalFailedReplays = Database.SettingsDb().FailedReplays.GetTotalReplaysCount();

                    ReceiveFailedReplays(failedReplaysList);

                    Database.SettingsDb().UserSettings.RequeueAllFailedReplays = false;
                }

                TotalSavedInDatabase = GetTotalReplayDbCount();
            });

            CurrentStatus = "Scan completed";
        }

        private long GetTotalReplayDbCount()
        {
            return Database.ReplaysDb().MatchReplay.GetTotalReplayCount();
        }

        private void HotsLogsStartButton()
        {
            if (IsParsingReplaysOn) // still parsing replays
            {
                if (IsHotsLogsUploaderQueueOn)
                {
                    HotsLogsUploaderStatus = "Uploader stopped";
                    IsHotsLogsUploaderQueueOn = false;
                    HotsLogsStartButtonText = "START";
                }
                else
                {
                    HotsLogsUploaderStatus = "Uploading";
                    IsHotsLogsUploaderQueueOn = true;
                    HotsLogsStartButtonText = "[STOP]";
                }
            }
            else
            {
                HotsLogsUploaderStatus = "Uploader stopped";
                IsHotsLogsUploaderQueueOn = false;
                HotsLogsStartButtonText = "START";
            }
        }

        /// <summary>
        /// Parse all the replay files in ReplayFilesCollection
        /// </summary>
        /// <returns></returns>
        private async Task ParseReplays()
        {
            int currentCount = 0;

            // check if continuing parsing while all replays have been parsed
            while (IsParsingReplaysOn)
            {
                for (; currentCount < ReplayFileCollection.Count(); currentCount++)
                {
                    // check if continuing parsing while still having non-parsed replays
                    if (!IsParsingReplaysOn)
                        break;

                    string tempReplayFile = Path.GetTempFileName();
                    ReplayFile originalfile = ReplayFileCollection[currentCount];

                    CurrentStatus = $"Parsing file {originalfile.FileName}";

                    try
                    {
                        if (!File.Exists(originalfile.FilePath))
                        {
                            originalfile.Status = ReplayResult.FileNotFound;
                            UnParsedReplaysLog.Log(LogLevel.Info, $"{originalfile.FileName}: {originalfile.Status}");
                            TotalParsedGrid++;
                            CurrentStatus = $"Failed to find file {originalfile.FileName}";
                            continue;
                        }
                        else if (originalfile.Status == ReplayResult.Saved)
                        {
                            continue;
                        }

                        // copy the contents of the replay file to the tempReplayFile file
                        File.Copy(originalfile.FilePath, tempReplayFile, overwrite: true);

                        var replayParsed = ParseReplay(tempReplayFile, ignoreErrors: false, deleteFile: false);
                        originalfile.Build = replayParsed.Item2.ReplayBuild;

                        if (replayParsed.Item1 == ReplayParseResult.Success)
                        {
                            replayParsed.Item2.Units = null;
                            replayParsed.Item2.TrackerEvents = null;

                            originalfile.Status = ReplayResult.Success;

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
                            if (replayParsed.Item2.ReplayBuild > AssemblyVersions.HeroesReplayParserVersion().Version.Revision)
                                originalfile.Status = ReplayResult.NotYetSupported;
                            else
                                originalfile.Status = ReplayResult.ParserException;

                            AddToFailedReplaysQueue(originalfile);
                        }
                        else
                        {
                            originalfile.Status = (ReplayResult)Enum.Parse(typeof(ReplayResult), replayParsed.Item1.ToString());
                            AddToFailedReplaysQueue(originalfile);
                        }
                    }
                    catch (Exception ex)
                    {
                        originalfile.Status = ReplayResult.Exception;
                        ExceptionLog.Log(LogLevel.Error, ex);
                    }
                    finally
                    {
                        TotalParsedGrid++;
                        CurrentStatus = $"Parsed {originalfile.FileName}";

                        if (File.Exists(tempReplayFile))
                            File.Delete(tempReplayFile);
                    }
                } // end for

                // if no watch is selected and if all replays got parsed then automatically end
                if (!IsReplayWatch && currentCount == ReplayFileCollection.Count)
                {
                    CurrentStatus = "Processing completed";
                    IsParsingReplaysOn = false;
                    return;
                }
                else if (IsReplayWatch && currentCount == ReplayFileCollection.Count)
                {
                    CurrentStatus = "Watching for new replays...";
                    await Task.Delay(2000);
                }
            } // end while

            CurrentStatus = "Processing stopped";
        }

        private void InitializeReplaySaveDataQueue()
        {
            Task.Run(async () =>
            {
                ReplayFile currentReplayFile;
                Tuple<Replay, ReplayFile> dequeuedItem;
                ReplayFileData replayFileData = null;
                HeroesIcons heroesIcons = new HeroesIcons(true);

                while (true)
                {
                    currentReplayFile = null;
                    dequeuedItem = null;

                    if (ReplayDataQueue.Count < 1)
                    {
                        IsSaveDataQueueSaving = false;

                        if (!IsParsingReplaysOn)
                        {
                            CurrentStatus = "Processing stopped";

                            if (!IsHotsLogsQueueUploading || IsHotsLogsMaintenance)
                                AreProcessButtonsEnabled = true;
                        }
                        await Task.Delay(1500);
                        continue;
                    }
                    else if (!IsParsingReplaysOn)
                    {
                        if (ReplayDataQueue.Count > 0)
                            CurrentStatus = "Processing stopped, waiting for parsed replays to be saved to database...";
                    }

                    IsSaveDataQueueSaving = true;
                    dequeuedItem = ReplayDataQueue.Dequeue();

                    try
                    {
                        currentReplayFile = ReplayFileCollection[ReplayFileLocations[dequeuedItem.Item2.FilePath]];

                        // save parsed data to database
                        replayFileData = new ReplayFileData(dequeuedItem.Item1, heroesIcons);
                        currentReplayFile.Status = replayFileData.SaveAllData(dequeuedItem.Item2.FileName);

                        currentReplayFile.ReplayId = replayFileData.ReplayId;
                        currentReplayFile.TimeStamp = replayFileData.ReplayTimeStamp;

                        if (currentReplayFile.Status == ReplayResult.Saved)
                        {
                            TotalSavedInDatabase++;
                            ReplaysLatestSaved = Database.ReplaysDb().MatchReplay.ReadLatestReplayByDateTime();
                            ReplaysLastSaved = replayFileData.ReplayTimeStamp.ToLocalTime();
                        }

                        if (IsHotsLogsUploaderEnabled && (currentReplayFile.Status == ReplayResult.Saved || currentReplayFile.Status == ReplayResult.Duplicate))
                            ReplayHotsLogsUploadQueue.Enqueue(currentReplayFile);
                    }
                    catch (TranslationException ex)
                    {
                        currentReplayFile.Status = ReplayResult.TranslationException;
                        TranslationsLog.Log(LogLevel.Error, ex.Message);
                        ReAddReplayFileCollectionKey(dequeuedItem.Item2.FilePath, Guid.NewGuid().ToString());
                        AddToFailedReplaysQueue(currentReplayFile);
                    }
                    catch (Exception ex)
                    {
                        currentReplayFile.Status = ReplayResult.Exception;
                        ExceptionLog.Log(LogLevel.Error, ex);
                        ReAddReplayFileCollectionKey(dequeuedItem.Item2.FilePath, Guid.NewGuid().ToString());
                        AddToFailedReplaysQueue(currentReplayFile);
                    }
                    finally
                    {
                        if (replayFileData != null)
                        {
                            replayFileData.Dispose();
                        }
                    }
                }
            });
        }

        private void InitializeReplayHotsLogsUploadQueue()
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

                    if (IsHotsLogsMaintenance)
                    {
                        HotsLogsUploaderStatus = "HOTSLogs.com is currently down for maintenance";

                        // wait 30 mintues before retrying again
                        for (int i = 1800000; i >= 0; i = i - 1000)
                        {
                            HotsLogsUploaderUploadStatus = $"(HOTSLogs.com maintenance) Retrying in {TimeSpan.FromMilliseconds(i).ToString("mm':'ss")} minutes";
                            await Task.Delay(1000);

                            if (AreProcessButtonsEnabled || !IsHotsLogsUploaderQueueOn || !IsParsingReplaysOn)
                            {
                                ReplayHotsLogsUploadQueue.Clear();
                                break;
                            }
                        }

                        IsHotsLogsMaintenance = false;
                        HotsLogsUploaderUploadStatus = string.Empty;
                    }

                    if (IsParsingReplaysOn) HotsLogsUploaderStatus = "Uploading";

                    if (ReplayHotsLogsUploadQueue.Count < 1 || !IsHotsLogsUploaderQueueOn)
                    {
                        IsHotsLogsQueueUploading = false;

                        if (!IsParsingReplaysOn)
                        {
                            if (!IsSaveDataQueueSaving)
                            {
                                HotsLogsStartButtonText = "STOP";
                                AreProcessButtonsEnabled = true;
                                IsHotsLogsStartButtonEnabled = false;
                            }
                        }

                        HotsLogsUploaderStatus = "Idle";
                        await Task.Delay(1500);
                        continue;
                    }
                    else if (!IsParsingReplaysOn)
                    {
                        if (ReplayHotsLogsUploadQueue.Count > 0)
                            HotsLogsUploaderStatus = "Processing stopped, waiting for parsed replays to be uploaded...";
                    }

                    IsHotsLogsQueueUploading = true;
                    onQueuedReplayFile = ReplayHotsLogsUploadQueue.Peek(); // just grab the replay, will remove it later

                    try
                    {
                        currentReplayFile = ReplayFileCollection[ReplayFileLocations[onQueuedReplayFile.FilePath]];
                        HotsLogsUploaderUploadStatus = $"Uploading {currentReplayFile.FileName}";
                        currentReplayFile.ReplayFileHotsLogsStatus = ReplayFileHotsLogsStatus.Uploading;

                        // check if file exists
                        if (!File.Exists(currentReplayFile.FilePath))
                        {
                            currentReplayFile.ReplayFileHotsLogsStatus = ReplayFileHotsLogsStatus.FileNotFound;
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

                        ReplayHotsLogsUpload replayHotsLogsUpload = new ReplayHotsLogsUpload
                        {
                            ReplayId = currentReplayFile.ReplayId,
                            Status = (int)ReplayFileHotsLogsStatus.Uploading,
                        };

                        // check if an upload record exists for the replay
                        if (Database.ReplaysDb().HotsLogsUpload.IsExistingRecord(replayHotsLogsUpload))
                        {
                            var existingStatus = Database.ReplaysDb().HotsLogsUpload.ReadUploadStatus(replayHotsLogsUpload);
                            if (existingStatus == (int)ReplayFileHotsLogsStatus.Success || existingStatus == (int)ReplayFileHotsLogsStatus.Duplicate)
                            {
                                // already added, so its a duplicate
                                currentReplayFile.ReplayFileHotsLogsStatus = ReplayFileHotsLogsStatus.Duplicate;

                                // remove it before we continue
                                ReplayHotsLogsUploadQueue.Dequeue();
                                continue;
                            }
                        }
                        else
                        {
                            Database.ReplaysDb().HotsLogsUpload.CreateRecord(replayHotsLogsUpload);
                        }

                        // upload it to the amazon bucket
                        // this will throw MaintenanceException if there is ongoing maintenance
                        var status = await HotsLogsUploader.UploadReplay(currentReplayFile.FilePath);

                        if (status == ReplayParseResult.Success || status == ReplayParseResult.Duplicate)
                        {
                            replayHotsLogsUpload.ReplayFileTimeStamp = currentReplayFile.TimeStamp; // the date/time of the replay itself

                            if (status == ReplayParseResult.Success)
                            {
                                replayHotsLogsUpload.Status = (int)ReplayFileHotsLogsStatus.Success;
                                currentReplayFile.ReplayFileHotsLogsStatus = ReplayFileHotsLogsStatus.Success;
                            }
                            else if (status == ReplayParseResult.Duplicate)
                            {
                                replayHotsLogsUpload.Status = (int)ReplayFileHotsLogsStatus.Duplicate;
                                currentReplayFile.ReplayFileHotsLogsStatus = ReplayFileHotsLogsStatus.Duplicate;
                            }

                            Database.ReplaysDb().HotsLogsUpload.UpdateHotsLogsUploadedDateTime(replayHotsLogsUpload);

                            ReplaysLatestHotsLogs = Database.ReplaysDb().HotsLogsUpload.ReadLatestReplayHotsLogsUploadedByDateTime();
                            ReplaysLastHotsLogs = replayHotsLogsUpload.ReplayFileTimeStamp.Value.ToLocalTime();
                        }
                        else
                        {
                            currentReplayFile.ReplayFileHotsLogsStatus = ReplayFileHotsLogsStatus.Failed;
                        }

                        ReplayHotsLogsUploadQueue.Dequeue(); // we're done with the replay so remove it from the queue
                    }
                    catch (MaintenanceException)
                    {
                        currentReplayFile.ReplayFileHotsLogsStatus = ReplayFileHotsLogsStatus.Maintenance;
                        IsHotsLogsMaintenance = true;
                    }
                    catch (AmazonS3Exception ex) // note: the replay is still in front of the queue
                    {
                        currentReplayFile.ReplayFileHotsLogsStatus = ReplayFileHotsLogsStatus.UploadError;
                        HotsLogsLog.Log(LogLevel.Error, ex);
                        await Task.Delay(5000);
                    }
                    catch (Exception ex) // note: the replay is still in front of the queue
                    {
                        currentReplayFile.ReplayFileHotsLogsStatus = ReplayFileHotsLogsStatus.UploadError;
                        ExceptionLog.Log(LogLevel.Error, ex);
                        await Task.Delay(5000);
                    }
                }

                HotsLogsUploaderStatus = "Off";
            });
        }

        private void ViewFailedReplays()
        {
            CreateWindow.ShowFailedReplaysWindow();
        }

        private void AddToFailedReplaysQueue(ReplayFile replayFile)
        {
            if (replayFile.Status.HasValue && (replayFile.Status.Value == ReplayResult.ComputerPlayerFound ||
                replayFile.Status.Value == ReplayResult.PreAlphaWipe ||
                replayFile.Status.Value == ReplayResult.PTRRegion ||
                replayFile.Status.Value == ReplayResult.TryMeMode))
                return;

            FailedReplay replay = new FailedReplay()
            {
                Build = replayFile.Build ?? 0,
                FilePath = replayFile.FilePath,
                TimeStamp = replayFile.LastWriteTime,
                Status = replayFile.Status.ToString(),
            };

            if (!Database.SettingsDb().FailedReplays.IsExistingReplay(replay))
            {
                Database.SettingsDb().FailedReplays.CreateFailedReplay(replay);
                TotalFailedReplays = Database.SettingsDb().FailedReplays.GetTotalReplaysCount();
            }
        }

        private void ReceiveFailedReplays(List<FailedReplay> failedReplays)
        {
            TotalFailedReplays = Database.SettingsDb().FailedReplays.GetTotalReplaysCount();

            if (failedReplays != null)
            {
                int index = ReplayFileLocations.Count;

                foreach (var failedReplay in failedReplays)
                {
                    var replay = new ReplayFile()
                    {
                        FileName = Path.GetFileName(failedReplay.FilePath),
                        Build = failedReplay.Build,
                        FilePath = failedReplay.FilePath,
                        LastWriteTime = failedReplay.TimeStamp,
                    };

                    if (!ReplayFileLocations.ContainsKey(failedReplay.FilePath))
                    {
                        ReplayFileCollection.Add(replay);
                        ReplayFileLocations.Add(failedReplay.FilePath, index);
                        index++;
                    }
                }
            }
        }

        private void ReAddReplayFileCollectionKey(string oldKey, string newKey)
        {
            var value = ReplayFileLocations[oldKey];
            ReplayFileLocations.Remove(oldKey);
            ReplayFileLocations.Add(newKey, value);
        }

        #region IDisposable Support
#pragma warning disable SA1201 // Elements must appear in the correct order
        private bool disposedValue = false;
#pragma warning restore SA1201 // Elements must appear in the correct order

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    ((IDisposable)FileWatcher).Dispose();
                }

                FileWatcher = null;
                disposedValue = true;
            }
        }

#pragma warning disable SA1202 // Elements must be ordered by access
        public void Dispose()
#pragma warning restore SA1202 // Elements must be ordered by access
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        #endregion
    }
}
