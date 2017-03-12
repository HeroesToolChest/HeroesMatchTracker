using HeroesMatchData.Data.Models.Replays;
using System.Collections.Generic;

namespace HeroesMatchData.Core.ViewServices
{
    public interface IMatchSummaryReplayService
    {
        void LoadMatchSummary(ReplayMatch replayMatch, List<ReplayMatch> matchList);
    }
}