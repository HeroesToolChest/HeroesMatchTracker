using System;
using System.Collections.Generic;
using System.Windows.Media;
using System.Xml;

namespace Heroes.Icons.Xml
{
    internal class MapBackgroundsXml : XmlBase
    {
        private MapBackgroundsXml(string parentFile, string xmlBaseFolder)
        {
            XmlParentFile = parentFile;
            XmlBaseFolder = xmlBaseFolder;
            XmlFolder = xmlBaseFolder;
        }

        public Dictionary<string, Uri> MapUriByMapRealName { get; private set; } = new Dictionary<string, Uri>();
        public Dictionary<string, Color> MapFontGlowColorByMapRealName { get; private set; } = new Dictionary<string, Color>();
        public Dictionary<string, Uri> MapSmallUriByMapRealName { get; private set; } = new Dictionary<string, Uri>();
        public Dictionary<string, string> MapRealNameByMapAliasName { get; private set; } = new Dictionary<string, string>();
        public List<string> CustomOnlyMaps { get; private set; } = new List<string>();

        public static MapBackgroundsXml Initialize(string parentFile, string xmlBaseFolder)
        {
            MapBackgroundsXml xml = new MapBackgroundsXml(parentFile, xmlBaseFolder);
            xml.Parse();
            return xml;
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
