using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Xml;

namespace Heroes.Icons.Xml
{
    internal class HomeScreensXml : XmlBase, IHomeScreens
    {
        private readonly string IconFolderName = "Homescreens";

        private Dictionary<string, string> HomescreenStringByMapName = new Dictionary<string, string>();
        private Dictionary<string, Color> HomescreenFontGlowColorByMapName = new Dictionary<string, Color>();

        private HomeScreensXml(string parentFile, string xmlBaseFolder, int currentBuild, bool logger)
            : base(currentBuild, logger)
        {
            XmlParentFile = parentFile;
            XmlBaseFolder = xmlBaseFolder;
            XmlFolder = xmlBaseFolder;
        }

        public static HomeScreensXml Initialize(string parentFile, string xmlFolder, int currentBuild, bool logger)
        {
            HomeScreensXml xml = new HomeScreensXml(parentFile, xmlFolder, currentBuild, logger);
            xml.Parse();
            return xml;
        }

        public Stream GetHomescreen(string homescreenName)
        {
            try
            {
                if (HomescreenStringByMapName.ContainsKey(homescreenName))
                {
                    return Assembly.GetExecutingAssembly().GetManifestResourceStream(HomescreenStringByMapName[homescreenName]);
                }
                else
                {
                    LogReferenceNameNotFound($"Homescreen: {homescreenName}");
                    return null;
                }
            }
            catch (IOException)
            {
                LogReferenceNameNotFound($"Homescreen: {homescreenName}");
                return null;
            }
        }

        public Color GetHomescreenFontGlowColor(string homescreenName)
        {
            if (HomescreenFontGlowColorByMapName.TryGetValue(homescreenName, out Color color))
                return color;
            else
                return Color.Black;
        }

        public List<string> GetHomescreensList()
        {
            return new List<string>(HomescreenStringByMapName.Keys);
        }

        protected override void Parse()
        {
            ParseChildFiles();
        }

        protected override void ParseChildFiles()
        {
            try
            {
                if (!ValidateRequiredFiles())
                    return;

                using (XmlReader reader = XmlReader.Create(Path.Combine(XmlMainFolderName, XmlBaseFolder, XmlParentFile), GetXmlReaderSettings()))
                {
                    reader.MoveToContent();
                    if (reader.Name != XmlBaseFolder)
                        return;

                    while (reader.Read())
                    {
                        if (reader.IsStartElement())
                        {
                            string homescreenName = reader.Name;
                            string fontGlow = reader["fontglow"];

                            if (reader.Read())
                            {
                                HomescreenStringByMapName.Add(homescreenName, SetImageStreamString(IconFolderName, reader.Value));
                                HomescreenFontGlowColorByMapName.Add(homescreenName, ConvertHexToColor(fontGlow));
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new ParseXmlException($"Error on xml parsing of {XmlParentFile}", ex);
            }
        }
    }
}
