using Heroes.Icons.Models;
using System.Collections.Generic;

namespace Heroes.Icons.Xml
{
    public interface IHeroes
    {
        /// <summary>
        /// Returns a Hero object
        /// </summary>
        /// <param name="heroName">Can be the real hero name or alt name</param>
        /// <returns></returns>
        Hero GetHeroInfo(string heroName);

        /// <summary>
        /// Gets the english name of the given alias. Returns true if found, otherwise false
        /// </summary>
        /// <param name="heroNameAlias">Alias name</param>
        /// <param name="heroNameEnglish">English name</param>
        /// <returns></returns>
        bool HeroNameTranslation(string heroNameAlias, out string heroNameEnglish);

        /// <summary>
        /// Returns the real hero name from the hero's attribute id
        /// </summary>
        /// <param name="attributeId">Four character hero id</param>
        /// <returns>Full hero name</returns>
        string GetRealHeroNameFromAttributeId(string attributeId);

        /// <summary>
        /// Returns the real hero name from the alt name
        /// </summary>
        /// <param name="altName">Alt name of hero</param>
        /// <returns></returns>
        string GetRealHeroNameFromAltName(string altName);

        /// <summary>
        /// Checks to see if the hero name exists
        /// </summary>
        /// <param name="heroName">Real name of hero or alt name</param>
        /// <returns>True if found</returns>
        bool HeroExists(string heroName);

        /// <summary>
        /// Returns a list of (real) hero names for the given build
        /// </summary>
        /// <param name="build">The build number</param>
        /// <returns></returns>
        List<string> GetListOfHeroes(int build);

        /// <summary>
        /// Returns the total amount of heroes (latest build)
        /// </summary>
        /// <returns></returns>
        int TotalAmountOfHeroes();
    }
}
