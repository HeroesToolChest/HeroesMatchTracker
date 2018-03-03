using HeroesMatchTracker.Data.Databases;
using System.Configuration;
using System.Data;
using System.Data.SQLite;
using System.Linq;

namespace HeroesMatchTracker.Data.Migrations
{
    internal class MigrationMethods<T>
        where T : MatchDataDbContext, new()
    {
        private string DbConnectionStringName;

        public MigrationMethods()
        {
            DbConnectionStringName = ConnectionString.GetConnectionStringByType<T>();
        }

        /// <summary>
        /// Adds a new column to an existing table
        /// </summary>
        /// <param name="tableName">The existing table name</param>
        /// <param name="columnName">The new column to be added</param>
        /// <param name="dataType">The data type (INTEGER), may allow other parameters (DEFAULT 0)</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security", "CA2100:Review SQL queries for security vulnerabilities", Justification = "Not user input")]
        protected void AddColumnToTable(string tableName, string columnName, string dataType)
        {
            bool columnExists = false;
            using (var conn = new SQLiteConnection(ConfigurationManager.ConnectionStrings[DbConnectionStringName].ConnectionString))
            {
                using (var cmd = new SQLiteCommand($"PRAGMA table_info({tableName});", conn))
                {
                    var table = new DataTable();

                    cmd.Connection.Open();

                    using (SQLiteDataAdapter adapter = new SQLiteDataAdapter(cmd))
                    {
                        adapter.Fill(table);
                    }

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

        /// <summary>
        /// Drops the given table
        /// </summary>
        /// <param name="tableName">table name</param>
        protected void DropTable(string tableName)
        {
            using (var conn = new SQLiteConnection(ConfigurationManager.ConnectionStrings[DbConnectionStringName].ConnectionString))
            {
                using (var cmd = new SQLiteCommand($"DROP TABLE IF EXISTS {tableName};", conn))
                {
                    cmd.Connection.Open();
                    cmd.ExecuteNonQuery();
                }
            }
        }
    }
}
