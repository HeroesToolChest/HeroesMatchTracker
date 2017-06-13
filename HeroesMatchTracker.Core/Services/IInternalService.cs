using Heroes.Icons;
using HeroesMatchTracker.Core.User;
using HeroesMatchTracker.Data;

namespace HeroesMatchTracker.Core.Services
{
    public interface IInternalService
    {
        IDatabaseService Database { get; }
        IHeroesIconsService HeroesIcons { get; }
        ISelectedUserProfileService UserProfile { get; }
    }
}
