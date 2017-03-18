using GalaSoft.MvvmLight;
using Heroes.Icons;
using HeroesMatchData.Core.Services;
using HeroesMatchData.Core.User;
using HeroesMatchData.Data;
using NLog;
using System;
using System.Windows.Media.Imaging;

namespace HeroesMatchData.Core.ViewModels
{
    public class HmdViewModel : ViewModelBase
    {
        private BitmapImage _backgroundImage;

        protected HmdViewModel(IInternalService internalService)
        {
            InternalService = internalService;
            Database = internalService.Database;
            HeroesIcons = internalService.HeroesIcons;
            UserProfile = internalService.UserProfile;

            SetLoggers();
            SetRandomHomescreenBackgroundImage();
        }

        protected HmdViewModel(IHeroesIconsService heroesIcons)
        {
            HeroesIcons = heroesIcons;

            SetLoggers();
            SetRandomHomescreenBackgroundImage();
        }

        public BitmapImage BackgroundImage
        {
            get => _backgroundImage;
            set
            {
                _backgroundImage = value;
                RaisePropertyChanged();
            }
        }

        protected IInternalService InternalService { get;  }
        protected IDatabaseService Database { get; }
        protected IHeroesIconsService HeroesIcons { get; }
        protected IUserProfileService UserProfile { get; }

        protected Logger ExceptionLog { get; private set; }
        protected Logger WarningLog { get; private set; }
        protected Logger UnParsedReplaysLog { get; private set; }
        protected Logger TranslationsLog { get; private set; }
        protected Logger HotsLogsLog { get; private set; }

        protected void SetBackgroundImage(string mapRealName) => BackgroundImage = HeroesIcons.MapBackgrounds().GetMapBackground(mapRealName);

        private void SetLoggers()
        {
            ExceptionLog = LogManager.GetLogger(LogFileNames.Exceptions);
            WarningLog = LogManager.GetLogger(LogFileNames.WarningLogFileName);
            UnParsedReplaysLog = LogManager.GetLogger(LogFileNames.UnParsedReplaysLogFileName);
            TranslationsLog = LogManager.GetLogger(LogFileNames.TranslationLogFileName);
            HotsLogsLog = LogManager.GetLogger(LogFileNames.HotsLogsLogFileName);
        }

        private void SetRandomHomescreenBackgroundImage()
        {
            Random random = new Random();
            var listOfBackgroundImages = HeroesIcons.HomeScreens().GetListOfHomeScreens();

            int num = random.Next(0, listOfBackgroundImages.Count);
            BackgroundImage = listOfBackgroundImages[num].Item1;
        }
    }
}
