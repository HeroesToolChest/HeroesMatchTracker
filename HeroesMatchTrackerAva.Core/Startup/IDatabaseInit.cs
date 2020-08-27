namespace HeroesMatchTracker.Core.Startup
{
    public interface IDatabaseInit
    {
        void HMT2ReplayDbCheck();

        void InitHeroesReplaysDb();
    }
}
