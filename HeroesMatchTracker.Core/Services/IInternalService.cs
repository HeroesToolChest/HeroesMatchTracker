using Heroes.Icons;
using HeroesMatchTracker.Core.User;
using HeroesMatchTracker.Data;

namespace HeroesMatchTracker.Core.Services
{
    public interface IInternalService
    {
        IDatabaseService Database { get; }
        IHeroesIcons HeroesIcons { get; }
        ISelectedUserProfileService UserProfile { get; }
    }
}
