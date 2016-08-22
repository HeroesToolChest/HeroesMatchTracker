using HeroesParserData.DataQueries;
using HeroesParserData.DataQueries.ReplayData;
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
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using static Heroes.ReplayParser.DataParser;

namespace HeroesParserData.ViewModels
{
    public class ReplaysViewModel : ViewModelBase, IDisposable
    {
        private string _currentStatus;
        private bool _isProcessSelected;
        private bool _areProcessButtonsEnabled;
        private bool _fastParseButtonEnabled;
        private int _totalParsedGrid;
        private int _totalReplaysGrid;
        private long _totalSavedInDatabase;
        private int _selectedProcessCount;
        private ObservableCollection<ReplayFile> _replayFiles = new ObservableCollection<ReplayFile>();

        private FileSystemWatcher _fileWatcher;

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

        public bool IsProcessSelected
        {
            get { return _isProcessSelected; }
            set
            {
                _isProcessSelected = value;
                RaisePropertyChangedEvent(nameof(IsProcessSelected));
            }
        }

        public bool IsProcessWatchChecked
        {
            get { return Settings.Default.ReplayWatchCheckBox; }
            set
            {
                Settings.Default.ReplayWatchCheckBox = value;
                RaisePropertyChangedEvent(nameof(IsProcessWatchChecked));
            }
        }

        public bool IsAutoScanChecked
        {
            get { return Settings.Default.ReplayAutoScanCheckBox; }
            set
            {
                Settings.Default.ReplayAutoScanCheckBox = value;
                RaisePropertyChangedEvent(nameof(IsAutoScanChecked));
            }
        }

        public bool AreProcessButtonsEnabled
        {
            get { return _areProcessButtonsEnabled; }
            set
            {
                _areProcessButtonsEnabled = value;
                FastParseButtonEnabled = value;
                RaisePropertyChangedEvent(nameof(AreProcessButtonsEnabled));
            }
        }

        public bool FastParseButtonEnabled
        {
            get { return _fastParseButtonEnabled; }
            set
            {
                _fastParseButtonEnabled = value;
                if (MaxProcessorCount < (Settings.Default.MinProcessors + 1))
                    _fastParseButtonEnabled = false;

                RaisePropertyChangedEvent(nameof(FastParseButtonEnabled));
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

        public DateTime ReplaysLatestParsed
        {
            get { return Settings.Default.ReplaysLatestParsed != DateTime.MinValue? Settings.Default.ReplaysLatestParsed :  Query.Replay.ReadLatestReplayByDateTime(); }
            set
            {
                Settings.Default.ReplaysLatestParsed = value;
                RaisePropertyChangedEvent(nameof(ReplaysLatestParsed));
            }
        }

        public DateTime ReplaysLastParsed
        {
            get { return Settings.Default.ReplaysLastParsed != DateTime.MinValue ? Settings.Default.ReplaysLastParsed : Query.Replay.ReadLastReplayByDateTime(); }
            set
            {
                Settings.Default.ReplaysLastParsed = value;
                RaisePropertyChangedEvent(nameof(ReplaysLastParsed));
            }
        }

        public ObservableCollection<ReplayFile> ReplayFiles
        {
            get { return _replayFiles; }
        }

        public string ReplaysLocation
        {
            get { return Settings.Default.ReplaysLocation; }
            set
            {
                Settings.Default.ReplaysLocation = value;
                RaisePropertyChangedEvent(nameof(ReplaysLocation));
            }
        }

        public int MaxProcessorCount
        {
            get { return Environment.ProcessorCount; }
        }

        public int SelectedProcessCount
        {
            get
            {
                if (_selectedProcessCount < 3)
                    return 3;
                else
                    return _selectedProcessCount;
            }
            set
            {
                if (value >= MaxProcessorCount)
                    _selectedProcessCount = MaxProcessorCount - 1;
                else if (value < Settings.Default.MinProcessors)
                    _selectedProcessCount = MaxProcessorCount;
                else
                    _selectedProcessCount = value;

                RaisePropertyChangedEvent(nameof(SelectedProcessCount));
            }
        }

        public bool LatestParsedChecked
        {
            get { return Settings.Default.ParsedDateTimeCheckBox; }
            set
            {
                Settings.Default.ParsedDateTimeCheckBox = value;
                RaisePropertyChangedEvent(nameof(LatestParsedChecked));
                RaisePropertyChangedEvent(nameof(LastParsedChecked));
            }
        }

        public bool LastParsedChecked
        {
            get { return !Settings.Default.ParsedDateTimeCheckBox; }
            set
            {
                Settings.Default.ParsedDateTimeCheckBox = !value;
                RaisePropertyChangedEvent(nameof(LastParsedChecked));
                RaisePropertyChangedEvent(nameof(LatestParsedChecked));
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

        public ICommand FastParseStart
        {
            get { return new DelegateCommand(MultiThreaderParser); }
        }
        #endregion

        /// <summary>
        /// Constructor
        /// </summary>
        public ReplaysViewModel()
        {
            AreProcessButtonsEnabled = true;
            LatestParsedChecked = true;
        }

        #region start processing/init
        private void StartScan()
        {
            AreProcessButtonsEnabled = false;
            Task.Run(() => 
            {
                LoadAccountDirectory();
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
        }

        private void StopProcessing()
        {
            IsProcessSelected = false;
            if (_fileWatcher != null && IsProcessWatchChecked)
            {
                _fileWatcher.EnableRaisingEvents = false;
                _fileWatcher = null;
            }

            if (!string.IsNullOrEmpty(CurrentStatus))
                CurrentStatus += " (Stopping, awaiting completion of current task)";
        }

        private void MultiThreaderParser()
        {
            IsProcessSelected = true;
            AreProcessButtonsEnabled = false;
            Task.Run(() =>
            {
                ParseReplaysMultiThreading();
                AreProcessButtonsEnabled = true;
                IsProcessSelected = false;
            });
           
        }

        /// <summary>
        /// Start the parsing. Runs on a separate thread.
        /// </summary>
        private void InitProcessing(bool isAutoScan)
        {
            Task.Run(() =>
            {
                if (isAutoScan)
                    LoadAccountDirectory();

                ParseReplays();
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
            ReplaysLatestParsed = ReplaysLatestParsed;
        }

        private void ReplaysDateTimeDefault()
        {
            ReplaysLatestParsed = Query.Replay.ReadLatestReplayByDateTime();
        }

        private void ReplaysDateTimeClear()
        {
            ReplaysLatestParsed = new DateTime(1);
        }

        private void LastReplaysDateTimeSet()
        {
            ReplaysLastParsed = ReplaysLastParsed;
        }

        private void LastReplaysDateTimeDefault()
        {
            ReplaysLastParsed = Query.Replay.ReadLastReplayByDateTime();
        }

        private void LastReplaysDateTimeClear()
        {
            ReplaysLastParsed = new DateTime(1);
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
            _fileWatcher = new FileSystemWatcher();

            _fileWatcher.Path = Settings.Default.ReplaysLocation;
            _fileWatcher.IncludeSubdirectories = true;
            _fileWatcher.NotifyFilter = NotifyFilters.LastWrite | NotifyFilters.FileName | NotifyFilters.Attributes;
            _fileWatcher.Filter = $"*.{Resources.HeroesReplayFileType}";

            _fileWatcher.Changed += new FileSystemEventHandler(OnChanged);
            _fileWatcher.Deleted += new FileSystemEventHandler(OnDeleted);

            _fileWatcher.EnableRaisingEvents = true;
        }

        private void OnChanged(object sender, FileSystemEventArgs e)
        {
            var filePath = e.FullPath;

            Application.Current.Dispatcher.Invoke(delegate
            {
                if (ReplayFiles.FirstOrDefault(x => x.FilePath == e.FullPath) == null)
                {
                    ReplayFiles.Add(new ReplayFile
                    {
                        FileName = Path.GetFileName(e.FullPath),
                        LastWriteTime = File.GetLastWriteTime(filePath),
                        FilePath = e.FullPath,
                        Status = null
                    });
                }
            });

            TotalReplaysGrid = ReplayFiles.Count;
        }

        private void OnDeleted(object sender, FileSystemEventArgs e)
        {
            var filePath = e.FullPath;

            Application.Current.Dispatcher.Invoke(delegate
            {
                var file = ReplayFiles.FirstOrDefault(x => x.FilePath == e.FullPath);
                if (file != null)
                    ReplayFiles.Remove(file);
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
        private void LoadAccountDirectory()
        {
            CurrentStatus = "Scanning replay folder(s)...";

            try
            {
                var dateTime = LatestParsedChecked == true ? ReplaysLatestParsed : ReplaysLastParsed;
                List <FileInfo> listFiles = new DirectoryInfo(Settings.Default.ReplaysLocation)
                    .GetFiles($"*.{Resources.HeroesReplayFileType}", SearchOption.AllDirectories)
                    .OrderBy(x => x.LastWriteTime)
                    .Where(x => x.LastWriteTime > dateTime)
                    .ToList();

                TotalReplaysGrid = listFiles.Count;

                Application.Current.Dispatcher.Invoke(delegate
                {
                    ReplayFiles.Clear();
                    TotalParsedGrid = 0;

                    foreach (var file in listFiles)
                    {
                        ReplayFiles.Add(new ReplayFile
                        {
                            FileName = file.Name,
                            LastWriteTime = file.LastWriteTime,
                            FilePath = file.FullName,
                            Status = null
                        });
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
        private void ParseReplays()
        {
            int i = 0;

            // check if continuing parsing while all replays have been parsed
            while (IsProcessSelected)
            {
                for (; i < ReplayFiles.Count(); i++)
                {
                    // check if continuing parsing while still having non-parsed replays
                    if (!IsProcessSelected)
                        break;

                    #region parse replay and save data
                    var tmpPath = Path.GetTempFileName();
                    var file = ReplayFiles[i];

                    CurrentStatus = $"Parsing file {file.FileName}";

                    try
                    {
                        File.Copy(file.FilePath, tmpPath, overwrite: true);

                        var replayParseResult = ParseReplay(tmpPath, ignoreErrors: false, deleteFile: false);
                        if (replayParseResult.Item1 == ReplayParseResult.Success)
                        {
                            file.Status = ReplayParseResult.Success;

                            // save the data on a seperate thread
                            Task.Run(() =>
                            {
                                try
                                {
                                    DateTime parsedDateTime;
                                    file.Status = new SaveAllReplayData(replayParseResult.Item2, file.FileName).SaveAllData(out parsedDateTime);
                                    if (file.Status == ReplayParseResult.Saved)
                                    {
                                        TotalSavedInDatabase++;
                                        ReplaysLatestParsed = Query.Replay.ReadLatestReplayByDateTime();
                                        ReplaysLastParsed = parsedDateTime.ToLocalTime();
                                    }
                                }
                                catch (Exception ex) when (ex is SqlException || ex is DbEntityValidationException)
                                {
                                    file.Status = ReplayParseResult.SqlException;
                                    SqlExceptionReplaysLog.Log(LogLevel.Error, ex);
                                    FailedReplaysLog.Log(LogLevel.Info, $"{file.FileName}: {file.Status}");
                                }
                                catch (Exception ex)
                                {
                                    file.Status = ReplayParseResult.Exception;
                                    ExceptionLog.Log(LogLevel.Error, ex);
                                    FailedReplaysLog.Log(LogLevel.Info, $"{file.FileName}: {file.Status}");
                                }
                            });
                        }
                        else
                        {
                            file.Status = replayParseResult.Item1;
                            FailedReplaysLog.Log(LogLevel.Info, $"{file.FileName}: {file.Status}");
                        }
                    }
                    catch (Exception ex)
                    {
                        file.Status = ReplayParseResult.Exception;
                        ExceptionLog.Log(LogLevel.Error, ex);
                        FailedReplaysLog.Log(LogLevel.Info, $"{file.FileName}: {file.Status}");

                    }
                    finally
                    {
                        TotalParsedGrid++;
                        CurrentStatus = $"Parsed {file.FileName}";

                        if (File.Exists(tmpPath))
                            File.Delete(tmpPath);
                    }
                    #endregion parse replay and save data
                } // end for

                // if no watch is selected and if all replays got parsed then automatically end
                if (!IsProcessWatchChecked && i == ReplayFiles.Count)
                {
                    CurrentStatus = "Processing completed";
                    IsProcessSelected = false;
                    AreProcessButtonsEnabled = true;
                    return;
                }
                else if (IsProcessWatchChecked && i == ReplayFiles.Count)
                {
                    CurrentStatus = "Watching for new replays...";
                    Thread.Sleep(2000);
                }

                i = ReplayFiles.Count();
            } // end while

            CurrentStatus = "Processing stopped";
            AreProcessButtonsEnabled = true;
        }

        /// <summary>
        /// Parses all relays using multithreading
        /// </summary>
        private void ParseReplaysMultiThreading()
        {
            try
            {
                CurrentStatus = "Parsing all replays...";
                Parallel.ForEach(ReplayFiles, new ParallelOptions { MaxDegreeOfParallelism = SelectedProcessCount }, file =>
                {
                    if (!IsProcessSelected)
                        return;

                    var tmpPath = Path.GetTempFileName();
                    try
                    {
                        File.Copy(file.FilePath, tmpPath, overwrite: true);

                        var replayParseResult = ParseReplay(tmpPath, ignoreErrors: false, deleteFile: false);
                        if (replayParseResult.Item1 == ReplayParseResult.Success)
                        {
                            file.Status = ReplayParseResult.Success;

                            DateTime parsedDateTime;
                            file.Status = new SaveAllReplayData(replayParseResult.Item2, file.FileName).SaveAllData(out parsedDateTime);
                            if (file.Status == ReplayParseResult.Saved)
                            {
                                TotalSavedInDatabase++;
                            }
                        }
                        else
                        {
                            file.Status = replayParseResult.Item1;
                            FailedReplaysLog.Log(LogLevel.Info, $"{file.FileName}: {file.Status}");
                        }
                    }
                    catch (Exception ex) when (ex is SqlException || ex is DbEntityValidationException)
                    {
                        file.Status = ReplayParseResult.SqlException;
                        SqlExceptionReplaysLog.Log(LogLevel.Error, ex);
                        FailedReplaysLog.Log(LogLevel.Info, $"{file.FileName}: {file.Status}");
                    }
                    catch (Exception ex)
                    {
                        file.Status = ReplayParseResult.Exception;
                        ExceptionLog.Log(LogLevel.Error, ex);
                        FailedReplaysLog.Log(LogLevel.Info, $"{file.FileName}: {file.Status}");
                    }
                    finally
                    {
                        TotalParsedGrid++;

                        if (File.Exists(tmpPath))
                            File.Delete(tmpPath);
                    }
                });

                if (IsProcessSelected)
                {
                    ReplaysLatestParsed = Query.Replay.ReadLatestReplayByDateTime();
                    ReplaysLastParsed = Query.Replay.ReadLastReplayByDateTime();
                    CurrentStatus = "Parsing completed";
                }
                else
                {
                    CurrentStatus = "Fast parse stopped";
                }
            }
            catch (Exception ex)
            {
                ExceptionLog.Log(LogLevel.Error, ex);
                CurrentStatus = "Fast parse error";
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
                    ((IDisposable)_fileWatcher).Dispose();
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
