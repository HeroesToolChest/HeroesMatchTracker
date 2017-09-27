using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using Heroes.Icons;
using Heroes.ReplayParser;
using HeroesMatchTracker.Core.Models.ReplayModels;
using HeroesMatchTracker.Core.Models.ReplayModels.Uploaders.HotsApi;
using HeroesMatchTracker.Core.Models.ReplayModels.Uploaders.HotsLogs;
using HeroesMatchTracker.Core.Services;
using HeroesMatchTracker.Core.ViewServices;
using HeroesMatchTracker.Data;
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
        private bool _isParsingReplaysOn;
        private bool _isSaveDataQueueSaving;
        private int _totalReplaysGrid;
        private int _totalParsedGrid;
        private int _totalFailedReplays;
        private long _totalSavedInDatabase;
        private string _currentStatus;

        private FileSystemWatcher FileWatcher;
        private IMainTabService MainTab;

        private ObservableCollection<ReplayFile> _replayFileCollection = new ObservableCollection<ReplayFile>();

        private Queue<Tuple<Replay, ReplayFile>> ReplayDataQueue = new Queue<Tuple<Replay, ReplayFile>>();

        /// <summary>
        /// Constructor
        /// </summary>
        public ReplaysControlViewModel(IInternalService internalService, IMainTabService mainTab)
            : base(internalService)
        {
            MainTab = mainTab;

            ParserCheckboxes = new ParserCheckboxes(InternalService.Database);
            HotsLogsUploader = new HotsLogsUploader(InternalService, mainTab, "HotsLogs");
            HotsApiUploader = new HotsApiUploader(InternalService, mainTab, "HotsApi");

            AreParserButtonsEnabled = true;
            IsParsingReplaysOn = false;

            IsReplayWatch = Database.SettingsDb().UserSettings.ReplayWatchCheckBox;

            TotalSavedInDatabase = Database.ReplaysDb().MatchReplay.GetTotalReplayCount();
            TotalFailedReplays = Database.SettingsDb().FailedReplays.GetTotalReplaysCount();

            Messenger.Default.Register<ReplayFile>(this, (message) => ReceivedReplayFile(message));
            Messenger.Default.Register<List<FailedReplay>>(this, (replays) => ReceiveFailedReplays(replays));

            Task.Run(async () => await InitializeReplaySaveDataQueueAsync());
        }

        public RelayCommand ScanCommand => new RelayCommand(Scan);
        public RelayCommand StartCommand => new RelayCommand(Start);
        public RelayCommand StopCommand => new RelayCommand(async () => await Stop());
        public RelayCommand ManualSelectFilesCommand => new RelayCommand(ManualSelectFiles);
        public RelayCommand ReplaysLocationBrowseCommand => new RelayCommand(ReplaysLocationBrowse);
        public RelayCommand LatestDateTimeDefaultCommand => new RelayCommand(LatestDateTimeDefault);
        public RelayCommand LatestDateTimeSetCommand => new RelayCommand(LatestDateTimeSet);
        public RelayCommand LastDateTimeDefaultCommandCommand => new RelayCommand(LastDateTimeDefault);
        public RelayCommand LastDateTimeSetCommand => new RelayCommand(LastDateTimeSet);
        public RelayCommand ViewFailedReplaysCommand => new RelayCommand(ViewFailedReplays);

        #region public properties
        public ParserCheckboxes ParserCheckboxes { get; }
        public HotsLogsUploader HotsLogsUploader { get; }
        public HotsApiUploader HotsApiUploader { get; }
        public IDatabaseService GetDatabaseService => Database;
        public ICreateWindowService CreateWindow => ServiceLocator.Current.GetInstance<ICreateWindowService>();

        public Dictionary<string, int> ReplayFileLocations { get; set; } = new Dictionary<string, int>();

        public bool AreParserButtonsEnabled
        {
            get => _areProcessButtonsEnabled;
            set
            {
                _areProcessButtonsEnabled = value;

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

        public string ReplaysFolderLocation
        {
            get => Database.SettingsDb().UserSettings.ReplaysLocation;
            set
            {
                Database.SettingsDb().UserSettings.ReplaysLocation = value;
                RaisePropertyChanged();
            }
        }

        /// <summary>
        /// Indicates whether the replay parser is in use (parsing only)
        /// </summary>
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

        /// <summary>
        /// Indicates whether a replay is being saving into the database
        /// </summary>
        public bool IsSaveDataQueueSaving
        {
            get => _isSaveDataQueueSaving;
            set
            {
                _isSaveDataQueueSaving = value;
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
        #endregion date/time options

        #region start processing/init
        private void Scan()
        {
            AreParserButtonsEnabled = false;
            HotsLogsUploader.ReplayUploadQueue.Clear();
            HotsApiUploader.ReplayUploadQueue.Clear();

            Task.Run(async () =>
            {
                await LoadAccountDirectory();
                AreParserButtonsEnabled = true;
            });
        }

        private void Start()
        {
            IsParsingReplaysOn = true;

            HotsLogsUploader.IsParsingReplaysOn = true;
            HotsLogsUploader.ReplayUploadQueue.Clear();
            HotsLogsUploader.RequestStart();

            HotsApiUploader.IsParsingReplaysOn = true;
            HotsApiUploader.ReplayUploadQueue.Clear();
            HotsApiUploader.RequestStart();

            AreParserButtonsEnabled = false;

            if (IsReplayWatch)
                InitializeReplayWatcher();

            ExecuteProcessing();
        }

        private async Task Stop()
        {
            IsParsingReplaysOn = false;
            HotsLogsUploader.IsParsingReplaysOn = false;
            HotsLogsUploader.RequestStop();
            HotsApiUploader.IsParsingReplaysOn = false;
            HotsApiUploader.RequestStop();

            if (FileWatcher != null && IsAutoScanStart)
            {
                FileWatcher.EnableRaisingEvents = false;
                FileWatcher = null;
            }

            if (!string.IsNullOrEmpty(CurrentStatus))
                CurrentStatus += " (Stopping, awaiting completion of current task)";

            await WaitForUploaders();
            AreParserButtonsEnabled = true;
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

            switch (Database.SettingsDb().UserSettings.SelectedScanDateTimeIndex)
            {
                case ParserCheckboxes.LatestParsedIndex:
                    dateTime = ReplaysLatestSaved;
                    break;
                case ParserCheckboxes.LastParsedIndex:
                    dateTime = ReplaysLastSaved;
                    break;
                case ParserCheckboxes.LatestHotsLogsUploaderIndex:
                    dateTime = HotsLogsUploader.ReplaysLatestUploaded;
                    break;
                case ParserCheckboxes.LastHotsLogsUploaderIndex:
                    dateTime = HotsLogsUploader.ReplaysLastUploaded;
                    break;
                case ParserCheckboxes.LatestHotsApiUploaderIndex:
                    dateTime = HotsApiUploader.ReplaysLatestUploaded;
                    break;
                case ParserCheckboxes.LastHotsApiUploaderIndex:
                    dateTime = HotsApiUploader.ReplaysLastUploaded;
                    break;
                default:
                    dateTime = ReplaysLatestSaved;
                    break;
            }

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

                        var replayParsed = ParseReplay(tempReplayFile, ignoreErrors: false, deleteFile: false, detailedBattleLobbyParsing: true);
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
                            if (replayParsed.Item2.ReplayBuild > Replay.LatestSupportedBuild)
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
                    AreParserButtonsEnabled = true;
                    return;
                }
                else if (IsReplayWatch && currentCount == ReplayFileCollection.Count)
                {
                    CurrentStatus = "Watching for new replays...";
                    await Task.Delay(2000);
                }
            } // end while

            CurrentStatus = "Processing stopped";
            await WaitForUploaders();
            CurrentStatus = string.Empty;
        }

        private async Task InitializeReplaySaveDataQueueAsync()
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

                    if (HotsLogsUploader.IsUploaderEnabled && (currentReplayFile.Status == ReplayResult.Saved || currentReplayFile.Status == ReplayResult.Duplicate))
                        HotsLogsUploader.ReplayUploadQueue.Enqueue(currentReplayFile);

                    if (HotsApiUploader.IsUploaderEnabled && (currentReplayFile.Status == ReplayResult.Saved || currentReplayFile.Status == ReplayResult.Duplicate))
                        HotsApiUploader.ReplayUploadQueue.Enqueue(currentReplayFile);
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

        private void ReceivedReplayFile(ReplayFile replayFile)
        {
            try
            {
                if (replayFile.HotsLogsUploadStatus != null)
                    ReplayFileCollection[ReplayFileLocations[replayFile.FilePath]].HotsLogsUploadStatus = replayFile.HotsLogsUploadStatus;
                if (replayFile.HotsApiUploadStatus != null)
                    ReplayFileCollection[ReplayFileLocations[replayFile.FilePath]].HotsApiUploadStatus = replayFile.HotsApiUploadStatus;
            }
            catch (Exception ex)
            {
                ExceptionLog.Log(LogLevel.Error, ex);
            }
        }

        private async Task WaitForUploaders()
        {
            CurrentStatus = "Waiting for uploaders to finish...";
            while (true)
            {
                if (HotsLogsUploader.IsIdleMode && HotsApiUploader.IsIdleMode)
                {
                    break;
                }

                await Task.Delay(1000);
            }
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
