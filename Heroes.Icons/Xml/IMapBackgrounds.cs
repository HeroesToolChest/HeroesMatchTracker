using System.Collections.Generic;
using System.Drawing;
using System.IO;

namespace Heroes.Icons.Xml
{
    public interface IMapBackgrounds
    {
        Stream GetMapBackground(string mapRealName);

        /// <summary>
        /// Returns the color associated with the map, returns black if map not found
        /// </summary>
        /// <param name="mapRealName">Real map name</param>
        /// <returns></returns>
        Color GetMapBackgroundFontGlowColor(string mapRealName);

        /// <summary>
        /// Returns a list of all maps (real names)
        /// </summary>
        /// <returns></returns>
        List<string> GetMapsList();

        /// <summary>
        /// Returns a list of all maps, except custom only maps
        /// </summary>
        /// <returns></returns>
        List<string> GetMapsListExceptCustomOnly();

        /// <summary>
        /// Returns a list of custom only maps
        /// </summary>
        /// <returns></returns>
        List<string> GetCustomOnlyMapsList();

        /// <summary>
        /// Returns the map name from the map alternative name
        /// </summary>
        /// <param name="mapAlternativeName">map's alternative name</param>
        /// <returns></returns>
        string GetMapNameByMapAlternativeName(string mapAlternativeName);

        /// <summary>
        /// Gets the english name of the given alias. Returns true if found, otherwise false
        /// </summary>
        /// <param name="mapNameAlias">Alias name</param>
        /// <param name="mapNameEnglish">English name</param>
        /// <returns></returns>
        bool MapNameTranslation(string mapNameAlias, out string mapNameEnglish);

        int TotalCountOfMaps();
    }
}
