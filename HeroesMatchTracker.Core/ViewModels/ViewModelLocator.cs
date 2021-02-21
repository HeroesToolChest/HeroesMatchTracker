using GalaSoft.MvvmLight.Ioc;
using Heroes.Icons;
using HeroesMatchTracker.Core.Services;
using HeroesMatchTracker.Core.User;
using HeroesMatchTracker.Core.ViewModels.Home;
using HeroesMatchTracker.Core.ViewModels.Matches;
using HeroesMatchTracker.Core.ViewModels.RawData;
using HeroesMatchTracker.Core.ViewModels.Replays;
using HeroesMatchTracker.Core.ViewModels.Statistics;
using HeroesMatchTracker.Core.ViewModels.TitleBar;
using HeroesMatchTracker.Data;
using HeroesMatchTracker.Data.Models.Replays;
using HeroesMatchTracker.Data.Queries.Replays;
using Microsoft.Practices.ServiceLocation;

namespace HeroesMatchTracker.Core.ViewModels
{
    /// <summary>
    /// This class contains static references to all the view models in the
    /// application and provides an entry point for the bindings.
    /// </summary>
    public class ViewModelLocator
    {
        /// <summary>
        /// Initializes a new instance of the ViewModelLocator class.
        /// </summary>
        static ViewModelLocator()
        {
            ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);

            // Services
            SimpleIoc.Default.Register<IDatabaseService, DatabaseService>();

            SimpleIoc.Default.Register(() => { return new HeroesIcons(); });
            SimpleIoc.Default.Register<IHeroesIcons>(() =>
            {
                return SimpleIoc.Default.GetInstance<HeroesIcons>();
            });

            SimpleIoc.Default.Register<ISelectedUserProfileService, SelectedUserProfile>();
            SimpleIoc.Default.Register<IWebsiteService, Website>();
            SimpleIoc.Default.Register<IInternalService, InternalService>();

            // start ups
            SimpleIoc.Default.Register<MainWindowViewModel>();
            SimpleIoc.Default.Register<StartupWindowViewModel>();

            // Home
            SimpleIoc.Default.Register<HomeViewModel>();
            SimpleIoc.Default.Register<UserProfileWindowViewModel>();
            SimpleIoc.Default.Register<ToasterUpdateWindowViewModel>();

            // TitleBar
            SimpleIoc.Default.Register<SettingsControlViewModel>();
            SimpleIoc.Default.Register<AboutControlViewModel>();
            SimpleIoc.Default.Register<WhatsNewWindowViewModel>();

            // Replays
            SimpleIoc.Default.Register<ReplaysControlViewModel>();
            SimpleIoc.Default.Register<FailedReplaysWindowViewModel>();

            // RawData
            SimpleIoc.Default.Register<IRawDataQueries<ReplayMatch>, MatchReplay>();
            SimpleIoc.Default.Register<IRawDataQueries<ReplayAllHotsPlayer>, HotsPlayer>();

            SimpleIoc.Default.Register<IRawDataQueries<ReplayHotsApiUpload>, HotsApiUpload>();
            SimpleIoc.Default.Register<IRawDataQueries<ReplayHeroesProfileUpload>, HeroesProfileUpload>();
            SimpleIoc.Default.Register<IRawDataQueries<ReplayMatchAward>, MatchAward>();
            SimpleIoc.Default.Register<IRawDataQueries<ReplayMatchMessage>, MatchMessage>();
            SimpleIoc.Default.Register<IRawDataQueries<ReplayMatchPlayer>, MatchPlayer>();
            SimpleIoc.Default.Register<IRawDataQueries<ReplayMatchPlayerScoreResult>, MatchPlayerScoreResult>();
            SimpleIoc.Default.Register<IRawDataQueries<ReplayMatchPlayerTalent>, MatchPlayerTalent>();
            SimpleIoc.Default.Register<IRawDataQueries<ReplayMatchTeamBan>, MatchTeamBan>();
            SimpleIoc.Default.Register<IRawDataQueries<ReplayMatchTeamExperience>, MatchTeamExperience>();
            SimpleIoc.Default.Register<IRawDataQueries<ReplayMatchTeamLevel>, MatchTeamLevel>();
            SimpleIoc.Default.Register<IRawDataQueries<ReplayMatchTeamObjective>, MatchTeamObjective>();
            SimpleIoc.Default.Register<IRawDataQueries<ReplayRenamedPlayer>, RenamedPlayer>();
            SimpleIoc.Default.Register<IRawDataQueries<ReplayMatchDraftPick>, MatchDraft>();

            SimpleIoc.Default.Register<RawMatchReplayViewModel>();
            SimpleIoc.Default.Register<RawAllHotsPlayerViewModel>();
            SimpleIoc.Default.Register<RawHotsApiUploadViewModel>();
            SimpleIoc.Default.Register<RawHeroesProfileUploadViewModel>();
            SimpleIoc.Default.Register<RawMatchAwardViewModel>();
            SimpleIoc.Default.Register<RawMatchMessageViewModel>();
            SimpleIoc.Default.Register<RawMatchPlayerScoreResultViewModel>();
            SimpleIoc.Default.Register<RawMatchPlayerTalentViewModel>();
            SimpleIoc.Default.Register<RawMatchPlayerViewModel>();
            SimpleIoc.Default.Register<RawMatchTeamBanViewModel>();
            SimpleIoc.Default.Register<RawMatchTeamExperienceViewModel>();
            SimpleIoc.Default.Register<RawMatchTeamLevelViewModel>();
            SimpleIoc.Default.Register<RawMatchTeamObjectiveViewModel>();
            SimpleIoc.Default.Register<RawRenamedPlayerViewModel>();
            SimpleIoc.Default.Register<RawMatchDraftViewModel>();

            // Matches
            SimpleIoc.Default.Register<MatchesViewModel>();
            SimpleIoc.Default.Register<AllMatchesViewModel>();
            SimpleIoc.Default.Register<BrawlViewModel>();
            SimpleIoc.Default.Register<ARAMViewModel>();
            SimpleIoc.Default.Register<CustomGameViewModel>();
            SimpleIoc.Default.Register<StormLeagueViewModel>();
            SimpleIoc.Default.Register<HeroLeagueViewModel>();
            SimpleIoc.Default.Register<QuickMatchViewModel>();
            SimpleIoc.Default.Register<TeamLeagueViewModel>();
            SimpleIoc.Default.Register<UnrankedDraftViewModel>();
            SimpleIoc.Default.Register<MatchSummaryViewModel>();
            SimpleIoc.Default.Register<PlayerNotesWindowViewModel>();

            // Statistics
            SimpleIoc.Default.Register<StatsOverviewViewModel>();
            SimpleIoc.Default.Register<StatsHeroesViewModel>();
            SimpleIoc.Default.Register<StatsAllHeroesViewModel>();
            SimpleIoc.Default.Register<StatsPartiesViewModel>();
        }

        // start ups
        public static MainWindowViewModel MainWindowViewModel => ServiceLocator.Current.GetInstance<MainWindowViewModel>();
        public static StartupWindowViewModel StartupWindowViewModel => ServiceLocator.Current.GetInstance<StartupWindowViewModel>();

        // Home
        public static HomeViewModel HomeViewModel => ServiceLocator.Current.GetInstance<HomeViewModel>();
        public static UserProfileWindowViewModel UserProfileWindowViewModel => ServiceLocator.Current.GetInstance<UserProfileWindowViewModel>();
        public static ToasterUpdateWindowViewModel ToasterUpdateWindowViewModel => ServiceLocator.Current.GetInstance<ToasterUpdateWindowViewModel>();

        // TitleBar
        public static SettingsControlViewModel SettingsControlViewModel => ServiceLocator.Current.GetInstance<SettingsControlViewModel>();
        public static AboutControlViewModel AboutControlViewModel => ServiceLocator.Current.GetInstance<AboutControlViewModel>();
        public static WhatsNewWindowViewModel WhatsNewWindowViewModel => ServiceLocator.Current.GetInstance<WhatsNewWindowViewModel>();

        // Replays
        public static ReplaysControlViewModel ReplaysControlViewModel => ServiceLocator.Current.GetInstance<ReplaysControlViewModel>();
        public static FailedReplaysWindowViewModel FailedReplaysWindowViewModel => ServiceLocator.Current.GetInstance<FailedReplaysWindowViewModel>();

        // RawData
        public static RawMatchReplayViewModel RawMatchReplayViewModel => ServiceLocator.Current.GetInstance<RawMatchReplayViewModel>();
        public static RawAllHotsPlayerViewModel RawAllHotsPlayerViewModel => ServiceLocator.Current.GetInstance<RawAllHotsPlayerViewModel>();
        public static RawHotsApiUploadViewModel RawHotsApiUploadViewModel => ServiceLocator.Current.GetInstance<RawHotsApiUploadViewModel>();
        public static RawHeroesProfileUploadViewModel RawHeroesProfileUploadViewModel => ServiceLocator.Current.GetInstance<RawHeroesProfileUploadViewModel>();
        public static RawMatchAwardViewModel RawMatchAwardViewModel => ServiceLocator.Current.GetInstance<RawMatchAwardViewModel>();
        public static RawMatchMessageViewModel RawMatchMessageViewModel => ServiceLocator.Current.GetInstance<RawMatchMessageViewModel>();
        public static RawMatchPlayerScoreResultViewModel RawMatchPlayerScoreResultViewModel => ServiceLocator.Current.GetInstance<RawMatchPlayerScoreResultViewModel>();
        public static RawMatchPlayerTalentViewModel RawMatchPlayerTalentViewModel => ServiceLocator.Current.GetInstance<RawMatchPlayerTalentViewModel>();
        public static RawMatchPlayerViewModel RawMatchPlayerViewModel => ServiceLocator.Current.GetInstance<RawMatchPlayerViewModel>();
        public static RawMatchTeamBanViewModel RawMatchTeamBanViewModel => ServiceLocator.Current.GetInstance<RawMatchTeamBanViewModel>();
        public static RawMatchTeamExperienceViewModel RawMatchTeamExperienceViewModel => ServiceLocator.Current.GetInstance<RawMatchTeamExperienceViewModel>();
        public static RawMatchTeamLevelViewModel RawMatchTeamLevelViewModel => ServiceLocator.Current.GetInstance<RawMatchTeamLevelViewModel>();
        public static RawMatchTeamObjectiveViewModel RawMatchTeamObjectiveViewModel => ServiceLocator.Current.GetInstance<RawMatchTeamObjectiveViewModel>();
        public static RawRenamedPlayerViewModel RawRenamedPlayerViewModel => ServiceLocator.Current.GetInstance<RawRenamedPlayerViewModel>();
        public static RawMatchDraftViewModel RawMatchDraftViewModel => ServiceLocator.Current.GetInstance<RawMatchDraftViewModel>();

        // Matches
        public static MatchesViewModel MatchesViewModel => ServiceLocator.Current.GetInstance<MatchesViewModel>();
        public static AllMatchesViewModel AllMatchesViewModel => ServiceLocator.Current.GetInstance<AllMatchesViewModel>();
        public static BrawlViewModel BrawlViewModel => ServiceLocator.Current.GetInstance<BrawlViewModel>();
        public static ARAMViewModel ARAMViewModel => ServiceLocator.Current.GetInstance<ARAMViewModel>();
        public static CustomGameViewModel CustomGameViewModel => ServiceLocator.Current.GetInstance<CustomGameViewModel>();
        public static StormLeagueViewModel StormLeagueViewModel => ServiceLocator.Current.GetInstance<StormLeagueViewModel>();
        public static HeroLeagueViewModel HeroLeagueViewModel => ServiceLocator.Current.GetInstance<HeroLeagueViewModel>();
        public static QuickMatchViewModel QuickMatchViewModel => ServiceLocator.Current.GetInstance<QuickMatchViewModel>();
        public static TeamLeagueViewModel TeamLeagueViewModel => ServiceLocator.Current.GetInstance<TeamLeagueViewModel>();
        public static UnrankedDraftViewModel UnrankedDraftViewModel => ServiceLocator.Current.GetInstance<UnrankedDraftViewModel>();
        public static MatchSummaryViewModel MatchSummaryViewModel => ServiceLocator.Current.GetInstance<MatchSummaryViewModel>();
        public static PlayerNotesWindowViewModel PlayerNotesWindowViewModel => ServiceLocator.Current.GetInstance<PlayerNotesWindowViewModel>();

        // Statistics
        public static StatsOverviewViewModel StatsOverviewViewModel => ServiceLocator.Current.GetInstance<StatsOverviewViewModel>();
        public static StatsHeroesViewModel StatsHeroesViewModel => ServiceLocator.Current.GetInstance<StatsHeroesViewModel>();
        public static StatsAllHeroesViewModel StatsAllHeroesViewModel => ServiceLocator.Current.GetInstance<StatsAllHeroesViewModel>();
        public static StatsPartiesViewModel StatsPartiesViewModel => ServiceLocator.Current.GetInstance<StatsPartiesViewModel>();

        public static void Cleanup()
        {
        }
    }
}