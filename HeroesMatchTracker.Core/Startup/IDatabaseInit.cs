namespace HeroesMatchTracker.Core.Startup
{
    public interface IDatabaseInit
    {
        /// <summary>
        /// Check for the HMT version 2 database and convert it to the current version.
        /// </summary>
        void HMT2ReplayDbCheck();

        /// <summary>
        /// Initiliaze the database for the heroes replays.
        /// </summary>
        void InitHeroesReplaysDb();
    }
}
