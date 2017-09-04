using GalaSoft.MvvmLight.Messaging;
using HeroesMatchTracker.Core.Messaging;
using HeroesMatchTracker.Core.ReplayModels.Uploaders.HotsLogs;
using HeroesMatchTracker.Core.ReplayUploads;
using HeroesMatchTracker.Core.Services;
using HeroesMatchTracker.Core.ViewServices;
using HeroesMatchTracker.Data.Models.Replays;
using NLog;
using System;
using System.Threading.Tasks;
using static Heroes.ReplayParser.DataParser;

namespace HeroesMatchTracker.Core.Models.ReplayModels.Uploaders.HotsLogs
{
    public class HotsLogsUploader : UploaderBaseModel
    {
        public HotsLogsUploader(IInternalService internalService, IMainTabService mainTab, string hostName)
            : base(internalService, mainTab, hostName)
        { }

        public override bool IsUploaderEnabled
        {
            get => InternalService.Database.SettingsDb().UserSettings.IsHotsLogsUploaderEnabled;
            set
            {
                InternalService.Database.SettingsDb().UserSettings.IsHotsLogsUploaderEnabled = value;
                StartButtonEnabled = false;
                if (value)
                {
                    MainTab.SetHotsLogsUploaderStatus(HotsLogsUploaderStatus.Enabled);
                    ButtonsEnabled = true;
                    Task.Run(async () => await InitiateUploadQueueAsync());
                }
                else
                {
                    MainTab.SetHotsLogsUploaderStatus(HotsLogsUploaderStatus.Disabled);
                    ButtonsEnabled = false;
                    IsIdleMode = true;
                    Messenger.Default.Send(new NotificationMessage(StaticMessage.HotsLogsUploaderDisabled));
                }

                RaisePropertyChanged();
            }
        }

        public override DateTime ReplaysLatestUploaded
        {
            get => InternalService.Database.SettingsDb().UserSettings.ReplaysLatestHotsLogs;
            set
            {
                InternalService.Database.SettingsDb().UserSettings.ReplaysLatestHotsLogs = value;
                RaisePropertyChanged();
            }
        }

        public override DateTime ReplaysLastUploaded
        {
            get => InternalService.Database.SettingsDb().UserSettings.ReplaysLastHotsLogs;
            set
            {
                InternalService.Database.SettingsDb().UserSettings.ReplaysLastHotsLogs = value;
                RaisePropertyChanged();
            }
        }

        protected override void LastDateTimeDefault()
        {
            ReplaysLastUploaded = InternalService.Database.ReplaysDb().HotsLogsUpload.ReadLastReplayHotsLogsUploaded();
        }

        protected override void LatestDateTimeDefault()
        {
            ReplaysLatestUploaded = InternalService.Database.ReplaysDb().HotsLogsUpload.ReadLatestReplayHotsLogsUploadedByDateTime();
        }

        protected override void UpdateUploadStatus(ReplayFileUploaderStatus status)
        {
            CurrentReplayFile.HotsLogsUploadStatus = status;
            Messenger.Default.Send(CurrentReplayFile);
        }

        protected override async Task Uploader()
        {
            try
            {
                CurrentReplayFile = ReplayUploadQueue.Peek();
                UploadFileStatus = $"Uploading {CurrentReplayFile.FileName}";

                UpdateUploadStatus(ReplayFileUploaderStatus.Uploading);

                if (!ReplayExists())
                {
                    UpdateUploadStatus(ReplayFileUploaderStatus.FileNotFound);
                    return;
                }

                if (InvalidReplayCheck())
                {
                    return;
                }

                ReplayHotsLogsUpload replayHotsLogsUpload = new ReplayHotsLogsUpload()
                {
                    ReplayId = CurrentReplayFile.ReplayId,
                    Status = (int)ReplayFileUploaderStatus.Uploading,
                };

                // check if an upload record exists for the replay
                if (InternalService.Database.ReplaysDb().HotsLogsUpload.IsExistingRecord(replayHotsLogsUpload))
                {
                    int? existingStatus = InternalService.Database.ReplaysDb().HotsLogsUpload.ReadUploadStatus(replayHotsLogsUpload);
                    if (existingStatus == (int)ReplayFileUploaderStatus.Success || existingStatus == (int)ReplayFileUploaderStatus.Duplicate)
                    {
                        // already added, so its a duplicate
                        UpdateUploadStatus(ReplayFileUploaderStatus.Duplicate);

                        // remove it before we continue
                        ReplayUploadQueue.Dequeue();
                        return;
                    }
                }
                else // create a new record
                {
                    InternalService.Database.ReplaysDb().HotsLogsUpload.CreateRecord(replayHotsLogsUpload);
                }

                // upload it to the amazon bucket
                // this will throw MaintenanceException if there is ongoing maintenance
                var status = await HotsLogsReplayUpload.UploadReplay(CurrentReplayFile.FilePath);

                if (status == ReplayParseResult.Success || status == ReplayParseResult.Duplicate)
                {
                    replayHotsLogsUpload.ReplayFileTimeStamp = CurrentReplayFile.TimeStamp; // the date/time of the replay itself

                    if (status == ReplayParseResult.Success)
                    {
                        replayHotsLogsUpload.Status = (int)ReplayFileUploaderStatus.Success;
                        UpdateUploadStatus(ReplayFileUploaderStatus.Success);
                    }
                    else if (status == ReplayParseResult.Duplicate)
                    {
                        replayHotsLogsUpload.Status = (int)ReplayFileUploaderStatus.Duplicate;
                        UpdateUploadStatus(ReplayFileUploaderStatus.Duplicate);
                    }

                    InternalService.Database.ReplaysDb().HotsLogsUpload.UpdateHotsLogsUploadedDateTime(replayHotsLogsUpload);

                    ReplaysLatestUploaded = InternalService.Database.ReplaysDb().HotsLogsUpload.ReadLatestReplayHotsLogsUploadedByDateTime();
                    ReplaysLastUploaded = replayHotsLogsUpload.ReplayFileTimeStamp.Value.ToLocalTime();
                }
                else
                {
                    UpdateUploadStatus(ReplayFileUploaderStatus.Failed);
                }

                ReplayUploadQueue.Dequeue(); // we're done with the replay so remove it from the queue
            }
            catch (MaintenanceException)
            {
                UpdateUploadStatus(ReplayFileUploaderStatus.Maintenance);
                IsHostUnderMaintenance = true;
            }
            catch (Exception ex) // note: the replay is still in front of the queue
            {
                UpdateUploadStatus(ReplayFileUploaderStatus.UploadError);
                UploaderLog.Log(LogLevel.Error, ex);
                await Task.Delay(5000);
            }

            await Task.CompletedTask;
        }
    }
}
