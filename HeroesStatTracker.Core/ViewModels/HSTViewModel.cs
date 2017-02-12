using GalaSoft.MvvmLight;
using Heroes.Icons;
using HeroesStatTracker.Data.Models.Replays;
using NLog;
using System;
using System.Collections.Generic;
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
            HotsLogsLog = LogManager.GetLogger(LogFileNames.HotsLogsLogFileName);

            SetRandomBackgroundImage();
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

        protected Dictionary<int, PartyIconColor> PlayerPartyIcons { get; private set; } = new Dictionary<int, PartyIconColor>();

        protected void FindPlayerParties(List<ReplayMatchPlayer> playersList)
        {
            Dictionary<long, List<int>> parties = new Dictionary<long, List<int>>();

            foreach (var player in playersList)
            {
                if (player.PartyValue != 0)
                {
                    if (!parties.ContainsKey(player.PartyValue))
                    {
                        var listOfMembers = new List<int>();
                        listOfMembers.Add(player.PlayerNumber);
                        parties.Add(player.PartyValue, listOfMembers);
                    }
                    else
                    {
                        var listOfMembers = parties[player.PartyValue];
                        listOfMembers.Add(player.PlayerNumber);
                        parties[player.PartyValue] = listOfMembers;
                    }
                }
            }

            PlayerPartyIcons = new Dictionary<int, PartyIconColor>();
            PartyIconColor color = 0;

            foreach (var party in parties)
            {
                foreach (int playerNum in party.Value)
                {
                    PlayerPartyIcons.Add(playerNum, color);
                }

                color++;
            }
        }

        protected void SetBackgroundImage(string mapRealName)
        {
            BackgroundImage = HeroesIcons.MapBackgrounds().GetMapBackground(mapRealName);
            HeroesIcons.MapBackgrounds().GetMapBackgroundFontGlowColor(mapRealName);
        }

        private void SetRandomBackgroundImage()
        {
            Random random = new Random();
            var listOfBackgroundImages = HeroesIcons.HomeScreens().GetListOfHomeScreens();

            int num = random.Next(0, listOfBackgroundImages.Count);
            BackgroundImage = listOfBackgroundImages[num].Item1;

            // LabelGlowColor = listOfBackgroundImages[num].Item2;
        }
    }
}
