namespace HeroesMatchTracker.Core.Startup
{
    public interface IStartupWindow
    {
        void CloseStartupWindow();

        void CreateMainWindow();
    }
}
