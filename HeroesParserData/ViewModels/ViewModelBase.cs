using HeroesIcons;
using NLog;
using System;
using System.Collections.Generic;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace HeroesParserData.ViewModels
{
    public class ViewModelBase : ObservableObject
    {
        private BitmapImage _backgroundMapImage;
        private Color _labelGlowColor;

        protected HeroesInfo HeroesInfo {get; private set; }
        protected Logger ExceptionLog { get; private set; }
        protected Logger FailedReplaysLog { get; private set; }
        protected Logger SqlExceptionReplaysLog { get; private set; }
        protected List<string> AllSeasonsList { get; private set; }
        protected List<string> AllGameModesList { get; private set; }

        public BitmapImage BackgroundMapImage
        {
            get { return _backgroundMapImage; }
            set
            {
                _backgroundMapImage = value;
                RaisePropertyChangedEvent(nameof(BackgroundMapImage));
            }
        }

        public Color LabelGlowColor
        {
            get { return _labelGlowColor; }
            set
            {
                _labelGlowColor = value;
                RaisePropertyChangedEvent(nameof(LabelGlowColor));
            }
        }

        protected ViewModelBase()
        {
            HeroesInfo = App.HeroesInfo;
            ExceptionLog = LogManager.GetLogger("ExceptionLogFile");
            FailedReplaysLog = LogManager.GetLogger("UnParsedReplaysLogFile");
            SqlExceptionReplaysLog = LogManager.GetLogger("SqlExceptionReplaysLogFile");

            AllSeasonsList = Utilities.GetSeasonList();
            AllGameModesList = Utilities.GetGameModeList();

            SetDefaultBackgroundImage();
        }

        private void SetDefaultBackgroundImage()
        {
            Random random = new Random();
            var listOfBackgroundImages = HeroesInfo.GetListOfHomeScreens();

            int num = random.Next(0, listOfBackgroundImages.Count);
            BackgroundMapImage = listOfBackgroundImages[num].Item1;
            LabelGlowColor = listOfBackgroundImages[num].Item2;
        }
    }
}