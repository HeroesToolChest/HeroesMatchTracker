using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using System.Xml;
using System.Xml.Linq;

namespace HeroesIcons
{
    public class HeroesInfo
    {
        /// <summary>
        /// key is hero name, value alt name
        /// </summary>
        private Dictionary<string, string> Heroes = new Dictionary<string, string>();
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
        /// <summary>
        /// key is attributeid
        /// </summary>
        private Dictionary<string, Uri> LeaderboardPortraits = new Dictionary<string, Uri>();

        public HeroesInfo()
        {
            ParseXmlHeroFiles();
        }

        public static HeroesInfo Initialize()
        {
            return new HeroesInfo();
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
            {
                Task.Run(() => Log("_ImageMissingLog.txt", $"Talent icon: {nameOfHeroTalent}"));

                return new BitmapImage(new Uri("pack://application:,,,/HeroesIcons;component/Icons/Talents/_Generic/storm_ui_icon_default.dds", UriKind.Absolute));
            }
                
            try
            {
                return new BitmapImage(talent.Item2);
            }
            catch (Exception ex)
            {
                throw new IconException($"Uri: {talent.Item2} NameOfHeroTalent: {nameOfHeroTalent}", ex);
            }
        }

        /// <summary>
        /// Returns a BitmapImage of the hero
        /// </summary>
        /// <param name="attributeId">attributeid</param>
        /// <returns>BitmpImage of the hero</returns>
        public BitmapImage GetHeroPortrait(string attributeId)
        {
            Uri uri;

            // no pick
            if (string.IsNullOrEmpty(attributeId))
                return new BitmapImage(new Uri($"pack://application:,,,/HeroesIcons;component/Icons/HeroPortraits/storm_ui_glues_draft_portrait_nopick.dds", UriKind.Absolute));

            // not found
            if (!HeroPortraits.TryGetValue(attributeId, out uri))
            {
                Task.Run(() => Log("_ImageMissingLog.txt", $"Hero portrait: {attributeId}"));

                return new BitmapImage(new Uri($"pack://application:,,,/HeroesIcons;component/Icons/HeroPortraits/storm_ui_glues_draft_portrait_notfound.dds", UriKind.Absolute));
            }
         
            try
            {
                return new BitmapImage(uri);
            }
            catch (Exception ex)
            {
                throw new IconException($"Uri: {uri} AttributeId: {attributeId}", ex);
            }
        }

        /// <summary>
        /// Returns a leaderboard portrait BitmapImage of the hero
        /// </summary>
        /// <param name="realHeroName">Real hero name</param>
        /// <returns>BitmpImage of the hero</returns>
        public BitmapImage GetHeroLeaderboardPortrait(string realHeroName)
        {
            Uri uri;

            // no pick
            if (string.IsNullOrEmpty(realHeroName))
                return new BitmapImage(new Uri($"pack://application:,,,/HeroesIcons;component/Icons/HeroLeaderboardPortraits/storm_ui_ingame_hero_leaderboard_nopick.dds", UriKind.Absolute));

            // not found
            if (!LeaderboardPortraits.TryGetValue(realHeroName, out uri))
            {
                Task.Run(() => Log("_ImageMissingLog.txt", $"Leader hero portrait: {realHeroName}"));

                return new BitmapImage(new Uri($"pack://application:,,,/HeroesIcons;component/Icons/HeroLeaderboardPortraits/storm_ui_ingame_hero_leaderboard_notfound.dds", UriKind.Absolute));
            }
           
            try
            {
                return new BitmapImage(uri);
            }
            catch (Exception ex)
            {
                throw new IconException($"Uri: {uri} RealHeroName: {realHeroName}", ex);
            }
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
            {
                Task.Run(() => Log("_ReferenceNameLog.txt", $"No name for reference: {nameOfHeroTalent}"));

                return nameOfHeroTalent;
            }

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
            {
                Task.Run(() => Log("_ReferenceNameLog.txt", $"No hero name for reference: {attributeId}"));

                return "Hero not found";
            }

            return heroName;
        }

        public bool HeroExists(string heroName)
        {
            return Heroes.ContainsKey(heroName);
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

        private Uri SetLeaderboardPortrait(string fileName)
        {
            return new Uri($"pack://application:,,,/HeroesIcons;component/Icons/HeroLeaderboardPortraits/{fileName}", UriKind.Absolute);
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
                    if (string.IsNullOrEmpty(realHeroName))
                        realHeroName = hero; // default to hero name

                    // get attributeid from hero name
                    string attributeId = reader["attributeid"];

                    // get portrait
                    string portraitName = reader["portrait"];

                    // get leaderboard portrait
                    string lbPortrait = reader["leader"];

                    if (!string.IsNullOrEmpty(attributeId))
                    {
                        HeroNamesFromAttId.Add(attributeId, realHeroName);

                        if (!string.IsNullOrEmpty(portraitName))
                            HeroPortraits.Add(attributeId, SetHeroPortraitUri(portraitName));
                    }

                    if (!string.IsNullOrEmpty(lbPortrait))
                        LeaderboardPortraits.Add(realHeroName, SetLeaderboardPortrait(lbPortrait));

                    Heroes.Add(realHeroName, hero);

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

        private void Log(string fileName, string message)
        {
            using (StreamWriter writer = new StreamWriter($"logs/{fileName}", true))
            {
                writer.WriteLine(message);
            }
        }
    }
}
