using Heroes.StormReplayParser.Replay;
using System;
using System.Globalization;
using System.IO;

namespace HeroesMatchTracker.Core.Models.ReplayParser
{
    public class ReplayFile
    {
        public ReplayFile(string fullFilePath, DateTime fileCreation)
        {
            FullFilePath = fullFilePath;
            FileCreation = fileCreation;
        }

        public string Id => FullFilePath + FileCreation.ToString("G", DateTimeFormatInfo.InvariantInfo);

        public string FileName => Path.GetFileName(FullFilePath) ?? string.Empty;

        public string FullFilePath { get; }

        public DateTime FileCreation { get; }

        public StormReplayVersion? Version { get; set; }

        public string ParsedStatus { get; set; } = string.Empty;

        public HeroesHostStatuses? HeroesHostStatus { get; set; }
    }
}
