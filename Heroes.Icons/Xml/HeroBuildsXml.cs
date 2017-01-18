using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;

namespace Heroes.Icons.Xml
{
    internal class HeroBuildsXml : XmlBase
    {
        private const string ShortTalentTooltipFileName = "_ShortTalentTooltips.txt";
        private const string FullTalentTooltipFileName = "_FullTalentTooltips.txt";

        private int SelectedBuild;
        private HeroesXml HeroesXml;

        private Dictionary<string, string> TalentShortTooltip = new Dictionary<string, string>();
        private Dictionary<string, string> TalentLongTooltip = new Dictionary<string, string>();

        private HeroBuildsXml(string parentFile, string xmlBaseFolder, HeroesXml heroesXml, int? build = null)
        {
            XmlParentFile = parentFile;
            XmlBaseFolder = xmlBaseFolder;
            HeroesXml = heroesXml;

            if (build == null)
                SetDefaultBuildDirectory();
            else
                SelectedBuild = build.Value;

            XmlFolder = Path.Combine(xmlBaseFolder, SelectedBuild.ToString());
        }

        public int CurrentLoadedHeroesBuild { get { return SelectedBuild; } }
        public int EarliestHeroesBuild { get; private set; } // cleared once initialized
        public int LatestHeroesBuild { get; private set; } // cleared once initialized
        public List<int> Builds { get; private set; } = new List<int>();

        /// <summary>
        /// key is reference name of talent
        /// Tuple: key is real name of talent
        /// </summary>
        public Dictionary<string, Tuple<string, Uri>> RealTalentNameUriByReferenceName { get; private set; } = new Dictionary<string, Tuple<string, Uri>>();

        /// <summary>
        /// key is real hero name
        /// value is a string of all talent reference names for that tier
        /// </summary>
        public Dictionary<string, Dictionary<TalentTier, List<string>>> HeroTalentsListByRealName { get; private set; } = new Dictionary<string, Dictionary<TalentTier, List<string>>>();

        /// <summary>
        /// key is the talent reference name
        /// </summary>
        public Dictionary<string, TalentTooltip> HeroTalentTooltipsByRealName { get; private set; } = new Dictionary<string, TalentTooltip>();

        public static HeroBuildsXml Initialize(string parentFile, string xmlBaseFolder, HeroesXml heroesXml, int? build = null)
        {
            if (heroesXml == null)
                return null;

            HeroBuildsXml xml = new HeroBuildsXml(parentFile, xmlBaseFolder, heroesXml, build);
            xml.Parse();
            return xml;
        }

        protected override void Parse()
        {
            LoadTalentTooltipStrings();
            base.Parse();
        }

        protected override void ParseChildFiles()
        {
            try
            {
                foreach (var hero in XmlChildFiles)
                {
                    using (XmlReader reader = XmlReader.Create($@"Xml\{XmlBaseFolder}\{SelectedBuild}\{hero}.xml"))
                    {
                        reader.MoveToContent();

                        string heroAltName = reader.Name;
                        if (heroAltName != hero)
                            continue;

                        var talentTiersForHero = new Dictionary<TalentTier, List<string>>();

                        // add talents, read each tier
                        while (reader.Read())
                        {
                            if (reader.IsStartElement())
                            {
                                var talentTierList = new List<string>();
                                TalentTier tier;

                                // is tier Level1, Level4, etc...
                                if (Enum.TryParse(reader.Name, out tier))
                                {
                                    // read each talent in tier
                                    while (reader.Read() && reader.Name != tier.ToString())
                                    {
                                        if (reader.NodeType == XmlNodeType.Element)
                                        {
                                            string refName = reader.Name; // reference name of talent
                                            string realName = reader["name"] == null ? string.Empty : reader["name"];  // real ingame name of talent
                                            string generic = reader["generic"] == null ? "false" : reader["generic"];  // is the icon being used generic
                                            string desc = reader["desc"] == null ? string.Empty : reader["desc"]; // reference name for talent desciptions

                                            SetTalentTooltip(refName, desc);

                                            bool isGeneric;
                                            if (!bool.TryParse(generic, out isGeneric))
                                                isGeneric = false;

                                            if (reader.Read())
                                            {
                                                if (refName.StartsWith("Generic") || refName.StartsWith("HeroGeneric") || refName.StartsWith("BattleMomentum"))
                                                    isGeneric = true;

                                                if (!RealTalentNameUriByReferenceName.ContainsKey(refName))
                                                    RealTalentNameUriByReferenceName.Add(refName, new Tuple<string, Uri>(realName, SetHeroTalentUri(hero, reader.Value, isGeneric)));

                                                talentTierList.Add(refName);
                                            }
                                        }
                                    }

                                    talentTiersForHero.Add(tier, talentTierList);
                                }
                            }
                        } // end while

                        if (!HeroesXml.RealHeroNameByAlternativeName.ContainsKey(heroAltName))
                            throw new ArgumentException($"Hero alt name not found: {heroAltName}");

                        HeroTalentsListByRealName.Add(HeroesXml.RealHeroNameByAlternativeName[heroAltName], talentTiersForHero);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new ParseXmlException("Error on parsing of hero xml files", ex);
            }
        }

        // this should only run once on startup
        private void SetDefaultBuildDirectory()
        {
            List<string> buildDirectories = Directory.GetDirectories($@"Xml\{XmlBaseFolder}").ToList();

            foreach (var directory in buildDirectories)
            {
                int buildNumber;
                if (int.TryParse(Path.GetFileName(directory), out buildNumber))
                    Builds.Add(buildNumber);
            }

            if (buildDirectories.Count < 1 || Builds.Count < 1)
                throw new ParseXmlException("No HeroBuilds folders found!");

            Builds = Builds.OrderByDescending(x => x).ToList();

            EarliestHeroesBuild = Builds[Builds.Count - 1];
            LatestHeroesBuild = SelectedBuild = Builds[0];
        }

        private Uri SetHeroPortraitUri(string fileName)
        {
            return new Uri($@"{ApplicationIconsPath}\HeroPortraits\{fileName}", UriKind.Absolute);
        }

        private Uri SetLoadingPortraitUri(string fileName)
        {
            return new Uri($@"{ApplicationIconsPath}\HeroLoadingScreenPortraits\{fileName}", UriKind.Absolute);
        }

        private Uri SetLeaderboardPortraitUri(string fileName)
        {
            return new Uri($@"{ApplicationIconsPath}\HeroLeaderboardPortraits\{fileName}", UriKind.Absolute);
        }

        private Uri SetHeroTalentUri(string hero, string fileName, bool isGenericTalent)
        {
            if (!(Path.GetExtension(fileName) != ".dds" || Path.GetExtension(fileName) != ".png"))
                throw new HeroesIconException($"Image file does not have .dds or .png extension [{fileName}]");

            if (!isGenericTalent)
                return new Uri($@"{ApplicationIconsPath}\Talents\{hero}\{fileName}", UriKind.Absolute);
            else
                return new Uri($@"{ApplicationIconsPath}\Talents\_Generic\{fileName}", UriKind.Absolute);
        }

        private void SetTalentTooltip(string talentReferenceName, string desc)
        {
            // checking keys because of generic talents
            if (!HeroTalentTooltipsByRealName.ContainsKey(talentReferenceName) && !string.IsNullOrEmpty(desc))
            {
                string shortDesc;
                string longDesc;
                if (!TalentShortTooltip.TryGetValue(desc, out shortDesc))
                    shortDesc = string.Empty;

                if (!TalentLongTooltip.TryGetValue(desc, out longDesc))
                    longDesc = string.Empty;

                HeroTalentTooltipsByRealName.Add(talentReferenceName, new TalentTooltip(shortDesc, longDesc));
            }
        }

        private void LoadTalentTooltipStrings()
        {
            try
            {
                using (StreamReader reader = new StreamReader($@"Xml\{XmlBaseFolder}\{SelectedBuild}\{ShortTalentTooltipFileName}"))
                {
                    while (!reader.EndOfStream)
                    {
                        string line = reader.ReadLine();
                        if (!line.StartsWith("--"))
                        {
                            string[] talent = line.Split(new char[] { '=' }, 2);
                            TalentShortTooltip.Add(talent[0], talent[1]);
                        }
                    }
                }

                using (StreamReader reader = new StreamReader($@"Xml\{XmlBaseFolder}\{SelectedBuild}\{FullTalentTooltipFileName}"))
                {
                    while (!reader.EndOfStream)
                    {
                        string line = reader.ReadLine();
                        if (!line.StartsWith("--"))
                        {
                            string[] talent = line.Split(new char[] { '=' }, 2);
                            TalentLongTooltip.Add(talent[0], talent[1]);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new ParseXmlException("Error on loading talent tooltips", ex);
            }
        }
    }
}