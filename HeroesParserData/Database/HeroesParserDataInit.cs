using HeroesParserData.Models.DbModels;
using System.Data.Entity.Migrations;
using System.Linq;

namespace HeroesParserData.Database
{
    public class HeroesParserDataInit
    {
        public static void InitiliazeHeroesParserDataStore()
        {
            System.Data.Entity.Database.SetInitializer(new System.Data.Entity.MigrateDatabaseToLatestVersion<HeroesParserDataContext, HeroesParserDataMigrationConfiguration>());

            var configuration = new HeroesParserDataMigrationConfiguration();
            var migrator = new DbMigrator(configuration);

            if (migrator.GetPendingMigrations().Any())
            {
                migrator.Update();
            }
        }
    }

    public sealed class HeroesParserDataMigrationConfiguration : DbMigrationsConfiguration<HeroesParserDataContext>
    {
        public HeroesParserDataMigrationConfiguration()
        {
            AutomaticMigrationsEnabled = true;
        }
    }
}

