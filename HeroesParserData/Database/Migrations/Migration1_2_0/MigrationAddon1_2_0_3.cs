using System.Threading.Tasks;

namespace HeroesParserData.Database.Migrations
{
    public class MigrationAddon1_2_0_3 : IMigrationAddon
    {
        public async Task Execute()
        {
            UserSettings.Default.SetDefaultSettings();

            await Task.CompletedTask;
        }
    }
}
