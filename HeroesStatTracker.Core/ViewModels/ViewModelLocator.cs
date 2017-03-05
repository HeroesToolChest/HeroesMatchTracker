using GalaSoft.MvvmLight.Ioc;
using Heroes.Icons;
using HeroesStatTracker.Core.User;
using HeroesStatTracker.Core.ViewModels.Home;
using HeroesStatTracker.Core.ViewModels.Matches;
using HeroesStatTracker.Core.ViewModels.RawData;
using HeroesStatTracker.Core.ViewModels.Replays;
using HeroesStatTracker.Core.ViewModels.TitleBar;
using HeroesStatTracker.Core.ViewServices;
using HeroesStatTracker.Data;
using HeroesStatTracker.Data.Models.Replays;
using HeroesStatTracker.Data.Queries.Replays;
using Microsoft.Practices.ServiceLocation;

namespace HeroesStatTracker.Core.ViewModels
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

            SimpleIoc.Default.Register(() => { return new HeroesIcons(true); });
            SimpleIoc.Default.Register<IHeroesIconsService>(() =>
            {
                return SimpleIoc.Default.GetInstance<HeroesIcons>();
            });

            SimpleIoc.Default.Register<IUserProfileService, UserProfile>();

            // start ups
            SimpleIoc.Default.Register<MainWindowViewModel>();
            SimpleIoc.Default.Register<StartupWindowViewModel>();
            SimpleIoc.Default.Register<ProfileWindowViewModel>();

            // Home
            SimpleIoc.Default.Register<HomeViewModel>();

            // TitleBar
            SimpleIoc.Default.Register<SettingsControlViewModel>();
            SimpleIoc.Default.Register<AboutControlViewModel>();
            SimpleIoc.Default.Register<WhatsNewWindowViewModel>();

            // Replays
            SimpleIoc.Default.Register<ReplaysControlViewModel>();

            // RawData
            SimpleIoc.Default.Register<IRawDataQueries<ReplayMatch>, MatchReplay>();
            SimpleIoc.Default.Register<IRawDataQueries<ReplayAllHotsPlayer>, HotsPlayer>();

            SimpleIoc.Default.Register<IRawDataQueries<ReplayAllHotsPlayerHero>, HotsPlayerHero>();
            SimpleIoc.Default.Register<IRawDataQueries<ReplayHotsLogsUpload>, HotsLogsUpload>();
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

            SimpleIoc.Default.Register<RawMatchReplayViewModel>();
            SimpleIoc.Default.Register<RawAllHotsPlayerViewModel>();
            SimpleIoc.Default.Register<RawAllHotsPlayerHeroesViewModel>();
            SimpleIoc.Default.Register<RawHotsLogsUploadViewModel>();
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

            // Matches
            SimpleIoc.Default.Register<MatchesViewModel>();
            SimpleIoc.Default.Register<AllMatchesViewModel>();
            SimpleIoc.Default.Register<BrawlViewModel>();
            SimpleIoc.Default.Register<CustomGameViewModel>();
            SimpleIoc.Default.Register<HeroLeagueViewModel>();
            SimpleIoc.Default.Register<QuickMatchViewModel>();
            SimpleIoc.Default.Register<TeamLeagueViewModel>();
            SimpleIoc.Default.Register<UnrankedDraftViewModel>();
            SimpleIoc.Default.Register<MatchSummaryViewModel>();
        }

        // start ups
        public static MainWindowViewModel MainWindowViewModel => ServiceLocator.Current.GetInstance<MainWindowViewModel>();
        public static StartupWindowViewModel StartupWindowViewModel => ServiceLocator.Current.GetInstance<StartupWindowViewModel>();
        public static ProfileWindowViewModel ProfileWindowViewModel => ServiceLocator.Current.GetInstance<ProfileWindowViewModel>();

        // Home
        public static HomeViewModel HomeViewModel => ServiceLocator.Current.GetInstance<HomeViewModel>();

        // TitleBar
        public static SettingsControlViewModel SettingsControlViewModel => ServiceLocator.Current.GetInstance<SettingsControlViewModel>();
        public static AboutControlViewModel AboutControlViewModel => ServiceLocator.Current.GetInstance<AboutControlViewModel>();
        public static WhatsNewWindowViewModel WhatsNewWindowViewModel => ServiceLocator.Current.GetInstance<WhatsNewWindowViewModel>();

        // Replays
        public static ReplaysControlViewModel ReplaysControlViewModel => ServiceLocator.Current.GetInstance<ReplaysControlViewModel>();

        // RawData
        public static RawMatchReplayViewModel RawMatchReplayViewModel => ServiceLocator.Current.GetInstance<RawMatchReplayViewModel>();
        public static RawAllHotsPlayerViewModel RawAllHotsPlayerViewModel => ServiceLocator.Current.GetInstance<RawAllHotsPlayerViewModel>();
        public static RawAllHotsPlayerHeroesViewModel RawAllHotsPlayerHeroesViewModel => ServiceLocator.Current.GetInstance<RawAllHotsPlayerHeroesViewModel>();
        public static RawHotsLogsUploadViewModel RawHotsLogsUploadViewModel => ServiceLocator.Current.GetInstance<RawHotsLogsUploadViewModel>();
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

        // Matches
        public static MatchesViewModel MatchesViewModel => ServiceLocator.Current.GetInstance<MatchesViewModel>();
        public static AllMatchesViewModel AllMatchesViewModel => ServiceLocator.Current.GetInstance<AllMatchesViewModel>();
        public static BrawlViewModel BrawlViewModel => ServiceLocator.Current.GetInstance<BrawlViewModel>();
        public static CustomGameViewModel CustomGameViewModel => ServiceLocator.Current.GetInstance<CustomGameViewModel>();
        public static HeroLeagueViewModel HeroLeagueViewModel => ServiceLocator.Current.GetInstance<HeroLeagueViewModel>();
        public static QuickMatchViewModel QuickMatchViewModel => ServiceLocator.Current.GetInstance<QuickMatchViewModel>();
        public static TeamLeagueViewModel TeamLeagueViewModel => ServiceLocator.Current.GetInstance<TeamLeagueViewModel>();
        public static UnrankedDraftViewModel UnrankedDraftViewModel => ServiceLocator.Current.GetInstance<UnrankedDraftViewModel>();
        public static MatchSummaryViewModel MatchSummaryViewModel => ServiceLocator.Current.GetInstance<MatchSummaryViewModel>();

        public static void Cleanup()
        {
            // TODO Clear the ViewModels
        }
    }
}