using System.Collections.Generic;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Heroes.Icons.Xml
{
    public interface IMapBackgrounds
    {
        BitmapImage GetMapBackground(string mapRealName);

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
        /// Returns true if mapName is a valid name
        /// </summary>
        /// <param name="mapName">The map name that needs to be checked</param>
        /// <returns></returns>
        bool IsValidMapName(string mapName);

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
