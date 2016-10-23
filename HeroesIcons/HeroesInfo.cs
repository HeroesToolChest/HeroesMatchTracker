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
        private HeroesXml HeroesXml;
        private MatchAwardsXml MatchAwardsXml;

        /// <summary>
        /// key is real hero name
        /// value is HeroRole
        /// </summary>
        private Dictionary<string, HeroRole> HeroesNonSupportHealingStat = new Dictionary<string, HeroRole>();

        private Dictionary<MapName, Uri> MapBackgrounds = new Dictionary<MapName, Uri>();
        private Dictionary<MapName, Uri> MapBackgroundsSmall = new Dictionary<MapName, Uri>();

        private Dictionary<PartyIconColor, Uri> PartyIcons = new Dictionary<PartyIconColor, Uri>();

        public List<Tuple<BitmapImage, Color>> HomeScreenBackgrounds { get; private set; } = new List<Tuple<BitmapImage, Color>>();

        private HeroesInfo()
        {
            HeroesXml = HeroesXml.Initialize("_AllHeroes.xml", "Heroes");
            MatchAwardsXml = MatchAwardsXml.Initialize("_AllMatchAwards.xml", "MatchAwards");

            SetNonSupportHeroesWithSupportStat();
            SetMapBackgrounds();
            SetHomeScreenBackgrounds();
            SetPartyIcons();
        }

        public static HeroesInfo Initialize()
        {
            return new HeroesInfo();
        }

        #region public methods
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
            if (!HeroesXml.TalentIcons.TryGetValue(nameOfHeroTalent, out talent))
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
            if (!HeroesXml.HeroPortraits.TryGetValue(realHeroName, out uri))
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
            if (!HeroesXml.LeaderboardPortraits.TryGetValue(realHeroName, out uri))
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
            if (!HeroesXml.TalentIcons.TryGetValue(nameOfHeroTalent, out talent))
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

        public bool IsNonSupportHeroWithHealingStat(string realHeroName)
        {
            return HeroesNonSupportHealingStat.ContainsKey(realHeroName);
        }

        /// <summary>
        /// Returns a dictionary of all the talents of a hero
        /// </summary>
        /// <param name="realHeroName">real hero name</param>
        /// <returns></returns>
        public Dictionary<TalentTier, List<string>> GetTalentsForHero(string realHeroName)
        {
            Dictionary<TalentTier, List<string>> talents;
            if (!HeroesXml.HeroesListOfTalents.TryGetValue(realHeroName, out talents))
            {
                Task.Run(() => Log(ReferenceLogName, $"No hero real name found [{nameof(GetTalentsForHero)}]: {realHeroName}"));
            }

            return talents;
        }
        #endregion public methods

        #region private methods
        private void SetNonSupportHeroesWithSupportStat()
        {
            HeroesNonSupportHealingStat.Add("Medivh", HeroRole.Support);
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

        private void Log(string fileName, string message)
        {
            using (StreamWriter writer = new StreamWriter($"logs/{fileName}", true))
            {
                writer.WriteLine(message);
            }
        }
        #endregion private methods
    }
}
