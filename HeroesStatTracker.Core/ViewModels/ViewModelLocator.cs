using GalaSoft.MvvmLight.Ioc;
using HeroesStatTracker.Core.ViewModels.RawData;
using HeroesStatTracker.Core.ViewModels.Replays;
using HeroesStatTracker.Core.ViewModels.TitleBar;
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
        public ViewModelLocator()
        {
            ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);

            SimpleIoc.Default.Register<MainWindowViewModel>();
            SimpleIoc.Default.Register<StartupWindowViewModel>();

            // TitleBar
            SimpleIoc.Default.Register<SettingsControlViewModel>();
            SimpleIoc.Default.Register<AboutControlViewModel>();
            SimpleIoc.Default.Register<PaletteSelectorWindowViewModel>();
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
        }

        public static MainWindowViewModel MainWindowViewModel => ServiceLocator.Current.GetInstance<MainWindowViewModel>();
        public static StartupWindowViewModel StartupWindowViewModel => ServiceLocator.Current.GetInstance<StartupWindowViewModel>();
        public static SettingsControlViewModel SettingsControlViewModel => ServiceLocator.Current.GetInstance<SettingsControlViewModel>();
        public static AboutControlViewModel AboutControlViewModel => ServiceLocator.Current.GetInstance<AboutControlViewModel>();
        public static PaletteSelectorWindowViewModel PaletteSelectorWindowViewModel => ServiceLocator.Current.GetInstance<PaletteSelectorWindowViewModel>();
        public static WhatsNewWindowViewModel WhatsNewWindowViewModel => ServiceLocator.Current.GetInstance<WhatsNewWindowViewModel>();
        public static ReplaysControlViewModel ReplaysControlViewModel => ServiceLocator.Current.GetInstance<ReplaysControlViewModel>();
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

        public static void Cleanup()
        {
            // TODO Clear the ViewModels
        }
    }
}