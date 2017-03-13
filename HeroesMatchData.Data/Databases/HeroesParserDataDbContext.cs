namespace HeroesMatchData.Data.Databases
{
    internal class HeroesParserDataDbContext : MatchDataDbContext
    {
        public HeroesParserDataDbContext()
            : base($"name={Properties.Settings.Default.OldHeroesParserDatabaseConnName}") { }
    }
}
