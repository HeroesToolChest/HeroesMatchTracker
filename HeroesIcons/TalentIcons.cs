using System;
using System.Collections.Generic;
using System.Windows.Media.Imaging;
using System.Xml;
using System.Xml.Linq;

namespace HeroesIcons
{
    public class TalentIcons
    {
        private Dictionary<string, Uri> Talents = new Dictionary<string, Uri>();

        public TalentIcons()
        {
            SetTalentNamesIcons();
        }

        public BitmapImage GetTalentIcon(string nameOfTalent)
        {
            Uri uri;
            if (string.IsNullOrEmpty(nameOfTalent))
                return null;

            if (!Talents.TryGetValue(nameOfTalent, out uri))
                uri = Talents["IconDefault"];

            return new BitmapImage(uri);
        }

        private Uri SetHeroTalentUri(string hero, string fileName)
        {
            return new Uri($@"/HeroesIcons;component//Icons/Talents/{hero}/{fileName}", UriKind.Relative);
        }

        private void SetTalentNamesIcons()
        {
            List<string> heroes = new List<string>();
            using (XmlTextReader reader = new XmlTextReader(@"Heroes/_AllHeroes.xml"))
            {
                reader.ReadStartElement("Heroes");

                while (reader.Read() && reader.NodeType != XmlNodeType.EndElement)
                {
                    if (reader.NodeType == XmlNodeType.Comment || reader.NodeType == XmlNodeType.Text || reader.NodeType == XmlNodeType.Whitespace)
                        continue;

                    XElement el = (XElement)XNode.ReadFrom(reader);
                    heroes.Add(el.Name.ToString());
                }
            }

            foreach (var hero in heroes)
            {
                using (XmlTextReader reader = new XmlTextReader($@"Heroes/{hero}.xml"))
                {
                    reader.ReadStartElement(hero);

                    while(reader.Read() && reader.NodeType != XmlNodeType.EndElement)
                    {
                        if (reader.NodeType == XmlNodeType.Comment || reader.NodeType == XmlNodeType.Text || reader.NodeType == XmlNodeType.Whitespace)
                            continue;

                        XElement el = (XElement)XNode.ReadFrom(reader);
                        Talents.Add(el.Name.ToString(), SetHeroTalentUri(hero, el.Value));
                    }
                }
            }
        }
    }
}
