using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Xml;

namespace Heroes.Icons.Xml
{
    internal class MapBackgroundsXml : XmlBase, IMapBackgrounds
    {
        private readonly string IconFolderName = "MapBackgrounds";

        private Dictionary<string, Uri> MapUriByMapRealName = new Dictionary<string, Uri>();
        private Dictionary<string, Color> MapFontGlowColorByMapRealName = new Dictionary<string, Color>();
        private Dictionary<string, string> MapRealNameByMapAliasName = new Dictionary<string, string>();
        private List<string> CustomOnlyMaps = new List<string>();

        private MapBackgroundsXml(string parentFile, string xmlBaseFolder, int currentBuild, bool logger)
            : base(currentBuild, logger)
        {
            XmlParentFile = parentFile;
            XmlBaseFolder = xmlBaseFolder;
            XmlFolder = xmlBaseFolder;
        }

        public static MapBackgroundsXml Initialize(string parentFile, string xmlBaseFolder, int currentBuild, bool logger)
        {
            MapBackgroundsXml xml = new MapBackgroundsXml(parentFile, xmlBaseFolder, currentBuild, logger);
            xml.Parse();
            return xml;
        }

        public Uri GetMapBackground(string mapRealName)
        {
            try
            {
                if (MapUriByMapRealName.ContainsKey(mapRealName))
                {
                    return MapUriByMapRealName[mapRealName];
                }
                else
                {
                    LogReferenceNameNotFound($"Map background: {mapRealName}");
                    return null;
                }
            }
            catch (IOException)
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
            if (MapFontGlowColorByMapRealName.TryGetValue(mapRealName, out Color color))
                return color;
            else
                return Color.Black;
        }

        /// <summary>
        /// Returns a list of all maps (real names)
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

        public int TotalCountOfMaps()
        {
            return XmlChildFiles.Count;
        }

        protected override void ParseChildFiles()
        {
            try
            {
                foreach (var mapBackground in XmlChildFiles)
                {
                    using (XmlReader reader = XmlReader.Create(Path.Combine(XmlMainFolderName, XmlBaseFolder, $"{mapBackground}{DefaultFileExtension}"), GetXmlReaderSettings()))
                    {
                        reader.MoveToContent();

                        if (reader.Name != mapBackground)
                            continue;

                        // get the real map background name
                        // example BattlefieldofEternity -> (real) Battlefield of Eternity
                        string realMapBackgroundName = reader["name"];
                        if (string.IsNullOrEmpty(realMapBackgroundName))
                            realMapBackgroundName = mapBackground; // default

                        string custom = reader["custom"] ?? "false";

                        if (!bool.TryParse(custom, out bool isCustomOnly))
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
                                        MapUriByMapRealName.Add(realMapBackgroundName, GetImageUri(IconFolderName, reader.Value));
                                        MapFontGlowColorByMapRealName.Add(realMapBackgroundName, ConvertHexToColor(fontGlow));
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
                throw new ParseXmlException($"Error on xml parsing on {XmlParentFile}", ex);
            }
        }
    }
}
