using System.Threading.Tasks;

namespace HeroesMatchTracker.Core.Services.Dialogs
{
    public interface IReplayParserControl
    {
        Task<string> OpenFolder();
    }
}
