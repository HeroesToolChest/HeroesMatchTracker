using System;

namespace HeroesMatchData.Core.HotsLogs
{
    [Flags]
    public enum ReplayFileHotsLogsStatus
    {
        Success = 0,
        Duplicate = 1 << 0,
        Failed = 1 << 1,
        UploadError = 1 << 2,
        Maintenance = 1 << 3,
        Uploading = 1 << 4,
        FileNotFound = 1 << 5,
    }
}
