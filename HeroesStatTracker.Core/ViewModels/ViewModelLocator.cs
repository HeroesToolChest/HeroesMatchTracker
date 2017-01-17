using GalaSoft.MvvmLight.Ioc;
using HeroesStatTracker.Core.ViewModels.Replays;
using HeroesStatTracker.Core.ViewModels.TitleBar;
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

            ////if (ViewModelBase.IsInDesignModeStatic)
            ////{
            ////    // Create design time view services and models
            ////    SimpleIoc.Default.Register<IDataService, DesignDataService>();
            ////}
            ////else
            ////{
            ////    // Create run time view services and models
            ////    SimpleIoc.Default.Register<IDataService, DataService>();
            ////}

            SimpleIoc.Default.Register<MainWindowViewModel>();
            SimpleIoc.Default.Register<StartupWindowViewModel>();

            // TitleBar
            SimpleIoc.Default.Register<SettingsControlViewModel>();
            SimpleIoc.Default.Register<AboutControlViewModel>();
            SimpleIoc.Default.Register<PaletteSelectorWindowViewModel>();
            SimpleIoc.Default.Register<WhatsNewWindowViewModel>();

            // Replays
            SimpleIoc.Default.Register<ReplaysControlViewModel>();
        }

        public static MainWindowViewModel MainWindowViewModel { get { return ServiceLocator.Current.GetInstance<MainWindowViewModel>(); } }
        public static StartupWindowViewModel StartupWindowViewModel { get { return ServiceLocator.Current.GetInstance<StartupWindowViewModel>(); } }
        public static SettingsControlViewModel SettingsControlViewModel { get { return ServiceLocator.Current.GetInstance<SettingsControlViewModel>(); } }
        public static AboutControlViewModel AboutControlViewModel { get { return ServiceLocator.Current.GetInstance<AboutControlViewModel>(); } }
        public static PaletteSelectorWindowViewModel PaletteSelectorWindowViewModel { get { return ServiceLocator.Current.GetInstance<PaletteSelectorWindowViewModel>(); } }
        public static WhatsNewWindowViewModel WhatsNewWindowViewModel { get { return ServiceLocator.Current.GetInstance<WhatsNewWindowViewModel>(); } }
        public static ReplaysControlViewModel ReplaysControlViewModel { get { return ServiceLocator.Current.GetInstance<ReplaysControlViewModel>(); } }

        public static void Cleanup()
        {
            // TODO Clear the ViewModels
        }
    }
}