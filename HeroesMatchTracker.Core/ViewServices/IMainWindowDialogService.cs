using System.Threading.Tasks;

namespace HeroesMatchTracker.Core.ViewServices
{
    public interface IMainWindowDialogService
    {
        /// <summary>
        /// Show a message to user
        /// </summary>
        /// <param name="title">Title of message</param>
        /// <param name="message">The message itself</param>
        /// <returns></returns>
        Task ShowSimpleMessageAsync(string title, string message);

        /// <summary>
        /// Check if user BattleTag is set, if not set, then show warning dialog and return true
        /// </summary>
        /// <returns></returns>
        Task<bool> CheckBattleTagSetDialog();
    }
}
