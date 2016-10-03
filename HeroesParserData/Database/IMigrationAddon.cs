using System.Threading.Tasks;

namespace HeroesParserData.Database
{
    public interface IMigrationAddon
    {
        Task Execute();
    }
}
