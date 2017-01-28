using System.Collections.Generic;
using System.Windows.Media.Imaging;

namespace Heroes.Icons.Xml
{
    public interface IHeroBuilds
    {
        BitmapImage GetTalentIcon(string talentReferenceName);
        string GetTrueTalentName(string talentReferenceName);
        Dictionary<TalentTier, List<string>> GetTalentsForHero(string realHeroName);
        TalentTooltip GetTalentTooltips(string talentReferenceName);
        bool GetHeroNameFromTalentReferenceName(string talentName, out string heroRealName);
        List<int> GetListOfHeroesBuilds();
    }
}
