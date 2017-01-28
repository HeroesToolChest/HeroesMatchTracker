using Heroes.Icons.Xml;
using System.Windows.Media.Imaging;

namespace Heroes.Icons
{
    public interface IHeroesIconsService
    {
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
