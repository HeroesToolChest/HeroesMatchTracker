using System;
using System.Collections.Generic;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Xml;

namespace Heroes.Icons.Xml
{
    internal class MapBackgroundsXml : XmlBase, IMapBackgrounds
    {
        private Dictionary<string, Uri> MapUriByMapRealName = new Dictionary<string, Uri>();
        private Dictionary<string, Color> MapFontGlowColorByMapRealName = new Dictionary<string, Color>();
        private Dictionary<string, Uri> MapSmallUriByMapRealName = new Dictionary<string, Uri>();
        private Dictionary<string, string> MapRealNameByMapAliasName = new Dictionary<string, string>();
        private List<string> CustomOnlyMaps = new List<string>();

        private MapBackgroundsXml(string parentFile, string xmlBaseFolder, int currentBuild)
            : base(currentBuild)
        {
            XmlParentFile = parentFile;
            XmlBaseFolder = xmlBaseFolder;
            XmlFolder = xmlBaseFolder;
        }

        public static MapBackgroundsXml Initialize(string parentFile, string xmlBaseFolder, int currentBuild)
        {
            MapBackgroundsXml xml = new MapBackgroundsXml(parentFile, xmlBaseFolder, currentBuild);
            xml.Parse();
            return xml;
        }

        public BitmapImage GetMapBackground(string mapRealName, bool useSmallImage = false)
        {
            try
            {
                if (useSmallImage == false)
                {
                    BitmapImage image = new BitmapImage(MapUriByMapRealName[mapRealName]);
                    image.Freeze();
                    return image;
                }
                else
                {
                    BitmapImage image = new BitmapImage(MapSmallUriByMapRealName[mapRealName]);
                    image.Freeze();
                    return image;
                }
            }
            catch (Exception)
            {
                LogReferenceNameNotFound($"Map background: {mapRealName}");
                return null;
            }
        }

        /// <summary>
        /// Returns the color associated with the map, returns black if map not found
        /// </summary>
        /// <param name="mapRealName">Real map name</param>
        /// <returns></returns>
        public Color GetMapBackgroundFontGlowColor(string mapRealName)
        {
            Color color;
            if (MapFontGlowColorByMapRealName.TryGetValue(mapRealName, out color))
                return color;
            else
                return Colors.Black;
        }

        /// <summary>
        /// Returns a list of all maps
        /// </summary>
        /// <returns></returns>
        public List<string> GetMapsList()
        {
            return new List<string>(MapUriByMapRealName.Keys);
        }

        /// <summary>
        /// Returns a list of all maps, except custom only maps
        /// </summary>
        /// <returns></returns>
        public List<string> GetMapsListExceptCustomOnly()
        {
            var allMaps = new Dictionary<string, Uri>(MapUriByMapRealName);
            foreach (var customMap in GetCustomOnlyMapsList())
            {
                if (allMaps.ContainsKey(customMap))
                {
                    allMaps.Remove(customMap);
                }
            }

            return new List<string>(allMaps.Keys);
        }

        /// <summary>
        /// Returns a list of custom only maps
        /// </summary>
        /// <returns></returns>
        public List<string> GetCustomOnlyMapsList()
        {
            return CustomOnlyMaps;
        }

        /// <summary>
        /// Returns true if mapName is a valid name
        /// </summary>
        /// <param name="mapName">The map name that needs to be checked</param>
        /// <returns></returns>
        public bool IsValidMapName(string mapName)
        {
            return MapUriByMapRealName.ContainsKey(mapName);
        }

        /// <summary>
        /// Gets the english name of the given alias. Returns true if found, otherwise false
        /// </summary>
        /// <param name="mapNameAlias">Alias name</param>
        /// <param name="mapNameEnglish">English name</param>
        /// <returns></returns>
        public bool MapNameTranslation(string mapNameAlias, out string mapNameEnglish)
        {
            return MapRealNameByMapAliasName.TryGetValue(mapNameAlias, out mapNameEnglish);
        }

        protected override void ParseChildFiles()
        {
            try
            {
                foreach (var mapBackground in XmlChildFiles)
                {
                    using (XmlReader reader = XmlReader.Create($@"Xml\{XmlBaseFolder}\{mapBackground}.xml"))
                    {
                        reader.MoveToContent();

                        if (reader.Name != mapBackground)
                            continue;

                        // get the real map background name
                        // example BattlefieldofEternity -> (real) Battlefield of Eternity
                        string realMapBackgroundName = reader["name"];
                        if (string.IsNullOrEmpty(realMapBackgroundName))
                            realMapBackgroundName = mapBackground; // default

                        string custom = reader["custom"] == null ? "false" : reader["custom"];

                        bool isCustomOnly;
                        if (!bool.TryParse(custom, out isCustomOnly))
                            isCustomOnly = false;

                        while (reader.Read())
                        {
                            if (reader.IsStartElement())
                            {
                                string element = reader.Name;
                                string fontGlow = reader["fontglow"];

                                if (reader.Read())
                                {
                                    if (isCustomOnly)
                                        CustomOnlyMaps.Add(realMapBackgroundName);

                                    if (element == "Normal")
                                    {
                                        MapUriByMapRealName.Add(realMapBackgroundName, SetMapBackgroundUri(reader.Value));
                                        MapFontGlowColorByMapRealName.Add(realMapBackgroundName, (Color)ColorConverter.ConvertFromString(fontGlow));
                                    }
                                    else if (element == "Small")
                                    {
                                        MapSmallUriByMapRealName.Add(realMapBackgroundName, SetMapBackgroundUri(reader.Value));
                                    }
                                    else if (element == "Aliases")
                                    {
                                        string[] aliases = reader.Value.Split(',');

                                        // add the english name
                                        MapRealNameByMapAliasName.Add(realMapBackgroundName, realMapBackgroundName);

                                        // add all the other aliases
                                        foreach (var alias in aliases)
                                        {
                                            if (MapRealNameByMapAliasName.ContainsKey(alias))
                                                throw new ArgumentException($"Alias already added to {realMapBackgroundName}: {alias}");

                                            MapRealNameByMapAliasName.Add(alias, realMapBackgroundName);
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new ParseXmlException("Error on parsing of xml files", ex);
            }
        }

        private Uri SetMapBackgroundUri(string fileName)
        {
            return new Uri($@"{ApplicationIconsPath}\MapBackgrounds\{fileName}", UriKind.Absolute);
        }
    }
}
