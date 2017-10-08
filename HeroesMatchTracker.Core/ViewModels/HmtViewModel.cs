using GalaSoft.MvvmLight;
using Heroes.Icons;
using HeroesMatchTracker.Core.Services;
using HeroesMatchTracker.Core.User;
using HeroesMatchTracker.Data;
using NLog;
using System;
using System.IO;

namespace HeroesMatchTracker.Core.ViewModels
{
    public class HmtViewModel : ViewModelBase
    {
        private Stream _backgroundImage;

        protected HmtViewModel(IInternalService internalService)
        {
            InternalService = internalService;
            Database = internalService.Database;
            HeroesIcons = internalService.HeroesIcons;
            UserProfile = internalService.UserProfile;

            SetLoggers();
            SetRandomHomescreenBackgroundImage();
        }

        protected HmtViewModel(IHeroesIconsService heroesIcons)
        {
            HeroesIcons = heroesIcons;

            SetLoggers();
            SetRandomHomescreenBackgroundImage();
        }

        public Stream BackgroundImage
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
        protected ISelectedUserProfileService UserProfile { get; }

        protected Logger ExceptionLog { get; private set; }
        protected Logger WarningLog { get; private set; }
        protected Logger UnParsedReplaysLog { get; private set; }
        protected Logger TranslationsLog { get; private set; }
        protected Logger ReplayUploaderLog { get; private set; }

        protected void SetBackgroundImage(string mapRealName) => BackgroundImage = HeroesIcons.MapBackgrounds().GetMapBackground(mapRealName);

        private void SetLoggers()
        {
            ExceptionLog = LogManager.GetLogger(LogFileNames.Exceptions);
            WarningLog = LogManager.GetLogger(LogFileNames.WarningLogFileName);
            UnParsedReplaysLog = LogManager.GetLogger(LogFileNames.UnParsedReplaysLogFileName);
            TranslationsLog = LogManager.GetLogger(LogFileNames.TranslationLogFileName);
            ReplayUploaderLog = LogManager.GetLogger(LogFileNames.ReplayUploaderLogFileName);
        }

        private void SetRandomHomescreenBackgroundImage()
        {
            Random random = new Random();
            var listOfBackgroundImages = HeroesIcons.Homescreens().GetHomescreensList();

            int num = random.Next(0, listOfBackgroundImages.Count);
            BackgroundImage = HeroesIcons.Homescreens().GetHomescreen(listOfBackgroundImages[num]);
        }
    }
}
