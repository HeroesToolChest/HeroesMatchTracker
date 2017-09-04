using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Ioc;
using GalaSoft.MvvmLight.Messaging;
using Heroes.Helpers;
using HeroesMatchTracker.Core.Messaging;
using HeroesMatchTracker.Core.Models.ReplayModels.Uploaders.HotsApi;
using HeroesMatchTracker.Core.ReplayModels.Uploaders.HotsLogs;
using HeroesMatchTracker.Core.User;
using HeroesMatchTracker.Core.ViewServices;
using HeroesMatchTracker.Data;
using Microsoft.Practices.ServiceLocation;

namespace HeroesMatchTracker.Core.ViewModels
{
    public class MainWindowViewModel : ViewModelBase, IMatchSummaryFlyoutService, IMainTabService, ILoadingOverlayWindowService
    {
        private int _selectedMainTab;
        private long _totalParsedReplays;
        private bool _matchSummaryIsOpen;
        private bool _isExtendedAboutTextVisible;
        private bool _isLoadingOverlayVisible;
        private string _matchSummaryHeader;
        private string _applicationStatus;
        private string _parserStatus;
        private string _parserWatchStatus;
        private string _hotsLogsStatus;
        private string _hotsApiStatus;
        private string _extendedAboutText;

        private IDatabaseService Database;
        private ISelectedUserProfileService UserProfile;

        public MainWindowViewModel(IDatabaseService database, ISelectedUserProfileService userProfile)
        {
            Database = database;
            UserProfile = userProfile;

            MatchSummaryIsOpen = false;
            MatchSummaryHeader = "Match Summary";
            IsLoadingOverlayVisible = false;

            Database.SettingsDb().UserSettings.IsUpdateAvailableKnown = false;

            SimpleIoc.Default.Register<IMatchSummaryFlyoutService>(() => this);
            SimpleIoc.Default.Register<ILoadingOverlayWindowService>(() => this);
            SimpleIoc.Default.Register<IMainTabService>(() => this);
            Messenger.Default.Register<NotificationMessage>(this, (message) => ReceivedMessage(message));
        }

        public IDatabaseService GetDatabaseService => Database;

#if DEBUG
        public string ApplicationTitle => $"[DEBUG] Heroes Match Tracker {AssemblyVersions.HeroesMatchTrackerVersion().ToString()}";
#else
        public string ApplicationTitle => $"Heroes Match Tracker {AssemblyVersions.HeroesMatchTrackerVersion().ToString()}";
#endif
        public RelayCommand OpenWhatsNewWindowCommand => new RelayCommand(OpenWhatsNewWindow);
        public RelayCommand UserDropDownProfileCommand => new RelayCommand(UserDropDownProfile);

        public ICreateWindowService CreateWindow => ServiceLocator.Current.GetInstance<ICreateWindowService>();

        public bool MatchSummaryIsOpen
        {
            get => _matchSummaryIsOpen;
            set
            {
                _matchSummaryIsOpen = value;
                RaisePropertyChanged();
            }
        }

        public bool IsLoadingOverlayVisible
        {
            get => _isLoadingOverlayVisible;
            set
            {
                _isLoadingOverlayVisible = value;
                RaisePropertyChanged();
            }
        }

        public string UserBattleTag
        {
            get
            {
                if (!string.IsNullOrEmpty(UserProfile.BattleTagName))
                    return HeroesHelpers.BattleTags.GetNameFromBattleTagName(UserProfile.BattleTagName);
                else
                    return "No BattleTag Set";
            }
            set
            {
                RaisePropertyChanged();
            }
        }

        public string MatchSummaryHeader
        {
            get => _matchSummaryHeader;
            set
            {
                _matchSummaryHeader = value;
                RaisePropertyChanged();
            }
        }

        public string ApplicationStatus
        {
            get => _applicationStatus;
            set
            {
                _applicationStatus = value;
                RaisePropertyChanged();
            }
        }

        public long TotalParsedReplays
        {
            get => _totalParsedReplays;
            set
            {
                _totalParsedReplays = value;
                RaisePropertyChanged();
            }
        }

        public string ParserStatus
        {
            get => _parserStatus;
            set
            {
                _parserStatus = value;
                RaisePropertyChanged();
            }
        }

        public string ParserWatchStatus
        {
            get => _parserWatchStatus;
            set
            {
                _parserWatchStatus = value;
                RaisePropertyChanged();
            }
        }

        public string HotsLogsUploaderCurrentStatus
        {
            get => _hotsLogsStatus;
            set
            {
                _hotsLogsStatus = value;
                RaisePropertyChanged();
            }
        }

        public string HotsApiUploaderCurrentStatus
        {
            get => _hotsApiStatus;
            set
            {
                _hotsApiStatus = value;
                RaisePropertyChanged();
            }
        }

        public int SelectedMainTab
        {
            get => _selectedMainTab;
            set
            {
                _selectedMainTab = value;
                RaisePropertyChanged();
            }
        }

        public bool IsExtendedAboutTextVisible
        {
            get => _isExtendedAboutTextVisible;
            set
            {
                _isExtendedAboutTextVisible = value;
                RaisePropertyChanged();
            }
        }

        public string ExtendedAboutText
        {
            get => _extendedAboutText;
            set
            {
                _extendedAboutText = value;
                RaisePropertyChanged();
            }
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

        public void SwitchToTab(MainPage selectedMainTab)
        {
            SelectedMainTab = (int)selectedMainTab;
        }

        public void SetApplicationStatus(string status)
        {
            if (ApplicationStatus != status)
                ApplicationStatus = status;
        }

        public void SetReplayParserStatus(ReplayParserStatus status)
        {
            if (status == ReplayParserStatus.Parsing)
                ParserStatus = "Replay Parser [PARSING]";
            else if (status == ReplayParserStatus.Stopped)
                ParserStatus = "Replay Parser [STOPPED]";
        }

        public void SetReplayParserWatchStatus(ReplayParserWatchStatus status)
        {
            if (status == ReplayParserWatchStatus.Enabled)
                ParserWatchStatus = "Watch [ENABLED]";
            else if (status == ReplayParserWatchStatus.Disabled)
                ParserWatchStatus = "Watch [DISABLED]";
        }

        public void SetHotsLogsUploaderStatus(HotsLogsUploaderStatus status)
        {
            if (status == HotsLogsUploaderStatus.Enabled)
                HotsLogsUploaderCurrentStatus = "HOTS Logs Uploader [ENABLED]";
            else if (status == HotsLogsUploaderStatus.Disabled)
                HotsLogsUploaderCurrentStatus = "HOTS Logs Uploader [DISABLED]";
        }

        public void SetHotsApiUploaderStatus(HotsApiUploaderStatus status)
        {
            if (status == HotsApiUploaderStatus.Enabled)
                HotsApiUploaderCurrentStatus = "HotsApi Uploader [ENABLED]";
            else if (status == HotsApiUploaderStatus.Disabled)
                HotsApiUploaderCurrentStatus = "HotsApi Uploader [DISABLED]";
        }

        public void SetTotalParsedReplays(long amount)
        {
            TotalParsedReplays = amount;
        }

        public void SetExtendedAboutText(string message)
        {
            ExtendedAboutText = message;
            IsExtendedAboutTextVisible = true;
        }

        public void CloseLoadingOverlay()
        {
            IsLoadingOverlayVisible = false;
        }

        public void ShowLoadingOverlay()
        {
            IsLoadingOverlayVisible = true;
        }

        private void OpenWhatsNewWindow()
        {
            CreateWindow.ShowWhatsNewWindow();
        }

        private void UserDropDownProfile()
        {
            CreateWindow.ShowUserProfileWindow();
        }

        private void ReceivedMessage(NotificationMessage message)
        {
            if (message.Notification == StaticMessage.UpdateUserBattleTag)
                UserBattleTag = UserProfile.BattleTagName;
        }
    }
}