using System.Collections.Generic;

namespace HeroesMatchData.Data.Migrations
{
    internal class ContextMigrator
    {
        public Dictionary<int, List<string>> Migrations { get; set; } = new Dictionary<int, List<string>>();
        public Dictionary<int, List<IMigrationAddon>> MigrationAddons { get; set; } = new Dictionary<int, List<IMigrationAddon>>();
        protected List<IMigrationCommand> IMigrationList { get; private set; } = new List<IMigrationCommand>();

        protected void ExecuteMigrationCommands()
        {
            foreach (var migration in IMigrationList)
            {
                migration.Command(Migrations, MigrationAddons);
            }
        }
    }
}
