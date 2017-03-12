using Heroes.Icons;
using HeroesMatchData.Core.User;
using HeroesMatchData.Data;

namespace HeroesMatchData.Core.Services
{
    public interface IInternalService
    {
        IDatabaseService Database { get; }
        IHeroesIconsService HeroesIcons { get; }
        IUserProfileService UserProfile { get; }
    }
}
