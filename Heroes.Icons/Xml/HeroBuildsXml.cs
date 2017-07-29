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

        /// <summary>
        /// key is the build number
        /// </summary>
        private Dictionary<int, Tuple<string, string>> BuildPatchNotesByBuildNumber = new Dictionary<int, Tuple<string, string>>();

        private HeroBuildsXml(string parentFile, string xmlBaseFolder, HeroesXml heroesXml, bool logger, int? build = null)
            : base(build ?? 0, logger)
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
        public bool BuildsVerifyStatus { get; private set; } = false;

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
                return HeroesBitmapImage($@"Talents\_Generic\{NoTalentIconPick}");

            if (RealTalentNameUriByReferenceName.TryGetValue(talentReferenceName, out Tuple<string, Uri> talent))
            {
                try
                {
                    return HeroesBitmapImage(talent.Item2);
                }
                catch (IOException)
                {
                    LogMissingImage($"Missing image: {talent.Item2}");
                    return HeroesBitmapImage($@"Talents\_Generic\{NoTalentIconFound}");
                }
            }
            else
            {
                LogReferenceNameNotFound($"Talent icon: {talentReferenceName}");
                return HeroesBitmapImage($@"Talents\_Generic\{NoTalentIconFound}");
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
            TalentTooltip talentTooltip = new TalentTooltip();

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

        /// <summary>
        /// Get the patch notes link from the given build number. Returns null if not found
        /// </summary>
        /// <param name="build">The build number</param>
        /// <returns></returns>
        public Tuple<string, string> GetPatchNotes(int build)
        {
            if (BuildPatchNotesByBuildNumber.TryGetValue(build, out Tuple<string, string> notes))
                return notes;
            else
                return null;
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
            foreach (var hero in XmlChildFiles)
            {
                using (XmlReader reader = XmlReader.Create($@"Xml\{XmlBaseFolder}\{SelectedBuild}\{hero}.xml"))
                {
                    reader.MoveToContent();

                    string heroAltName = reader.Name;
                    if (heroAltName != hero)
                        continue;

                    HeroMana heroEnergy = HeroMana.Mana;
                    string energy = reader["energy"] ?? string.Empty;
                    switch (energy)
                    {
                        case "Brew":
                            heroEnergy = HeroMana.Brew;
                            break;
                        case "Fury":
                            heroEnergy = HeroMana.Fury;
                            break;
                        default:
                            heroEnergy = HeroMana.Mana;
                            break;
                    }

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
                                        int? mana = ConvertToNullableInt(reader["mana"]); // mana/brew/fury/etc... of the talent
                                        string perManaCost = reader["per-mana"] ?? "false"; // the time cost for mana
                                        int? cooldown = ConvertToNullableInt(reader["cooldown"]); // cooldown of the talent
                                        string charge = reader["ch-cooldown"] ?? "false"; // is the cooldown a charge cooldown

                                        if (!bool.TryParse(perManaCost, out bool isPerManaCost))
                                            isPerManaCost = false;

                                        if (!bool.TryParse(charge, out bool isCharge))
                                            isCharge = false;

                                        SetTalentTooltip(refName, desc, mana, isPerManaCost, cooldown, isCharge, heroEnergy);

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

        // this should only run once on startup
        private void SetDefaultBuildDirectory()
        {
            // load up the builds from Builds.xml
            using (XmlReader reader = XmlReader.Create($@"Xml\{XmlBaseFolder}\Builds.xml"))
            {
                reader.MoveToContent();
                if (reader.Name != "Builds")
                    return;

                while (reader.Read())
                {
                    if (reader.IsStartElement())
                    {
                        string type = reader["type"];
                        string link = reader["link"];
                        string pre = reader["pre"];

                        try
                        {
                            if (reader.Read())
                            {
                                if (string.IsNullOrEmpty(pre))
                                {
                                    BuildPatchNotesByBuildNumber.Add(Convert.ToInt32(reader.Value), new Tuple<string, string>(type, link));
                                }
                                else
                                {
                                    var previousBuild = BuildPatchNotesByBuildNumber[Convert.ToInt32(pre)];
                                    BuildPatchNotesByBuildNumber.Add(Convert.ToInt32(reader.Value), new Tuple<string, string>(previousBuild.Item1, previousBuild.Item2));
                                }
                            }
                        }
                        catch (FormatException ex)
                        {
                            throw new ParseXmlException($"Could not convert to Int32: {pre} | {reader.Value}", ex);
                        }
                    }
                }
            }

            foreach (var build in BuildPatchNotesByBuildNumber)
            {
                int buildNumber = build.Key;

                if (!Directory.Exists($@"Xml\{XmlBaseFolder}\{buildNumber}"))
                    throw new ParseXmlException($"Could not find required Build Folder: Xml\\{XmlBaseFolder}\\{buildNumber}");
                else
                    Builds.Add(buildNumber);
            }

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

        private void SetTalentTooltip(string talentReferenceName, string desc, int? mana, bool isPerManaCost, int? cooldown, bool isChargeCooldown, HeroMana manaType)
        {
            // checking keys because of generic talents
            if (!HeroTalentTooltipsByRealName.ContainsKey(talentReferenceName) && !string.IsNullOrEmpty(desc))
            {
                if (!TalentShortTooltip.TryGetValue(desc, out string shortDesc))
                    shortDesc = string.Empty;

                if (!TalentLongTooltip.TryGetValue(desc, out string longDesc))
                    longDesc = string.Empty;

                HeroTalentTooltipsByRealName.Add(talentReferenceName, new TalentTooltip(shortDesc, longDesc, mana, isPerManaCost, cooldown, isChargeCooldown, manaType));
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

                            if (TalentShortTooltip.ContainsKey(talent[0]))
                                throw new ArgumentException($"An item with the same key has already been added in Short Tooltips: {talent[0]}");

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

                            if (TalentLongTooltip.ContainsKey(talent[0]))
                                throw new ArgumentException($"An item with the same key has already been added in Full Tooltips: {talent[0]}");

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