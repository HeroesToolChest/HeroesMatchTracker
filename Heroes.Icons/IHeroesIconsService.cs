using Heroes.Icons.Models;
using Heroes.Icons.Xml;
using System.Collections.Generic;
using System.IO;

namespace Heroes.Icons
{
    public interface IHeroesIconsService
    {
        int GetLatestHeroesBuild();

        /// <summary>
        /// Load a specific build, other than the latest one. Use LoadLatestHeroesBuild to load latest build.
        /// </summary>
        /// <param name="replayBuild">The replay build to load</param>
        /// <returns></returns>
        void LoadHeroesBuild(int? build);

        /// <summary>
        /// Load the latest build
        /// </summary>
        void LoadLatestHeroesBuild();
        IHeroBuilds HeroBuilds();
        IMatchAwards MatchAwards();
        IMapBackgrounds MapBackgrounds();
        IHomeScreens Homescreens();

        Stream GetPartyIcon(PartyIconColor partyIconColor);
        Stream GetOtherIcon(OtherIcon otherIcon);
        Stream GetRoleIcon(HeroRole heroRole);
        Stream GetFranchiseIcon(HeroFranchise heroFranchise);

        bool IsNonSupportHeroWithHealingStat(string realHeroName);
        List<string> GetListOfHeroesBuilds();
    }
}
