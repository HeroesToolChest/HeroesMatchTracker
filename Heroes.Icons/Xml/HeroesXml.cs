using System;
using System.Collections.Generic;
using System.Windows.Media.Imaging;
using System.Xml;

namespace Heroes.Icons.Xml
{
    internal class HeroesXml : XmlBase, IHeroes
    {
        private bool Logger;

        /// <summary>
        /// key is attributeid, value is hero name
        /// </summary>
        private Dictionary<string, string> RealHeroNameByAttributeId = new Dictionary<string, string>();

        /// <summary>
        /// key is real hero name, value alt name (if any)
        /// example: Anub'arak, Anubarak
        /// </summary>
        private Dictionary<string, string> AlternativeHeroNameByRealName = new Dictionary<string, string>();

        /// <summary>
        /// key is alt hero name, value real name
        /// example: Anubarak, Anub'arak
        /// </summary>
        private Dictionary<string, string> RealHeroNameByAlternativeName = new Dictionary<string, string>();

        /// <summary>
        /// key is real hero name
        /// value is HeroFrancise
        /// </summary>
        private Dictionary<string, HeroFranchise> HeroFranchiseByRealName = new Dictionary<string, HeroFranchise>();

        /// <summary>
        /// key is real hero name
        /// value is HeroRole
        /// </summary>
        private Dictionary<string, List<HeroRole>> HeroRolesListByRealName = new Dictionary<string, List<HeroRole>>();

        /// <summary>
        /// key is real hero name
        /// </summary>
        private Dictionary<string, Uri> HeroPortraitUriByRealName = new Dictionary<string, Uri>();

        /// <summary>
        /// key is real hero name
        /// </summary>
        private Dictionary<string, Uri> HeroLoadingPortraitUriByRealName = new Dictionary<string, Uri>();

        /// <summary>
        /// key is real hero name
        /// </summary>
        private Dictionary<string, Uri> HeroLeaderboardPortraitUriByRealName = new Dictionary<string, Uri>();

        /// <summary>
        /// key is alias name
        /// </summary>
        private Dictionary<string, string> HeroRealNameByHeroAliasName = new Dictionary<string, string>();

        private HeroesXml(string parentFile, string xmlBaseFolder, bool logger, int currentBuild)
            : base(currentBuild)
        {
            Logger = logger;
            XmlParentFile = parentFile;
            XmlBaseFolder = xmlBaseFolder;
        }

        public static HeroesXml Initialize(string parentFile, string xmlBaseFolder, bool logger, int currentBuild)
        {
            HeroesXml heroesXml = new HeroesXml(parentFile, xmlBaseFolder, logger, currentBuild);
            heroesXml.Parse();
            return heroesXml;
        }

        /// <summary>
        /// Gets the english name of the given alias. Returns true if found, otherwise false
        /// </summary>
        /// <param name="heroNameAlias">Alias name</param>
        /// <param name="heroNameEnglish">English name</param>
        /// <returns></returns>
        public bool HeroNameTranslation(string heroNameAlias, out string heroNameEnglish)
        {
            return HeroRealNameByHeroAliasName.TryGetValue(heroNameAlias, out heroNameEnglish);
        }

        /// <summary>
        /// Returns a BitmapImage of the hero
        /// </summary>
        /// <param name="realHeroName">Real hero name</param>
        /// <returns>BitmpImage of the hero</returns>
        public BitmapImage GetHeroPortrait(string realHeroName)
        {
            Uri uri;

            // no pick
            if (string.IsNullOrEmpty(realHeroName))
                return HeroesBitmapImage(@"HeroPortraits\storm_ui_ingame_heroselect_btn_nopick.dds");

            if (HeroPortraitUriByRealName.TryGetValue(realHeroName, out uri))
            {
                return new BitmapImage(uri);
            }
            else
            {
                if (Logger)
                    LogMissingImage($"Hero portrait: {realHeroName}");

                return HeroesBitmapImage(@"HeroPortraits\storm_ui_ingame_heroselect_btn_notfound.dds");
            }
        }

        /// <summary>
        /// Returns a loading portrait BitmapImage of the hero
        /// </summary>
        /// <param name="realHeroName">Real hero name</param>
        /// <returns>BitmpImage of the hero</returns>
        public BitmapImage GetHeroLoadingPortrait(string realHeroName)
        {
            Uri uri;

            // no pick
            if (string.IsNullOrEmpty(realHeroName))
                return HeroesBitmapImage(@"HeroLoadingScreenPortraits\storm_ui_ingame_hero_loadingscreen_nopick.dds");

            if (HeroLoadingPortraitUriByRealName.TryGetValue(realHeroName, out uri))
            {
                return new BitmapImage(uri);
            }
            else
            {
                if (Logger)
                    LogMissingImage($"Loading hero portrait: {realHeroName}");

                return HeroesBitmapImage(@"HeroLoadingScreenPortraits\storm_ui_ingame_hero_loadingscreen_notfound.dds");
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
                return HeroesBitmapImage(@"HeroLeaderboardPortraits\storm_ui_ingame_hero_leaderboard_nopick.dds");

            if (HeroLeaderboardPortraitUriByRealName.TryGetValue(realHeroName, out uri))
            {
                return new BitmapImage(uri);
            }
            else
            {
                if (Logger)
                    LogMissingImage($"Leader hero portrait: {realHeroName}");

                return HeroesBitmapImage(@"HeroLoadingScreenPortraits\storm_ui_ingame_hero_loadingscreen_notfound.dds");
            }
        }

        /// <summary>
        /// Returns the real hero name from the hero's attribute id
        /// </summary>
        /// <param name="attributeId">Four character hero id</param>
        /// <returns>Full hero name</returns>
        public string GetRealHeroNameFromAttributeId(string attributeId)
        {
            string heroName;

            // no pick
            if (string.IsNullOrEmpty(attributeId))
                return null;

            if (RealHeroNameByAttributeId.TryGetValue(attributeId, out heroName))
            {
                return heroName;
            }
            else
            {
                if (Logger)
                    LogReferenceNameNotFound($"No hero name for attribute: {attributeId}");

                return "Hero not found";
            }
        }

        public string GetAltNameFromRealHeroName(string realName)
        {
            string altName;

            // no pick
            if (string.IsNullOrEmpty(realName))
                return null;

            if (AlternativeHeroNameByRealName.TryGetValue(realName, out altName))
            {
                return altName;
            }
            else
            {
                if (Logger)
                    LogReferenceNameNotFound($"No hero alt name for reference: {realName}");

                return "Hero alt name not found";
            }
        }

        public string GetRealHeroNameFromAltName(string altName)
        {
            string realName;

            // no pick
            if (string.IsNullOrEmpty(altName))
                return null;

            if (RealHeroNameByAlternativeName.TryGetValue(altName, out realName))
            {
                return realName;
            }
            else
            {
                if (Logger)
                    LogReferenceNameNotFound($"No hero real name for reference: {altName}");

                return "Hero real name not found";
            }
        }

        /// <summary>
        /// Checks to see if the hero name exists
        /// </summary>
        /// <param name="heroName">Hero name</param>
        /// <param name="realName">Is the name a real name or alt name</param>
        /// <returns>True if found</returns>
        public bool HeroExists(string heroName, bool realName = true)
        {
            if (realName)
                return AlternativeHeroNameByRealName.ContainsKey(heroName);
            else
                return RealHeroNameByAlternativeName.ContainsKey(heroName);
        }

        /// <summary>
        /// Returns the hero's list of roles. Multiclass will be first if hero has multiple roles. Will return a role of Unknown if hero name not found.
        /// </summary>
        /// <param name="realName">Hero real name</param>
        /// <returns>HeroRole</returns>
        public List<HeroRole> GetHeroRole(string realName)
        {
            List<HeroRole> roleList;

            if (HeroRolesListByRealName.TryGetValue(realName, out roleList))
                return roleList;
            else
                return new List<HeroRole> { HeroRole.Unknown };
        }

        /// <summary>
        /// Returns the hero's franchise. Will return Unknown if hero not found
        /// </summary>
        /// <param name="realName">Heroes real name</param>
        /// <returns>HeroRole</returns>
        public HeroFranchise GetHeroFranchise(string realName)
        {
            HeroFranchise franchise;

            if (HeroFranchiseByRealName.TryGetValue(realName, out franchise))
                return franchise;
            else
                return HeroFranchise.Unknown;
        }

        public List<string> GetListOfHeroes()
        {
            List<string> heroes = new List<string>();
            foreach (var hero in AlternativeHeroNameByRealName)
            {
                heroes.Add(hero.Key);
            }

            heroes.Sort();
            return heroes;
        }

        public int TotalAmountOfHeroes()
        {
            return AlternativeHeroNameByRealName.Count;
        }

        protected override void Parse()
        {
            ParseChildFiles();
        }

        protected override void ParseChildFiles()
        {
            try
            {
                using (XmlReader reader = XmlReader.Create($@"Xml\{XmlBaseFolder}\{XmlParentFile}"))
                {
                    reader.MoveToContent();

                    // read each hero
                    while (reader.Read())
                    {
                        if (reader.IsStartElement())
                        {
                            string hero = reader.Name;

                            // get real name
                            // example: Anubarak-> (real)Anub'arak
                            string realHeroName = reader["name"];
                            if (string.IsNullOrEmpty(realHeroName))
                                realHeroName = hero; // default to hero name

                            // get attributeid from hero name
                            // example: Anub
                            string attributeId = reader["attributeid"];

                            // get the franchise: classic, diablo, overwatch, starcraft, warcraft
                            string franchise = reader["franchise"];

                            // get portrait
                            string portraitName = reader["portrait"];

                            // get loading portrait
                            string loadingPortrait = reader["loading"];

                            // get leaderboard portrait
                            string lbPortrait = reader["leader"];

                            if (!string.IsNullOrEmpty(attributeId))
                                RealHeroNameByAttributeId.Add(attributeId, realHeroName);

                            if (!string.IsNullOrEmpty(portraitName))
                                HeroPortraitUriByRealName.Add(realHeroName, SetHeroPortraitUri(portraitName));

                            if (!string.IsNullOrEmpty(loadingPortrait))
                                HeroLoadingPortraitUriByRealName.Add(realHeroName, SetLoadingPortraitUri(loadingPortrait));

                            if (!string.IsNullOrEmpty(lbPortrait))
                                HeroLeaderboardPortraitUriByRealName.Add(realHeroName, SetLeaderboardPortraitUri(lbPortrait));

                            AlternativeHeroNameByRealName.Add(realHeroName, hero);
                            RealHeroNameByAlternativeName.Add(hero, realHeroName);

                            switch (franchise)
                            {
                                case "Classic":
                                    HeroFranchiseByRealName.Add(realHeroName, HeroFranchise.Classic);
                                    break;
                                case "Diablo":
                                    HeroFranchiseByRealName.Add(realHeroName, HeroFranchise.Diablo);
                                    break;
                                case "Overwatch":
                                    HeroFranchiseByRealName.Add(realHeroName, HeroFranchise.Overwatch);
                                    break;
                                case "Starcraft":
                                    HeroFranchiseByRealName.Add(realHeroName, HeroFranchise.Starcraft);
                                    break;
                                case "Warcraft":
                                    HeroFranchiseByRealName.Add(realHeroName, HeroFranchise.Warcraft);
                                    break;
                                default:
                                    HeroFranchiseByRealName.Add(realHeroName, HeroFranchise.Unknown);
                                    break;
                            }

                            while (reader.Read() && reader.Name != hero)
                            {
                                if (reader.NodeType == XmlNodeType.Element)
                                {
                                    if (reader.Name == "Roles")
                                    {
                                        reader.Read();
                                        string[] roles = reader.Value.Split(',');

                                        List<HeroRole> rolesList = new List<HeroRole>();

                                        foreach (var role in roles)
                                        {
                                            switch (role)
                                            {
                                                case "Multiclass":
                                                    rolesList.Add(HeroRole.Multiclass);
                                                    break;
                                                case "Warrior":
                                                    rolesList.Add(HeroRole.Warrior);
                                                    break;
                                                case "Assassin":
                                                    rolesList.Add(HeroRole.Assassin);
                                                    break;
                                                case "Support":
                                                    rolesList.Add(HeroRole.Support);
                                                    break;
                                                case "Specialist":
                                                    rolesList.Add(HeroRole.Specialist);
                                                    break;
                                                default:
                                                    rolesList.Add(HeroRole.Unknown);
                                                    break;
                                            }
                                        }

                                        HeroRolesListByRealName.Add(realHeroName, rolesList);
                                    }
                                    else if (reader.Name == "Aliases")
                                    {
                                        reader.Read();
                                        string[] aliases = reader.Value.Split(',');

                                        // add the english name
                                        HeroRealNameByHeroAliasName.Add(realHeroName, realHeroName);

                                        // add all the other aliases
                                        foreach (var alias in aliases)
                                        {
                                            if (HeroRealNameByHeroAliasName.ContainsKey(alias))
                                                throw new ArgumentException($"Alias already added to {realHeroName}: {alias}");

                                            HeroRealNameByHeroAliasName.Add(alias, realHeroName);
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new ParseXmlException($"Error on loading {XmlParentFile}", ex);
            }
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
    }
}
