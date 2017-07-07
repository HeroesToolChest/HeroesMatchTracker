using Heroes.Icons.Xml;
using System.Collections.Generic;
using System.Windows.Media.Imaging;

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
        IHeroes Heroes();
        IHeroBuilds HeroBuilds();
        IMatchAwards MatchAwards();
        IMapBackgrounds MapBackgrounds();
        IHomeScreens HomeScreens();

        BitmapImage GetPartyIcon(PartyIconColor partyIconColor);
        BitmapImage GetOtherIcon(OtherIcon otherIcon);
        BitmapImage GetRoleIcon(HeroRole heroRole);
        BitmapImage GetFranchiseIcon(HeroFranchise heroFranchise);
        bool IsNonSupportHeroWithHealingStat(string realHeroName);
        List<int> GetListOfHeroesBuilds();
    }
}
