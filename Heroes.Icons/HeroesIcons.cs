using Heroes.Icons.Xml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Heroes.Icons
{
    public class HeroesIcons : HeroesBase
    {
        private bool Logger;
        private int EarliestHeroesBuild;
        private int LatestHeroesBuild;
        private HeroesXml HeroesXml;
        private HeroBuildsXml HeroBuildsXml;
        private HeroBuildsXml HeroBuildsXmlLatest;
        private MatchAwardsXml MatchAwardsXml;
        private MapBackgroundsXml MapBackgroundsXml;
        private HomeScreensXml HomeScreensXml;

        /// <summary>
        /// key is real hero name
        /// value is HeroRole
        /// </summary>
        private Dictionary<string, HeroRole> HeroesNonSupportHealingStat = new Dictionary<string, HeroRole>();
        private Dictionary<PartyIconColor, Uri> PartyIcons = new Dictionary<PartyIconColor, Uri>();
        private Dictionary<HeroRole, Uri> RoleIcons = new Dictionary<HeroRole, Uri>();
        private Dictionary<HeroFranchise, Uri> FranchiseIcons = new Dictionary<HeroFranchise, Uri>();
        private Dictionary<OtherIcon, Uri> OtherIcons = new Dictionary<OtherIcon, Uri>();

        private HeroesIcons(bool logger)
        {
            Logger = logger;

            HeroesXml = HeroesXml.Initialize("Heroes.xml", "Heroes");
            HeroBuildsXmlLatest = HeroBuildsXml = HeroBuildsXml.Initialize("_AllHeroes.xml", "HeroBuilds", HeroesXml);
            MatchAwardsXml = MatchAwardsXml.Initialize("_AllMatchAwards.xml", "MatchAwards");
            MapBackgroundsXml = MapBackgroundsXml.Initialize("_AllMapBackgrounds.xml", "MapBackgrounds");
            HomeScreensXml = HomeScreensXml.Initialize("HomeScreens.xml", "HomeScreens");

            EarliestHeroesBuild = HeroBuildsXml.EarliestHeroesBuild;
            LatestHeroesBuild = HeroBuildsXml.LatestHeroesBuild;

            SetNonSupportHeroesWithSupportStat();
            SetPartyIcons();
            SetRoleIcons();
            SetFranchiseIcons();
            SetOtherIcons();
        }

        /// <summary>
        /// Initialize all Xml files and sets all icons/images
        /// </summary>
        /// <param name="logger">Set to true to enable logging</param>
        /// <returns></returns>
        public static HeroesIcons Initialize(bool logger = false)
        {
            return new HeroesIcons(logger);
        }

        /// <summary>
        /// Load a specific build, other than the latest one
        /// </summary>
        /// <param name="replayBuild">The replay build to load</param>
        /// <returns></returns>
        public void LoadHeroesBuild(int? build)
        {
            if (build == null)
                return;
            else if (build < EarliestHeroesBuild)
                build = EarliestHeroesBuild;
            else if (build > LatestHeroesBuild)
                build = LatestHeroesBuild;

            if (build == LatestHeroesBuild)
            {
                // if the build is already loaded into memory then don't reload
                if (build != HeroBuildsXml.CurrentLoadedHeroesBuild)
                    HeroBuildsXml = HeroBuildsXmlLatest;
            }
            else
            {
                HeroBuildsXml = HeroBuildsXml.Initialize("_AllHeroes.xml", "Heroes", HeroesXml, build);
            }
        }

        #region public methods

        #region HeroesXml

        /// <summary>
        /// Returns a BitmapImage of the talent
        /// </summary>
        /// <param name="talentReferenceName">Reference talent name</param>
        /// <returns>BitmapImage of the talent</returns>
        public BitmapImage GetTalentIcon(string talentReferenceName)
        {
            Tuple<string, Uri> talent;

            // no pick
            if (string.IsNullOrEmpty(talentReferenceName))
                return HeroesBitmapImage(@"Talents\_Generic\storm_ui_icon_no_pick.dds");

            if (HeroBuildsXml.RealTalentNameUriByReferenceName.TryGetValue(talentReferenceName, out talent))
            {
                return new BitmapImage(talent.Item2);
            }
            else
            {
                if (Logger)
                    LogReferenceNameNotFound($"Talent icon: {talentReferenceName}");

                return HeroesBitmapImage(@"Talents\_Generic\storm_ui_icon_default.dds");
            }
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

            if (HeroesXml.HeroPortraitUriByRealName.TryGetValue(realHeroName, out uri))
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

            if (HeroesXml.HeroLoadingPortraitUriByRealName.TryGetValue(realHeroName, out uri))
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

            if (HeroesXml.HeroLeaderboardPortraitUriByRealName.TryGetValue(realHeroName, out uri))
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
        /// Returns the talent name from the talent reference name
        /// </summary>
        /// <param name="talentReferenceName">Reference talent name</param>
        /// <returns>Talent name</returns>
        public string GetTrueTalentName(string talentReferenceName)
        {
            Tuple<string, Uri> talent;

            // no pick
            if (string.IsNullOrEmpty(talentReferenceName))
                return "No pick";

            if (HeroBuildsXml.RealTalentNameUriByReferenceName.TryGetValue(talentReferenceName, out talent))
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
        public Dictionary<TalentTier, List<string>> GetTalentsForHero(string realHeroName)
        {
            Dictionary<TalentTier, List<string>> talents;
            if (HeroBuildsXml.HeroTalentsListByRealName.TryGetValue(realHeroName, out talents))
            {
                return talents;
            }
            else
            {
                if (Logger)
                    LogReferenceNameNotFound($"No hero real name found [{nameof(GetTalentsForHero)}]: {realHeroName}");

                return null;
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

            if (HeroesXml.RealHeroNameByAttributeId.TryGetValue(attributeId, out heroName))
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

            if (HeroesXml.AlternativeHeroNameByRealName.TryGetValue(realName, out altName))
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

            if (HeroesXml.RealHeroNameByAlternativeName.TryGetValue(altName, out realName))
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
                return HeroesXml.AlternativeHeroNameByRealName.ContainsKey(heroName);
            else
                return HeroesXml.RealHeroNameByAlternativeName.ContainsKey(heroName);
        }

        /// <summary>
        /// Returns the hero's list of roles. Multiclass will be first if hero has multiple roles. Will return a role of Unknown if hero name not found.
        /// </summary>
        /// <param name="realName">Hero real name</param>
        /// <returns>HeroRole</returns>
        public List<HeroRole> GetHeroRole(string realName)
        {
            List<HeroRole> roleList;

            if (HeroesXml.HeroRolesListByRealName.TryGetValue(realName, out roleList))
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

            if (HeroesXml.HeroFranchiseByRealName.TryGetValue(realName, out franchise))
                return franchise;
            else
                return HeroFranchise.Unknown;
        }

        public List<string> GetListOfHeroes()
        {
            List<string> heroes = new List<string>();
            foreach (var hero in HeroesXml.AlternativeHeroNameByRealName)
            {
                heroes.Add(hero.Key);
            }

            heroes.Sort();
            return heroes;
        }

        public int TotalAmountOfHeroes()
        {
            return HeroesXml.AlternativeHeroNameByRealName.Count;
        }

        /// <summary>
        /// Returns a TalentTooltip object which contains the short and full tooltips of the talent
        /// </summary>
        /// <param name="talentReferenceName">Talent reference name</param>
        /// <returns></returns>
        public TalentTooltip GetTalentTooltips(string talentReferenceName)
        {
            TalentTooltip talentTooltip = new TalentTooltip(string.Empty, string.Empty);

            if (string.IsNullOrEmpty(talentReferenceName) || !HeroBuildsXml.HeroTalentTooltipsByRealName.ContainsKey(talentReferenceName))
                return talentTooltip;

            HeroBuildsXml.HeroTalentTooltipsByRealName.TryGetValue(talentReferenceName, out talentTooltip);

            return talentTooltip;
        }
        #endregion Heroes Xml

        #region MatchAwardsXml

        /// <summary>
        /// Returns the MVPScreen award BitmapImage of the given mvpAwardType and color
        /// </summary>
        /// <param name="mvpAwardType">Reference name of award</param>
        /// <param name="mvpColor">Color of icon</param>
        /// <param name="awardName"></param>
        /// <returns></returns>
        public BitmapImage GetMVPScreenAward(string mvpAwardType, MVPScreenColor mvpColor, out string awardName)
        {
            try
            {
                var award = MatchAwardsXml.MVPScreenAwardByAwardType[mvpAwardType];
                var uriString = award.Item2.AbsoluteUri.Replace("%7BmvpColor%7D", mvpColor.ToString());

                awardName = award.Item1;

                return new BitmapImage(new Uri(uriString, UriKind.Absolute));
            }
            catch (Exception)
            {
                LogReferenceNameNotFound($"MVP screen award type: {mvpAwardType}");
                awardName = "Unknown";
                return null;
            }
        }

        /// <summary>
        /// Returns the ScoreScreen award BitmapImage of the given mvpAwardType and color
        /// </summary>
        /// <param name="mvpAwardType">Reference name of award</param>
        /// <param name="mvpColor">Color of icon</param>
        /// <param name="awardName"></param>
        /// <returns></returns>
        public BitmapImage GetMVPScoreScreenAward(string mvpAwardType, MVPScoreScreenColor mvpColor, out string awardName)
        {
            try
            {
                var award = MatchAwardsXml.MVPScoreScreenAwardByAwardType[mvpAwardType];
                var uriString = award.Item2.AbsoluteUri.Replace("%7BmvpColor%7D", mvpColor.ToString());

                awardName = award.Item1;

                return new BitmapImage(new Uri(uriString, UriKind.Absolute));
            }
            catch (Exception)
            {
                LogReferenceNameNotFound($"MVP score screen award type: {mvpAwardType}");
                awardName = "Unknown";
                return new BitmapImage(null);
            }
        }

        /// <summary>
        /// Returns the decription of the award
        /// </summary>
        /// <param name="mvpAwardType">Reference name of award</param>
        /// <returns></returns>
        public string GetMatchAwardDescription(string mvpAwardType)
        {
            if (string.IsNullOrEmpty(mvpAwardType) || !MatchAwardsXml.MVPAwardDescriptionByAwardType.ContainsKey(mvpAwardType))
                return string.Empty;

            return MatchAwardsXml.MVPAwardDescriptionByAwardType[mvpAwardType];
        }

        /// <summary>
        /// Returns a list of all the match awards (reference names)
        /// </summary>
        /// <returns></returns>
        public List<string> GetMatchAwardsList()
        {
            return new List<string>(MatchAwardsXml.MVPScoreScreenAwardByAwardType.Keys);
        }
        #endregion MatchAwardsXml

        #region MapBackgroundsXml
        public BitmapImage GetMapBackground(string mapRealName, bool useSmallImage = false)
        {
            try
            {
                if (useSmallImage == false)
                    return new BitmapImage(MapBackgroundsXml.MapUriByMapRealName[mapRealName]);
                else
                    return new BitmapImage(MapBackgroundsXml.MapSmallUriByMapRealName[mapRealName]);
            }
            catch (Exception)
            {
                LogReferenceNameNotFound($"Map background: {mapRealName}");
                return null;
            }
        }

        /// <summary>
        /// Returns the color associated with the map, returns black if map not found
        /// </summary>
        /// <param name="mapRealName">Real map name</param>
        /// <returns></returns>
        public Color GetMapBackgroundFontGlowColor(string mapRealName)
        {
            Color color;
            if (MapBackgroundsXml.MapFontGlowColorByMapRealName.TryGetValue(mapRealName, out color))
                return color;
            else
                return Colors.Black;
        }

        /// <summary>
        /// Returns a list of all maps
        /// </summary>
        /// <returns></returns>
        public List<string> GetMapsList()
        {
            return new List<string>(MapBackgroundsXml.MapUriByMapRealName.Keys);
        }

        /// <summary>
        /// Returns a list of all maps, except custom only maps
        /// </summary>
        /// <returns></returns>
        public List<string> GetMapsListExceptCustomOnly()
        {
            var allMaps = new Dictionary<string, Uri>(MapBackgroundsXml.MapUriByMapRealName);
            foreach (var customMap in GetCustomOnlyMapsList())
            {
                if (allMaps.ContainsKey(customMap))
                {
                    allMaps.Remove(customMap);
                }
            }

            return new List<string>(allMaps.Keys);
        }

        /// <summary>
        /// Returns a list of custom only maps
        /// </summary>
        /// <returns></returns>
        public List<string> GetCustomOnlyMapsList()
        {
            return MapBackgroundsXml.CustomOnlyMaps;
        }

        /// <summary>
        /// Returns true if mapName is a valid name
        /// </summary>
        /// <param name="mapName">The map name that needs to be checked</param>
        /// <returns></returns>
        public bool IsValidMapName(string mapName)
        {
            return MapBackgroundsXml.MapUriByMapRealName.ContainsKey(mapName);
        }
        #endregion MapBackgroundsXml

        #region HomeScreensXml
        public List<Tuple<BitmapImage, Color>> GetListOfHomeScreens()
        {
            return HomeScreensXml.HomeScreenBackgrounds;
        }
        #endregion HomeScreensXml

        public BitmapImage GetPartyIcon(PartyIconColor partyIconColor)
        {
            try
            {
                return new BitmapImage(PartyIcons[partyIconColor]);
            }
            catch (Exception)
            {
                LogReferenceNameNotFound($"Party Icons: {partyIconColor}");
                return null;
            }
        }

        public BitmapImage GetOtherIcon(OtherIcon otherIcon)
        {
            try
            {
                return new BitmapImage(OtherIcons[otherIcon]);
            }
            catch (Exception)
            {
                LogReferenceNameNotFound($"Other Icons: {otherIcon}");
                return null;
            }
        }

        public BitmapImage GetRoleIcon(HeroRole heroRole)
        {
            try
            {
                return new BitmapImage(RoleIcons[heroRole]);
            }
            catch (Exception)
            {
                LogReferenceNameNotFound($"Hero role icon: {heroRole}");
                return null;
            }
        }

        public BitmapImage GetFranchiseIcon(HeroFranchise heroFranchise)
        {
            try
            {
                return new BitmapImage(FranchiseIcons[heroFranchise]);
            }
            catch (Exception)
            {
                LogReferenceNameNotFound($"Franchise icons: {heroFranchise}");
                return null;
            }
        }

        public bool IsNonSupportHeroWithHealingStat(string realHeroName)
        {
            return HeroesNonSupportHealingStat.ContainsKey(realHeroName);
        }

        public List<int> GetListOfHeroesBuilds()
        {
            return HeroBuildsXml.Builds;
        }
        #endregion public methods

        #region private methods
        private void SetNonSupportHeroesWithSupportStat()
        {
            HeroesNonSupportHealingStat.Add("Medivh", HeroRole.Support);
        }

        private void SetPartyIcons()
        {
            PartyIcons.Add(PartyIconColor.Purple, new Uri($"{ApplicationIconsPath}PartyIcons/ui_ingame_loadscreen_partylink_purple.png", UriKind.Absolute));
            PartyIcons.Add(PartyIconColor.Yellow, new Uri($"{ApplicationIconsPath}PartyIcons/ui_ingame_loadscreen_partylink_yellow.png", UriKind.Absolute));
            PartyIcons.Add(PartyIconColor.Brown, new Uri($"{ApplicationIconsPath}PartyIcons/ui_ingame_loadscreen_partylink_brown.png", UriKind.Absolute));
            PartyIcons.Add(PartyIconColor.Teal, new Uri($"{ApplicationIconsPath}PartyIcons/ui_ingame_loadscreen_partylink_teal.png", UriKind.Absolute));
        }

        private void SetRoleIcons()
        {
            RoleIcons.Add(HeroRole.Warrior, new Uri($"{ApplicationIconsPath}Roles/hero_role_warrior.png", UriKind.Absolute));
            RoleIcons.Add(HeroRole.Assassin, new Uri($"{ApplicationIconsPath}Roles/hero_role_assassin.png", UriKind.Absolute));
            RoleIcons.Add(HeroRole.Support, new Uri($"{ApplicationIconsPath}Roles/hero_role_support.png", UriKind.Absolute));
            RoleIcons.Add(HeroRole.Specialist, new Uri($"{ApplicationIconsPath}Roles/hero_role_specialist.png", UriKind.Absolute));
        }

        private void SetFranchiseIcons()
        {
            FranchiseIcons.Add(HeroFranchise.Classic, new Uri($"{ApplicationIconsPath}Roles/hero_franchise_classic.png", UriKind.Absolute));
            FranchiseIcons.Add(HeroFranchise.Diablo, new Uri($"{ApplicationIconsPath}Roles/hero_franchise_diablo.png", UriKind.Absolute));
            FranchiseIcons.Add(HeroFranchise.Overwatch, new Uri($"{ApplicationIconsPath}Roles/hero_franchise_overwatch.png", UriKind.Absolute));
            FranchiseIcons.Add(HeroFranchise.Starcraft, new Uri($"{ApplicationIconsPath}Roles/hero_franchise_starcraft.png", UriKind.Absolute));
            FranchiseIcons.Add(HeroFranchise.Warcraft, new Uri($"{ApplicationIconsPath}Roles/hero_franchise_warcraft.png", UriKind.Absolute));
        }

        private void SetOtherIcons()
        {
            OtherIcons.Add(OtherIcon.Quest, new Uri($"{ApplicationIconsPath}storm_ui_ingame_talentpanel_upgrade_quest_icon.dds", UriKind.Absolute));
            OtherIcons.Add(OtherIcon.Silence, new Uri($"{ApplicationIconsPath}storm_ui_silencepenalty.dds", UriKind.Absolute));
        }

        private void LogMissingImage(string message)
        {
            using (StreamWriter writer = new StreamWriter($"{LogFileName}/{ImageMissingLogName}", true))
            {
                writer.WriteLine($"[{HeroBuildsXml.CurrentLoadedHeroesBuild}] {message}");
            }
        }

        private void LogReferenceNameNotFound(string message)
        {
            using (StreamWriter writer = new StreamWriter($"{LogFileName}/{ReferenceLogName}", true))
            {
                writer.WriteLine($"[{HeroBuildsXml.CurrentLoadedHeroesBuild}] {message}");
            }
        }
        #endregion private methods
    }
}
