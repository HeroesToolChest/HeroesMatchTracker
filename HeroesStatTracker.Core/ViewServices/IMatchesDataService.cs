using HeroesStatTracker.Core.Messaging;

namespace HeroesStatTracker.Core.ViewServices
{
    public interface IMatchesDataService
    {
        /// <summary>
        /// Sends data to the Match Listings for the Search options
        /// </summary>
        /// <param name="matchesDataMessage"></param>
        void SendSearchData(MatchesDataMessage matchesDataMessage);
    }
}
