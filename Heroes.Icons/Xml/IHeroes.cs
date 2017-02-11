using System.Collections.Generic;
using System.Windows.Media.Imaging;

namespace Heroes.Icons.Xml
{
    public interface IHeroes
    {
        /// <summary>
        /// Gets the english name of the given alias. Returns true if found, otherwise false
        /// </summary>
        /// <param name="heroNameAlias">Alias name</param>
        /// <param name="heroNameEnglish">English name</param>
        /// <returns></returns>
        bool HeroNameTranslation(string heroNameAlias, out string heroNameEnglish);

        /// <summary>
        /// Returns a BitmapImage of the hero
        /// </summary>
        /// <param name="realHeroName">Real hero name</param>
        /// <returns>BitmpImage of the hero</returns>
        BitmapImage GetHeroPortrait(string realHeroName);

        /// <summary>
        /// Returns a loading portrait BitmapImage of the hero
        /// </summary>
        /// <param name="realHeroName">Real hero name</param>
        /// <returns>BitmpImage of the hero</returns>
        BitmapImage GetHeroLoadingPortrait(string realHeroName);

        /// <summary>
        /// Returns a leaderboard portrait BitmapImage of the hero
        /// </summary>
        /// <param name="realHeroName">Real hero name</param>
        /// <returns>BitmpImage of the hero</returns>
        BitmapImage GetHeroLeaderboardPortrait(string realHeroName);

        /// <summary>
        /// Returns the real hero name from the hero's attribute id
        /// </summary>
        /// <param name="attributeId">Four character hero id</param>
        /// <returns>Full hero name</returns>
        string GetRealHeroNameFromAttributeId(string attributeId);

        string GetAltNameFromRealHeroName(string realName);

        string GetRealHeroNameFromAltName(string altName);

        /// <summary>
        /// Checks to see if the hero name exists
        /// </summary>
        /// <param name="heroName">Hero name</param>
        /// <param name="realName">Is the name a real name or alt name</param>
        /// <returns>True if found</returns>
        bool HeroExists(string heroName, bool realName = true);

        /// <summary>
        /// Returns the hero's list of roles. Multiclass will be first if hero has multiple roles. Will return a role of Unknown if hero name not found.
        /// </summary>
        /// <param name="realName">Hero real name</param>
        /// <returns>HeroRole</returns>
        List<HeroRole> GetHeroRoleList(string realName);

        /// <summary>
        /// Returns the hero's franchise. Will return Unknown if hero not found
        /// </summary>
        /// <param name="realName">Heroes real name</param>
        /// <returns>HeroRole</returns>
        HeroFranchise GetHeroFranchise(string realName);

        List<string> GetListOfHeroes();

        int TotalAmountOfHeroes();
    }
}
