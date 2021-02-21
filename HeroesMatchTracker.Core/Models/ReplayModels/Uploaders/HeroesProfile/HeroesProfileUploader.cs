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

namespace HeroesMatchTracker.Core.Models.ReplayModels.Uploaders.HeroesProfile
{
    public class HeroesProfileUploader : UploaderBaseModel
    {
        public HeroesProfileUploader(IInternalService internalService, IMainTabService mainTab, string hostName)
            : base(internalService, mainTab, hostName)
        {
            IsUploaderEnabled = InternalService.Database.SettingsDb().UserSettings.IsHeroesProfileUploaderEnabled;
        }

        public override bool IsUploaderEnabled
        {
            get => InternalService.Database.SettingsDb().UserSettings.IsHeroesProfileUploaderEnabled;
            set
            {
                InternalService.Database.SettingsDb().UserSettings.IsHeroesProfileUploaderEnabled = value;
                StartButtonEnabled = false;
                if (value)
                {
                    MainTab.SetHeroesProfileUploaderStatus(HeroesProfileUploaderStatus.Enabled);
                    ButtonsEnabled = true;
                    Task.Run(async () => await InitiateUploadQueueAsync());
                }
                else
                {
                    MainTab.SetHeroesProfileUploaderStatus(HeroesProfileUploaderStatus.Disabled);
                    ButtonsEnabled = false;
                    IsIdleMode = true;
                    Messenger.Default.Send(new NotificationMessage(StaticMessage.HeroesProfileUploaderDisabled));
                }

                RaisePropertyChanged();
            }
        }

        public override DateTime ReplaysLatestUploaded
        {
            get => InternalService.Database.SettingsDb().UserSettings.ReplaysLatestHeroesProfile;
            set
            {
                InternalService.Database.SettingsDb().UserSettings.ReplaysLatestHeroesProfile = value;
                RaisePropertyChanged();
            }
        }

        public override DateTime ReplaysLastUploaded
        {
            get => InternalService.Database.SettingsDb().UserSettings.ReplaysLastHeroesProfile;
            set
            {
                InternalService.Database.SettingsDb().UserSettings.ReplaysLastHeroesProfile = value;
                RaisePropertyChanged();
            }
        }

        protected override void LastDateTimeDefault()
        {
            ReplaysLastUploaded = InternalService.Database.ReplaysDb().HeroesProfileUpload.ReadLastReplayHeroesProfileUploaded();
        }

        protected override void LatestDateTimeDefault()
        {
            ReplaysLatestUploaded = InternalService.Database.ReplaysDb().HeroesProfileUpload.ReadLatestReplayHeroesProfileUploadedByDateTime();
        }

        protected override void UpdateUploadStatus(ReplayFileUploaderStatus status)
        {
            CurrentReplayFile.HeroesProfileUploadStatus = status;
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

                ReplayHeroesProfileUpload replayHeroesProfileUpload = new ReplayHeroesProfileUpload()
                {
                    ReplayId = CurrentReplayFile.ReplayId,
                    Status = (int)ReplayFileUploaderStatus.Uploading,
                };

                // check if an upload record exists for the replay
                if (InternalService.Database.ReplaysDb().HeroesProfileUpload.IsExistingRecord(replayHeroesProfileUpload))
                {
                    int? existingStatus = InternalService.Database.ReplaysDb().HeroesProfileUpload.ReadUploadStatus(replayHeroesProfileUpload);
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
                    InternalService.Database.ReplaysDb().HeroesProfileUpload.CreateRecord(replayHeroesProfileUpload);
                }

                // this will throw MaintenanceException if there is ongoing maintenance
                var status = await HeroesProfileReplayUpload.UploadReplay(CurrentReplayFile.FilePath);

                if (status == ReplayParseResult.Success || status == ReplayParseResult.Duplicate)
                {
                    replayHeroesProfileUpload.ReplayFileTimeStamp = CurrentReplayFile.TimeStamp; // the date/time of the replay itself

                    if (status == ReplayParseResult.Success)
                    {
                        replayHeroesProfileUpload.Status = (int)ReplayFileUploaderStatus.Success;
                        UpdateUploadStatus(ReplayFileUploaderStatus.Success);
                    }
                    else if (status == ReplayParseResult.Duplicate)
                    {
                        replayHeroesProfileUpload.Status = (int)ReplayFileUploaderStatus.Duplicate;
                        UpdateUploadStatus(ReplayFileUploaderStatus.Duplicate);
                    }

                    InternalService.Database.ReplaysDb().HeroesProfileUpload.UpdateReplayHeroesProfileUploadedDateTime(replayHeroesProfileUpload);

                    ReplaysLatestUploaded = InternalService.Database.ReplaysDb().HeroesProfileUpload.ReadLatestReplayHeroesProfileUploadedByDateTime();
                    ReplaysLastUploaded = replayHeroesProfileUpload.ReplayFileTimeStamp.Value.ToLocalTime();
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
