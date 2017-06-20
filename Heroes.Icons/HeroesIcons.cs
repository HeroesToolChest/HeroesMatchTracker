using Heroes.Icons.Xml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Media.Imaging;

namespace Heroes.Icons
{
    public class HeroesIcons : HeroesBase, IHeroesIconsService
    {
        private readonly HeroBuildsXml HeroBuildsXmlLatest; // holds the latest build info

        private bool Logger;
        private int EarliestHeroesBuild;
        private int LatestHeroesBuild;
        private HeroesXml HeroesXml;
        private HeroBuildsXml HeroBuildsXml; // the one that is in use
        private HeroBuildsXml HeroBuildsXmlHolder; // used for swapping between the one that is in use and latest
        private MatchAwardsXml MatchAwardsXml;
        private MapBackgroundsXml MapBackgroundsXml;
        private HomeScreensXml HomeScreensXml;
        private List<int> ListHeroBuilds;

        /// <summary>
        /// key is real hero name
        /// value is HeroRole
        /// </summary>
        private Dictionary<string, HeroRole> HeroesNonSupportHealingStat = new Dictionary<string, HeroRole>();
        private Dictionary<PartyIconColor, Uri> PartyIcons = new Dictionary<PartyIconColor, Uri>();
        private Dictionary<HeroRole, Uri> RoleIcons = new Dictionary<HeroRole, Uri>();
        private Dictionary<HeroFranchise, Uri> FranchiseIcons = new Dictionary<HeroFranchise, Uri>();
        private Dictionary<OtherIcon, Uri> OtherIcons = new Dictionary<OtherIcon, Uri>();

        public HeroesIcons(bool logger)
        {
            Logger = logger;

            HeroesXml = HeroesXml.Initialize("Heroes.xml", "Heroes", Logger, -1);
            HeroBuildsXmlLatest = HeroBuildsXmlHolder = HeroBuildsXml = HeroBuildsXml.Initialize("_AllHeroes.xml", "HeroBuilds", HeroesXml, Logger); // load in all three

            EarliestHeroesBuild = HeroBuildsXml.EarliestHeroesBuild;
            LatestHeroesBuild = HeroBuildsXml.LatestHeroesBuild;
            ListHeroBuilds = HeroBuildsXml.Builds;

            MatchAwardsXml = MatchAwardsXml.Initialize("_AllMatchAwards.xml", "MatchAwards", LatestHeroesBuild);
            MapBackgroundsXml = MapBackgroundsXml.Initialize("_AllMapBackgrounds.xml", "MapBackgrounds", LatestHeroesBuild);
            HomeScreensXml = HomeScreensXml.Initialize("HomeScreens.xml", "HomeScreens", LatestHeroesBuild);

            SetNonSupportHeroesWithSupportStat();
            SetPartyIcons();
            SetRoleIcons();
            SetFranchiseIcons();
            SetOtherIcons();
        }

        public int LatestSupportedBuild() => LatestHeroesBuild;

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

            // only load the build if it's not in memory already or in the in xmlHolder
            if (build != HeroBuildsXml.CurrentLoadedHeroesBuild)
            {
                if (build != HeroBuildsXmlHolder.CurrentLoadedHeroesBuild)
                    HeroBuildsXml = HeroBuildsXml.Initialize("_AllHeroes.xml", "HeroBuilds", HeroesXml, Logger, build);
                else
                    HeroBuildsXml = HeroBuildsXmlHolder;
            }
        }

        public void LoadLatestHeroesBuild()
        {
            if (HeroBuildsXml.CurrentLoadedHeroesBuild != HeroBuildsXmlLatest.CurrentLoadedHeroesBuild)
            {
                HeroBuildsXmlHolder = HeroBuildsXml; // hold the current
                HeroBuildsXml = HeroBuildsXmlLatest; // switch to latest
            }
        }

        public IHeroes Heroes()
        {
            return HeroesXml;
        }

        public IHeroBuilds HeroBuilds()
        {
            return HeroBuildsXml;
        }

        public IMatchAwards MatchAwards()
        {
            return MatchAwardsXml;
        }

        public IMapBackgrounds MapBackgrounds()
        {
            return MapBackgroundsXml;
        }

        public IHomeScreens HomeScreens()
        {
            return HomeScreensXml;
        }

        public BitmapImage GetPartyIcon(PartyIconColor partyIconColor)
        {
            if (PartyIcons.ContainsKey(partyIconColor))
            {
                try
                {
                    BitmapImage image = new BitmapImage(PartyIcons[partyIconColor]);
                    image.Freeze();

                    return image;
                }
                catch (IOException)
                {
                    LogMissingImage($"Missing image: {PartyIcons[partyIconColor]}");
                    return null;
                }
            }
            else
            {
                LogReferenceNameNotFound($"Other Icons: {partyIconColor}");
                return null;
            }
        }

        public BitmapImage GetOtherIcon(OtherIcon otherIcon)
        {
            if (OtherIcons.ContainsKey(otherIcon))
            {
                try
                {
                    BitmapImage image = new BitmapImage(OtherIcons[otherIcon]);
                    image.Freeze();

                    return image;
                }
                catch (IOException)
                {
                    LogMissingImage($"Missing image: {OtherIcons[otherIcon]}");
                    return null;
                }
            }
            else
            {
                LogReferenceNameNotFound($"Other Icons: {otherIcon}");
                return null;
            }
        }

        public BitmapImage GetRoleIcon(HeroRole heroRole)
        {
            if (RoleIcons.ContainsKey(heroRole))
            {
                try
                {
                    BitmapImage image = new BitmapImage(RoleIcons[heroRole]);
                    image.Freeze();

                    return image;
                }
                catch (IOException)
                {
                    LogMissingImage($"Missing image: {RoleIcons[heroRole]}");
                    return null;
                }
            }
            else
            {
                LogReferenceNameNotFound($"Other Icons: {heroRole}");
                return null;
            }
        }

        public BitmapImage GetFranchiseIcon(HeroFranchise heroFranchise)
        {
            if (FranchiseIcons.ContainsKey(heroFranchise))
            {
                try
                {
                    BitmapImage image = new BitmapImage(FranchiseIcons[heroFranchise]);
                    image.Freeze();

                    return image;
                }
                catch (IOException)
                {
                    LogMissingImage($"Missing image: {FranchiseIcons[heroFranchise]}");
                    return null;
                }
            }
            else
            {
                LogReferenceNameNotFound($"Other Icons: {heroFranchise}");
                return null;
            }
        }

        public bool IsNonSupportHeroWithHealingStat(string realHeroName)
        {
            return HeroesNonSupportHealingStat.ContainsKey(realHeroName);
        }

        public List<int> GetListOfHeroesBuilds()
        {
            return ListHeroBuilds;
        }

        #region private methods
        private void SetNonSupportHeroesWithSupportStat()
        {
            HeroesNonSupportHealingStat.Add("Medivh", HeroRole.Specialist);
            HeroesNonSupportHealingStat.Add("Abathur", HeroRole.Specialist);
            HeroesNonSupportHealingStat.Add("Zarya", HeroRole.Warrior);
            HeroesNonSupportHealingStat.Add("Tyrael", HeroRole.Warrior);
            HeroesNonSupportHealingStat.Add("E.T.C", HeroRole.Warrior);
            HeroesNonSupportHealingStat.Add("Chen", HeroRole.Warrior);
        }

        private void SetPartyIcons()
        {
            PartyIcons.Add(PartyIconColor.Purple, new Uri($"{ApplicationIconsPath}/PartyIcons/ui_ingame_loadscreen_partylink_purple.png", UriKind.Absolute));
            PartyIcons.Add(PartyIconColor.Yellow, new Uri($"{ApplicationIconsPath}/PartyIcons/ui_ingame_loadscreen_partylink_yellow.png", UriKind.Absolute));
            PartyIcons.Add(PartyIconColor.Brown, new Uri($"{ApplicationIconsPath}/PartyIcons/ui_ingame_loadscreen_partylink_brown.png", UriKind.Absolute));
            PartyIcons.Add(PartyIconColor.Teal, new Uri($"{ApplicationIconsPath}/PartyIcons/ui_ingame_loadscreen_partylink_teal.png", UriKind.Absolute));
        }

        private void SetRoleIcons()
        {
            RoleIcons.Add(HeroRole.Warrior, new Uri($"{ApplicationIconsPath}/Roles/hero_role_warrior.png", UriKind.Absolute));
            RoleIcons.Add(HeroRole.Assassin, new Uri($"{ApplicationIconsPath}/Roles/hero_role_assassin.png", UriKind.Absolute));
            RoleIcons.Add(HeroRole.Support, new Uri($"{ApplicationIconsPath}/Roles/hero_role_support.png", UriKind.Absolute));
            RoleIcons.Add(HeroRole.Specialist, new Uri($"{ApplicationIconsPath}/Roles/hero_role_specialist.png", UriKind.Absolute));
        }

        private void SetFranchiseIcons()
        {
            FranchiseIcons.Add(HeroFranchise.Classic, new Uri($"{ApplicationIconsPath}/Roles/hero_franchise_classic.png", UriKind.Absolute));
            FranchiseIcons.Add(HeroFranchise.Diablo, new Uri($"{ApplicationIconsPath}/Roles/hero_franchise_diablo.png", UriKind.Absolute));
            FranchiseIcons.Add(HeroFranchise.Overwatch, new Uri($"{ApplicationIconsPath}/Roles/hero_franchise_overwatch.png", UriKind.Absolute));
            FranchiseIcons.Add(HeroFranchise.Starcraft, new Uri($"{ApplicationIconsPath}/Roles/hero_franchise_starcraft.png", UriKind.Absolute));
            FranchiseIcons.Add(HeroFranchise.Warcraft, new Uri($"{ApplicationIconsPath}/Roles/hero_franchise_warcraft.png", UriKind.Absolute));
        }

        private void SetOtherIcons()
        {
            OtherIcons.Add(OtherIcon.Quest, new Uri($"{ApplicationIconsPath}/storm_ui_ingame_talentpanel_upgrade_quest_icon.dds", UriKind.Absolute));
            OtherIcons.Add(OtherIcon.Silence, new Uri($"{ApplicationIconsPath}/storm_ui_silencepenalty.dds", UriKind.Absolute));
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
