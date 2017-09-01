using HeroesMatchTracker.Data.Models.Replays;

namespace HeroesMatchTracker.Core.ViewServices
{
    public interface ICreateWindowService
    {
        void ShowWhatsNewWindow();
        void ShowUserProfileWindow();
        void ShowFailedReplaysWindow();
        void ShowPlayerNotesWindow(ReplayMatchPlayer player);
        void ShowDataFolderWindow();
    }
}
