using HeroesParserData.Models.DbModels;
using System.Configuration;
using System.Data;
using System.Data.SQLite;
using System.Linq;
using System.Threading.Tasks;

namespace HeroesParserData.Database.Migrations
{
    public class MigrationAddon1_1_0_1 : IMigrationAddon
    {
        public async Task Execute()
        {
            bool columnExists = false;
            using (var conn = new SQLiteConnection(ConfigurationManager.ConnectionStrings["HeroesParserData"].ConnectionString))
            {
                using (var cmd = new SQLiteCommand("PRAGMA table_info(ReplayMatchPlayers);"))
                {
                    var table = new DataTable();

                    cmd.Connection = conn;
                    await cmd.Connection.OpenAsync();

                    SQLiteDataAdapter adp = null;
                    adp = new SQLiteDataAdapter(cmd);
                    adp.Fill(table);

                    var columnNames = table.AsEnumerable().Select(x => x["name"].ToString()).ToList();
                    columnExists = columnNames.Contains("PartyValue");
                }
            }

            if (!columnExists)
            {
                using (HeroesParserDataContext db = new HeroesParserDataContext())
                {
                    await db.Database.ExecuteSqlCommandAsync("ALTER TABLE ReplayMatchPlayers ADD COLUMN PartyValue INTEGER DEFAULT 0");
                }
            }
        }
    }
}
