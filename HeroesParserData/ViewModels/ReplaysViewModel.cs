﻿using HeroesParserData.DataQueries;
using HeroesParserData.DataQueries.ReplayData;
using HeroesParserData.Models;
using HeroesParserData.Properties;
using NLog;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    public class ReplaysViewModel : ViewModelBase
    {
        private string _currentStatus;
        private bool _isProcessSelected;
        private bool _isProcessWatchSelected;
        private bool _areProcessButtonsEnabled;
        private int _totalParsed;
        private int _totalReplays;
        private ObservableCollection<ReplayFile> _replayFiles = new ObservableCollection<ReplayFile>();

        private FileSystemWatcher _fileWatcher;

        #region public properties
        public string CurrentStatus
        {
            get { return _currentStatus; }
            set
            {
                _currentStatus = value;
                RaisePropertyChangedEvent("CurrentStatus");
            }
        }

        public bool IsProcessSelected
        {
            get { return _isProcessSelected; }
            set
            {
                _isProcessSelected = value;
                RaisePropertyChangedEvent("IsProcessSelected");
            }
        }

        public bool IsProcessWatchSelected
        {
            get { return _isProcessWatchSelected; }
            set
            {
                _isProcessWatchSelected = value;
                RaisePropertyChangedEvent("IsProcessWatchSelected");
            }
        }

        public bool AreProcessButtonsEnabled
        {
            get { return _areProcessButtonsEnabled; }
            set
            {
                _areProcessButtonsEnabled = value;
                RaisePropertyChangedEvent("AreProcessButtonsEnabled");
            }
        }

        public int TotalParsed
        {
            get { return _totalParsed; }
            set
            {
                _totalParsed = value;
                RaisePropertyChangedEvent("TotalParsed");
            }
        }

        public int TotalReplays
        {
            get { return _totalReplays; }
            set
            {
                _totalReplays = value;
                RaisePropertyChangedEvent("TotalReplays");
            }
        }
        #endregion

        #region Button Commands
        public ObservableCollection<ReplayFile> ReplayFiles
        {
            get { return _replayFiles; }
        }

        public ICommand Start
        {
            get { return new DelegateCommand(StartProcessingOnly); }
        }

        public ICommand StartWatch
        {
            get { return new DelegateCommand(StartProcessingAndWatch); }
        }

        public ICommand Stop
        {
            get { return new DelegateCommand(StopProcessingOnly); }
        }

        public ICommand StopWatch
        {
            get { return new DelegateCommand(StopProcessingAndWatch); }
        }
        #endregion

        /// <summary>
        /// Constructor
        /// </summary>
        public ReplaysViewModel()
        {
            AreProcessButtonsEnabled = true;
        }

        private void StartProcessingOnly()
        {
            IsProcessSelected = true;
            AreProcessButtonsEnabled = false;

            InitProcessing();
        }

        private void StartProcessingAndWatch()
        {
            IsProcessWatchSelected = true;
            AreProcessButtonsEnabled = false;

            InitReplayWatcher();
            InitProcessing();
        }

        private void StopProcessingOnly()
        {
            IsProcessSelected = false;
            AreProcessButtonsEnabled = true;
        }

        private void StopProcessingAndWatch()
        {
            _fileWatcher.EnableRaisingEvents = false;

            IsProcessWatchSelected = false;
            AreProcessButtonsEnabled = true;
        }

        private void InitProcessing()
        {
            //IsProcessing = true;
            Task.Run(() =>
            {
                LoadAccountDirectory();
                ParseReplays();
                //IsProcessing = false;
            });           
        }

        private void InitReplayWatcher()
        {
            _fileWatcher = new FileSystemWatcher();

            _fileWatcher.Path = Settings.Default.ReplaysLocation;
            _fileWatcher.IncludeSubdirectories = true;
            _fileWatcher.NotifyFilter = NotifyFilters.LastWrite | NotifyFilters.FileName;
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
                ReplayFiles.Add(new ReplayFile
                { 
                    FileName = Path.GetFileName(e.FullPath),
                    CreationTime = File.GetCreationTime(filePath),
                    FilePath = e.FullPath,
                    Status = null
                });
            });

            TotalReplays = ReplayFiles.Count;
        }

        private void OnDeleted(object sender, FileSystemEventArgs e)
        {
            var filePath = e.FullPath;

            Application.Current.Dispatcher.Invoke(delegate
            {
                ReplayFiles.Remove(ReplayFiles.SingleOrDefault(x => x.FilePath == e.FullPath));
            });

            TotalReplays = ReplayFiles.Count;
        }

        private void LoadAccountDirectory()
        {
            CurrentStatus = "Scanning replay folder(s)...";
            try
            {             
                List<FileInfo> listFiles = new DirectoryInfo(Settings.Default.ReplaysLocation)
                    .GetFiles($"*.{Resources.HeroesReplayFileType}", SearchOption.AllDirectories)
                    .OrderBy(x => x.CreationTimeUtc)
                    .Where(x => x.CreationTimeUtc > Query.Replay.LatestReplayByDateTimeUTC()) // last parsed replay
                    .ToList();

                TotalReplays = listFiles.Count;

                foreach (var file in listFiles)
                {
                    Application.Current.Dispatcher.Invoke(delegate
                    {
                        ReplayFiles.Add(new ReplayFile
                        {
                            FileName = file.Name,
                            CreationTime = file.CreationTime,
                            FilePath = file.FullName,
                            Status = null
                        });
                    });
                }

                CurrentStatus = "Scan completed";
            }
            catch (SqlException ex)
            {
                CurrentStatus = "Database error";
                Logger.Log(LogLevel.Error, ex);
            }
            catch (Exception ex)
            {
                CurrentStatus = "Error scanning folder";
                Logger.Log(LogLevel.Error, ex);
            }
        }

        private void ParseReplays()
        {
            int i = 0;

            // check if continuing parsing while all replays have been parsed
            while (IsProcessSelected || IsProcessWatchSelected) 
            {
                for (; i < ReplayFiles.Count(); i++)
                {
                    // check if continuing parsing while still having non-parsed replays
                    if (!IsProcessSelected && !IsProcessWatchSelected)
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
                                    file.Status = new SaveAllReplayData(replayParseResult.Item2).SaveAllData();
                                    if (file.Status == ReplayParseResult.Success)
                                    {
                                        Settings.Default.LastFileParseDate = file.CreationTime;
                                    }
                                }
                                catch (SqlException ex)
                                {
                                    Logger.Log(LogLevel.Error, "Sql exception", ex);
                                    file.Status = ReplayParseResult.Exception;
                                }
                            });
                        }
                        else
                        {
                            file.Status = replayParseResult.Item1;
                        }
                    }
                    catch (Exception ex)
                    {
                        Logger.Log(LogLevel.Error, ex);
                        file.Status = ReplayParseResult.Exception;
                    }
                    finally
                    {
                        TotalParsed++;
                        CurrentStatus = $"Parsed {file.FileName}";

                        if (File.Exists(tmpPath))
                            File.Delete(tmpPath);
                    }
                    #endregion
                } // end for

                i = ReplayFiles.Count();
                Thread.Sleep(2000);
            } // end while

            CurrentStatus = "Processing cancelled";
        }
    }      
}
