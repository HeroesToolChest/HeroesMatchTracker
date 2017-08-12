using Heroes.Icons.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;

namespace Heroes.Icons.Xml
{
    internal class HeroBuildsXml : XmlBase, IHeroBuilds
    {
        private const string ShortTalentTooltipFileName = "_ShortTalentTooltips.txt";
        private const string FullTalentTooltipFileName = "_FullTalentTooltips.txt";
        private const string HeroDescriptionsFileName = "_HeroDescriptions.txt";

        private int SelectedBuild;
        private HeroesXml HeroesXml;

        /// <summary>
        /// Inner dictionary key is talent reference name and values are real hero names
        /// </summary>
        private Dictionary<TalentTier, Dictionary<string, string>> RealHeroNameByTalentTierReferenceName = new Dictionary<TalentTier, Dictionary<string, string>>();

        /// <summary>
        /// key is the build number
        /// </summary>
        private Dictionary<int, Tuple<string, string>> BuildPatchNotesByBuildNumber = new Dictionary<int, Tuple<string, string>>();

        /// <summary>
        /// key is the real hero name
        /// value is a dictionary of Talent(s) for each talent tier
        /// </summary>
        private Dictionary<string, Dictionary<TalentTier, List<Talent>>> HeroTalentsByHeroName = new Dictionary<string, Dictionary<TalentTier, List<Talent>>>();

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

        public static HeroBuildsXml Initialize(string parentFile, string xmlBaseFolder, HeroesXml heroesXml, bool logger, int? build = null)
        {
            if (heroesXml == null)
                return null;

            HeroBuildsXml xml = new HeroBuildsXml(parentFile, xmlBaseFolder, heroesXml, logger, build);
            xml.Parse();
            return xml;
        }

        /// <summary>
        /// Returns a dictionary of all the talents of the given hero in talent tiers
        /// </summary>
        /// <param name="realHeroName">real hero name</param>
        public Dictionary<TalentTier, List<Talent>> GetHeroTalents(string realHeroName)
        {
            if (HeroTalentsByHeroName.TryGetValue(realHeroName, out Dictionary<TalentTier, List<Talent>> talents))
            {
                return talents;
            }
            else
            {
                LogReferenceNameNotFound($"No hero talents found for [{nameof(realHeroName)}]: {realHeroName}");
                return null;
            }
        }

        /// <summary>
        /// Returns a Talent object from the hero name, tier, and reference name of talent
        /// </summary>
        /// <param name="realHeroName">real hero name</param>
        /// <param name="tier">The tier that the talent exists in</param>
        /// <param name="talentReferenceName">reference name of talent</param>
        /// <returns></returns>
        public Talent GetHeroTalent(string realHeroName, TalentTier tier, string talentReferenceName)
        {
            if (string.IsNullOrEmpty(talentReferenceName))
            {
                return new Talent // no pick talent
                {
                    Name = "No pick",
                    IsIconGeneric = true,
                    IsGeneric = true,
                    IconUri = SetHeroTalentUri(string.Empty, NoTalentIconPick, true),
                };
            }

            var allTalents = GetHeroTalents(realHeroName);

            if (allTalents == null)
                return null;

            var talents = allTalents[tier];
            foreach (var talent in talents)
            {
                if (talent.ReferenceName == talentReferenceName)
                    return talent;
            }

            // we couldn't find a talent
            LogReferenceNameNotFound($"Talent icon: {talentReferenceName}");

            return new Talent
            {
                Name = talentReferenceName,
                IconUri = SetHeroTalentUri(string.Empty, NoTalentIconFound, true),
            };
        }

        /// <summary>
        /// Gets the hero name associated with the given talent. Returns true is found, otherwise returns false
        /// </summary>
        /// <param name="tier">The tier that the talent resides in</param>
        /// <param name="talentName">The talent reference name</param>
        /// <param name="heroRealName">The hero name</param>
        /// <returns></returns>
        public bool GetHeroNameFromTalentReferenceName(TalentTier tier, string talentName, out string heroRealName)
        {
            return RealHeroNameByTalentTierReferenceName[tier].TryGetValue(talentName, out heroRealName);
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
            ParseChildFiles();
        }

        protected override void ParseChildFiles()
        {
            // create local variables for tooltips
            Dictionary<string, string> talentShortTooltip = new Dictionary<string, string>();
            Dictionary<string, string> talentLongTooltip = new Dictionary<string, string>();

            // key is the real hero name, value is the description of the hero
            Dictionary<string, string> heroDescriptionByHeroName = new Dictionary<string, string>();

            // load up all the talents
            LoadTalentTooltipStrings(talentShortTooltip, talentLongTooltip);

            // load up hero descriptions
            LoadHeroDescriptions(heroDescriptionByHeroName);

            foreach (var heroName in XmlChildFiles)
            {
                using (XmlReader reader = XmlReader.Create($@"Xml\{XmlBaseFolder}\{SelectedBuild}\{heroName}.xml"))
                {
                    reader.MoveToContent();

                    string heroAltName = reader.Name;
                    if (heroAltName != heroName)
                        continue;

                    // set hero mana type
                    HeroMana heroManaType = Enum.TryParse(reader["mana"], out HeroMana heroMana) ? heroMana : HeroMana.Mana;

                    // set hero description
                    Hero hero = HeroesXml.GetHeroInfo(heroName);
                    hero.Description = heroDescriptionByHeroName[heroName];

                    var talentTiersForHero = new Dictionary<TalentTier, List<Talent>>();

                    // add talents, read each tier
                    while (reader.Read())
                    {
                        if (reader.IsStartElement())
                        {
                            var talentTierList = new List<Talent>();

                            // is tier Level1, Level4, etc...
                            if (Enum.TryParse(reader.Name, out TalentTier tier))
                            {
                                // read each talent in tier
                                while (reader.Read() && reader.Name != tier.ToString())
                                {
                                    if (reader.NodeType == XmlNodeType.Element)
                                    {
                                        string refTalentName = reader.Name; // reference name of talent
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

                                        if (!bool.TryParse(generic, out bool isGeneric))
                                            isGeneric = false;

                                        if (reader.Read())
                                        {
                                            bool isGenericTalent = false;

                                            if (refTalentName.StartsWith("Generic") || refTalentName.StartsWith("HeroGeneric") || refTalentName.StartsWith("BattleMomentum"))
                                            {
                                                isGeneric = true;
                                                isGenericTalent = true;
                                            }

                                            // create the tooltip
                                            if (!talentShortTooltip.TryGetValue(desc, out string shortDesc))
                                                shortDesc = string.Empty;

                                            if (!talentLongTooltip.TryGetValue(desc, out string longDesc))
                                                longDesc = string.Empty;

                                            // create the talent
                                            talentTierList.Add(new Talent
                                            {
                                                Name = realName,
                                                ReferenceName = refTalentName,
                                                IsIconGeneric = isGeneric,
                                                IsGeneric = isGenericTalent,
                                                TooltipDescriptionName = desc,
                                                IconUri = SetHeroTalentUri(heroName, reader.Value, isGeneric),
                                                Tier = tier,
                                                Tooltip = new TalentTooltip
                                                {
                                                    Short = shortDesc,
                                                    Full = longDesc,
                                                    ManaType = heroManaType,
                                                    Mana = mana,
                                                    IsPerManaCost = isPerManaCost,
                                                    Cooldown = cooldown,
                                                    IsChargeCooldown = isCharge,
                                                },
                                            });

                                            if (!isGenericTalent && tier != TalentTier.Old)
                                            {
                                                if (!HeroesXml.HeroExists(heroAltName))
                                                    throw new ArgumentException($"Hero alt name not found: {heroAltName}");

                                                if (RealHeroNameByTalentTierReferenceName.ContainsKey(tier))
                                                {
                                                    if (RealHeroNameByTalentTierReferenceName[tier].ContainsKey(refTalentName))
                                                        throw new ArgumentException($"Same key {refTalentName} [{heroName}]");

                                                    RealHeroNameByTalentTierReferenceName[tier].Add(refTalentName, HeroesXml.GetRealHeroNameFromAltName(heroAltName));
                                                }
                                                else
                                                {
                                                    RealHeroNameByTalentTierReferenceName.Add(tier, new Dictionary<string, string>() { { refTalentName, HeroesXml.GetRealHeroNameFromAltName(heroAltName) } });
                                                }
                                            }
                                        }
                                    }
                                }

                                talentTiersForHero.Add(tier, talentTierList);
                            }
                        }
                    } // end while

                    if (!HeroesXml.HeroExists(heroAltName))
                        throw new ArgumentException($"Hero alt name not found: {heroAltName}");

                    HeroTalentsByHeroName.Add(HeroesXml.GetRealHeroNameFromAltName(heroAltName), talentTiersForHero);
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

        private void LoadTalentTooltipStrings(Dictionary<string, string> talentShortTooltip, Dictionary<string, string> talentFullTooltip)
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

                            if (talentShortTooltip.ContainsKey(talent[0]))
                                throw new ArgumentException($"An item with the same key has already been added in Short Tooltips: {talent[0]}");

                            talentShortTooltip.Add(talent[0], talent[1]);
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

                            if (talentFullTooltip.ContainsKey(talent[0]))
                                throw new ArgumentException($"An item with the same key has already been added in Full Tooltips: {talent[0]}");

                            talentFullTooltip.Add(talent[0], talent[1]);
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

        private void LoadHeroDescriptions(Dictionary<string, string> heroDescriptions)
        {
            try
            {
                using (StreamReader reader = new StreamReader($@"Xml\{XmlBaseFolder}\{SelectedBuild}\{HeroDescriptionsFileName}"))
                {
                    while (!reader.EndOfStream)
                    {
                        string line = reader.ReadLine();
                        if (!line.StartsWith("--"))
                        {
                            string[] description = line.Split(new char[] { '=' }, 2);

                            if (heroDescriptions.ContainsKey(description[0]))
                                throw new ArgumentException($"An item with the same key has already been added in hero descriptions: {description[0]}");

                            heroDescriptions.Add(description[0], description[1]);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new ParseXmlException("Error on loading hero desciptions", ex);
            }
        }
    }
}