namespace HeroesStatTracker.Data.Migrations
{
    internal interface IMigrator
    {
        void Initialize(bool logger = false);
    }
}
