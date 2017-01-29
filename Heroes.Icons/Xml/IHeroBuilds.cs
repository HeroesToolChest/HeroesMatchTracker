using System.Collections.Generic;
using System.Windows.Media.Imaging;

namespace Heroes.Icons.Xml
{
    public interface IHeroBuilds
    {
        /// <summary>
        /// Returns a BitmapImage of the talent
        /// </summary>
        /// <param name="talentReferenceName">Reference talent name</param>
        /// <returns>BitmapImage of the talent</returns>
        BitmapImage GetTalentIcon(string talentReferenceName);

        /// <summary>
        /// Returns the talent name from the talent reference name
        /// </summary>
        /// <param name="talentReferenceName">Reference talent name</param>
        /// <returns>Talent name</returns>
        string GetTrueTalentName(string talentReferenceName);

        /// <summary>
        /// Returns a dictionary of all the talents of a hero
        /// </summary>
        /// <param name="realHeroName">real hero name</param>
        /// <returns></returns>
        Dictionary<TalentTier, List<string>> GetTalentsForHero(string realHeroName);

        /// <summary>
        /// Returns a TalentTooltip object which contains the short and full tooltips of the talent
        /// </summary>
        /// <param name="talentReferenceName">Talent reference name</param>
        /// <returns></returns>
        TalentTooltip GetTalentTooltips(string talentReferenceName);

        /// <summary>
        /// Gets the hero name associated with the given talent. Returns true is found, otherwise returns false
        /// </summary>
        /// <param name="talentName">The talent reference name</param>
        /// <param name="heroRealName">The hero name</param>
        /// <returns></returns>
        bool GetHeroNameFromTalentReferenceName(string talentName, out string heroRealName);
        List<int> GetListOfHeroesBuilds();
    }
}
