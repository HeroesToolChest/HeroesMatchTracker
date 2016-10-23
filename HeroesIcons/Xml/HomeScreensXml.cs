using System;
using System.Collections.Generic;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Xml;

namespace HeroesIcons.Xml
{
    internal class HomeScreensXml : XmlBase
    {
        public List<Tuple<BitmapImage, Color>> HomeScreenBackgrounds { get; private set; } = new List<Tuple<BitmapImage, Color>>();

        private HomeScreensXml(string parentFile, string xmlFolder)
        {
            XmlParentFile = parentFile;
            XmlFolder = xmlFolder;
        }

        public static HomeScreensXml Initialize(string parentFile, string xmlFolder)
        {
            HomeScreensXml xml = new HomeScreensXml(parentFile, xmlFolder);
            xml.Parse();
            return xml;
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

                using (XmlReader reader = XmlReader.Create($@"Xml/{XmlFolder}/{XmlParentFile}"))
                {
                    reader.MoveToContent();
                    if (reader.Name != XmlFolder)
                        return;

                    while (reader.Read())
                    {
                        if (reader.IsStartElement())
                        {
                            string fontGlow = reader["fontglow"];

                            if (reader.Read())
                            {
                                HomeScreenBackgrounds.Add(new Tuple<BitmapImage, Color>(SetHomeScreenBitmapImage(reader.Value), (Color)ColorConverter.ConvertFromString(fontGlow)));
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

        private BitmapImage SetHomeScreenBitmapImage(string fileName)
        {
            return new BitmapImage(new Uri($"{ApplicationPath}Homescreens/{fileName}", UriKind.Absolute));
        }
    }
}
