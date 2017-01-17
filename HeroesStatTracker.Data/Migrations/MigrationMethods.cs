using HeroesStatTracker.Data.Databases;
using System.Configuration;
using System.Data;
using System.Data.SQLite;
using System.Linq;

namespace HeroesStatTracker.Data.Migrations
{
    internal class MigrationMethods<T>
        where T : StatTrackerDbContext, new()
    {
        private string DbConnectionStringName;

        public MigrationMethods(string dbConnectionStringName)
        {
            DbConnectionStringName = dbConnectionStringName;
        }

        /// <summary>
        /// Adds a new column to an existing table
        /// </summary>
        /// <param name="tableName">The existing table name</param>
        /// <param name="columnName">The new column to be added</param>
        /// <param name="dataType">The data type (INTEGER), may allow other parameters (DEFAULT 0)</param>
        protected void AddColumnToTable(string tableName, string columnName, string dataType)
        {
            bool columnExists = false;
            using (var conn = new SQLiteConnection(ConfigurationManager.ConnectionStrings[DbConnectionStringName].ConnectionString))
            {
                using (var cmd = new SQLiteCommand($"PRAGMA table_info({tableName});"))
                {
                    var table = new DataTable();

                    cmd.Connection = conn;
                    cmd.Connection.Open();

                    SQLiteDataAdapter adp = null;
                    adp = new SQLiteDataAdapter(cmd);
                    adp.Fill(table);

                    var columnNames = table.AsEnumerable().Select(x => x["name"].ToString()).ToList();
                    columnExists = columnNames.Contains($"{columnName}");
                }
            }

            if (!columnExists)
            {
                using (T db = new T())
                {
                    db.Database.ExecuteSqlCommand($"ALTER TABLE {tableName} ADD COLUMN {columnName} {dataType}");
                }
            }
        }
    }
}
