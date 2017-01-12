using HeroesStatTracker.Data.Databases;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HeroesStatTracker.Data.Migrations.Replays
{
    internal class MigrationAddon1_v2_0_0_1 : MigrationMethods<ReplaysContext>, IMigrationAddon
    {
        public MigrationAddon1_v2_0_0_1()
            :base(Properties.Settings.Default.ReplaysConnNameDb)
        {

        }

        public void Execute()
        {
            //AddColumnToTable("ReplayMatchPlayers", "PartyValue", "INTEGER DEFAULT 0");
        }
    }
}
