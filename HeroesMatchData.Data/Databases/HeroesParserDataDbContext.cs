namespace HeroesMatchData.Data.Databases
{
    internal class HeroesParserDataDbContext : MatchDataDbContext
    {
        /// <summary>
        /// This is the legacy (1.x.x) database context
        /// </summary>
        public HeroesParserDataDbContext()
            : base($"name={Properties.Settings.Default.OldHeroesParserDatabaseConnName}") { }
    }
}
