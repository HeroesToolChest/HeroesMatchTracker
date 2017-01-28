using System.Collections.Generic;
using System.Windows.Media.Imaging;

namespace Heroes.Icons.Xml
{
    public interface IHeroes
    {
        bool HeroNameTranslation(string heroNameAlias, out string heroNameEnglish);
        BitmapImage GetHeroPortrait(string realHeroName);
        BitmapImage GetHeroLoadingPortrait(string realHeroName);
        BitmapImage GetHeroLeaderboardPortrait(string realHeroName);
        string GetRealHeroNameFromAttributeId(string attributeId);
        string GetAltNameFromRealHeroName(string realName);
        string GetRealHeroNameFromAltName(string altName);
        bool HeroExists(string heroName, bool realName = true);
        List<HeroRole> GetHeroRole(string realName);
        HeroFranchise GetHeroFranchise(string realName);
        List<string> GetListOfHeroes();
        int TotalAmountOfHeroes();
    }
}
