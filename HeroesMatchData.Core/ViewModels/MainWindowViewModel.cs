using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Ioc;
using GalaSoft.MvvmLight.Messaging;
using Heroes.Icons;
using HeroesMatchData.Core.HotsLogs;
using HeroesMatchData.Core.Messaging;
using HeroesMatchData.Core.User;
using HeroesMatchData.Core.ViewServices;
using HeroesMatchData.Data;
using Microsoft.Practices.ServiceLocation;
using System;

namespace HeroesMatchData.Core.ViewModels
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
        private string _extendedAboutText;

        private IDatabaseService Database;
        private IHeroesIconsService HeroesIcons;
        private IUserProfileService UserProfile;

        public MainWindowViewModel(IDatabaseService database, IHeroesIconsService heroesIcons, IUserProfileService userProfile)
        {
            Database = database;
            UserProfile = userProfile;
            HeroesIcons = heroesIcons;

            MatchSummaryIsOpen = false;
            MatchSummaryHeader = "Match Summary";
            IsLoadingOverlayVisible = false;

            SimpleIoc.Default.Register<IMatchSummaryFlyoutService>(() => this);
            SimpleIoc.Default.Register<ILoadingOverlayWindowService>(() => this);
            SimpleIoc.Default.Register<IMainTabService>(() => this);
            Messenger.Default.Register<NotificationMessage>(this, (message) => ReceivedMessage(message));
        }

        public IDatabaseService GetDatabaseService => Database;

#if DEBUG
        public string ApplicationTitle => $"[DEBUG] Heroes Match Data {AssemblyVersions.HeroesMatchDataVersion().ToString()}";
#else
        public string ApplicationTitle => $"Heroes Match Data {AssemblyVersions.HeroesMatchDataVersion().ToString()}";
#endif
        public RelayCommand OpenWhatsNewWindowCommand => new RelayCommand(OpenWhatsNewWindow);
        public RelayCommand UserDropDownProfileCommand => new RelayCommand(UserDropDownProfile);

        public IWhatsNewWindowService WhatsNewWindow => ServiceLocator.Current.GetInstance<IWhatsNewWindowService>();
        public IProfileWindowService ProfileWindow => ServiceLocator.Current.GetInstance<IProfileWindowService>();

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
                    return UserProfile.BattleTagName;
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

        public string HotsLogsStatus
        {
            get => _hotsLogsStatus;
            set
            {
                _hotsLogsStatus = value;
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

        public void SetReplayParserHotsLogsStatus(ReplayParserHotsLogsStatus status)
        {
            if (status == ReplayParserHotsLogsStatus.Enabled)
                HotsLogsStatus = "HotsLogs Uploader [ENABLED]";
            else if (status == ReplayParserHotsLogsStatus.Disabled)
                HotsLogsStatus = "HotsLogs Uploader [DISABLED]";
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
            WhatsNewWindow.CreateWhatsNewWindow();
        }

        private void UserDropDownProfile()
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