using System;
using System.Collections.Generic;
using System.Windows.Media;
using System.Xml;

namespace HeroesIcons.Xml
{
    internal class MapBackgroundsXml : XmlBase
    {
        public Dictionary<string, Uri> MapBackgrounds { get; set; } = new Dictionary<string, Uri>();
        public Dictionary<string, Color> MapBackgroundFontGlowColor { get; set; } = new Dictionary<string, Color>();
        public Dictionary<string, Uri> MapBackgroundsSmall { get; set; } = new Dictionary<string, Uri>();
        public List<string> CustomOnlyMaps { get; set; } = new List<string>();

        private MapBackgroundsXml(string parentFile, string xmlFolder)
        {
            XmlParentFile = parentFile;
            XmlFolder = xmlFolder;
        }

        public static MapBackgroundsXml Initialize(string parentFile, string xmlFolder)
        {
            MapBackgroundsXml xml = new MapBackgroundsXml(parentFile, xmlFolder);
            xml.Parse();
            return xml;
        }

        protected override void ParseChildFiles()
        {
            try
            {
                foreach (var mapBackground in XmlChildFiles)
                {
                    using (XmlReader reader = XmlReader.Create($@"Xml/{XmlFolder}/{mapBackground}.xml"))
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
                                string mapSize = reader.Name;
                                string fontGlow = reader["fontglow"];

                                if (reader.Read())
                                {
                                    if (isCustomOnly)
                                        CustomOnlyMaps.Add(realMapBackgroundName);

                                    if (mapSize == "Normal")
                                    {
                                        MapBackgrounds.Add(realMapBackgroundName, SetMapBackgroundUri(reader.Value));
                                        MapBackgroundFontGlowColor.Add(realMapBackgroundName, (Color)ColorConverter.ConvertFromString(fontGlow));
                                    }
                                    else if (mapSize == "Small")
                                        MapBackgroundsSmall.Add(realMapBackgroundName, SetMapBackgroundUri(reader.Value));
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
            return new Uri($"{ApplicationPath}MapBackgrounds/{fileName}", UriKind.Absolute);
        }
    }
}
