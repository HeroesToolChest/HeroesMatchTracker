namespace HeroesMatchTracker.Core.ViewServices
{
    public interface IMatchesTabService
    {
        /// <summary>
        /// Sets the selected tab in the Matches tab (Quick Match, Unranked Draft, etc...)
        /// </summary>
        /// <param name="selectedMatchesTab"></param>
        void SwitchToTab(MatchesTab selectedMatchesTab);

        /// <summary>
        /// Gets the currently selected matches tab
        /// </summary>
        /// <returns></returns>
        MatchesTab GetCurrentlySelectedTab();
    }
}
