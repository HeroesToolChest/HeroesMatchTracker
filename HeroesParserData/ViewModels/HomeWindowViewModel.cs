using GalaSoft.MvvmLight.Messaging;
using Heroes.ReplayParser;
using HeroesParserData.DataQueries.ReplayData;
using HeroesParserData.Messages;
using HeroesParserData.Properties;
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
        private int _totalQuickMatchGames;
        private int _totalUnrankedDraftGames;
        private int _totalHeroLeagueGames;
        private int _totalTeamLeagueGames;
        private int _totalCustomGames;
        private int _seasonQuickMatchGames;
        private int _seasonUnrankedDraftGames;
        private int _seasonHeroLeagueGames;
        private int _seasonTeamLeagueGames;
        private int _seasonCustomGames;
        private string _selectedSeason;
        private bool IsRefreshDataOn;

        private List<string> _seasonList = new List<string>();
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

        public int TotalQuickMatchGames
        {
            get { return _totalQuickMatchGames; }
            set
            {
                _totalQuickMatchGames = value;
                RaisePropertyChangedEvent(nameof(TotalQuickMatchGames));
            }
        }

        public int TotalUnrankedDraftGames
        {
            get { return _totalUnrankedDraftGames; }
            set
            {
                _totalUnrankedDraftGames = value;
                RaisePropertyChangedEvent(nameof(TotalUnrankedDraftGames));
            }
        }

        public int TotalHeroLeagueGames
        {
            get { return _totalHeroLeagueGames; }
            set
            {
                _totalHeroLeagueGames = value;
                RaisePropertyChangedEvent(nameof(TotalHeroLeagueGames));
            }
        }

        public int TotalTeamLeagueGames
        {
            get { return _totalTeamLeagueGames; }
            set
            {
                _totalTeamLeagueGames = value;
                RaisePropertyChangedEvent(nameof(TotalTeamLeagueGames));
            }
        }

        public int TotalCustomGames
        {
            get { return _totalCustomGames; }
            set
            {
                _totalCustomGames = value;
                RaisePropertyChangedEvent(nameof(TotalCustomGames));
            }
        }

        public int SeasonQuickMatchGames
        {
            get { return _seasonQuickMatchGames; }
            set
            {
                _seasonQuickMatchGames = value;
                RaisePropertyChangedEvent(nameof(SeasonQuickMatchGames));
            }
        }

        public int SeasonUnrankedDraftGames
        {
            get { return _seasonUnrankedDraftGames; }
            set
            {
                _seasonUnrankedDraftGames = value;
                RaisePropertyChangedEvent(nameof(SeasonUnrankedDraftGames));
            }
        }

        public int SeasonHeroLeagueGames
        {
            get { return _seasonHeroLeagueGames; }
            set
            {
                _seasonHeroLeagueGames = value;
                RaisePropertyChangedEvent(nameof(SeasonHeroLeagueGames));
            }
        }

        public int SeasonTeamLeagueGames
        {
            get { return _seasonTeamLeagueGames; }
            set
            {
                _seasonTeamLeagueGames = value;
                RaisePropertyChangedEvent(nameof(SeasonTeamLeagueGames));
            }
        }

        public int SeasonCustomGames
        {
            get { return _seasonCustomGames; }
            set
            {
                _seasonCustomGames = value;
                RaisePropertyChangedEvent(nameof(SeasonCustomGames));
            }
        }

        public string SelectedSeason
        {
            get { return _selectedSeason; }
            set
            {
                _selectedSeason = value;
                SetSeasonStats();
                RaisePropertyChangedEvent(nameof(SelectedSeason));
            }
        }

        public List<string> SeasonList
        {
            get { return _seasonList;}
            set
            {
                _seasonList = value;
                RaisePropertyChangedEvent(nameof(SeasonList));
            }
        }
        #endregion public properties

        public HomeWindowViewModel()
            :base()
        {
            Messenger.Default.Register<HomeWindowMessage>(this, (action) => ReceiveMessage(action));
            SetBackgroundImages();
            SetRandomBackgroundImage();
            SetSeasonList();
            SetData();
        }

        private void SetBackgroundImages()
        {
            string uri = "pack://application:,,,/HeroesIcons;component/Icons/Homescreens/";

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

        private void SetSeasonList()
        {
            SelectedSeason = Settings.Default.SelectedSeason;
            SeasonList.Add("Preseason");
            SeasonList.Add("Season 1");
            SeasonList.Add("Season 2");
        }

        private void SetData()
        {
            ReplaysInDatabase = Query.Replay.GetTotalReplayCount();
            LatestUploadedReplay = Query.Replay.ReadLatestReplayByDateTime();
            LastUploadedReplay = Query.Replay.ReadLastReplayByDateTime();
            TotalQuickMatchGames = Query.Replay.ReadTotalGames(GameMode.QuickMatch);
            TotalUnrankedDraftGames = Query.Replay.ReadTotalGames(GameMode.UnrankedDraft);
            TotalHeroLeagueGames = Query.Replay.ReadTotalGames(GameMode.HeroLeague);
            TotalTeamLeagueGames = Query.Replay.ReadTotalGames(GameMode.TeamLeague);
            TotalCustomGames = Query.Replay.ReadTotalGames(GameMode.Custom);

            SetSeasonStats();
        }

        private void SetSeasonStats()
        {
            SeasonQuickMatchGames = Query.Replay.ReadTotalGamesForSeason(GameMode.QuickMatch, SelectedSeason);
            SeasonUnrankedDraftGames = Query.Replay.ReadTotalGamesForSeason(GameMode.UnrankedDraft, SelectedSeason);
            SeasonHeroLeagueGames = Query.Replay.ReadTotalGamesForSeason(GameMode.HeroLeague, SelectedSeason);
            SeasonTeamLeagueGames = Query.Replay.ReadTotalGamesForSeason(GameMode.TeamLeague, SelectedSeason);
            SeasonCustomGames = Query.Replay.ReadTotalGamesForSeason(GameMode.Custom, SelectedSeason);
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
                        SetData();
                        while (App.IsProcessingReplays)
                        {
                            await Task.Delay(3000);
                            SetData();
                        }
                        IsRefreshDataOn = false;
                    });
                }
            }
        }
    }
}
