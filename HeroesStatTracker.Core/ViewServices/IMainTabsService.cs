namespace HeroesStatTracker.Core.ViewServices
{
    public interface IMainTabsService
    {
        /// <summary>
        /// Sets the selected Main Tab
        /// </summary>
        /// <param name="selectedMainTab"></param>
        void SwitchToTab(MainTabs selectedMainTab);
    }
}
