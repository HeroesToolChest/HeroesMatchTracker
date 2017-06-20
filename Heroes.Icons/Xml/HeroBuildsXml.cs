using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Media.Imaging;
using System.Xml;

namespace Heroes.Icons.Xml
{
    internal class HeroBuildsXml : XmlBase, IHeroBuilds
    {
        private const string ShortTalentTooltipFileName = "_ShortTalentTooltips.txt";
        private const string FullTalentTooltipFileName = "_FullTalentTooltips.txt";

        private int SelectedBuild;
        private HeroesXml HeroesXml;
        private bool Logger;

        private Dictionary<string, string> TalentShortTooltip = new Dictionary<string, string>();
        private Dictionary<string, string> TalentLongTooltip = new Dictionary<string, string>();

        /// <summary>
        /// key is reference name of talent
        /// Tuple: key is real name of talent
        /// </summary>
        private Dictionary<string, Tuple<string, Uri>> RealTalentNameUriByReferenceName = new Dictionary<string, Tuple<string, Uri>>();

        /// <summary>
        /// key is real hero name
        /// value is a string of all talent reference names for that tier
        /// </summary>
        private Dictionary<string, Dictionary<TalentTier, List<string>>> HeroTalentsListByRealName = new Dictionary<string, Dictionary<TalentTier, List<string>>>();

        /// <summary>
        /// key is the talent reference name
        /// </summary>
        private Dictionary<string, TalentTooltip> HeroTalentTooltipsByRealName = new Dictionary<string, TalentTooltip>();

        /// <summary>
        /// key is the talent reference name
        /// </summary>
        private Dictionary<string, string> RealHeroNameByTalentReferenceName = new Dictionary<string, string>();

        private HeroBuildsXml(string parentFile, string xmlBaseFolder, HeroesXml heroesXml, bool logger, int? build = null)
            : base(build ?? 0)
        {
            Logger = logger;
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

        public static HeroBuildsXml Initialize(string parentFile, string xmlBaseFolder, HeroesXml heroesXml, bool logger, int? build = null)
        {
            if (heroesXml == null)
                return null;

            HeroBuildsXml xml = new HeroBuildsXml(parentFile, xmlBaseFolder, heroesXml, logger, build);
            xml.Parse();
            return xml;
        }

        /// <summary>
        /// Returns a BitmapImage of the talent
        /// </summary>
        /// <param name="talentReferenceName">Reference talent name</param>
        /// <returns>BitmapImage of the talent</returns>
        public BitmapImage GetTalentIcon(string talentReferenceName)
        {
            // no pick
            if (string.IsNullOrEmpty(talentReferenceName))
                return HeroesBitmapImage(@"Talents\_Generic\storm_ui_ingame_leader_talent_unselected.png");

            if (RealTalentNameUriByReferenceName.TryGetValue(talentReferenceName, out Tuple<string, Uri> talent))
            {
                try
                {
                    BitmapImage image = new BitmapImage(talent.Item2);
                    image.Freeze();

                    return image;
                }
                catch (IOException)
                {
                    if (Logger)
                        LogMissingImage($"Missing image: {talent.Item2}");

                    return HeroesBitmapImage(@"Talents\_Generic\storm_ui_icon_default.dds");
                }
            }
            else
            {
                if (Logger)
                    LogReferenceNameNotFound($"Talent icon: {talentReferenceName}");

                return HeroesBitmapImage(@"Talents\_Generic\storm_ui_icon_default.dds");
            }
        }

        /// <summary>
        /// Returns the talent name from the talent reference name
        /// </summary>
        /// <param name="talentReferenceName">Reference talent name</param>
        /// <returns>Talent name</returns>
        public string GetTrueTalentName(string talentReferenceName)
        {
            // no pick
            if (string.IsNullOrEmpty(talentReferenceName))
                return "No pick";

            if (RealTalentNameUriByReferenceName.TryGetValue(talentReferenceName, out Tuple<string, Uri> talent))
            {
                return talent.Item1;
            }
            else
            {
                if (Logger)
                    LogReferenceNameNotFound($"No name for reference: {talentReferenceName}");

                return talentReferenceName;
            }
        }

        /// <summary>
        /// Returns a dictionary of all the talents of a hero
        /// </summary>
        /// <param name="realHeroName">real hero name</param>
        /// <returns></returns>
        public Dictionary<TalentTier, List<string>> GetAllTalentsForHero(string realHeroName)
        {
            if (HeroTalentsListByRealName.TryGetValue(realHeroName, out Dictionary<TalentTier, List<string>> talents))
            {
                return talents;
            }
            else
            {
                if (Logger)
                    LogReferenceNameNotFound($"No hero real name found [{nameof(GetAllTalentsForHero)}]: {realHeroName}");

                return null;
            }
        }

        /// <summary>
        /// Returns a list of all the talents of a hero given a talent tier
        /// </summary>
        /// <param name="realHeroName">real hero name</param>
        /// <param name="talentTier">talent tier</param>
        /// <returns></returns>
        public List<string> GetTierTalentsForHero(string realHeroName, TalentTier talentTier)
        {
            if (HeroTalentsListByRealName.TryGetValue(realHeroName, out Dictionary<TalentTier, List<string>> talents))
            {
                return talents[talentTier];
            }
            else
            {
                if (Logger)
                    LogReferenceNameNotFound($"No hero real name found [{nameof(GetAllTalentsForHero)}]: {realHeroName}");

                return null;
            }
        }

        /// <summary>
        /// Returns a TalentTooltip object which contains the short and full tooltips of the talent
        /// </summary>
        /// <param name="talentReferenceName">Talent reference name</param>
        /// <returns></returns>
        public TalentTooltip GetTalentTooltips(string talentReferenceName)
        {
            TalentTooltip talentTooltip = new TalentTooltip(string.Empty, string.Empty);

            if (string.IsNullOrEmpty(talentReferenceName) || !HeroTalentTooltipsByRealName.ContainsKey(talentReferenceName))
                return talentTooltip;

            HeroTalentTooltipsByRealName.TryGetValue(talentReferenceName, out talentTooltip);

            return talentTooltip;
        }

        /// <summary>
        /// Gets the hero name associated with the given talent. Returns true is found, otherwise returns false
        /// </summary>
        /// <param name="talentName">The talent reference name</param>
        /// <param name="heroRealName">The hero name</param>
        /// <returns></returns>
        public bool GetHeroNameFromTalentReferenceName(string talentName, out string heroRealName)
        {
            return RealHeroNameByTalentReferenceName.TryGetValue(talentName, out heroRealName);
        }

        protected override void Parse()
        {
            DuplicateBuildCheck();
            ParseParentFile();
            LoadTalentTooltipStrings();
            ParseChildFiles();
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

                                // is tier Level1, Level4, etc...
                                if (Enum.TryParse(reader.Name, out TalentTier tier))
                                {
                                    // read each talent in tier
                                    while (reader.Read() && reader.Name != tier.ToString())
                                    {
                                        if (reader.NodeType == XmlNodeType.Element)
                                        {
                                            string refName = reader.Name; // reference name of talent
                                            string realName = reader["name"] ?? string.Empty;  // real ingame name of talent
                                            string generic = reader["generic"] ?? "false";  // is the icon being used generic
                                            string desc = reader["desc"] ?? string.Empty; // reference name for talent tooltips

                                            SetTalentTooltip(refName, desc);

                                            if (!bool.TryParse(generic, out bool isGeneric))
                                                isGeneric = false;

                                            if (reader.Read())
                                            {
                                                bool isGenericTalent = false;

                                                if (refName.StartsWith("Generic") || refName.StartsWith("HeroGeneric") || refName.StartsWith("BattleMomentum"))
                                                {
                                                    isGeneric = true;
                                                    isGenericTalent = true;
                                                }

                                                if (!RealTalentNameUriByReferenceName.ContainsKey(refName))
                                                    RealTalentNameUriByReferenceName.Add(refName, new Tuple<string, Uri>(realName, SetHeroTalentUri(hero, reader.Value, isGeneric)));

                                                talentTierList.Add(refName);

                                                if (!isGenericTalent)
                                                {
                                                    if (!HeroesXml.HeroExists(heroAltName, false))
                                                        throw new ArgumentException($"Hero alt name not found: {heroAltName}");

                                                    if (RealHeroNameByTalentReferenceName.ContainsKey(refName) && tier != TalentTier.Old)
                                                        throw new ArgumentException($"Same key {refName}");

                                                    if (tier != TalentTier.Old)
                                                        RealHeroNameByTalentReferenceName.Add(refName, HeroesXml.GetRealHeroNameFromAltName(heroAltName));
                                                }
                                            }
                                        }
                                    }

                                    talentTiersForHero.Add(tier, talentTierList);
                                }
                            }
                        } // end while

                        if (!HeroesXml.HeroExists(heroAltName, false))
                            throw new ArgumentException($"Hero alt name not found: {heroAltName}");

                        HeroTalentsListByRealName.Add(HeroesXml.GetRealHeroNameFromAltName(heroAltName), talentTiersForHero);
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
                if (int.TryParse(Path.GetFileName(directory), out int buildNumber))
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
                if (!TalentShortTooltip.TryGetValue(desc, out string shortDesc))
                    shortDesc = string.Empty;

                if (!TalentLongTooltip.TryGetValue(desc, out string longDesc))
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

        private void DuplicateBuildCheck()
        {
            while (CurrentBuild != 47479)
            {
                using (XmlTextReader reader = new XmlTextReader($@"Xml\{XmlFolder}\{XmlParentFile}"))
                {
                    reader.MoveToContent();
                    string previousBuild = reader["pre"]; // check to see if we should load up a previous build

                    if (string.IsNullOrEmpty(previousBuild))
                        return;

                    if (int.TryParse(previousBuild, out int build))
                    {
                        XmlFolder = $@"{XmlBaseFolder}/{build}";
                        SelectedBuild = build;
                    }
                }
            }
        }
    }
}