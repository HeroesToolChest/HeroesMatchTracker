using System.Collections.Generic;

namespace HeroesStatTracker.Data.Migrations
{
    interface IMigrationCommand
    {
        void Command(Dictionary<int, List<string>> migrations, Dictionary<int, List<IMigrationAddon>> migrationAddons);
    }
}
