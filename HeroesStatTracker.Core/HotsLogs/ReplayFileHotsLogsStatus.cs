namespace HeroesStatTracker.Core.HotsLogs
{
    public enum ReplayFileHotsLogsStatus
    {
        Success = 0,
        Duplicate = 1,
        Failed = 2,
        UploadError = 3,
        Maintenance = 4,
        Uploading = 5,
        FileNotFound = 6,
    }
}
