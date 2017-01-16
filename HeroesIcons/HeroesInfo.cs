using HeroesIcons.Xml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace HeroesIcons
{
    public class HeroesInfo : HeroesIconsBase
    {
        private int EarliestHeroesBuild;
        private int LatestHeroesBuild;
        private HeroesXml HeroesXml;
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

        private HeroesInfo()
        {
            HeroesXml = HeroesXml.Initialize("_AllHeroes.xml", "Heroes");
            MatchAwardsXml = MatchAwardsXml.Initialize("_AllMatchAwards.xml", "MatchAwards");
            MapBackgroundsXml = MapBackgroundsXml.Initialize("_AllMapBackgrounds.xml", "MapBackgrounds");
            HomeScreensXml = HomeScreensXml.Initialize("HomeScreens.xml", "HomeScreens");

            EarliestHeroesBuild = HeroesXml.EarliestHeroesBuild;
            LatestHeroesBuild = HeroesXml.LatestHeroesBuild;

            SetNonSupportHeroesWithSupportStat();
            SetPartyIcons();
            SetRoleIcons();
            SetFranchiseIcons();
            SetOtherIcons();
        }

        /// <summary>
        /// Initialize all Xml files and sets all icons
        /// </summary>
        /// <returns></returns>
        public static HeroesInfo Initialize()
        {
            return new HeroesInfo();
        }

        /// <summary>
        /// Reinitialize a specific heroes xml build
        /// </summary>
        /// <param name="replayBuild">The replay build to load</param>
        /// <returns></returns>
        public void ReInitializeSpecificHeroesXml(int? build)
        {
            if (build == null)
                return;

            if (build < EarliestHeroesBuild)
                build = EarliestHeroesBuild;
            else if (build > LatestHeroesBuild)
                build = LatestHeroesBuild;

            // if the build is already loaded into memory then don't reload
            if (build != HeroesXml.CurrentLoadedHeroesBuild)
            {
                HeroesXml = HeroesXml.Initialize("_AllHeroes.xml", "Heroes", build);
            }
        }

        /// <summary>
        /// Reinitialize the latest heroes xml build
        /// </summary>
        public void ReInitializeLatestHeroesXml()
        {
            // if the build is already loaded into memory then don't reload
            if (HeroesXml.CurrentLoadedHeroesBuild != LatestHeroesBuild)
                HeroesXml = HeroesXml.Initialize("_AllHeroes.xml", "Heroes");
        }

        #region public methods

        #region HeroesXml
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
                return new BitmapImage(new Uri($@"{ApplicationPath}Talents\_Generic\storm_ui_icon_no_pick.dds", UriKind.Absolute));


            // not found
            if (!HeroesXml.TalentIcons.TryGetValue(nameOfHeroTalent, out talent))
            {
                Task.Run(() => Log(ImageMissingLogName, $"Talent icon: {nameOfHeroTalent}"));

                return new BitmapImage(new Uri($@"{ApplicationPath}Talents\_Generic\storm_ui_icon_default.dds", UriKind.Absolute));
            }
                
            try
            {
                return new BitmapImage(talent.Item2);
            }
            catch (Exception)
            {
                Task.Run(() => Log(ImageMissingLogName, $"Talent icon: {nameOfHeroTalent}"));
                return new BitmapImage(new Uri($@"{ApplicationPath}Talents\_Generic\storm_ui_icon_default.dds", UriKind.Absolute));
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
                return new BitmapImage(new Uri($@"{ApplicationPath}HeroPortraits\storm_ui_ingame_heroselect_btn_nopick.dds", UriKind.Absolute));


            // translation
            if (!HeroesXml.HeroTranslationsNames.TryGetValue(realHeroName, out realHeroName))
            {
                Task.Run(() => Log(ImageMissingLogName, $"Hero portrait: {realHeroName}"));

                return new BitmapImage(new Uri($@"{ApplicationPath}HeroPortraits\storm_ui_ingame_heroselect_btn_notfound.dds", UriKind.Absolute));
            }

            // not found
            if (!HeroesXml.HeroPortraits.TryGetValue(realHeroName, out uri))
            {
                Task.Run(() => Log(ImageMissingLogName, $"Hero portrait: {realHeroName}"));

                return new BitmapImage(new Uri($@"{ApplicationPath}HeroPortraits\storm_ui_ingame_heroselect_btn_notfound.dds", UriKind.Absolute));
            }
         
            try
            {
                return new BitmapImage(uri);
            }
            catch (Exception)
            {
                Task.Run(() => Log(ImageMissingLogName, $"Hero portrait: {realHeroName}"));
                return new BitmapImage(new Uri($@"{ApplicationPath}HeroPortraits\storm_ui_ingame_heroselect_btn_notfound.dds", UriKind.Absolute));
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
                return new BitmapImage(new Uri($@"{ApplicationPath}HeroLoadingScreenPortraits\storm_ui_ingame_hero_loadingscreen_nopick.dds", UriKind.Absolute));

            // translation
            if (!HeroesXml.HeroTranslationsNames.TryGetValue(realHeroName, out realHeroName))
            {
                Task.Run(() => Log(ImageMissingLogName, $"Loading hero portrait: {realHeroName}"));

                return new BitmapImage(new Uri($@"{ApplicationPath}HeroLoadingScreenPortraits\storm_ui_ingame_hero_loadingscreen_notfound.dds", UriKind.Absolute));
            }

            // not found
            if (!HeroesXml.LoadingPortraits.TryGetValue(realHeroName, out uri))
            {
                Task.Run(() => Log(ImageMissingLogName, $"Loading hero portrait: {realHeroName}"));

                return new BitmapImage(new Uri($@"{ApplicationPath}HeroLoadingScreenPortraits\storm_ui_ingame_hero_loadingscreen_notfound.dds", UriKind.Absolute));
            }

            try
            {
                return new BitmapImage(uri);
            }
            catch (Exception)
            {
                Task.Run(() => Log(ImageMissingLogName, $"Loading hero portrait: {realHeroName}"));
                return new BitmapImage(new Uri($@"{ApplicationPath}HeroLoadingScreenPortraits\storm_ui_ingame_hero_loadingscreen_notfound.dds", UriKind.Absolute));
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
                return new BitmapImage(new Uri($@"{ApplicationPath}HeroLeaderboardPortraits\storm_ui_ingame_hero_leaderboard_nopick.dds", UriKind.Absolute));

            // translation
            if (!HeroesXml.HeroTranslationsNames.TryGetValue(realHeroName, out realHeroName))
            {
                Task.Run(() => Log(ImageMissingLogName, $"Leader hero portrait: {realHeroName}"));

                return new BitmapImage(new Uri($@"{ApplicationPath}HeroLeaderboardPortraits\storm_ui_ingame_hero_leaderboard_notfound.dds", UriKind.Absolute));
            }

            // not found
            if (!HeroesXml.LeaderboardPortraits.TryGetValue(realHeroName, out uri))
            {
                Task.Run(() => Log(ImageMissingLogName, $"Leader hero portrait: {realHeroName}"));

                return new BitmapImage(new Uri($@"{ApplicationPath}HeroLeaderboardPortraits\storm_ui_ingame_hero_leaderboard_notfound.dds", UriKind.Absolute));
            }
           
            try
            {
                return new BitmapImage(uri);
            }
            catch (Exception)
            {
                Task.Run(() => Log(ImageMissingLogName, $"Leader hero portrait: {realHeroName}"));
                return new BitmapImage(new Uri($@"{ApplicationPath}HeroLeaderboardPortraits\storm_ui_ingame_hero_leaderboard_notfound.dds", UriKind.Absolute));
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
            if (!HeroesXml.TalentIcons.TryGetValue(nameOfHeroTalent, out talent))
            {
                Task.Run(() => Log(ReferenceLogName, $"No name for reference: {nameOfHeroTalent}"));

                return nameOfHeroTalent;
            }

            return talent.Item1;
        }

        /// <summary>
        /// Returns a dictionary of all the talents of a hero
        /// </summary>
        /// <param name="realHeroName">real hero name</param>
        /// <returns></returns>
        public Dictionary<TalentTier, List<string>> GetTalentsForHero(string realHeroName)
        {
            // translation
            if (!HeroesXml.HeroTranslationsNames.TryGetValue(realHeroName, out realHeroName))
            {
                Task.Run(() => Log(ReferenceLogName, $"No hero real name found [{nameof(GetTalentsForHero)}]: {realHeroName}"));
                return null;
            }

            Dictionary<TalentTier, List<string>> talents;
            if (HeroesXml.HeroesListOfTalents.TryGetValue(realHeroName, out talents))
            {
                return talents;
            }
            else
            {
                Task.Run(() => Log(ReferenceLogName, $"No hero real name found [{nameof(GetTalentsForHero)}]: {realHeroName}"));
                return null;
            }

        }

        /// <summary>
        /// Returns the real hero name from the hero's attribute id
        /// </summary>
        /// <param name="attributeId">Four character hero id</param>
        /// <returns>Full hero name</returns>
        public string GetRealHeroNameFromAttId(string attributeId)
        {
            string heroName;

            // no pick
            if (string.IsNullOrEmpty(attributeId))
                return null;

            // not found
            if (!HeroesXml.HeroNamesFromAttributeId.TryGetValue(attributeId, out heroName))
            {
                Task.Run(() => Log(ReferenceLogName, $"No hero name for reference: {attributeId}"));

                return "Hero not found";
            }

            return heroName;
        }

        public string GetAltNameFromRealHeroName(string realName)
        {
            string altName;

            // no pick
            if (string.IsNullOrEmpty(realName))
                return null;

            // not found
            if (!HeroesXml.HeroesAlternativeName.TryGetValue(realName, out altName))
            {
                Task.Run(() => Log(ReferenceLogName, $"No hero alt name for reference: {realName}"));

                return "Hero alt name not found";
            }

            return altName;
        }

        public string GetRealHeroNameFromAltName(string altName)
        {
            string realName;

            // no pick
            if (string.IsNullOrEmpty(altName))
                return null;

            // not found
            if (!HeroesXml.HeroesAlternativeName.TryGetValue(altName, out realName))
            {
                Task.Run(() => Log(ReferenceLogName, $"No hero real name for reference: {altName}"));

                return "Hero real name not found";
            }

            return realName;
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
                return HeroesXml.HeroesRealName.ContainsKey(heroName);
            else
                return HeroesXml.HeroesAlternativeName.ContainsKey(heroName);
        }

        /// <summary>
        /// Returns the hero's role: Warrior, Assassin, Support, or Specialist. Will return Unknown if hero not found
        /// </summary>
        /// <param name="realName">Heroes real name</param>
        /// <returns>HeroRole</returns>
        public HeroRole GetHeroRole(string realName)
        {
            HeroRole role;

            // translation
            if (!HeroesXml.HeroTranslationsNames.TryGetValue(realName, out realName))
                return HeroRole.Unknown;

            if (HeroesXml.HeroesRole.TryGetValue(realName, out role))
                return role;
            else
                return HeroRole.Unknown;

        }

        /// <summary>
        /// Returns the hero's franchise. Will return Unknown if hero not found
        /// </summary>
        /// <param name="realName">Heroes real name</param>
        /// <returns>HeroRole</returns>
        public HeroFranchise GetHeroFranchise(string realName)
        {
            HeroFranchise franchise;

            if (HeroesXml.HeroesFranchise.TryGetValue(realName, out franchise))
                return franchise;
            else
                return HeroFranchise.Unknown;
        }

        public List<string> GetListOfHeroes()
        {
            List<string> heroes = new List<string>();
            foreach (var hero in HeroesXml.HeroesRealName)
            {
                heroes.Add(hero.Key);
            }
            heroes.Sort();
            return heroes;
        }

        public int TotalAmountOfHeroes()
        {
            return HeroesXml.HeroesRealName.Count;
        }

        /// <summary>
        /// Returns a TalentDescription object which contains the short and full descriptions of the talent
        /// </summary>
        /// <param name="talentReferenceName">Talent reference name</param>
        /// <returns></returns>
        public TalentDescription GetTalentDescriptions(string talentReferenceName)
        {
            TalentDescription talentDesc = new TalentDescription(string.Empty, string.Empty);

            if (string.IsNullOrEmpty(talentReferenceName) || !HeroesXml.TalentDesciptions.ContainsKey(talentReferenceName))
                return talentDesc;

            HeroesXml.TalentDesciptions.TryGetValue(talentReferenceName, out talentDesc);

            return talentDesc;
        }
        #endregion Heroes Xml

        #region MatchAwardsXml
        /// <summary>
        /// Returns the MVPScreen award BitmapImage of the given mvpAwardType annd color
        /// </summary>
        /// <param name="mvpAwardType">Reference name of award</param>
        /// <param name="mvpColor">Color of icon</param>
        /// <param name="awardName"></param>
        /// <returns></returns>
        public BitmapImage GetMVPScreenAward(string mvpAwardType, MVPScreenColor mvpColor, out string awardName)
        {
            try
            {
                var award = MatchAwardsXml.MVPScreenAwards[mvpAwardType];
                var uriString = award.Item2.AbsoluteUri.Replace("%7BmvpColor%7D", mvpColor.ToString());

                awardName = award.Item1;

                return new BitmapImage(new Uri(uriString, UriKind.Absolute));
            }
            catch (Exception)
            {
                Task.Run(() => Log(ImageMissingLogName, $"MVP screen award type: {mvpAwardType}"));
                awardName = "Unknown";
                return null;
            }
        }

        /// <summary>
        /// Returns the ScoreScreen award BitmapImage of the given mvpAwardType annd color
        /// </summary>
        /// <param name="mvpAwardType">Reference name of award</param>
        /// <param name="mvpColor">Color of icon</param>
        /// <param name="awardName"></param>
        /// <returns></returns>
        public BitmapImage GetMVPScoreScreenAward(string mvpAwardType, MVPScoreScreenColor mvpColor, out string awardName)
        {
            try
            {
                var award = MatchAwardsXml.MVPScoreScreenAwards[mvpAwardType];
                var uriString = award.Item2.AbsoluteUri.Replace("%7BmvpColor%7D", mvpColor.ToString());

                awardName = award.Item1;

                return new BitmapImage(new Uri(uriString, UriKind.Absolute));
            }
            catch (Exception)
            {
                Task.Run(() => Log(ImageMissingLogName, $"MVP score screen award type: {mvpAwardType}"));
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
            if (string.IsNullOrEmpty(mvpAwardType) || !MatchAwardsXml.MVPAwardDescriptions.ContainsKey(mvpAwardType))
                return string.Empty;

            return MatchAwardsXml.MVPAwardDescriptions[mvpAwardType];
        }

        /// <summary>
        /// Returns a list of all the match awards (reference names)
        /// </summary>
        /// <returns></returns>
        public List<string> GetMatchAwardsList()
        {
            return new List<string>(MatchAwardsXml.MVPScoreScreenAwards.Keys);
        }
        #endregion MatchAwardsXml

        #region MapBackgroundsXml
        public BitmapImage GetMapBackground(string mapRealName, bool useSmallImage = false)
        {
            try
            {
                MapBackgroundsXml.MapTranslationsNames.TryGetValue(mapRealName, out mapRealName);

                if (useSmallImage == false)
                    return new BitmapImage(MapBackgroundsXml.MapBackgrounds[mapRealName]);
                else
                    return new BitmapImage(MapBackgroundsXml.MapBackgroundsSmall[mapRealName]);
            }
            catch (Exception)
            {
                Task.Run(() => Log(ImageMissingLogName, $"Map background: {mapRealName}"));
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
            if (!MapBackgroundsXml.MapTranslationsNames.TryGetValue(mapRealName, out mapRealName))
                return Colors.Black;

            Color color;
            if (MapBackgroundsXml.MapBackgroundFontGlowColor.TryGetValue(mapRealName, out color))
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
            return new List<string>(MapBackgroundsXml.MapBackgrounds.Keys);
        }

        /// <summary>
        /// Returns a list of all maps, except custom only maps
        /// </summary>
        /// <returns></returns>
        public List<string> GetMapsListExceptCustomOnly()
        {
            var allMaps = new Dictionary<string, Uri>(MapBackgroundsXml.MapBackgrounds);
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
            return MapBackgroundsXml.MapBackgrounds.ContainsKey(mapName);
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
                Task.Run(() => Log(ImageMissingLogName, $"Party Icons: {partyIconColor}"));
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
                Task.Run(() => Log(ImageMissingLogName, $"Other Icons: {otherIcon}"));
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
                Task.Run(() => Log(ImageMissingLogName, $"Hero role icon: {heroRole}"));
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
                Task.Run(() => Log(ImageMissingLogName, $"Franchise icons: {heroFranchise}"));
                return null;
            }
        }

        public bool IsNonSupportHeroWithHealingStat(string realHeroName)
        {
            return HeroesNonSupportHealingStat.ContainsKey(realHeroName);
        }

        public List<int> GetListOfHeroesBuilds()
        {
            return HeroesXml.Builds;
        }
        #endregion public methods

        #region private methods
        private void SetNonSupportHeroesWithSupportStat()
        {
            HeroesNonSupportHealingStat.Add("Medivh", HeroRole.Support);
        }

        private void SetPartyIcons()
        {
            PartyIcons.Add(PartyIconColor.Purple, new Uri($"{ApplicationPath}PartyIcons/ui_ingame_loadscreen_partylink_purple.png", UriKind.Absolute));
            PartyIcons.Add(PartyIconColor.Yellow, new Uri($"{ApplicationPath}PartyIcons/ui_ingame_loadscreen_partylink_yellow.png", UriKind.Absolute));
            PartyIcons.Add(PartyIconColor.Brown, new Uri($"{ApplicationPath}PartyIcons/ui_ingame_loadscreen_partylink_brown.png", UriKind.Absolute));
            PartyIcons.Add(PartyIconColor.Teal, new Uri($"{ApplicationPath}PartyIcons/ui_ingame_loadscreen_partylink_teal.png", UriKind.Absolute));
        }

        private void SetRoleIcons()
        {
            RoleIcons.Add(HeroRole.Warrior, new Uri($"{ApplicationPath}Roles/hero_role_warrior.png", UriKind.Absolute));
            RoleIcons.Add(HeroRole.Assassin, new Uri($"{ApplicationPath}Roles/hero_role_assassin.png", UriKind.Absolute));
            RoleIcons.Add(HeroRole.Support, new Uri($"{ApplicationPath}Roles/hero_role_support.png", UriKind.Absolute));
            RoleIcons.Add(HeroRole.Specialist, new Uri($"{ApplicationPath}Roles/hero_role_specialist.png", UriKind.Absolute));
        }

        private void SetFranchiseIcons()
        {
            FranchiseIcons.Add(HeroFranchise.Classic, new Uri($"{ApplicationPath}Roles/hero_franchise_classic.png", UriKind.Absolute));
            FranchiseIcons.Add(HeroFranchise.Diablo, new Uri($"{ApplicationPath}Roles/hero_franchise_diablo.png", UriKind.Absolute));
            FranchiseIcons.Add(HeroFranchise.Overwatch, new Uri($"{ApplicationPath}Roles/hero_franchise_overwatch.png", UriKind.Absolute));
            FranchiseIcons.Add(HeroFranchise.Starcraft, new Uri($"{ApplicationPath}Roles/hero_franchise_starcraft.png", UriKind.Absolute));
            FranchiseIcons.Add(HeroFranchise.Warcraft, new Uri($"{ApplicationPath}Roles/hero_franchise_warcraft.png", UriKind.Absolute));
        }

        private void SetOtherIcons()
        {
            OtherIcons.Add(OtherIcon.Quest, new Uri($"{ApplicationPath}storm_ui_ingame_talentpanel_upgrade_quest_icon.dds", UriKind.Absolute));
            OtherIcons.Add(OtherIcon.Silence, new Uri($"{ApplicationPath}storm_ui_silencepenalty.dds", UriKind.Absolute));
        }

        private void Log(string fileName, string message)
        {
            using (StreamWriter writer = new StreamWriter($"logs/{fileName}", true))
            {
                writer.WriteLine($"[{HeroesXml.CurrentLoadedHeroesBuild}] {message}");
            }
        }
        #endregion private methods
    }
}
