using System.Threading.Tasks;

namespace HeroesParserData.Database.Migrations
{
    public interface IMigrationAddon
    {
        Task Execute();
    }
}
