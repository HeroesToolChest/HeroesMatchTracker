using Heroes.Icons;
using HeroesMatchData.Core.User;
using HeroesMatchData.Data;

namespace HeroesMatchData.Core.Services
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
