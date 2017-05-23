using System.Threading.Tasks;

namespace HeroesMatchTracker.Core.ViewServices
{
    public interface IMainWindowDialogsService
    {
        /// <summary>
        /// Check if user BattleTag is set, if not set, then show warning dialog and return true
        /// </summary>
        /// <returns></returns>
        Task<bool> CheckBattleTagSetDialog();
    }
}
