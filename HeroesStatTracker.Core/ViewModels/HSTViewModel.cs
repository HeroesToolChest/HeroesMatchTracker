using GalaSoft.MvvmLight;
using Heroes.Icons;
using NLog;
using System;
using System.Windows.Media.Imaging;

namespace HeroesStatTracker.Core.ViewModels
{
    public class HstViewModel : ViewModelBase
    {
        private BitmapImage _backgroundImage;

        protected HstViewModel(IHeroesIconsService heroesIcons)
        {
            HeroesIcons = heroesIcons;

            ExceptionLog = LogManager.GetLogger(LogFileNames.Exceptions);
            WarningLog = LogManager.GetLogger(LogFileNames.WarningLogFileName);
            UnParsedReplaysLog = LogManager.GetLogger(LogFileNames.UnParsedReplaysLogFileName);
            TranslationsLog = LogManager.GetLogger(LogFileNames.TranslationLogFileName);
            HotsLogsLog = LogManager.GetLogger(LogFileNames.TranslationLogFileName);

            SetBackgroundImage();
        }

        public BitmapImage BackgroundImage
        {
            get { return _backgroundImage; }
            set
            {
                _backgroundImage = value;
                RaisePropertyChanged();
            }
        }

        protected IHeroesIconsService HeroesIcons { get; }
        protected Logger ExceptionLog { get; }
        protected Logger WarningLog { get; }
        protected Logger UnParsedReplaysLog { get; }
        protected Logger TranslationsLog { get; }
        protected Logger HotsLogsLog { get; }

        private void SetBackgroundImage()
        {
            Random random = new Random();
            var listOfBackgroundImages = HeroesIcons.HomeScreens().GetListOfHomeScreens();

            int num = random.Next(0, listOfBackgroundImages.Count);
            BackgroundImage = listOfBackgroundImages[num].Item1;

            // LabelGlowColor = listOfBackgroundImages[num].Item2;
        }
    }
}
