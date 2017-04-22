namespace HeroesMatchTracker.Core.ViewServices
{
    public interface ILoadingOverlayWindowService
    {
        /// <summary>
        /// Closes the Loading Overlay
        /// </summary>
        void CloseLoadingOverlay();

        /// <summary>
        /// Show Loading Overlay
        /// </summary>
        void ShowLoadingOverlay();
    }
}
