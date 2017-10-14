using Heroes.Icons.Models;
using Heroes.Icons.Xml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace Heroes.Icons
{
    public class HeroesIcons : HeroesBase, IHeroesIconsService
    {
        private const string PartyIconFolderName = "PartyIcons";
        private const string RoleIconFolderName = "Roles";
        private const string FranchiseIconFolderName = "Franchises";
        private const string OtherFolderName = "Other";

        private static Assembly HeroesIconsAssembly = Assembly.GetExecutingAssembly();
        private static Dictionary<OtherIcon, string> OtherIcons = new Dictionary<OtherIcon, string>();

        private readonly HeroBuildsXml HeroBuildsXmlLatest; // holds the latest build info

        private Dictionary<PartyIconColor, string> PartyIcons = new Dictionary<PartyIconColor, string>();
        private Dictionary<HeroRole, string> RoleIcons = new Dictionary<HeroRole, string>();
        private Dictionary<HeroFranchise, string> FranchiseIcons = new Dictionary<HeroFranchise, string>();
        private Dictionary<string, HeroRole> HeroesNonSupportHealingStat = new Dictionary<string, HeroRole>();

        private int EarliestHeroesBuild;
        private int LatestHeroesBuild;
        private bool Logger;

        private HeroBuildsXml HeroBuildsXml; // the one that is in use
        private HeroBuildsXml HeroBuildsXmlHolder; // used for swapping between the one that is in use and latest
        private MatchAwardsXml MatchAwardsXml;
        private MapBackgroundsXml MapBackgroundsXml;
        private HomeScreensXml HomeScreensXml;
        private List<int> ListHeroBuilds;

        public HeroesIcons(bool logger)
        {
            Logger = logger;

            try
            {
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

        public static Stream GetOtherIcon(OtherIcon otherIcon)
        {
            if (OtherIcons.ContainsKey(otherIcon))
            {
                return HeroesIconsAssembly.GetManifestResourceStream(OtherIcons[otherIcon]);
            }

            return null;
        }

        public static Assembly GetHeroesIconsAssembly()
        {
            return HeroesIconsAssembly;
        }

        public Stream GetPartyIcon(PartyIconColor partyIconColor)
        {
            if (PartyIcons.ContainsKey(partyIconColor))
            {
                return HeroesIconsAssembly.GetManifestResourceStream(PartyIcons[partyIconColor]);
            }

            return null;
        }

        public Stream GetRoleIcon(HeroRole heroRole)
        {
            if (heroRole == HeroRole.Multiclass)
                return null;

            if (RoleIcons.ContainsKey(heroRole))
            {
                return HeroesIconsAssembly.GetManifestResourceStream(RoleIcons[heroRole]);
            }

            return null;
        }

        public Stream GetFranchiseIcon(HeroFranchise heroFranchise)
        {
            if (FranchiseIcons.ContainsKey(heroFranchise))
            {
                return HeroesIconsAssembly.GetManifestResourceStream(FranchiseIcons[heroFranchise]);
            }

            return null;
        }

        Stream IHeroesIconsService.GetOtherIcon(OtherIcon otherIcon) => GetOtherIcon(otherIcon);

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

        public IHomeScreens Homescreens()
        {
            return HomeScreensXml;
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
            PartyIcons.Add(PartyIconColor.Purple, SetImageStreamString(PartyIconFolderName, "ui_ingame_loadscreen_partylink_purple.png"));
            PartyIcons.Add(PartyIconColor.Yellow, SetImageStreamString(PartyIconFolderName, "ui_ingame_loadscreen_partylink_yellow.png"));
            PartyIcons.Add(PartyIconColor.Brown, SetImageStreamString(PartyIconFolderName, "ui_ingame_loadscreen_partylink_brown.png"));
            PartyIcons.Add(PartyIconColor.Teal, SetImageStreamString(PartyIconFolderName, "ui_ingame_loadscreen_partylink_teal.png"));
        }

        private void SetRoleIcons()
        {
            RoleIcons.Add(HeroRole.Unknown, SetImageStreamString(HeroPortraitsFolderName, "storm_ui_ingame_heroselect_btn_notfound.dds"));

            RoleIcons.Add(HeroRole.Warrior, SetImageStreamString(RoleIconFolderName, "hero_role_warrior.png"));
            RoleIcons.Add(HeroRole.Assassin, SetImageStreamString(RoleIconFolderName, "hero_role_assassin.png"));
            RoleIcons.Add(HeroRole.Support, SetImageStreamString(RoleIconFolderName, "hero_role_support.png"));
            RoleIcons.Add(HeroRole.Specialist, SetImageStreamString(RoleIconFolderName, "hero_role_specialist.png"));
        }

        private void SetFranchiseIcons()
        {
            FranchiseIcons.Add(HeroFranchise.Unknown, SetImageStreamString(HeroPortraitsFolderName, "storm_ui_ingame_heroselect_btn_notfound.dds"));

            FranchiseIcons.Add(HeroFranchise.Classic, SetImageStreamString(FranchiseIconFolderName, "hero_franchise_classic.png"));
            FranchiseIcons.Add(HeroFranchise.Diablo, SetImageStreamString(FranchiseIconFolderName, "hero_franchise_diablo.png"));
            FranchiseIcons.Add(HeroFranchise.Overwatch, SetImageStreamString(FranchiseIconFolderName, "hero_franchise_overwatch.png"));
            FranchiseIcons.Add(HeroFranchise.Starcraft, SetImageStreamString(FranchiseIconFolderName, "hero_franchise_starcraft.png"));
            FranchiseIcons.Add(HeroFranchise.Warcraft, SetImageStreamString(FranchiseIconFolderName, "hero_franchise_warcraft.png"));
        }

        private void SetOtherIcons()
        {
            if (OtherIcons.Count > 0)
                return;

            OtherIcons.Add(OtherIcon.Quest, SetImageStreamString(OtherFolderName, "storm_ui_taskbar_buttonicon_quests.png"));
            OtherIcons.Add(OtherIcon.UpgradeQuest, SetImageStreamString(OtherFolderName, "storm_ui_ingame_talentpanel_upgrade_quest_icon.png"));
            OtherIcons.Add(OtherIcon.Silence, SetImageStreamString(OtherFolderName, "storm_ui_silencepenalty.png"));

            OtherIcons.Add(OtherIcon.LongarrowLeftDisabled, SetImageStreamString(OtherFolderName, "storm_ui_glues_button_longarrow_left_disabled.png"));
            OtherIcons.Add(OtherIcon.LongarrowLeftDown, SetImageStreamString(OtherFolderName, "storm_ui_glues_button_longarrow_left_down.png"));
            OtherIcons.Add(OtherIcon.LongarrowLeftHover, SetImageStreamString(OtherFolderName, "storm_ui_glues_button_longarrow_left_hover.png"));
            OtherIcons.Add(OtherIcon.LongarrowLeftNormal, SetImageStreamString(OtherFolderName, "storm_ui_glues_button_longarrow_left_normal.png"));
            OtherIcons.Add(OtherIcon.LongarrowRightDisabled, SetImageStreamString(OtherFolderName, "storm_ui_glues_button_longarrow_right_disabled.png"));
            OtherIcons.Add(OtherIcon.LongarrowRightDown, SetImageStreamString(OtherFolderName, "storm_ui_glues_button_longarrow_right_down.png"));
            OtherIcons.Add(OtherIcon.LongarrowRightHover, SetImageStreamString(OtherFolderName, "storm_ui_glues_button_longarrow_right_hover.png"));
            OtherIcons.Add(OtherIcon.LongarrowRightNormal, SetImageStreamString(OtherFolderName, "storm_ui_glues_button_longarrow_right_normal.png"));

            OtherIcons.Add(OtherIcon.TalentUnavailable, SetImageStreamString(OtherFolderName, "storm_ui_ingame_leader_talent_unavailable.png"));

            OtherIcons.Add(OtherIcon.StatusResistShieldDefault, SetImageStreamString(OtherFolderName, "storm_ui_ingame_status_resistshieldc3_default.png"));
            OtherIcons.Add(OtherIcon.StatusResistShieldPhysical, SetImageStreamString(OtherFolderName, "storm_ui_ingame_status_resistshieldc3_physical.png"));
            OtherIcons.Add(OtherIcon.StatusResistShieldSpell, SetImageStreamString(OtherFolderName, "storm_ui_ingame_status_resistshieldc3_spell.png"));

            OtherIcons.Add(OtherIcon.FilterAssassin, SetImageStreamString(OtherFolderName, "storm_ui_play_filter_assassin-on.png"));
            OtherIcons.Add(OtherIcon.FilterMulticlass, SetImageStreamString(OtherFolderName, "storm_ui_play_filter_multiclass-on.png"));
            OtherIcons.Add(OtherIcon.FilterSpecialist, SetImageStreamString(OtherFolderName, "storm_ui_play_filter_specialist-on.png"));
            OtherIcons.Add(OtherIcon.FilterSupport, SetImageStreamString(OtherFolderName, "storm_ui_play_filter_support-on.png"));
            OtherIcons.Add(OtherIcon.FilterWarrior, SetImageStreamString(OtherFolderName, "storm_ui_play_filter_warrior-on.png"));

            OtherIcons.Add(OtherIcon.Kills, SetImageStreamString(OtherFolderName, "storm_ui_scorescreen_icon_kill.png"));
            OtherIcons.Add(OtherIcon.Assist, SetImageStreamString(OtherFolderName, "storm_ui_scorescreen_icon_assist.png"));
            OtherIcons.Add(OtherIcon.Death, SetImageStreamString(OtherFolderName, "storm_ui_scorescreen_icon_death.png"));

            OtherIcons.Add(OtherIcon.KillsBlue, SetImageStreamString(OtherFolderName, "storm_ui_scorescreen_icon_kills_blue.png"));
            OtherIcons.Add(OtherIcon.KillsRed, SetImageStreamString(OtherFolderName, "storm_ui_scorescreen_icon_kills_red.png"));
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
