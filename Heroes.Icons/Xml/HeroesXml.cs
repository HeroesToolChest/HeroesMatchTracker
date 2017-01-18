using System;
using System.Collections.Generic;
using System.Xml;

namespace Heroes.Icons.Xml
{
    internal class HeroesXml : XmlBase
    {
        private HeroesXml(string parentFile, string xmlBaseFolder)
        {
            XmlParentFile = parentFile;
            XmlBaseFolder = xmlBaseFolder;
        }

        /// <summary>
        /// key is attributeid, value is hero name
        /// </summary>
        public Dictionary<string, string> RealHeroNameByAttributeId { get; private set; } = new Dictionary<string, string>();

        /// <summary>
        /// key is real hero name, value alt name (if any)
        /// example: Anub'arak, Anubarak
        /// </summary>
        public Dictionary<string, string> AlternativeHeroNameByRealName { get; private set; } = new Dictionary<string, string>();

        /// <summary>
        /// key is alt hero name, value real name
        /// example: Anubarak, Anub'arak
        /// </summary>
        public Dictionary<string, string> RealHeroNameByAlternativeName { get; private set; } = new Dictionary<string, string>();

        /// <summary>
        /// key is real hero name
        /// value is HeroFrancise
        /// </summary>
        public Dictionary<string, HeroFranchise> HeroFranchiseByRealName { get; private set; } = new Dictionary<string, HeroFranchise>();

        /// <summary>
        /// key is real hero name
        /// value is HeroRole
        /// </summary>
        public Dictionary<string, List<HeroRole>> HeroRolesListByRealName { get; private set; } = new Dictionary<string, List<HeroRole>>();

        /// <summary>
        /// key is real hero name
        /// </summary>
        public Dictionary<string, Uri> HeroPortraitUriByRealName { get; private set; } = new Dictionary<string, Uri>();

        /// <summary>
        /// key is real hero name
        /// </summary>
        public Dictionary<string, Uri> HeroLoadingPortraitUriByRealName { get; private set; } = new Dictionary<string, Uri>();

        /// <summary>
        /// key is real hero name
        /// </summary>
        public Dictionary<string, Uri> HeroLeaderboardPortraitUriByRealName { get; private set; } = new Dictionary<string, Uri>();

        /// <summary>
        /// key is alias name
        /// </summary>
        public Dictionary<string, string> HeroRealNameByHeroAliasName { get; private set; } = new Dictionary<string, string>();

        public static HeroesXml Initialize(string parentFile, string xmlBaseFolder)
        {
            HeroesXml heroesXml = new HeroesXml(parentFile, xmlBaseFolder);
            heroesXml.Parse();
            return heroesXml;
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
