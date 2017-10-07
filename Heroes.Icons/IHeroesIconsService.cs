using Heroes.Icons.Models;
using Heroes.Icons.Xml;
using System;
using System.Collections.Generic;

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
        IHomeScreens HomeScreens();

        Uri GetPartyIcon(PartyIconColor partyIconColor);
        Uri GetOtherIcon(OtherIcon otherIcon);
        Uri GetRoleIcon(HeroRole heroRole);
        Uri GetFranchiseIcon(HeroFranchise heroFranchise);
        bool IsNonSupportHeroWithHealingStat(string realHeroName);
        List<string> GetListOfHeroesBuilds();
    }
}
