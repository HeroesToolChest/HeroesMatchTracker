using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Xml;

namespace Heroes.Icons.Xml
{
    internal class MapBackgroundsXml : XmlBase, IMapBackgrounds
    {
        private readonly string IconFolderName = "MapBackgrounds";

        private Dictionary<string, string> MapStringByMapRealName = new Dictionary<string, string>();
        private Dictionary<string, Color> MapFontGlowColorByMapRealName = new Dictionary<string, Color>();
        private Dictionary<string, string> MapRealNameByMapAlternativeName = new Dictionary<string, string>();
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

        public Stream GetMapBackground(string mapRealName)
        {
            try
            {
                if (MapStringByMapRealName.ContainsKey(mapRealName))
                {
                    return Assembly.GetExecutingAssembly().GetManifestResourceStream(MapStringByMapRealName[mapRealName]);
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
            return new List<string>(MapStringByMapRealName.Keys);
        }

        /// <summary>
        /// Returns a list of all maps, except custom only maps
        /// </summary>
        /// <returns></returns>
        public List<string> GetMapsListExceptCustomOnly()
        {
            var allMaps = new Dictionary<string, string>(MapStringByMapRealName);
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
            return MapStringByMapRealName.ContainsKey(mapName);
        }

        /// <summary>
        /// Returns the map name from the map alternative name
        /// </summary>
        /// <param name="mapAlternativeName">map's alternative name</param>
        /// <returns></returns>
        public string GetMapNameByMapAlternativeName(string mapAlternativeName)
        {
            // no pick
            if (string.IsNullOrEmpty(mapAlternativeName))
                return string.Empty;

            if (MapRealNameByMapAlternativeName.TryGetValue(mapAlternativeName, out string mapName))
                return mapName;
            else
                return null;
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

                        string alternativeName = reader["alt"];
                        string custom = reader["custom"] ?? "false";

                        if (!bool.TryParse(custom, out bool isCustomOnly))
                            isCustomOnly = false;

                        MapRealNameByMapAlternativeName.Add(alternativeName, realMapBackgroundName);

                        reader.Read();
                        string element = reader.Name;
                        string fontGlow = reader["fontglow"];

                        if (reader.Read())
                        {
                            if (isCustomOnly)
                                CustomOnlyMaps.Add(realMapBackgroundName);

                            if (element == "Normal")
                            {
                                MapStringByMapRealName.Add(realMapBackgroundName, SetImageStreamString(IconFolderName, reader.Value));
                                MapFontGlowColorByMapRealName.Add(realMapBackgroundName, ConvertHexToColor(fontGlow));
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
