using DynamicData;
using HeroesMatchTracker.Core.Models.ReplayParser;
using System;
using System.Collections.Generic;

namespace HeroesMatchTracker.Core.Services.ReplayParser
{
    public interface IReplayCollector
    {
        IObservable<IChangeSet<ReplayFile, string>> Connect();

        void AddOrUpdate(string fullFilePath, DateTime fileCreation);

        void Remove(string fullFilePath, DateTime fileCreation);

        void Refresh(IEnumerable<ReplayFile> replayFiles);
    }
}
