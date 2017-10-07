using Heroes.Icons.Models;
using System;
using System.Collections.Generic;

namespace Heroes.Icons.Xml
{
    public interface IHeroBuilds
    {
        /// <summary>
        /// Gets the hero name associated with the given talent. Returns true is found, otherwise returns false
        /// </summary>
        /// <param name="tier">The tier that the talent resides in</param>
        /// <param name="talentName">The talent reference name</param>
        /// <param name="heroRealName">The hero name</param>
        /// <returns></returns>
        bool GetHeroNameFromTalentReferenceName(TalentTier tier, string talentName, out string heroRealName);

        /// <summary>
        /// Get the patch notes link from the given build number. Returns null if not found
        /// </summary>
        /// <param name="build">The build number</param>
        /// <returns></returns>
        Tuple<string, string> GetPatchNotes(int build);

        /// <summary>
        /// Returns a dictionary of all the talents of the given hero in talent tiers
        /// </summary>
        /// <param name="heroName">real hero name</param>
        /// <returns></returns>
        Dictionary<string, Talent> GetHeroTalents(string heroName);

        /// <summary>
        /// Returns a dictionary of all the talents of the given tier
        /// </summary>
        /// <param name="heroName">real hero name</param>
        /// <param name="tier">the talent tier</param>
        /// <returns></returns>
        Dictionary<string, Talent> GetHeroTalentsInTier(string heroName, TalentTier tier);

        /// <summary>
        /// Returns a Talent object from the hero name, tier, and reference name of talent
        /// </summary>
        /// <param name="heroName">real hero name</param>
        /// <param name="tier">The tier that the talent exists in</param>
        /// <param name="talentReferenceName">reference name of talent</param>
        /// <returns></returns>
        Talent GetHeroTalent(string heroName, TalentTier tier, string talentReferenceName);
    }
}
