using GalaSoft.MvvmLight.Messaging;
using HeroesParserData.DataQueries.ReplayData;
using HeroesParserData.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace HeroesParserData.ViewModels
{
    public class HomeWindowViewModel : ViewModelBase
    {
        private BitmapImage _backgroundMapImage;
        private Color _labelGlowColor;
        private long _replaysInDatabase;
        private DateTime _latestUploadedReplay;
        private DateTime _lastUploadedReplay;
        private bool IsRefreshDataOn;
        private List<Tuple<BitmapImage, Color>> ListOfBackgroundImages;

        #region public properties
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

        public long ReplaysInDatabase
        {
            get { return _replaysInDatabase; }
            set
            {
                _replaysInDatabase = value;
                RaisePropertyChangedEvent(nameof(ReplaysInDatabase));
            }
        }

        public DateTime LatestUploadedReplay
        {
            get { return _latestUploadedReplay; }
            set
            {
                _latestUploadedReplay = value;
                RaisePropertyChangedEvent(nameof(LatestUploadedReplay));
            }
        }

        public DateTime LastUploadedReplay
        {
            get { return _lastUploadedReplay; }
            set
            {
                _lastUploadedReplay = value;
                RaisePropertyChangedEvent(nameof(LastUploadedReplay));
            }
        }

        #endregion public properties

        public HomeWindowViewModel()
            :base()
        {
            Messenger.Default.Register<HomeWindowMessage>(this, (action) => ReceiveMessage(action));
            SetBackgroundImages();
            SetRandomBackgroundImage();
            RefreshData();
        }

        private void SetBackgroundImages()
        {
            string uri = "pack://application:,,,/HeroesParserData;component/Resources/Images/Homescreens/";

            ListOfBackgroundImages = new List<Tuple<BitmapImage, Color>>();
            ListOfBackgroundImages.Add(new Tuple<BitmapImage, Color>(new BitmapImage(new Uri(string.Concat(uri, "storm_ui_homescreenbackground_alarak.dds"), UriKind.Absolute)), Colors.Purple));
            ListOfBackgroundImages.Add(new Tuple<BitmapImage, Color>(new BitmapImage(new Uri(string.Concat(uri, "storm_ui_homescreenbackground_chromie.dds"), UriKind.Absolute)), Colors.Gold));
            ListOfBackgroundImages.Add(new Tuple<BitmapImage, Color>(new BitmapImage(new Uri(string.Concat(uri, "storm_ui_homescreenbackground_diablo.dds"), UriKind.Absolute)), Colors.Red));
            ListOfBackgroundImages.Add(new Tuple<BitmapImage, Color>(new BitmapImage(new Uri(string.Concat(uri, "storm_ui_homescreenbackground_diablotristram.dds"), UriKind.Absolute)), Colors.Gray));
            ListOfBackgroundImages.Add(new Tuple<BitmapImage, Color>(new BitmapImage(new Uri(string.Concat(uri, "storm_ui_homescreenbackground_eternalconflict.dds"), UriKind.Absolute)), Colors.DarkRed));
            ListOfBackgroundImages.Add(new Tuple<BitmapImage, Color>(new BitmapImage(new Uri(string.Concat(uri, "storm_ui_homescreenbackground_eternalconflict_dark.dds"), UriKind.Absolute)), Colors.DarkRed));
            ListOfBackgroundImages.Add(new Tuple<BitmapImage, Color>(new BitmapImage(new Uri(string.Concat(uri, "storm_ui_homescreenbackground_greymane.dds"), UriKind.Absolute)), Colors.LightBlue));
            ListOfBackgroundImages.Add(new Tuple<BitmapImage, Color>(new BitmapImage(new Uri(string.Concat(uri, "storm_ui_homescreenbackground_guldan.dds"), UriKind.Absolute)), Colors.Green));
            ListOfBackgroundImages.Add(new Tuple<BitmapImage, Color>(new BitmapImage(new Uri(string.Concat(uri, "storm_ui_homescreenbackground_lunara.dds"), UriKind.Absolute)), Colors.Purple));
            ListOfBackgroundImages.Add(new Tuple<BitmapImage, Color>(new BitmapImage(new Uri(string.Concat(uri, "storm_ui_homescreenbackground_lunarnewyear.dds"), UriKind.Absolute)), Colors.Purple));
            ListOfBackgroundImages.Add(new Tuple<BitmapImage, Color>(new BitmapImage(new Uri(string.Concat(uri, "storm_ui_homescreenbackground_medivh.dds"), UriKind.Absolute)), Colors.Gray));
            ListOfBackgroundImages.Add(new Tuple<BitmapImage, Color>(new BitmapImage(new Uri(string.Concat(uri, "storm_ui_homescreenbackground_nexus.dds"), UriKind.Absolute)), Colors.Purple));
            ListOfBackgroundImages.Add(new Tuple<BitmapImage, Color>(new BitmapImage(new Uri(string.Concat(uri, "storm_ui_homescreenbackground_overwatchhangar.dds"), UriKind.Absolute)), Colors.Gray));
            ListOfBackgroundImages.Add(new Tuple<BitmapImage, Color>(new BitmapImage(new Uri(string.Concat(uri, "storm_ui_homescreenbackground_shrines.dds"), UriKind.Absolute)), Colors.Red));
            ListOfBackgroundImages.Add(new Tuple<BitmapImage, Color>(new BitmapImage(new Uri(string.Concat(uri, "storm_ui_homescreenbackground_shrines_dusk.dds"), UriKind.Absolute)), Colors.Red));
            ListOfBackgroundImages.Add(new Tuple<BitmapImage, Color>(new BitmapImage(new Uri(string.Concat(uri, "storm_ui_homescreenbackground_starcraft.dds"), UriKind.Absolute)), Colors.DarkBlue));
            ListOfBackgroundImages.Add(new Tuple<BitmapImage, Color>(new BitmapImage(new Uri(string.Concat(uri, "storm_ui_homescreenbackground_starcraft_protoss.dds"), UriKind.Absolute)), Colors.Cyan));
            ListOfBackgroundImages.Add(new Tuple<BitmapImage, Color>(new BitmapImage(new Uri(string.Concat(uri, "storm_ui_homescreenbackground_starcraft_zerg.dds"), UriKind.Absolute)), Colors.DarkRed));
            ListOfBackgroundImages.Add(new Tuple<BitmapImage, Color>(new BitmapImage(new Uri(string.Concat(uri, "storm_ui_homescreenbackground_varian.dds"), UriKind.Absolute)), Colors.Red));
            ListOfBackgroundImages.Add(new Tuple<BitmapImage, Color>(new BitmapImage(new Uri(string.Concat(uri, "storm_ui_homescreenbackground_zarya.dds"), UriKind.Absolute)), Colors.Purple));
        }

        private void SetRandomBackgroundImage()
        {
            Random random = new Random();
            int num = random.Next(0, ListOfBackgroundImages.Count);
            BackgroundMapImage = ListOfBackgroundImages[num].Item1;
            LabelGlowColor = ListOfBackgroundImages[num].Item2;
        }

        private void RefreshData()
        {
            ReplaysInDatabase = Query.Replay.GetTotalReplayCount();
            LatestUploadedReplay = Query.Replay.ReadLatestReplayByDateTime();
            LastUploadedReplay = Query.Replay.ReadLastReplayByDateTime();
        }

        private void ReceiveMessage(HomeWindowMessage action)
        {
            if (action.Trigger == Trigger.Update)
            {
                if (!IsRefreshDataOn)
                {
                    IsRefreshDataOn = true;

                    Task.Run(async () =>
                    {
                        RefreshData();
                        while (App.IsProcessingReplays)
                        {
                            await Task.Delay(3000);
                            RefreshData();
                        }
                        IsRefreshDataOn = false;
                    });
                }
            }
        }
    }
}
