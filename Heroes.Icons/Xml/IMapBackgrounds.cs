using System.Collections.Generic;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Heroes.Icons.Xml
{
    public interface IMapBackgrounds
    {
        BitmapImage GetMapBackground(string mapRealName, bool useSmallImage = false);
        Color GetMapBackgroundFontGlowColor(string mapRealName);
        List<string> GetMapsList();
        List<string> GetMapsListExceptCustomOnly();
        List<string> GetCustomOnlyMapsList();
        bool IsValidMapName(string mapName);
        bool MapNameTranslation(string mapNameAlias, out string mapNameEnglish);
    }
}
