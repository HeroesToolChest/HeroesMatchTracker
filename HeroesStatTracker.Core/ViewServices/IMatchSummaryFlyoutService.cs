namespace HeroesStatTracker.Core.ViewServices
{
    public interface IMatchSummaryFlyoutService
    {
        void ToggleMatchSummaryFlyout();
        void SetMatchSummaryHeader(string headerTitle);
    }
}
