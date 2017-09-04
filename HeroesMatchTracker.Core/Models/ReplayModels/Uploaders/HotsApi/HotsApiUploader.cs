using GalaSoft.MvvmLight.Messaging;
using HeroesMatchTracker.Core.Messaging;
using HeroesMatchTracker.Core.ReplayUploads;
using HeroesMatchTracker.Core.Services;
using HeroesMatchTracker.Core.ViewServices;
using HeroesMatchTracker.Data.Models.Replays;
using NLog;
using System;
using System.Threading.Tasks;
using static Heroes.ReplayParser.DataParser;

namespace HeroesMatchTracker.Core.Models.ReplayModels.Uploaders.HotsApi
{
    public class HotsApiUploader : UploaderBaseModel
    {
        public HotsApiUploader(IInternalService internalService, IMainTabService mainTab, string hostName)
            : base(internalService, mainTab, hostName)
        { }

        public override bool IsUploaderEnabled
        {
            get => InternalService.Database.SettingsDb().UserSettings.IsHotsApiUploaderEnabled;
            set
            {
                InternalService.Database.SettingsDb().UserSettings.IsHotsApiUploaderEnabled = value;
                StartButtonEnabled = false;
                if (value)
                {
                    MainTab.SetHotsApiUploaderStatus(HotsApiUploaderStatus.Enabled);
                    ButtonsEnabled = true;
                    Task.Run(async () => await InitiateUploadQueueAsync());
                }
                else
                {
                    MainTab.SetHotsApiUploaderStatus(HotsApiUploaderStatus.Disabled);
                    ButtonsEnabled = false;
                    IsIdleMode = true;
                    Messenger.Default.Send(new NotificationMessage(StaticMessage.HotsApiUploaderDisabled));
                }

                RaisePropertyChanged();
            }
        }

        public override DateTime ReplaysLatestUploaded
        {
            get => InternalService.Database.SettingsDb().UserSettings.ReplaysLatestHotsApi;
            set
            {
                InternalService.Database.SettingsDb().UserSettings.ReplaysLatestHotsApi = value;
                RaisePropertyChanged();
            }
        }

        public override DateTime ReplaysLastUploaded
        {
            get => InternalService.Database.SettingsDb().UserSettings.ReplaysLastHotsApi;
            set
            {
                InternalService.Database.SettingsDb().UserSettings.ReplaysLastHotsApi = value;
                RaisePropertyChanged();
            }
        }

        protected override void LastDateTimeDefault()
        {
            ReplaysLastUploaded = InternalService.Database.ReplaysDb().HotsApiUpload.ReadLastReplayHotsApiUploaded();
        }

        protected override void LatestDateTimeDefault()
        {
            ReplaysLatestUploaded = InternalService.Database.ReplaysDb().HotsApiUpload.ReadLatestReplayHotsApiUploadedByDateTime();
        }

        protected override void UpdateUploadStatus(ReplayFileUploaderStatus status)
        {
            CurrentReplayFile.HotsApiUploadStatus = status;
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

                ReplayHotsApiUpload replayHotsApiUpload = new ReplayHotsApiUpload()
                {
                    ReplayId = CurrentReplayFile.ReplayId,
                    Status = (int)ReplayFileUploaderStatus.Uploading,
                };

                // check if an upload record exists for the replay
                if (InternalService.Database.ReplaysDb().HotsApiUpload.IsExistingRecord(replayHotsApiUpload))
                {
                    int? existingStatus = InternalService.Database.ReplaysDb().HotsApiUpload.ReadUploadStatus(replayHotsApiUpload);
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
                    InternalService.Database.ReplaysDb().HotsApiUpload.CreateRecord(replayHotsApiUpload);
                }

                // upload it to the amazon bucket
                // this will throw MaintenanceException if there is ongoing maintenance
                var status = await HotsApiReplayUpload.UploadReplay(CurrentReplayFile.FilePath);

                if (status == ReplayParseResult.Success || status == ReplayParseResult.Duplicate)
                {
                    replayHotsApiUpload.ReplayFileTimeStamp = CurrentReplayFile.TimeStamp; // the date/time of the replay itself

                    if (status == ReplayParseResult.Success)
                    {
                        replayHotsApiUpload.Status = (int)ReplayFileUploaderStatus.Success;
                        UpdateUploadStatus(ReplayFileUploaderStatus.Success);
                    }
                    else if (status == ReplayParseResult.Duplicate)
                    {
                        replayHotsApiUpload.Status = (int)ReplayFileUploaderStatus.Duplicate;
                        UpdateUploadStatus(ReplayFileUploaderStatus.Duplicate);
                    }

                    InternalService.Database.ReplaysDb().HotsApiUpload.UpdateHotsApiUploadedDateTime(replayHotsApiUpload);

                    ReplaysLatestUploaded = InternalService.Database.ReplaysDb().HotsApiUpload.ReadLatestReplayHotsApiUploadedByDateTime();
                    ReplaysLastUploaded = replayHotsApiUpload.ReplayFileTimeStamp.Value.ToLocalTime();
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
