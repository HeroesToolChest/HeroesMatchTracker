using Heroes.Icons.Models;
using System;
using System.Collections.Generic;
using System.Xml;

namespace Heroes.Icons.Xml
{
    internal class HeroesXml : XmlBase, IHeroes
    {
        /// <summary>
        /// key is attributeid, value is hero name
        /// </summary>
        private Dictionary<string, string> RealHeroNameByAttributeId = new Dictionary<string, string>();

        /// <summary>
        /// key is alt hero name, value real name
        /// example: Anubarak, Anub'arak
        /// </summary>
        private Dictionary<string, string> RealHeroNameByAlternativeName = new Dictionary<string, string>();

        /// <summary>
        /// key is alias name
        /// </summary>
        private Dictionary<string, string> HeroRealNameByHeroAliasName = new Dictionary<string, string>();

        /// <summary>
        /// key is real hero name
        /// </summary>
        private Dictionary<string, Hero> HeroByHeroName = new Dictionary<string, Hero>();

        private HeroesXml(string parentFile, string xmlBaseFolder, bool logger, int? currentBuild)
            : base(currentBuild ?? 0, logger)
        {
            XmlParentFile = parentFile;
            XmlBaseFolder = xmlBaseFolder;
        }

        public static HeroesXml Initialize(string parentFile, string xmlBaseFolder, bool logger, int? currentBuild)
        {
            HeroesXml heroesXml = new HeroesXml(parentFile, xmlBaseFolder, logger, currentBuild);
            heroesXml.Parse();
            return heroesXml;
        }

        /// <summary>
        /// Returns a Hero object
        /// </summary>
        /// <param name="heroName">Can be the real hero name or alt name</param>
        /// <returns></returns>
        public Hero GetHeroInfo(string heroName)
        {
            string realName = GetRealHeroNameFromAltName(heroName);

            if (string.IsNullOrEmpty(realName))
                realName = heroName;

            if (HeroByHeroName.TryGetValue(realName, out Hero hero))
            {
                return hero;
            }
            else
            {
                return new Hero
                {
                    Name = heroName,
                    Franchise = HeroFranchise.Unknown,
                    HeroPortrait = new Uri($@"{ApplicationIconsPath}\HeroPortraits\{NoPortraitFound}", UriKind.Absolute),
                    LoadingPortrait = new Uri($@"{ApplicationIconsPath}\HeroLoadingScreenPortraits\{NoLoadingScreenFound}", UriKind.Absolute),
                    LeaderboardPortrait = new Uri($@"{ApplicationIconsPath}\HeroLeaderboardPortraits\{NoLeaderboardFound}", UriKind.Absolute),
                };
            }
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
        /// Returns the real hero name from the hero's attribute id
        /// </summary>
        /// <param name="attributeId">Four character hero id</param>
        /// <returns>Full hero name</returns>
        public string GetRealHeroNameFromAttributeId(string attributeId)
        {
            // no pick
            if (string.IsNullOrEmpty(attributeId))
                return string.Empty;

            if (RealHeroNameByAttributeId.TryGetValue(attributeId, out string heroName))
            {
                return heroName;
            }
            else
            {
                LogReferenceNameNotFound($"No hero name for attribute: {attributeId}");
                return null;
            }
        }

        public string GetRealHeroNameFromAltName(string altName)
        {
            // no pick
            if (string.IsNullOrEmpty(altName))
                return string.Empty;

            if (RealHeroNameByAlternativeName.TryGetValue(altName, out string realName))
                return realName;
            else
                return null;
        }

        /// <summary>
        /// Checks to see if the hero name exists
        /// </summary>
        /// <param name="heroName">Real name of hero or alt name</param>
        /// <returns>True if found</returns>
        public bool HeroExists(string heroName)
        {
            string realName = GetRealHeroNameFromAltName(heroName);

            if (string.IsNullOrEmpty(realName))
                realName = heroName;

            return HeroByHeroName.ContainsKey(realName);
        }

        /// <summary>
        /// Returns a list of (real) hero names for the given build
        /// </summary>
        /// <param name="build">The build number</param>
        /// <returns></returns>
        public List<string> GetListOfHeroes(int build)
        {
            List<string> heroes = new List<string>();
            foreach (var hero in HeroByHeroName)
            {
                if (hero.Value.BuildAvailable <= build)
                    heroes.Add(hero.Value.Name);
            }

            heroes.Sort();
            return heroes;
        }

        /// <summary>
        /// Returns the total amount of heroes (latest build)
        /// </summary>
        /// <returns></returns>
        public int TotalAmountOfHeroes()
        {
            return HeroByHeroName.Count;
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
                            Hero hero = new Hero()
                            {
                                AltName = reader.Name,
                            };

                            // get real name
                            // example: Anubarak-> (real) Anub'arak
                            hero.Name = reader["name"];
                            if (string.IsNullOrEmpty(hero.Name))
                                hero.Name = hero.AltName; // default to hero name

                            // set the build that the hero is added
                            hero.BuildAvailable = int.TryParse(reader["available"], out int buildAvailable) ? buildAvailable : 0;

                            // set attributeid from hero name
                            // example: Anub
                            hero.AttributeId = reader["attributeid"];

                            // set the franchise: classic, diablo, overwatch, starcraft, warcraft
                            hero.Franchise = Enum.TryParse(reader["franchise"], out HeroFranchise heroFranchise) ? heroFranchise : HeroFranchise.Unknown;

                            // set the hero type - melee or ranged
                            hero.Type = Enum.TryParse(reader["type"], out HeroType heroType) ? heroType : HeroType.Unknown;

                            // set the difficulty of the hero - easy/medium/hard/etc...
                            hero.Difficulty = Enum.TryParse(reader["difficulty"], out HeroDifficulty heroDifficulty) ? heroDifficulty : HeroDifficulty.Unknown;

                            // set portrait
                            hero.HeroPortrait = SetHeroPortraitUri(reader["portrait"]);

                            // set loading portrait
                            hero.LoadingPortrait = SetLoadingPortraitUri(reader["loading"]);

                            // set leaderboard portrait
                            hero.LeaderboardPortrait = SetLeaderboardPortraitUri(reader["leader"]);

                            RealHeroNameByAttributeId.Add(hero.AttributeId, hero.Name);
                            RealHeroNameByAlternativeName.Add(hero.AltName, hero.Name);

                            while (reader.Read() && reader.Name != hero.AltName)
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
                                            rolesList.Add(Enum.TryParse(role, out HeroRole heroRole) ? heroRole : HeroRole.Unknown);
                                        }

                                        hero.Roles = rolesList;
                                    }
                                    else if (reader.Name == "Aliases")
                                    {
                                        reader.Read();
                                        string[] aliases = reader.Value.Split(',');

                                        List<string> aliasList = new List<string>
                                        {
                                            hero.Name,
                                        };
                                        aliasList.AddRange(aliases);
                                        hero.Aliases = aliasList;

                                        // add the english name
                                        HeroRealNameByHeroAliasName.Add(hero.Name, hero.Name);

                                        // add all the other aliases
                                        foreach (var alias in aliases)
                                        {
                                            if (string.IsNullOrEmpty(alias))
                                                continue;

                                            if (HeroRealNameByHeroAliasName.ContainsKey(alias))
                                                throw new ArgumentException($"Alias already added to {hero.Name}: {alias}");

                                            HeroRealNameByHeroAliasName.Add(alias, hero.Name);
                                        }
                                    }
                                }
                            }

                            HeroByHeroName.Add(hero.Name, hero);
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
            if (string.IsNullOrEmpty(fileName))
                return null;

            return new Uri($@"{ApplicationIconsPath}\HeroPortraits\{fileName}", UriKind.Absolute);
        }

        private Uri SetLoadingPortraitUri(string fileName)
        {
            if (string.IsNullOrEmpty(fileName))
                return null;

            return new Uri($@"{ApplicationIconsPath}\HeroLoadingScreenPortraits\{fileName}", UriKind.Absolute);
        }

        private Uri SetLeaderboardPortraitUri(string fileName)
        {
            if (string.IsNullOrEmpty(fileName))
                return null;

            return new Uri($@"{ApplicationIconsPath}\HeroLeaderboardPortraits\{fileName}", UriKind.Absolute);
        }
    }
}
