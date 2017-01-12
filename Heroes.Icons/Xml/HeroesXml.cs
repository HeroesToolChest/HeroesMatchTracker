using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;

namespace Heroes.Icons.Xml
{
    internal class HeroesXml : XmlBase
    {
        private int SelectedBuild;

        private Dictionary<string, string> TalentShortTooltip = new Dictionary<string, string>();
        private Dictionary<string, string> TalentLongTooltip = new Dictionary<string, string>();

        public int CurrentLoadedHeroesBuild { get { return SelectedBuild; } }
        public int EarliestHeroesBuild { get; private set; } // cleared once initialized
        public int LatestHeroesBuild { get; private set; } // cleared once initialized
        public List<int> Builds { get; private set; } = new List<int>();

        /// <summary>
        /// key is attributeid, value is hero name
        /// </summary>
        public Dictionary<string, string> HeroNamesFromAttributeId { get; private set; } = new Dictionary<string, string>();

        /// <summary>
        /// key is real hero name, value alt name (if any)
        /// example: Anub'arak, Anubarak
        /// </summary>
        public Dictionary<string, string> HeroesRealName { get; private set; } = new Dictionary<string, string>();

        /// <summary>
        /// key is alt hero name, value real name
        /// example: Anubarak, Anub'arak
        /// </summary>
        public Dictionary<string, string> HeroesAlternativeName { get; private set; } = new Dictionary<string, string>();

        /// <summary>
        /// key is real hero name
        /// value is HeroRole
        /// </summary>
        public Dictionary<string, HeroRole> HeroesRole { get; private set; } = new Dictionary<string, HeroRole>();

        /// <summary>
        /// key is real hero name
        /// value is HeroFrancise
        /// </summary>
        public Dictionary<string, HeroFranchise> HeroesFranchise { get; private set; } = new Dictionary<string, HeroFranchise>();

        /// <summary>
        /// key is reference name of talent
        /// Tuple: key is real name of talent
        /// </summary>
        public Dictionary<string, Tuple<string, Uri>> TalentIcons { get; private set; } = new Dictionary<string, Tuple<string, Uri>>();

        /// <summary>
        /// key is TalentTier enum
        /// value is a string of all talent reference names for that tier
        /// </summary>
        public Dictionary<string, Dictionary<TalentTier, List<string>>> HeroesListOfTalents { get; private set; } = new Dictionary<string, Dictionary<TalentTier, List<string>>>();

        /// <summary>
        /// key is the talent reference name
        /// </summary>
        public Dictionary<string, TalentTooltip> TalentTooltips { get; private set; } = new Dictionary<string, TalentTooltip>();

        /// <summary>
        /// key is real hero name
        /// </summary>
        public Dictionary<string, Uri> HeroPortraits { get; private set; } = new Dictionary<string, Uri>();

        /// <summary>
        /// key is real hero name
        /// </summary>
        public Dictionary<string, Uri> LoadingPortraits { get; private set; } = new Dictionary<string, Uri>();

        /// <summary>
        /// key is real hero name
        /// </summary>
        public Dictionary<string, Uri> LeaderboardPortraits { get; private set; } = new Dictionary<string, Uri>();

        private HeroesXml(string parentFile, string xmlFolder, int? build = null)
        {
            if (build == null)
                SetDefaultBuildDirectory();
            else
                SelectedBuild = build.Value;

            XmlParentFile = parentFile;
            XmlFolder = $@"{xmlFolder}\{SelectedBuild}";
        }

        public static HeroesXml Initialize(string parentFile, string xmlFolder, int? build = null)
        {
            HeroesXml heroesXml = new HeroesXml(parentFile, xmlFolder, build);
            heroesXml.Parse();
            return heroesXml;
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
                    using (XmlReader reader = XmlReader.Create($@"Xml\{XmlFolder}\{hero}.xml"))
                    {
                        reader.MoveToContent();

                        if (reader.Name != hero)
                            continue;

                        // get real name
                        // example: Anubarak -> (real)Anub'arak
                        string realHeroName = reader["name"];
                        if (string.IsNullOrEmpty(realHeroName))
                            realHeroName = hero; // default to hero name

                        // get attributeid from hero name
                        // example: Anub
                        string attributeId = reader["attributeid"];

                        // get the role: warrior, assassin, support, specialist
                        string role = reader["role"];

                        // get the franchise: classic, diablo, overwatch, starcraft, warcraft
                        string franchise = reader["franchise"];

                        // get portrait
                        string portraitName = reader["portrait"];

                        // get loading portrait
                        string loadingPortrait = reader["loading"];

                        // get leaderboard portrait
                        string lbPortrait = reader["leader"];

                        if (!string.IsNullOrEmpty(attributeId))
                        {
                            HeroNamesFromAttributeId.Add(attributeId, realHeroName);
                        }

                        if (!string.IsNullOrEmpty(portraitName))
                            HeroPortraits.Add(realHeroName, SetHeroPortraitUri(portraitName));

                        if (!string.IsNullOrEmpty(loadingPortrait))
                            LoadingPortraits.Add(realHeroName, SetLoadingPortraitUri(loadingPortrait));

                        if (!string.IsNullOrEmpty(lbPortrait))
                            LeaderboardPortraits.Add(realHeroName, SetLeaderboardPortraitUri(lbPortrait));

                        HeroesRealName.Add(realHeroName, hero);
                        HeroesAlternativeName.Add(hero, realHeroName);

                        switch (role)
                        {
                            case "Warrior":
                                HeroesRole.Add(realHeroName, HeroRole.Warrior);
                                break;
                            case "Assassin":
                                HeroesRole.Add(realHeroName, HeroRole.Assassin);
                                break;
                            case "Support":
                                HeroesRole.Add(realHeroName, HeroRole.Support);
                                break;
                            case "Specialist":
                                HeroesRole.Add(realHeroName, HeroRole.Specialist);
                                break;
                            case "Multiclass":
                                HeroesRole.Add(realHeroName, HeroRole.Multiclass);
                                break;
                            default:
                                HeroesRole.Add(realHeroName, HeroRole.Unknown);
                                break;
                        }

                        switch (franchise)
                        {
                            case "Classic":
                                HeroesFranchise.Add(realHeroName, HeroFranchise.Classic);
                                break;
                            case "Diablo":
                                HeroesFranchise.Add(realHeroName, HeroFranchise.Diablo);
                                break;
                            case "Overwatch":
                                HeroesFranchise.Add(realHeroName, HeroFranchise.Overwatch);
                                break;
                            case "Starcraft":
                                HeroesFranchise.Add(realHeroName, HeroFranchise.Starcraft);
                                break;
                            case "Warcraft":
                                HeroesFranchise.Add(realHeroName, HeroFranchise.Warcraft);
                                break;
                            default:
                                HeroesFranchise.Add(realHeroName, HeroFranchise.Unknown);
                                break;
                        }

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
                                            string name = reader.Name; // reference name of talent
                                            string realName = reader["name"] == null ? string.Empty : reader["name"];  // real ingame name of talent
                                            string generic = reader["generic"] == null ? "false" : reader["generic"];  // is the icon being used generic
                                            string desc = reader["desc"] == null ? string.Empty : reader["desc"]; // reference name for talent desciptions

                                            SetTalentTooltip(name, desc);

                                            bool isGeneric;
                                            if (!bool.TryParse(generic, out isGeneric))
                                                isGeneric = false;

                                            if (reader.Read())
                                            {
                                                if (name.StartsWith("Generic") || name.StartsWith("HeroGeneric") || name.StartsWith("BattleMomentum"))
                                                    isGeneric = true;

                                                if (!TalentIcons.ContainsKey(name))
                                                    TalentIcons.Add(name, new Tuple<string, Uri>(realName, SetHeroTalentUri(hero, reader.Value, isGeneric)));

                                                talentTierList.Add(name);
                                            }
                                        }
                                    }

                                    talentTiersForHero.Add(tier, talentTierList);
                                }
                            }
                        } // end while

                        HeroesListOfTalents.Add(realHeroName, talentTiersForHero);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new ParseXmlException("Error on parsing of xml files", ex);
            }
        }

        // this should only run once on startup
        private void SetDefaultBuildDirectory()
        {
            List<string> buildDirectories = Directory.GetDirectories(@"Xml\Heroes").ToList();

            foreach (var directory in buildDirectories)
            {
                int buildNumber;
                if (int.TryParse(Path.GetFileName(directory), out buildNumber))
                    Builds.Add(buildNumber);
            }

            if (buildDirectories.Count < 1 || Builds.Count < 1)
                throw new ParseXmlException("No Heroes Xml build folders found!");

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
            if (!TalentTooltips.ContainsKey(talentReferenceName) && !string.IsNullOrEmpty(desc))
            {
                string shortDesc;
                string longDesc;
                if (!TalentShortTooltip.TryGetValue(desc, out shortDesc))
                    shortDesc = string.Empty;

                if (!TalentLongTooltip.TryGetValue(desc, out longDesc))
                    longDesc = string.Empty;


                TalentTooltips.Add(talentReferenceName, new TalentTooltip(shortDesc, longDesc));
            }
        }

        private void LoadTalentTooltipStrings()
        {
            try
            {
                using (StreamReader reader = new StreamReader($@"Xml\Heroes\{SelectedBuild}\_ShortTalentTooltips.txt"))
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
                using (StreamReader reader = new StreamReader($@"Xml\Heroes\{SelectedBuild}\_FullTalentTooltips.txt"))
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