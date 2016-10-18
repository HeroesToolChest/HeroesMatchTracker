using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Xml;
using System.Xml.Linq;

namespace HeroesIcons
{
    public class HeroesInfo
    {
        private readonly string ImageMissingLogName = "_ImageMissingLog.txt";
        private readonly string ReferenceLogName = "_ReferenceNameLog.txt";
        private readonly string ApplicationPath = "pack://application:,,,/HeroesIcons;component/Icons/";

        /// <summary>
        /// key is real hero name, value alt name (if any)
        /// example: Anub'arak, Anubarak
        /// </summary>
        private Dictionary<string, string> HeroesRealName = new Dictionary<string, string>();
        /// <summary>
        /// key is alt hero name, value real name
        /// example: Anubarak, Anub'arak
        /// </summary>
        private Dictionary<string, string> HeroesAltName = new Dictionary<string, string>();
        /// <summary>
        /// key is reference name of talent
        /// Tuple: key is real name of talent
        /// </summary>
        private Dictionary<string, Tuple<string, Uri>> Talents = new Dictionary<string, Tuple<string, Uri>>();
        /// <summary>
        /// key is real hero name
        /// </summary>
        private Dictionary<string, Uri> HeroPortraits = new Dictionary<string, Uri>();
        /// <summary>
        /// key is attributeid, value is hero name
        /// </summary>
        private Dictionary<string, string> HeroNamesFromAttId = new Dictionary<string, string>();
        /// <summary>
        /// key is real hero name
        /// </summary>
        private Dictionary<string, Uri> LeaderboardPortraits = new Dictionary<string, Uri>();
        /// <summary>
        /// key is real hero name
        /// value is HeroRole
        /// </summary>
        private Dictionary<string, HeroRole> HeroesRole = new Dictionary<string, HeroRole>();

        private Dictionary<MapName, Uri> MapBackgrounds = new Dictionary<MapName, Uri>();
        private Dictionary<MapName, Uri> MapBackgroundsSmall = new Dictionary<MapName, Uri>();
        private Dictionary<MVPAwardType, Tuple<Uri, string>> MVPScreenAwards = new Dictionary<MVPAwardType, Tuple<Uri, string>>();
        private Dictionary<MVPAwardType, Tuple<Uri, string>> MVPScoreScreenAwards = new Dictionary<MVPAwardType, Tuple<Uri, string>>();

        private Dictionary<PartyIconColor, Uri> PartyIcons = new Dictionary<PartyIconColor, Uri>();

        public List<Tuple<BitmapImage, Color>> HomeScreenBackgrounds { get; private set; } = new List<Tuple<BitmapImage, Color>>();

        private HeroesInfo()
        {
            ParseXmlHeroFiles();
            SetMapBackgrounds();
            SetMVPAwards();
            SetHomeScreenBackgrounds();
            SetPartyIcons();
        }

        public static HeroesInfo Initialize()
        {
            return new HeroesInfo();
        }

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
                return new BitmapImage(new Uri($"{ApplicationPath}Talents/_Generic/storm_ui_icon_no_pick.dds", UriKind.Absolute));


            // not found
            if (!Talents.TryGetValue(nameOfHeroTalent, out talent))
            {
                Task.Run(() => Log(ImageMissingLogName, $"Talent icon: {nameOfHeroTalent}"));

                return new BitmapImage(new Uri($"{ApplicationPath}Talents/_Generic/storm_ui_icon_default.dds", UriKind.Absolute));
            }
                
            try
            {
                return new BitmapImage(talent.Item2);
            }
            catch (Exception)
            {
                Task.Run(() => Log(ImageMissingLogName, $"Talent icon: {nameOfHeroTalent}"));
                return new BitmapImage(new Uri($"{ApplicationPath}Talents/_Generic/storm_ui_icon_default.dds", UriKind.Absolute));
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
                return new BitmapImage(new Uri($"{ApplicationPath}HeroPortraits/storm_ui_glues_draft_portrait_nopick.dds", UriKind.Absolute));

            // not found
            if (!HeroPortraits.TryGetValue(realHeroName, out uri))
            {
                Task.Run(() => Log(ImageMissingLogName, $"Hero portrait: {realHeroName}"));

                return new BitmapImage(new Uri($"{ApplicationPath}HeroPortraits/storm_ui_glues_draft_portrait_notfound.dds", UriKind.Absolute));
            }
         
            try
            {
                return new BitmapImage(uri);
            }
            catch (Exception)
            {
                Task.Run(() => Log(ImageMissingLogName, $"Hero portrait: {realHeroName}"));
                return new BitmapImage(new Uri($"{ApplicationPath}HeroPortraits/storm_ui_glues_draft_portrait_notfound.dds", UriKind.Absolute));
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
                return new BitmapImage(new Uri($"{ApplicationPath}HeroLeaderboardPortraits/storm_ui_ingame_hero_leaderboard_nopick.dds", UriKind.Absolute));

            // not found
            if (!LeaderboardPortraits.TryGetValue(realHeroName, out uri))
            {
                Task.Run(() => Log(ImageMissingLogName, $"Leader hero portrait: {realHeroName}"));

                return new BitmapImage(new Uri($"{ApplicationPath}HeroLeaderboardPortraits/storm_ui_ingame_hero_leaderboard_notfound.dds", UriKind.Absolute));
            }
           
            try
            {
                return new BitmapImage(uri);
            }
            catch (Exception)
            {
                Task.Run(() => Log(ImageMissingLogName, $"Leader hero portrait: {realHeroName}"));
                return new BitmapImage(new Uri($"{ApplicationPath}HeroLeaderboardPortraits/storm_ui_ingame_hero_leaderboard_notfound.dds", UriKind.Absolute));
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
            if (!Talents.TryGetValue(nameOfHeroTalent, out talent))
            {
                Task.Run(() => Log(ReferenceLogName, $"No name for reference: {nameOfHeroTalent}"));

                return nameOfHeroTalent;
            }

            return talent.Item1;
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
            if (!HeroNamesFromAttId.TryGetValue(attributeId, out heroName))
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
            if (!HeroesAltName.TryGetValue(realName, out altName))
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
            if (!HeroesAltName.TryGetValue(altName, out realName))
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
                return HeroesRealName.ContainsKey(heroName);
            else
                return HeroesAltName.ContainsKey(heroName);
        }

        /// <summary>
        /// Returns the hero's role: Warrior, Assassin, Support, or Specialist. Will return Unknown if hero not found
        /// </summary>
        /// <param name="realName">Heroes real name</param>
        /// <returns>HeroRole</returns>
        public HeroRole GetHeroRole(string realName)
        {
            HeroRole role;

            if (HeroesRole.TryGetValue(realName, out role))
                return role;
            else
                return HeroRole.Unknown;

        }

        public List<string> GetListOfHeroes()
        {
            List<string> heroes = new List<string>();
            foreach (var hero in HeroesRealName)
            {
                heroes.Add(hero.Key);
            }
            heroes.Sort();
            return heroes;
        }

        public int TotalAmountOfHeroes()
        {
            return HeroesRealName.Count;
        }

        public BitmapImage GetMapBackground(MapName mapName, bool useSmallImage = false)
        {
            try
            {
                if (useSmallImage == false)
                    return new BitmapImage(MapBackgrounds[mapName]);
                else
                    return new BitmapImage(MapBackgroundsSmall[mapName]);
            }
            catch (Exception)
            {
                Task.Run(() => Log(ImageMissingLogName, $"Map background: {mapName}"));
                return null;
            }
        }

        public BitmapImage GetMVPScreenAward(MVPAwardType mvpAwardType, MVPScreenColor mvpColor, out string awardName)
        {
            if (mvpAwardType == MVPAwardType.Unknown)
            {
                Task.Run(() => Log(ImageMissingLogName, $"MVP screen award: {mvpAwardType}"));
                awardName = string.Empty;
                return null;
            }

            try
            {
                var uriString = MVPScreenAwards[mvpAwardType].Item1.AbsoluteUri.Replace("%7BmvpColor%7D", mvpColor.ToString());

                awardName = MVPScreenAwards[mvpAwardType].Item2;
                return new BitmapImage(new Uri(uriString, UriKind.Absolute));
            }
            catch (Exception)
            {
                Task.Run(() => Log(ImageMissingLogName, $"MVP screen award: {mvpAwardType}"));
                awardName = "Unknown";
                return null;
            }
        }

        public BitmapImage GetMVPScoreScreenAward(MVPAwardType mvpAwardType, MVPScoreScreenColor mvpColor, out string awardName)
        {
            if (mvpAwardType == MVPAwardType.Unknown)
            {
                Task.Run(() => Log(ImageMissingLogName, $"MVP score screen award: {mvpAwardType}"));
                awardName = string.Empty;
                return null;
            }
            try
            {
                var uriString = MVPScoreScreenAwards[mvpAwardType].Item1.AbsoluteUri.Replace("%7BmvpColor%7D", mvpColor.ToString());

                awardName = MVPScoreScreenAwards[mvpAwardType].Item2;
                return new BitmapImage(new Uri(uriString, UriKind.Absolute));
            }
            catch (Exception)
            {
                Task.Run(() => Log(ImageMissingLogName, $"MVP score screen award: {mvpAwardType}"));
                awardName = "Unknown";
                return new BitmapImage(null);
            }
        }

        /// <summary>
        /// Gets the MVPAwardType enum from a given string, returns Unknown if no enum equivalent is found
        /// </summary>
        /// <param name="awardNameType">string name of enum</param>
        /// <returns></returns>
        public MVPAwardType GetMVPAwardTypeFromString(string awardNameType)
        {
            MVPAwardType mvpAwardType;
            if (Enum.TryParse(awardNameType, true, out mvpAwardType))
                return mvpAwardType;
            else
                return MVPAwardType.Unknown;
        }

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

        private Uri SetHeroTalentUri(string hero, string fileName, bool isGenericTalent)
        {
            if (Path.GetExtension(fileName) != ".dds")
                throw new IconException($"Image file does not have .dds extension [{fileName}]");

            if (!isGenericTalent)
                return new Uri($"{ApplicationPath}Talents/{hero}/{fileName}", UriKind.Absolute);
            else
                return new Uri($"{ApplicationPath}Talents/_Generic/{fileName}", UriKind.Absolute);
        }

        private Uri SetHeroPortraitUri(string fileName)
        {
            return new Uri($"{ApplicationPath}HeroPortraits/{fileName}", UriKind.Absolute);
        }

        private Uri SetLeaderboardPortrait(string fileName)
        {
            return new Uri($"{ApplicationPath}HeroLeaderboardPortraits/{fileName}", UriKind.Absolute);
        }

        private void SetMapBackgrounds()
        {
            try
            {
                MapBackgrounds.Add(MapName.BattlefieldofEternity, new Uri($"{ApplicationPath}MapBackgrounds/ui_ingame_mapmechanic_loadscreen_battlefieldofeternity.jpg", UriKind.Absolute));
                MapBackgrounds.Add(MapName.BlackheartsBay, new Uri($"{ApplicationPath}MapBackgrounds/ui_ingame_mapmechanic_loadscreen_blackheartsbay.jpg", UriKind.Absolute));
                MapBackgrounds.Add(MapName.BraxisHoldout, new Uri($"{ApplicationPath}MapBackgrounds/storm_ui_homescreenbackground_braxisholdout.jpg", UriKind.Absolute));
                MapBackgrounds.Add(MapName.CursedHollow, new Uri($"{ApplicationPath}MapBackgrounds/ui_ingame_mapmechanic_loadscreen_cursedhollow.jpg", UriKind.Absolute));
                MapBackgrounds.Add(MapName.DragonShire, new Uri($"{ApplicationPath}MapBackgrounds/ui_ingame_mapmechanic_loadscreen_dragonshire.jpg", UriKind.Absolute));
                MapBackgrounds.Add(MapName.GardenofTerror, new Uri($"{ApplicationPath}MapBackgrounds/ui_ingame_mapmechanic_loadscreen_gardenofterror.jpg", UriKind.Absolute));
                MapBackgrounds.Add(MapName.HauntedMines, new Uri($"{ApplicationPath}MapBackgrounds/ui_ingame_mapmechanic_loadscreen_hauntedmines.jpg", UriKind.Absolute));
                MapBackgrounds.Add(MapName.InfernalShrines, new Uri($"{ApplicationPath}MapBackgrounds/ui_ingame_mapmechanic_loadscreen_shrines.jpg", UriKind.Absolute));
                MapBackgrounds.Add(MapName.LostCavern, new Uri($"{ApplicationPath}MapBackgrounds/storm_ui_homescreenbackground_lostcavern.jpg", UriKind.Absolute));
                MapBackgrounds.Add(MapName.SkyTemple, new Uri($"{ApplicationPath}MapBackgrounds/ui_ingame_mapmechanic_loadscreen_skytemple.jpg", UriKind.Absolute));
                MapBackgrounds.Add(MapName.TomboftheSpiderQueen, new Uri($"{ApplicationPath}MapBackgrounds/ui_ingame_mapmechanic_loadscreen_tombofthespiderqueen.jpg", UriKind.Absolute));
                MapBackgrounds.Add(MapName.TowersofDoom, new Uri($"{ApplicationPath}MapBackgrounds/ui_ingame_mapmechanic_loadscreen_towersofdoom.jpg", UriKind.Absolute));
                MapBackgrounds.Add(MapName.WarheadJunction, new Uri($"{ApplicationPath}MapBackgrounds/storm_ui_homescreenbackground_warhead.jpg", UriKind.Absolute));

                MapBackgroundsSmall.Add(MapName.BattlefieldofEternity, new Uri($"{ApplicationPath}MapBackgrounds/ui_ingame_mapmechanic_loadscreen_battlefieldofeternity_small.jpg", UriKind.Absolute));
                MapBackgroundsSmall.Add(MapName.BlackheartsBay, new Uri($"{ApplicationPath}MapBackgrounds/ui_ingame_mapmechanic_loadscreen_blackheartsbay_small.jpg", UriKind.Absolute));
                MapBackgroundsSmall.Add(MapName.BraxisHoldout, new Uri($"{ApplicationPath}MapBackgrounds/storm_ui_homescreenbackground_braxisholdout_small.jpg", UriKind.Absolute));
                MapBackgroundsSmall.Add(MapName.CursedHollow, new Uri($"{ApplicationPath}MapBackgrounds/ui_ingame_mapmechanic_loadscreen_cursedhollow_small.jpg", UriKind.Absolute));
                MapBackgroundsSmall.Add(MapName.DragonShire, new Uri($"{ApplicationPath}MapBackgrounds/ui_ingame_mapmechanic_loadscreen_dragonshire_small.jpg", UriKind.Absolute));
                MapBackgroundsSmall.Add(MapName.GardenofTerror, new Uri($"{ApplicationPath}MapBackgrounds/ui_ingame_mapmechanic_loadscreen_gardenofterror_small.jpg", UriKind.Absolute));
                MapBackgroundsSmall.Add(MapName.HauntedMines, new Uri($"{ApplicationPath}MapBackgrounds/ui_ingame_mapmechanic_loadscreen_hauntedmines_small.jpg", UriKind.Absolute));
                MapBackgroundsSmall.Add(MapName.InfernalShrines, new Uri($"{ApplicationPath}MapBackgrounds/ui_ingame_mapmechanic_loadscreen_shrines_small.jpg", UriKind.Absolute));
                MapBackgroundsSmall.Add(MapName.LostCavern, new Uri($"{ApplicationPath}MapBackgrounds/storm_ui_homescreenbackground_lostcavern_small.jpg", UriKind.Absolute));
                MapBackgroundsSmall.Add(MapName.SkyTemple, new Uri($"{ApplicationPath}MapBackgrounds/ui_ingame_mapmechanic_loadscreen_skytemple_small.jpg", UriKind.Absolute));
                MapBackgroundsSmall.Add(MapName.TomboftheSpiderQueen, new Uri($"{ApplicationPath}MapBackgrounds/ui_ingame_mapmechanic_loadscreen_tombofthespiderqueen_small.jpg", UriKind.Absolute));
                MapBackgroundsSmall.Add(MapName.TowersofDoom, new Uri($"{ApplicationPath}MapBackgrounds/ui_ingame_mapmechanic_loadscreen_towersofdoom_small.jpg", UriKind.Absolute));
                MapBackgroundsSmall.Add(MapName.WarheadJunction, new Uri($"{ApplicationPath}MapBackgrounds/storm_ui_homescreenbackground_warhead_small.jpg", UriKind.Absolute));
            }
            catch (Exception ex)
            {
                throw new IconException("Failed to set all map backgrounds", ex);
            }
        }

        private void SetMVPAwards()
        {
            try
            {
                MVPScreenAwards.Add(MVPAwardType.MostDamageTaken, new Tuple<Uri, string>(new Uri($"{ApplicationPath}Awards/storm_ui_mvp_bulwark_{{mvpColor}}.png", UriKind.Absolute), "Bulwark"));
                MVPScreenAwards.Add(MVPAwardType.MostAltarDamage, new Tuple<Uri, string>(new Uri($"{ApplicationPath}Awards/storm_ui_mvp_cannoneer_{{mvpColor}}.png", UriKind.Absolute), "Cannoneer"));
                MVPScreenAwards.Add(MVPAwardType.ClutchHealer, new Tuple<Uri, string>(new Uri($"{ApplicationPath}Awards/storm_ui_mvp_clutchhealer_{{mvpColor}}.png", UriKind.Absolute), "Clutch Healer"));
                MVPScreenAwards.Add(MVPAwardType.MostNukeDamageDone, new Tuple<Uri, string>(new Uri($"{ApplicationPath}Awards/storm_ui_mvp_dabomb_{{mvpColor}}.png", UriKind.Absolute), "DaBomb"));
                MVPScreenAwards.Add(MVPAwardType.HighestKillStreak, new Tuple<Uri, string>(new Uri($"{ApplicationPath}Awards/storm_ui_mvp_skull_{{mvpColor}}.png", UriKind.Absolute), "Dominator"));
                MVPScreenAwards.Add(MVPAwardType.MostXPContribution, new Tuple<Uri, string>(new Uri($"{ApplicationPath}Awards/storm_ui_mvp_experienced_{{mvpColor}}.png", UriKind.Absolute), "Experienced"));
                MVPScreenAwards.Add(MVPAwardType.MostKills, new Tuple<Uri, string>(new Uri($"{ApplicationPath}Awards/storm_ui_mvp_finisher_{{mvpColor}}.png", UriKind.Absolute), "Finisher"));
                MVPScreenAwards.Add(MVPAwardType.MostDamageToPlants, new Tuple<Uri, string>(new Uri($"{ApplicationPath}Awards/storm_ui_mvp_gardenterror_{{mvpColor}}.png", UriKind.Absolute), "Garden Terror"));
                MVPScreenAwards.Add(MVPAwardType.MostDamageToMinions, new Tuple<Uri, string>(new Uri($"{ApplicationPath}Awards/storm_ui_mvp_guardianslayer_{{mvpColor}}.png", UriKind.Absolute), "Guardian Slayer"));
                MVPScreenAwards.Add(MVPAwardType.HatTrick, new Tuple<Uri, string>(new Uri($"{ApplicationPath}Awards/storm_ui_mvp_hattrick_{{mvpColor}}.png", UriKind.Absolute), "Hat Trick"));
                MVPScreenAwards.Add(MVPAwardType.MostMercCampsCaptured, new Tuple<Uri, string>(new Uri($"{ApplicationPath}Awards/storm_ui_mvp_headhunter_{{mvpColor}}.png", UriKind.Absolute), "Headhunter"));
                MVPScreenAwards.Add(MVPAwardType.MostImmortalDamage, new Tuple<Uri, string>(new Uri($"{ApplicationPath}Awards/storm_ui_mvp_immortalslayer_{{mvpColor}}.png", UriKind.Absolute), "Immortal Slayer"));
                MVPScreenAwards.Add(MVPAwardType.MostGemsTurnedIn, new Tuple<Uri, string>(new Uri($"{ApplicationPath}Awards/storm_ui_mvp_jeweler_{{mvpColor}}.png", UriKind.Absolute), "Jeweler"));
                MVPScreenAwards.Add(MVPAwardType.MostHealing, new Tuple<Uri, string>(new Uri($"{ApplicationPath}Awards/storm_ui_mvp_mainhealer_{{mvpColor}}.png", UriKind.Absolute), "Main Healer"));
                MVPScreenAwards.Add(MVPAwardType.MostCurseDamageDone, new Tuple<Uri, string>(new Uri($"{ApplicationPath}Awards/storm_ui_mvp_masterofthecurse_{{mvpColor}}.png", UriKind.Absolute), "Master of the Curse"));
                MVPScreenAwards.Add(MVPAwardType.MostCoinsPaid, new Tuple<Uri, string>(new Uri($"{ApplicationPath}Awards/storm_ui_mvp_moneybags_{{mvpColor}}.png", UriKind.Absolute), "Moneybags"));
                MVPScreenAwards.Add(MVPAwardType.MVP, new Tuple<Uri, string>(new Uri($"{ApplicationPath}Awards/storm_ui_mvp_mvp_{{mvpColor}}.png", UriKind.Absolute), "MVP"));
                MVPScreenAwards.Add(MVPAwardType.MostHeroDamageDone, new Tuple<Uri, string>(new Uri($"{ApplicationPath}Awards/storm_ui_mvp_painbringer_{{mvpColor}}.png", UriKind.Absolute), "Painbringer"));
                MVPScreenAwards.Add(MVPAwardType.MostProtection, new Tuple<Uri, string>(new Uri($"{ApplicationPath}Awards/storm_ui_mvp_protector_{{mvpColor}}.png", UriKind.Absolute), "Protector"));
                MVPScreenAwards.Add(MVPAwardType.MostDragonShrinesCaptured, new Tuple<Uri, string>(new Uri($"{ApplicationPath}Awards/storm_ui_mvp_shriner_{{mvpColor}}.png", UriKind.Absolute), "Shriner"));
                MVPScreenAwards.Add(MVPAwardType.MostSiegeDamageDone, new Tuple<Uri, string>(new Uri($"{ApplicationPath}Awards/storm_ui_mvp_siegemaster_{{mvpColor}}.png", UriKind.Absolute), "Siege Master"));
                MVPScreenAwards.Add(MVPAwardType.ZeroDeaths, new Tuple<Uri, string>(new Uri($"{ApplicationPath}Awards/storm_ui_mvp_solesurvivor_{{mvpColor}}.png", UriKind.Absolute), "Sole Survior"));
                MVPScreenAwards.Add(MVPAwardType.MostStuns, new Tuple<Uri, string>(new Uri($"{ApplicationPath}Awards/storm_ui_mvp_stunner_{{mvpColor}}.png", UriKind.Absolute), "Stunner"));
                MVPScreenAwards.Add(MVPAwardType.MostTimeInTemple, new Tuple<Uri, string>(new Uri($"{ApplicationPath}Awards/storm_ui_mvp_templemaster_{{mvpColor}}.png", UriKind.Absolute), "Temple Master"));
                MVPScreenAwards.Add(MVPAwardType.MostRoots, new Tuple<Uri, string>(new Uri($"{ApplicationPath}Awards/storm_ui_mvp_trapper_{{mvpColor}}.png", UriKind.Absolute), "Trapper"));
                MVPScreenAwards.Add(MVPAwardType.MostDamageDoneToZerg, new Tuple<Uri, string>(new Uri($"{ApplicationPath}Awards/storm_ui_mvp_zergcrusher_{{mvpColor}}.png", UriKind.Absolute), "Zerg Crusher"));

                MVPScoreScreenAwards.Add(MVPAwardType.MostDamageTaken, new Tuple<Uri, string>(new Uri($"{ApplicationPath}Awards/storm_ui_scorescreen_mvp_bulwark_{{mvpColor}}.png", UriKind.Absolute), "Bulwark"));
                MVPScoreScreenAwards.Add(MVPAwardType.MostAltarDamage, new Tuple<Uri, string>(new Uri($"{ApplicationPath}Awards/storm_ui_scorescreen_mvp_cannoneer_{{mvpColor}}.png", UriKind.Absolute), "Cannoneer"));
                MVPScoreScreenAwards.Add(MVPAwardType.ClutchHealer, new Tuple<Uri, string>(new Uri($"{ApplicationPath}Awards/storm_ui_scorescreen_mvp_clutchhealer_{{mvpColor}}.png", UriKind.Absolute), "Clutch Healer"));
                MVPScoreScreenAwards.Add(MVPAwardType.MostNukeDamageDone, new Tuple<Uri, string>(new Uri($"{ApplicationPath}Awards/storm_ui_scorescreen_mvp_dabomb_{{mvpColor}}.png", UriKind.Absolute), "DaBomb"));
                MVPScoreScreenAwards.Add(MVPAwardType.HighestKillStreak, new Tuple<Uri, string>(new Uri($"{ApplicationPath}Awards/storm_ui_scorescreen_mvp_skull_{{mvpColor}}.png", UriKind.Absolute), "Dominator"));
                MVPScoreScreenAwards.Add(MVPAwardType.MostXPContribution, new Tuple<Uri, string>(new Uri($"{ApplicationPath}Awards/storm_ui_scorescreen_mvp_experienced_{{mvpColor}}.png", UriKind.Absolute), "Experienced"));
                MVPScoreScreenAwards.Add(MVPAwardType.MostKills, new Tuple<Uri, string>(new Uri($"{ApplicationPath}Awards/storm_ui_scorescreen_mvp_finisher_{{mvpColor}}.png", UriKind.Absolute), "Finisher"));
                MVPScoreScreenAwards.Add(MVPAwardType.MostDamageToPlants, new Tuple<Uri, string>(new Uri($"{ApplicationPath}Awards/storm_ui_scorescreen_mvp_gardenterror_{{mvpColor}}.png", UriKind.Absolute), "Garden Terror"));
                MVPScoreScreenAwards.Add(MVPAwardType.MostDamageToMinions, new Tuple<Uri, string>(new Uri($"{ApplicationPath}Awards/storm_ui_scorescreen_mvp_guardianslayer_{{mvpColor}}.png", UriKind.Absolute), "Guardian Slayer"));
                MVPScoreScreenAwards.Add(MVPAwardType.HatTrick, new Tuple<Uri, string>( new Uri($"{ApplicationPath}Awards/storm_ui_scorescreen_mvp_hattrick_{{mvpColor}}.png", UriKind.Absolute), "Hat Trick"));
                MVPScoreScreenAwards.Add(MVPAwardType.MostMercCampsCaptured, new Tuple<Uri, string>(new Uri($"{ApplicationPath}Awards/storm_ui_scorescreen_mvp_headhunter_{{mvpColor}}.png", UriKind.Absolute), "Headhunter"));
                MVPScoreScreenAwards.Add(MVPAwardType.MostImmortalDamage, new Tuple<Uri, string>(new Uri($"{ApplicationPath}Awards/storm_ui_scorescreen_mvp_immortalslayer_{{mvpColor}}.png", UriKind.Absolute), "Immortal Slayer"));
                MVPScoreScreenAwards.Add(MVPAwardType.MostGemsTurnedIn, new Tuple<Uri, string>(new Uri($"{ApplicationPath}Awards/storm_ui_scorescreen_mvp_jeweler_{{mvpColor}}.png", UriKind.Absolute), "Jeweler"));
                MVPScoreScreenAwards.Add(MVPAwardType.MostHealing, new Tuple<Uri, string>(new Uri($"{ApplicationPath}Awards/storm_ui_scorescreen_mvp_mainhealer_{{mvpColor}}.png", UriKind.Absolute), "Main Healer"));
                MVPScoreScreenAwards.Add(MVPAwardType.MostCurseDamageDone, new Tuple<Uri, string>(new Uri($"{ApplicationPath}Awards/storm_ui_scorescreen_mvp_masterofthecurse_{{mvpColor}}.png", UriKind.Absolute), "Master of the Curse"));
                MVPScoreScreenAwards.Add(MVPAwardType.MostCoinsPaid, new Tuple<Uri, string>(new Uri($"{ApplicationPath}Awards/storm_ui_scorescreen_mvp_moneybags_{{mvpColor}}.png", UriKind.Absolute), "Moneybags"));
                MVPScoreScreenAwards.Add(MVPAwardType.MVP, new Tuple<Uri, string>(new Uri($"{ApplicationPath}Awards/storm_ui_scorescreen_mvp_mvp_{{mvpColor}}.png", UriKind.Absolute), "MVP"));
                MVPScoreScreenAwards.Add(MVPAwardType.MostHeroDamageDone, new Tuple<Uri, string>(new Uri($"{ApplicationPath}Awards/storm_ui_scorescreen_mvp_painbringer_{{mvpColor}}.png", UriKind.Absolute), "Painbringer"));
                MVPScoreScreenAwards.Add(MVPAwardType.MostProtection, new Tuple<Uri, string>(new Uri($"{ApplicationPath}Awards/storm_ui_scorescreen_mvp_protector_{{mvpColor}}.png", UriKind.Absolute), "Protector"));
                MVPScoreScreenAwards.Add(MVPAwardType.MostDragonShrinesCaptured, new Tuple<Uri, string>( new Uri($"{ApplicationPath}Awards/storm_ui_scorescreen_mvp_shriner_{{mvpColor}}.png", UriKind.Absolute), "Shriner"));
                MVPScoreScreenAwards.Add(MVPAwardType.MostSiegeDamageDone, new Tuple<Uri, string>(new Uri($"{ApplicationPath}Awards/storm_ui_scorescreen_mvp_siegemaster_{{mvpColor}}.png", UriKind.Absolute), "Siege Master"));
                MVPScoreScreenAwards.Add(MVPAwardType.ZeroDeaths, new Tuple<Uri, string>(new Uri($"{ApplicationPath}Awards/storm_ui_scorescreen_mvp_solesurvivor_{{mvpColor}}.png", UriKind.Absolute), "Sole Survior"));
                MVPScoreScreenAwards.Add(MVPAwardType.MostStuns, new Tuple<Uri, string>(new Uri($"{ApplicationPath}Awards/storm_ui_scorescreen_mvp_stunner_{{mvpColor}}.png", UriKind.Absolute), "Stunner"));
                MVPScoreScreenAwards.Add(MVPAwardType.MostTimeInTemple, new Tuple<Uri, string>(new Uri($"{ApplicationPath}Awards/storm_ui_scorescreen_mvp_templemaster_{{mvpColor}}.png", UriKind.Absolute), "Temple Master"));
                MVPScoreScreenAwards.Add(MVPAwardType.MostRoots, new Tuple<Uri, string>(new Uri($"{ApplicationPath}Awards/storm_ui_scorescreen_mvp_trapper_{{mvpColor}}.png", UriKind.Absolute), "Trapper"));
                MVPScoreScreenAwards.Add(MVPAwardType.MostDamageDoneToZerg, new Tuple<Uri, string>(new Uri($"{ApplicationPath}Awards/storm_ui_scorescreen_mvp_zergcrusher_{{mvpColor}}.png", UriKind.Absolute), "Zerg Crusher"));
            }
            catch (Exception ex)
            {
                throw new IconException("Failed to set all mvp awards", ex);
            }
        }

        private void SetHomeScreenBackgrounds()
        {
            HomeScreenBackgrounds.Add(new Tuple<BitmapImage, Color>(new BitmapImage(new Uri($"{ApplicationPath}Homescreens/storm_ui_homescreenbackground_alarak.jpg", UriKind.Absolute)), Colors.Purple));
            HomeScreenBackgrounds.Add(new Tuple<BitmapImage, Color>(new BitmapImage(new Uri($"{ApplicationPath}Homescreens/storm_ui_homescreenbackground_chromie.jpg", UriKind.Absolute)), Colors.Gold));
            HomeScreenBackgrounds.Add(new Tuple<BitmapImage, Color>(new BitmapImage(new Uri($"{ApplicationPath}Homescreens/storm_ui_homescreenbackground_diablo.jpg", UriKind.Absolute)), Colors.Red));
            HomeScreenBackgrounds.Add(new Tuple<BitmapImage, Color>(new BitmapImage(new Uri($"{ApplicationPath}Homescreens/storm_ui_homescreenbackground_diablotristram.jpg", UriKind.Absolute)), Colors.Gray));
            HomeScreenBackgrounds.Add(new Tuple<BitmapImage, Color>(new BitmapImage(new Uri($"{ApplicationPath}Homescreens/storm_ui_homescreenbackground_eternalconflict.jpg", UriKind.Absolute)), Colors.DarkRed));
            HomeScreenBackgrounds.Add(new Tuple<BitmapImage, Color>(new BitmapImage(new Uri($"{ApplicationPath}Homescreens/storm_ui_homescreenbackground_eternalconflict_dark.jpg", UriKind.Absolute)), Colors.DarkRed)); ;
            HomeScreenBackgrounds.Add(new Tuple<BitmapImage, Color>(new BitmapImage(new Uri($"{ApplicationPath}Homescreens/storm_ui_homescreenbackground_greymane.jpg", UriKind.Absolute)), Colors.LightBlue));
            HomeScreenBackgrounds.Add(new Tuple<BitmapImage, Color>(new BitmapImage(new Uri($"{ApplicationPath}Homescreens/storm_ui_homescreenbackground_guldan.jpg", UriKind.Absolute)), Colors.Green));
            HomeScreenBackgrounds.Add(new Tuple<BitmapImage, Color>(new BitmapImage(new Uri($"{ApplicationPath}Homescreens/storm_ui_homescreenbackground_lunara.jpg", UriKind.Absolute)), Colors.Purple));
            HomeScreenBackgrounds.Add(new Tuple<BitmapImage, Color>(new BitmapImage(new Uri($"{ApplicationPath}Homescreens/storm_ui_homescreenbackground_lunarnewyear.jpg", UriKind.Absolute)), Colors.Purple));
            HomeScreenBackgrounds.Add(new Tuple<BitmapImage, Color>(new BitmapImage(new Uri($"{ApplicationPath}Homescreens/storm_ui_homescreenbackground_medivh.jpg", UriKind.Absolute)), Colors.Gray));
            HomeScreenBackgrounds.Add(new Tuple<BitmapImage, Color>(new BitmapImage(new Uri($"{ApplicationPath}Homescreens/storm_ui_homescreenbackground_nexus.jpg", UriKind.Absolute)), Colors.Purple));
            HomeScreenBackgrounds.Add(new Tuple<BitmapImage, Color>(new BitmapImage(new Uri($"{ApplicationPath}Homescreens/storm_ui_homescreenbackground_overwatchhangar.jpg", UriKind.Absolute)), Colors.Gray));
            HomeScreenBackgrounds.Add(new Tuple<BitmapImage, Color>(new BitmapImage(new Uri($"{ApplicationPath}Homescreens/storm_ui_homescreenbackground_samuro.jpg", UriKind.Absolute)), Colors.Orange));
            HomeScreenBackgrounds.Add(new Tuple<BitmapImage, Color>(new BitmapImage(new Uri($"{ApplicationPath}Homescreens/storm_ui_homescreenbackground_shrines.jpg", UriKind.Absolute)), Colors.Red));
            HomeScreenBackgrounds.Add(new Tuple<BitmapImage, Color>(new BitmapImage(new Uri($"{ApplicationPath}Homescreens/storm_ui_homescreenbackground_shrines_dusk.jpg", UriKind.Absolute)), Colors.Red));
            HomeScreenBackgrounds.Add(new Tuple<BitmapImage, Color>(new BitmapImage(new Uri($"{ApplicationPath}Homescreens/storm_ui_homescreenbackground_starcraft.jpg", UriKind.Absolute)), Colors.DarkBlue));
            HomeScreenBackgrounds.Add(new Tuple<BitmapImage, Color>(new BitmapImage(new Uri($"{ApplicationPath}Homescreens/storm_ui_homescreenbackground_starcraft_protoss.jpg", UriKind.Absolute)), Colors.Cyan));
            HomeScreenBackgrounds.Add(new Tuple<BitmapImage, Color>(new BitmapImage(new Uri($"{ApplicationPath}Homescreens/storm_ui_homescreenbackground_starcraft_zerg.jpg", UriKind.Absolute)), Colors.DarkRed));
            HomeScreenBackgrounds.Add(new Tuple<BitmapImage, Color>(new BitmapImage(new Uri($"{ApplicationPath}Homescreens/storm_ui_homescreenbackground_varian.jpg", UriKind.Absolute)), Colors.Red));
            HomeScreenBackgrounds.Add(new Tuple<BitmapImage, Color>(new BitmapImage(new Uri($"{ApplicationPath}Homescreens/storm_ui_homescreenbackground_zarya.jpg", UriKind.Absolute)), Colors.Purple));
        }

        private void SetPartyIcons()
        {
            PartyIcons.Add(PartyIconColor.Purple, new Uri($"{ApplicationPath}PartyIcons/ui_ingame_loadscreen_partylink_purple.png", UriKind.Absolute));
            PartyIcons.Add(PartyIconColor.Yellow, new Uri($"{ApplicationPath}PartyIcons/ui_ingame_loadscreen_partylink_yellow.png", UriKind.Absolute));
            PartyIcons.Add(PartyIconColor.Brown, new Uri($"{ApplicationPath}PartyIcons/ui_ingame_loadscreen_partylink_brown.png", UriKind.Absolute));
            PartyIcons.Add(PartyIconColor.Teal, new Uri($"{ApplicationPath}PartyIcons/ui_ingame_loadscreen_partylink_teal.png", UriKind.Absolute));
        }

        private void ParseXmlHeroFiles()
        {
            List<string> heroes = new List<string>();

            try
            {
                using (XmlTextReader reader = new XmlTextReader(@"Heroes/_AllHeroes.xml"))
                {
                    reader.ReadStartElement("Heroes");

                    while (reader.Read() && reader.NodeType != XmlNodeType.EndElement)
                    {
                        if (reader.NodeType == XmlNodeType.Comment || reader.NodeType == XmlNodeType.Text || reader.NodeType == XmlNodeType.Whitespace)
                            continue;

                        XElement el = (XElement)XNode.ReadFrom(reader);
                        heroes.Add(el.Name.ToString());
                    }
                }

                foreach (var hero in heroes)
                {
                    using (XmlReader reader = XmlReader.Create($@"Heroes/{hero}.xml"))
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

                        // get portrait
                        string portraitName = reader["portrait"];

                        // get leaderboard portrait
                        string lbPortrait = reader["leader"];

                        if (!string.IsNullOrEmpty(attributeId))
                        {
                            HeroNamesFromAttId.Add(attributeId, realHeroName);
                        }

                        if (!string.IsNullOrEmpty(portraitName))
                            HeroPortraits.Add(realHeroName, SetHeroPortraitUri(portraitName));

                        if (!string.IsNullOrEmpty(lbPortrait))
                            LeaderboardPortraits.Add(realHeroName, SetLeaderboardPortrait(lbPortrait));

                        HeroesRealName.Add(realHeroName, hero);
                        HeroesAltName.Add(hero, realHeroName);

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
                        }

                        // add talents
                        while (reader.Read())
                        {
                            if (reader.IsStartElement())
                            {
                                string element = reader.Name;
                                if (element == "Level1" || element == "Level4" || element == "Level7" ||
                                    element == "Level10" || element == "Level13" || element == "Level16" ||
                                    element == "Level20" || element == "Old")
                                {
                                    while (reader.Read() && reader.Name != element)
                                    {
                                        if (reader.NodeType == XmlNodeType.Element)
                                        {
                                            string name = reader.Name; // raw name of talent
                                            string realName = reader["name"] == null ? string.Empty : reader["name"];  // real ingame name of talent
                                            string generic = reader["generic"] == null ? "false" : reader["generic"];  // is the icon being used generic

                                            bool isGeneric;
                                            if (!bool.TryParse(generic, out isGeneric))
                                                isGeneric = false;

                                            if (reader.Read())
                                            {
                                                if (name.StartsWith("Generic") || name.StartsWith("HeroGeneric") || name.StartsWith("BattleMomentum"))
                                                    isGeneric = true;

                                                if (!Talents.ContainsKey(name))
                                                    Talents.Add(name, new Tuple<string, Uri>(realName, SetHeroTalentUri(hero, reader.Value, isGeneric)));
                                            }
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
                throw new ParseXmlException("Error on parsing of xml files", ex);
            }
        }

        private void Log(string fileName, string message)
        {
            using (StreamWriter writer = new StreamWriter($"logs/{fileName}", true))
            {
                writer.WriteLine(message);
            }
        }
    }
}
