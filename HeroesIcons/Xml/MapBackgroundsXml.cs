using System;
using System.Collections.Generic;
using System.Windows.Media;
using System.Xml;

namespace HeroesIcons.Xml
{
    internal class MapBackgroundsXml : XmlBase
    {
        public Dictionary<string, Uri> MapBackgrounds { get; private set; } = new Dictionary<string, Uri>();
        public Dictionary<string, Color> MapBackgroundFontGlowColor { get; private set; } = new Dictionary<string, Color>();
        public Dictionary<string, Uri> MapBackgroundsSmall { get; private set; } = new Dictionary<string, Uri>();
        public Dictionary<string, string> MapAlternativeName { get; private set; } = new Dictionary<string, string>();
        public Dictionary<string, string> MapTranslationsNames { get; private set; } = new Dictionary<string, string>();
        public List<string> CustomOnlyMaps { get; private set; } = new List<string>();

        private MapBackgroundsXml(string parentFile, string xmlFolder)
        {
            XmlParentFile = parentFile;
            XmlFolder = xmlFolder;
        }

        public static MapBackgroundsXml Initialize(string parentFile, string xmlFolder)
        {
            MapBackgroundsXml xml = new MapBackgroundsXml(parentFile, xmlFolder);
            xml.Parse();
            xml.LoadTranslationMapNames();
            return xml;
        }

        protected override void ParseChildFiles()
        {
            try
            {
                foreach (var mapBackground in XmlChildFiles)
                {
                    using (XmlReader reader = XmlReader.Create($@"Xml\{XmlFolder}/{mapBackground}.xml"))
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

                        MapAlternativeName.Add(mapBackground, realMapBackgroundName);

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
            return new Uri($@"{ApplicationPath}MapBackgrounds\{fileName}", UriKind.Absolute);
        }

        private void LoadTranslationMapNames()
        {
            using (XmlReader reader = XmlReader.Create($@"Xml\MapBackgrounds\MapTranslations.xml"))
            {
                reader.MoveToContent();

                while (reader.Read())
                {
                    if (reader.IsStartElement())
                    {
                        string mapName = reader.Name;

                        if (reader.Read())
                        {
                            string languageNames = reader.Value;

                            var diffNames = languageNames.Split(',');
                            foreach (var name in diffNames)
                            {
                                if (!MapTranslationsNames.ContainsKey(name) && MapAlternativeName.ContainsKey(mapName))
                                    MapTranslationsNames.Add(name, MapAlternativeName[mapName]);
                            }
                        }
                    }
                }
            }
        }
    }
}
