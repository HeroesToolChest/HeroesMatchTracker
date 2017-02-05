using HeroesStatTracker.Data.Models.Replays;
using System.Collections.Generic;

namespace HeroesStatTracker.Core.ViewServices
{
    public interface IMatchSummaryReplayService
    {
        void LoadMatchSummary(ReplayMatch replayMatch, List<ReplayMatch> matchList);
    }
}