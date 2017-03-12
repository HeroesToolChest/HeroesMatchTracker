namespace HeroesMatchData.Core.ViewServices
{
    public interface IMatchesTabService
    {
        /// <summary>
        /// Sets the selected tab in the Matches tab (Quick Match, Unranked Draft, etc...)
        /// </summary>
        /// <param name="selectedMatchesTab"></param>
        void SwitchToTab(MatchesTab selectedMatchesTab);
    }
}
