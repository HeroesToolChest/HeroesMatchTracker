using HeroesMatchData.Migrations;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Linq;
using System.Threading.Tasks;

namespace HeroesMatchData.Data.Databases
{
    internal class MatchDataDbContext : DbContext
    {
        protected MatchDataDbContext(string nameOrConnectionString)
            : base(nameOrConnectionString) { }

        public virtual DbSet<SchemaInfo> SchemaInfo { get; set; }

        public override int SaveChanges()
        {
            try
            {
                return base.SaveChanges();
            }
            catch (DbEntityValidationException ex)
            {
                throw new DbEntityValidationException(CustomErrorMessage(ex), ex.EntityValidationErrors);
            }
        }

        public override async Task<int> SaveChangesAsync()
        {
            try
            {
                return await base.SaveChangesAsync();
            }
            catch (DbEntityValidationException ex)
            {
                throw new DbEntityValidationException(CustomErrorMessage(ex), ex.EntityValidationErrors);
            }
        }

        private static string CustomErrorMessage(DbEntityValidationException ex)
        {
            // Retrieve the error messages as a list of strings.
            var errorMessages = ex.EntityValidationErrors
                    .SelectMany(x => x.ValidationErrors)
                    .Select(x => x.ErrorMessage);

            // Join the list to a single string.
            var fullErrorMessage = string.Join("; ", errorMessages);

            // Combine the original exception message with the new one.
            return string.Concat(ex.Message, " The validation errors are: ", fullErrorMessage);
        }
    }
}
