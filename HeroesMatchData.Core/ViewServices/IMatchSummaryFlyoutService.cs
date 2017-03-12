namespace HeroesMatchData.Core.ViewServices
{
    public interface IMatchSummaryFlyoutService
    {
        /// <summary>
        /// Open the Match Summary if closed and vice-versa
        /// </summary>
        void ToggleMatchSummaryFlyout();

        /// <summary>
        /// Close the Match Summary
        /// </summary>
        void CloseMatchSummaryFlyout();

        /// <summary>
        /// Open the Match Summary
        /// </summary>
        void OpenMatchSummaryFlyout();

        /// <summary>
        /// Set the Title header on the Match Summary
        /// </summary>
        /// <param name="headerTitle"></param>
        void SetMatchSummaryHeader(string headerTitle);
    }
}
