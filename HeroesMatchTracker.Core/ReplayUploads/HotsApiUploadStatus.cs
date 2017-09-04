namespace HeroesMatchTracker.Core.ReplayUploads
{
    public enum HotsApiUploadStatus
    {
        None,
        Success,
        InProgress,
        UploadError,
        Duplicate,
        AiDetected,
        CustomGame,
        PtrRegion,
        Incomplete,
        TooOld,
    }
}
