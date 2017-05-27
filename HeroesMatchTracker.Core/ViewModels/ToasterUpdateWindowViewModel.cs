using HeroesMatchTracker.Data;

namespace HeroesMatchTracker.Core.ViewModels
{
    public class ToasterUpdateWindowViewModel
    {
        private IDatabaseService Database;

        public ToasterUpdateWindowViewModel(IDatabaseService database)
        {
            Database = database;
        }

        public IDatabaseService GetDatabaseService => Database;
    }
}
