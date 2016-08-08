using System;
using System.Collections.Generic;
using System.IO;
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

            // no pick
            if (string.IsNullOrEmpty(nameOfHeroTalent))
                return new BitmapImage(new Uri("pack://application:,,,/HeroesIcons;component/Icons/Talents/_Generic/storm_ui_icon_no_pick.dds", UriKind.Absolute));


            // not found
            if (!Talents.TryGetValue(nameOfHeroTalent, out talent))
                return new BitmapImage(new Uri("pack://application:,,,/HeroesIcons;component/Icons/Talents/_Generic/storm_ui_icon_default.dds", UriKind.Absolute));


            return new BitmapImage(talent.Item2);
        }

        public string GetTrueTalentName(string nameOfHeroTalent)
        {
            Tuple<string, Uri> talent;

            // no pick
            if (string.IsNullOrEmpty(nameOfHeroTalent))
                return "No pick";

            // not found
            if (!Talents.TryGetValue(nameOfHeroTalent, out talent))
                return nameOfHeroTalent;

            return talent.Item1;
        }

        private Uri SetHeroTalentUri(string hero, string fileName, bool isGenericTalent)
        {
            if (!isGenericTalent)
                return new Uri($"pack://application:,,,/HeroesIcons;component/Icons/Talents/{hero}/{fileName}", UriKind.Absolute);
            else
                return new Uri($"pack://application:,,,/HeroesIcons;component/Icons/Talents/_Generic/{fileName}", UriKind.Absolute);
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
                using (XmlReader reader = XmlReader.Create($@"Heroes/{hero}.xml"))
                {
                    reader.ReadStartElement(hero);

                    while (reader.Read())
                    {
                        if (reader.IsStartElement())
                        {
                            string element = reader.Name;
                            if (element == "Level1" || element == "Level4" || element == "Level7" ||
                                element == "Level10" || element == "Level13" || element == "Level16" ||
                                element == "Level20")
                            {
                                while (reader.Read() && reader.Name != element)
                                {
                                    if (reader.NodeType == XmlNodeType.Element)
                                    {
                                        string name = reader.Name; // raw name
                                        string realName = reader["name"]; // real name
                                        if (realName == null)
                                            realName = string.Empty;

                                        if (reader.Read())
                                        {
                                            bool isGeneric = false;
                                            if (name.StartsWith("Generic") || name.StartsWith("HeroGeneric") || name.StartsWith("BattleMomentum"))
                                                isGeneric = true;

                                            if (!Talents.ContainsKey(name))
                                                Talents.Add(name, new Tuple<string, Uri>(realName, SetHeroTalentUri(hero, reader.Value, isGeneric)));
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
    }
}
