using HeroesMatchTracker.Data.Models.Replays;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HeroesMatchTracker.Core.ViewServices
{
    public interface IMatchSummaryReplayService
    {
        Task LoadMatchSummaryAsync(ReplayMatch replayMatch, List<ReplayMatch> matchList);
    }
}