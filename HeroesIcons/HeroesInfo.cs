using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Media.Imaging;
using System.Xml;
using System.Xml.Linq;

namespace HeroesIcons
{
    public class HeroesInfo
    {
        /// <summary>
        /// key is reference name of talent
        /// Tuple: key is real name of talent
        /// </summary>
        private Dictionary<string, Tuple<string, Uri>> Talents = new Dictionary<string, Tuple<string, Uri>>();
        /// <summary>
        /// key is attributeid
        /// </summary>
        private Dictionary<string, Uri> HeroPortraits = new Dictionary<string, Uri>();
        /// <summary>
        /// key is attributeid, value is hero name
        /// </summary>
        private Dictionary<string, string> HeroNamesFromAttId = new Dictionary<string, string>();

        public HeroesInfo()
        {
            ParseXmlHeroFiles();
        }

        /// <summary>
        /// Returns a BitmapImage of the talent
        /// </summary>
        /// <param name="nameOfHeroTalent">Reference talent name</param>
        /// <returns>BitmapImage of the talent</returns>
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

        /// <summary>
        /// Returns a BitmapImage of the hero
        /// </summary>
        /// <param name="attributeId">attributeid</param>
        /// <returns>BitmpImage of the hero</returns>
        public BitmapImage GetHeroPortrait(string heroName)
        {
            Uri uri;

            // no pick
            if (string.IsNullOrEmpty(heroName))
                return new BitmapImage(new Uri($"pack://application:,,,/HeroesIcons;component/Icons/HeroPortraits/storm_ui_glues_draft_portrait_nopick.dds", UriKind.Absolute));

            // not found
            if (!HeroPortraits.TryGetValue(heroName, out uri))
                return new BitmapImage(new Uri($"pack://application:,,,/HeroesIcons;component/Icons/HeroPortraits/storm_ui_glues_draft_portrait_notfound.dds", UriKind.Absolute));

            return new BitmapImage(uri);
        }

        /// <summary>
        /// Returns the talent name from the talent reference name
        /// </summary>
        /// <param name="nameOfHeroTalent">Reference talent name</param>
        /// <returns>Talent name</returns>
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

        /// <summary>
        /// Returns the real hero name from the hero's attribute id
        /// </summary>
        /// <param name="attributeId">Four character hero id</param>
        /// <returns>Full hero name</returns>
        public string GeRealHeroNameFromAttId(string attributeId)
        {
            string heroName;

            // no pick
            if (string.IsNullOrEmpty(attributeId))
                return null;

            // not found
            if (!HeroNamesFromAttId.TryGetValue(attributeId, out heroName))
                return "Hero not found";

            return heroName;
        }

        private Uri SetHeroTalentUri(string hero, string fileName, bool isGenericTalent)
        {
            if (!isGenericTalent)
                return new Uri($"pack://application:,,,/HeroesIcons;component/Icons/Talents/{hero}/{fileName}", UriKind.Absolute);
            else
                return new Uri($"pack://application:,,,/HeroesIcons;component/Icons/Talents/_Generic/{fileName}", UriKind.Absolute);
        }

        private Uri SetHeroPortraitUri(string fileName)
        {
            return new Uri($"pack://application:,,,/HeroesIcons;component/Icons/HeroPortraits/{fileName}", UriKind.Absolute);
        }

        private void ParseXmlHeroFiles()
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
                    reader.MoveToContent();

                    if (reader.Name != hero)
                        continue;

                    // get real name
                    string realHeroName = reader["name"];

                    // add attributeid from hero name
                    string attributeId = reader["attributeid"];
                    if (!string.IsNullOrEmpty(attributeId))
                    {
                        if (!string.IsNullOrEmpty(realHeroName))
                            HeroNamesFromAttId.Add(attributeId, realHeroName);
                        else
                            HeroNamesFromAttId.Add(attributeId, hero);


                        // add portrait
                        string portraitName = reader["portrait"];
                        if (!string.IsNullOrEmpty(portraitName))
                            HeroPortraits.Add(attributeId, SetHeroPortraitUri(portraitName));
                    }

                    // add talents
                    while (reader.Read())
                    {
                        if (reader.IsStartElement())
                        {
                            string element = reader.Name;
                            if (element == "Level1" || element == "Level4" || element == "Level7" ||
                                element == "Level10" || element == "Level13" || element == "Level16" ||
                                element == "Level20" || element == "Old")
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
