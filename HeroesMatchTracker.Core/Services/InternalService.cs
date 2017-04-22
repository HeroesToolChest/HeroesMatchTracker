using Heroes.Icons;
using HeroesMatchTracker.Core.User;
using HeroesMatchTracker.Data;

namespace HeroesMatchTracker.Core.Services
{
    public class InternalService : IInternalService
    {
        public InternalService(IDatabaseService database, IHeroesIconsService heroesIcons, IUserProfileService userProfile, IWebsiteService website)
        {
            Database = database;
            HeroesIcons = heroesIcons;
            UserProfile = userProfile;
            Website = website;
        }

        public IDatabaseService Database { get; }

        public IHeroesIconsService HeroesIcons { get; }

        public IUserProfileService UserProfile { get; }

        public IWebsiteService Website { get; }
    }
}
