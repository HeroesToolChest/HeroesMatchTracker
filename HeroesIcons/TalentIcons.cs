using System;
using System.Collections.Generic;
using System.Windows.Media.Imaging;
using System.Xml;
using System.Xml.Linq;

namespace HeroesIcons
{
    public class TalentIcons
    {
        private Dictionary<string, Tuple<string, Uri>> Talents = new Dictionary<string, Tuple<string, Uri>>();

        public TalentIcons()
        {
            SetTalentNamesIcons();
        }

        public BitmapImage GetTalentIcon(string nameOfHeroTalent)
        {
            Tuple<string, Uri> talent;

            if (string.IsNullOrEmpty(nameOfHeroTalent))
                return null;

            if (!Talents.TryGetValue(nameOfHeroTalent, out talent))
                talent = Talents["IconDefault"];

            return new BitmapImage(talent.Item2);
        }

        public string GetTrueTalentName(string nameOfHeroTalent)
        {
            Tuple<string, Uri> talent;

            if (string.IsNullOrEmpty(nameOfHeroTalent))
                return null;

            if (!Talents.TryGetValue(nameOfHeroTalent, out talent))
                return nameOfHeroTalent;

            return talent.Item1;
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
                        string talentName;
                        var attributeName = el.Attribute("name");

                        if (attributeName == null)
                            talentName = el.Name.ToString();
                        else
                            talentName = attributeName.Value;

                        Talents.Add(el.Name.ToString(), new Tuple<string, Uri>(talentName, SetHeroTalentUri(hero, el.Value)));
                    }
                }
            }
        }
    }
}
