using Heroes.Icons.Xml;
using System.Windows.Media.Imaging;

namespace Heroes.Icons
{
    public interface IHeroesIconsService
    {
        int LatestSupportedBuild();

        /// <summary>
        /// Load a specific build, other than the latest one
        /// </summary>
        /// <param name="replayBuild">The replay build to load</param>
        /// <returns></returns>
        void LoadHeroesBuild(int? build);

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
    }
}
