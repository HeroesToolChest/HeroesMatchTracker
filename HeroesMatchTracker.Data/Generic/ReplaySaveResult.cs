using System;

namespace HeroesMatchTracker.Data
{
    [Flags]
    public enum ReplayResult : uint
    {
        Success = 0,
        ComputerPlayerFound = 1 << 0,
        Incomplete = 1 << 1,
        Duplicate = 1 << 2,
        TryMeMode = 1 << 3,
        UnexpectedResult = 1 << 4,
        Exception = 1 << 5,
        FileNotFound = 1 << 6,
        PreAlphaWipe = 1 << 7,
        FileSizeTooLarge = 1 << 8,
        PTRRegion = 1 << 9,
        Saved = 1 << 10,
        SqlException = 1 << 11,
        NotYetSupported = 1 << 12,
        ParserException = 1 << 13,
        TranslationException = 1 << 14,
        SuccessReplayDetail = 1 << 15
    }
}
