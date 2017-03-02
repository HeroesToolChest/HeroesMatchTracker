namespace HeroesStatTracker.Core.ViewServices
{
    public interface IMainPageService
    {
        /// <summary>
        /// Sets the selected Main Page
        /// </summary>
        /// <param name="selectedMainPage"></param>
        void SwitchToPage(MainPage selectedMainPage);
    }
}
