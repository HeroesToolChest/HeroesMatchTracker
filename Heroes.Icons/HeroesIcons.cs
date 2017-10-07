using Heroes.Icons.Models;
using Heroes.Icons.Xml;
using System;
using System.Collections.Generic;
using System.IO;

namespace Heroes.Icons
{
    public class HeroesIcons : HeroesBase, IHeroesIconsService
    {
        private readonly string PartyIconFolderName = "PartyIcons";
        private readonly string RoleIconFolderName = "Roles";
        private readonly string FranchiseIconFolderName = "Franchises";

        private readonly HeroBuildsXml HeroBuildsXmlLatest; // holds the latest build info

        private int EarliestHeroesBuild;
        private int LatestHeroesBuild;
        private bool Logger;

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

            try
            {
               // HeroesXml = HeroesXml.Initialize("Heroes.xml", "Heroes", Logger, null);
                HeroBuildsXmlLatest = HeroBuildsXmlHolder = HeroBuildsXml = HeroBuildsXml.Initialize("_AllHeroes.xml", "HeroBuilds", Logger); // load in all three

                EarliestHeroesBuild = HeroBuildsXml.EarliestHeroesBuild;
                LatestHeroesBuild = HeroBuildsXml.LatestHeroesBuild;
                ListHeroBuilds = HeroBuildsXml.Builds;

                MatchAwardsXml = MatchAwardsXml.Initialize("_AllMatchAwards.xml", "MatchAwards", LatestHeroesBuild, logger);
                MapBackgroundsXml = MapBackgroundsXml.Initialize("_AllMapBackgrounds.xml", "MapBackgrounds", LatestHeroesBuild, logger);
                HomeScreensXml = HomeScreensXml.Initialize("HomeScreens.xml", "HomeScreens", LatestHeroesBuild, logger);
            }
            catch (Exception ex)
            {
                if (logger)
                    LogErrors($"Error on HeroIcons initializing{Environment.NewLine}{ex}");
                throw;
            }

            SetNonSupportHeroesWithSupportStat();
            SetPartyIcons();
            SetRoleIcons();
            SetFranchiseIcons();
            SetOtherIcons();
        }

        public int GetLatestHeroesBuild() => LatestHeroesBuild;

        /// <summary>
        /// Load a specific build, other than the latest one. Use LoadLatestHeroesBuild to load latest build.
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

            try
            {
                // only load the build if it's not in memory already or in the in xmlHolder
                if (build != HeroBuildsXml.CurrentLoadedHeroesBuild)
                {
                    if (build != HeroBuildsXmlHolder.CurrentLoadedHeroesBuild)
                        HeroBuildsXml = HeroBuildsXml.Initialize("_AllHeroes.xml", "HeroBuilds", Logger, build);
                    else
                        HeroBuildsXml = HeroBuildsXmlHolder;
                }
            }
            catch (Exception ex)
            {
                throw new ParseXmlException($"Error on loading heroes build {build}{Environment.NewLine}{ex}");
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

        public Uri GetPartyIcon(PartyIconColor partyIconColor)
        {
            if (PartyIcons.ContainsKey(partyIconColor))
            {
                return PartyIcons[partyIconColor];
            }
            else
            {
                LogReferenceNameNotFound($"Other Icons: {partyIconColor}");
                return null;
            }
        }

        public Uri GetOtherIcon(OtherIcon otherIcon)
        {
            if (OtherIcons.ContainsKey(otherIcon))
            {
                return OtherIcons[otherIcon];
            }
            else
            {
                LogReferenceNameNotFound($"Other Icons: {otherIcon}");
                return null;
            }
        }

        public Uri GetRoleIcon(HeroRole heroRole)
        {
            if (RoleIcons.ContainsKey(heroRole))
            {
                return RoleIcons[heroRole];
            }
            else
            {
                LogReferenceNameNotFound($"Other Icons: {heroRole}");
                return null;
            }
        }

        public Uri GetFranchiseIcon(HeroFranchise heroFranchise)
        {
            if (FranchiseIcons.ContainsKey(heroFranchise))
            {
                return FranchiseIcons[heroFranchise];

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

        public List<string> GetListOfHeroesBuilds()
        {
            return ListHeroBuilds.ConvertAll(x => x.ToString());
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
            PartyIcons.Add(PartyIconColor.Purple, GetImageUri(PartyIconFolderName, "ui_ingame_loadscreen_partylink_purple.png"));
            PartyIcons.Add(PartyIconColor.Yellow, GetImageUri(PartyIconFolderName, "ui_ingame_loadscreen_partylink_yellow.png"));
            PartyIcons.Add(PartyIconColor.Brown, GetImageUri(PartyIconFolderName, "ui_ingame_loadscreen_partylink_brown.png"));
            PartyIcons.Add(PartyIconColor.Teal, GetImageUri(PartyIconFolderName, "ui_ingame_loadscreen_partylink_teal.png"));
        }

        private void SetRoleIcons()
        {
            RoleIcons.Add(HeroRole.Warrior, GetImageUri(RoleIconFolderName, "hero_role_warrior.png"));
            RoleIcons.Add(HeroRole.Assassin, GetImageUri(RoleIconFolderName, "hero_role_assassin.png"));
            RoleIcons.Add(HeroRole.Support, GetImageUri(RoleIconFolderName, "hero_role_support.png"));
            RoleIcons.Add(HeroRole.Specialist, GetImageUri(RoleIconFolderName, "hero_role_specialist.png"));
        }

        private void SetFranchiseIcons()
        {
            FranchiseIcons.Add(HeroFranchise.Classic, GetImageUri(FranchiseIconFolderName, "hero_franchise_classic.png"));
            FranchiseIcons.Add(HeroFranchise.Diablo, GetImageUri(FranchiseIconFolderName, "hero_franchise_diablo.png"));
            FranchiseIcons.Add(HeroFranchise.Overwatch, GetImageUri(FranchiseIconFolderName, "hero_franchise_overwatch.png"));
            FranchiseIcons.Add(HeroFranchise.Starcraft, GetImageUri(FranchiseIconFolderName, "hero_franchise_starcraft.png"));
            FranchiseIcons.Add(HeroFranchise.Warcraft, GetImageUri(FranchiseIconFolderName, "hero_franchise_warcraft.png"));
        }

        private void SetOtherIcons()
        {
            OtherIcons.Add(OtherIcon.Quest, GetImageUri(string.Empty, "storm_ui_taskbar_buttonicon_quests.png"));
            OtherIcons.Add(OtherIcon.Silence, GetImageUri(string.Empty, "storm_ui_silencepenalty.png"));
        }

        private void LogMissingImage(string message)
        {
            using (StreamWriter writer = new StreamWriter(File.Open(Path.Combine(LogFileName, ImageMissingLogName), FileMode.Append)))
            {
                writer.WriteLine($"[{HeroBuildsXml.CurrentLoadedHeroesBuild}] {message}");
            }
        }

        private void LogReferenceNameNotFound(string message)
        {
            using (StreamWriter writer = new StreamWriter(File.Open(Path.Combine(LogFileName, ReferenceLogName), FileMode.Append)))
            {
                writer.WriteLine($"[{HeroBuildsXml.CurrentLoadedHeroesBuild}] {message}");
            }
        }

        private void LogErrors(string message)
        {
            using (StreamWriter writer = new StreamWriter(File.Open(Path.Combine(LogFileName, XmlErrorsLogName), FileMode.Append)))
            {
                writer.WriteLine(message);
            }
        }
        #endregion private methods
    }
}
