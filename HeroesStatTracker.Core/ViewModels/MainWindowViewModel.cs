using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Ioc;
using GalaSoft.MvvmLight.Messaging;
using Heroes.Icons;
using HeroesStatTracker.Core.Messaging;
using HeroesStatTracker.Core.Models;
using HeroesStatTracker.Core.User;
using HeroesStatTracker.Core.ViewServices;
using HeroesStatTracker.Data;
using Microsoft.Practices.ServiceLocation;
using System.Collections.ObjectModel;

namespace HeroesStatTracker.Core.ViewModels
{
    public class MainWindowViewModel : ViewModelBase, IMatchSummaryFlyoutService, IMainTabsService
    {
        private bool _matchSummaryIsOpen;
        private bool _isHomeControlVisible;
        private bool _isMatchesControlVisible;
        private bool _isReplaysControlVisible;
        private bool _isRawDataControlVisible;
        private int _selectedMainTab;
        private string _matchSummaryHeader;
        private MainPageItem _selectedMainPage;

        private IDatabaseService Database;
        private IHeroesIconsService HeroesIcons;
        private IUserProfileService UserProfile;

        private ObservableCollection<MainPageItem> _mainPagesCollection = new ObservableCollection<MainPageItem>();

        public MainWindowViewModel(IDatabaseService database, IHeroesIconsService heroesIcons, IUserProfileService userProfile)
        {
            Database = database;
            UserProfile = userProfile;
            HeroesIcons = heroesIcons;

            MatchSummaryIsOpen = false;
            MatchSummaryHeader = "Match Summary";

            IsHomeControlVisible = true;
            IsMatchesControlVisible = false;

            MainPagesCollection.Add(new MainPageItem("Home"));
            MainPagesCollection.Add(new MainPageItem("Matches"));

            SimpleIoc.Default.Register<IMatchSummaryFlyoutService>(() => this);
            SimpleIoc.Default.Register<IMainTabsService>(() => this);
            Messenger.Default.Register<NotificationMessage>(this, (message) => ReceivedMessage(message));
        }

        public IDatabaseService GetDatabaseService { get { return Database; } }

        public string AppVersion { get { return AssemblyVersions.HeroesStatTrackerVersion().ToString(); } }

        public bool MatchSummaryIsOpen
        {
            get { return _matchSummaryIsOpen; }
            set
            {
                _matchSummaryIsOpen = value;
                RaisePropertyChanged();
            }
        }

        public RelayCommand OpenWhatsNewWindowCommand => new RelayCommand(OpenWhatsNewWindow);
        public RelayCommand OpenProfileCommand => new RelayCommand(OpenProfile);

        public IWhatsNewWindowService WhatsNewWindow
        {
            get { return ServiceLocator.Current.GetInstance<IWhatsNewWindowService>(); }
        }

        public IProfileWindowService ProfileWindow
        {
            get { return ServiceLocator.Current.GetInstance<IProfileWindowService>(); }
        }

        public string UserBattleTag
        {
            get { return UserProfile.BattleTagName; }
            set
            {
                RaisePropertyChanged();
            }
        }

        public int SelectedMainTab
        {
            get { return _selectedMainTab; }
            set
            {
                _selectedMainTab = value;
                RaisePropertyChanged();
            }
        }

        public string MatchSummaryHeader
        {
            get { return _matchSummaryHeader; }
            set
            {
                _matchSummaryHeader = value;
                RaisePropertyChanged();
            }
        }

        public bool IsHomeControlVisible
        {
            get { return _isHomeControlVisible; }
            set
            {
                if (_isHomeControlVisible != value)
                {
                    _isHomeControlVisible = value;
                    RaisePropertyChanged();
                }
            }
        }

        public bool IsMatchesControlVisible
        {
            get { return _isMatchesControlVisible; }
            set
            {
                if (_isMatchesControlVisible != value)
                {
                    SetMainPagesToFalse();
                    _isMatchesControlVisible = value;
                    RaisePropertyChanged();
                }
            }
        }

        public MainPageItem SelectedMainPage
        {
            get { return _selectedMainPage; }
            set
            {
                if (_selectedMainPage != value)
                {
                    _selectedMainPage = value;
                    RaisePropertyChanged();
                }
            }
        }

        public bool IsReplaysControlVisible
        {
            get { return _isReplaysControlVisible; }
            set
            {
                if (_isReplaysControlVisible != value)
                {
                    _isReplaysControlVisible = value;
                    RaisePropertyChanged();
                }
            }
        }

        public bool IsRawDataControlVisible
        {
            get { return _isRawDataControlVisible; }
            set
            {
                if (_isRawDataControlVisible != value)
                {
                    _isRawDataControlVisible = value;
                    RaisePropertyChanged();
                }
            }
        }

        public ObservableCollection<MainPageItem> MainPagesCollection
        {
            get { return _mainPagesCollection; }
        }

        public void ToggleMatchSummaryFlyout()
        {
            MatchSummaryIsOpen = !MatchSummaryIsOpen;
        }

        public void CloseMatchSummaryFlyout()
        {
            MatchSummaryIsOpen = false;
        }

        public void OpenMatchSummaryFlyout()
        {
            MatchSummaryIsOpen = true;
        }

        public void SetMatchSummaryHeader(string headerTitle)
        {
            MatchSummaryHeader = headerTitle;
        }

        public void SwitchToTab(MainTabs selectedMainTab)
        {
            SelectedMainTab = (int)selectedMainTab;
        }

        private void SetMainPagesToFalse()
        {
            IsHomeControlVisible = false;
            IsMatchesControlVisible = false;
            IsReplaysControlVisible = false;
            IsRawDataControlVisible = false;
        }

        private void OpenWhatsNewWindow()
        {
            WhatsNewWindow.CreateWhatsNewWindow();
        }

        private void OpenProfile()
        {
            ProfileWindow.CreateProfileWindow();
        }

        private void ReceivedMessage(NotificationMessage message)
        {
            if (message.Notification == StaticMessage.UpdateUserBattleTag)
                UserBattleTag = UserProfile.BattleTagName;
        }
    }
}