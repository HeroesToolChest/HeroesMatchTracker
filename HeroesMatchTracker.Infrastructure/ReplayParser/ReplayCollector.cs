using DynamicData;
using HeroesMatchTracker.Core.Models.ReplayParser;
using HeroesMatchTracker.Core.Services.ReplayParser;
using System;
using System.Collections.Generic;

namespace HeroesMatchTracker.Infrastructure
{
    public class ReplayCollector : IReplayCollector
    {
        private readonly SourceCache<ReplayFile, string> _files = new SourceCache<ReplayFile, string>(x => x.Id);

        public IObservable<IChangeSet<ReplayFile, string>> Connect() => _files.Connect();

        public void AddOrUpdate(string fullFilePath, DateTime fileCreation)
        {
            _files.AddOrUpdate(new ReplayFile(fullFilePath, fileCreation));
        }

        public void Remove(string fullFilePath, DateTime fileCreation)
        {
            _files.Remove(new ReplayFile(fullFilePath, fileCreation));
        }

        public void Refresh(IEnumerable<ReplayFile> replayFiles)
        {
            _files.Clear();

            _files.Edit(cache =>
            {
                foreach (ReplayFile item in replayFiles)
                {
                    cache.AddOrUpdate(item);
                }
            });
        }
    }
}
