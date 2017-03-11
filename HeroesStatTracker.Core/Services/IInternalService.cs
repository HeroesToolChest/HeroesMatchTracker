using Heroes.Icons;
using HeroesStatTracker.Core.User;
using HeroesStatTracker.Data;

namespace HeroesStatTracker.Core.Services
{
    public interface IInternalService
    {
        IDatabaseService Database { get; }
        IHeroesIconsService HeroesIcons { get; }
        IUserProfileService UserProfile { get; }
    }
}
