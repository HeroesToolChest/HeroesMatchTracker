using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Xml;

namespace Heroes.Icons.Xml
{
    internal class HomeScreensXml : XmlBase, IHomeScreens
    {
        private readonly string IconFolderName = "Homescreens";

        private HomeScreensXml(string parentFile, string xmlBaseFolder, int currentBuild, bool logger)
            : base(currentBuild, logger)
        {
            XmlParentFile = parentFile;
            XmlBaseFolder = xmlBaseFolder;
            XmlFolder = xmlBaseFolder;
        }

        public List<Tuple<Uri, Color>> HomeScreenBackgrounds { get; private set; } = new List<Tuple<Uri, Color>>();

        public static HomeScreensXml Initialize(string parentFile, string xmlFolder, int currentBuild, bool logger)
        {
            HomeScreensXml xml = new HomeScreensXml(parentFile, xmlFolder, currentBuild, logger);
            xml.Parse();
            return xml;
        }

        public List<Tuple<Uri, Color>> GetListOfHomeScreens()
        {
            return HomeScreenBackgrounds;
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
                            string fontGlow = reader["fontglow"];

                            if (reader.Read())
                            {
                                HomeScreenBackgrounds.Add(new Tuple<Uri, Color>(GetImageUri(IconFolderName, reader.Value), ConvertHexToColor(fontGlow)));
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
