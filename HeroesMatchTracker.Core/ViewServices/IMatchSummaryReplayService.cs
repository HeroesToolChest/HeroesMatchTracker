using HeroesMatchTracker.Data.Models.Replays;
using System.Collections.Generic;

namespace HeroesMatchTracker.Core.ViewServices
{
    public interface IMatchSummaryReplayService
    {
        void LoadMatchSummary(ReplayMatch replayMatch, List<ReplayMatch> matchList);
    }
}