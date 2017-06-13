using Heroes.Icons;
using HeroesMatchTracker.Core.User;
using HeroesMatchTracker.Data;

namespace HeroesMatchTracker.Core.Services
{
    public class InternalService : IInternalService
    {
        public InternalService(IDatabaseService database, IHeroesIconsService heroesIcons, ISelectedUserProfileService userProfile, IWebsiteService website)
        {
            Database = database;
            HeroesIcons = heroesIcons;
            UserProfile = userProfile;
            Website = website;
        }

        public IDatabaseService Database { get; }

        public IHeroesIconsService HeroesIcons { get; }

        public ISelectedUserProfileService UserProfile { get; }

        public IWebsiteService Website { get; }
    }
}
