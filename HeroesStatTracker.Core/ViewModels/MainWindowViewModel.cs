using System;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Ioc;
using GalaSoft.MvvmLight.Messaging;
using Heroes.Icons;
using HeroesStatTracker.Core.HotsLogs;
using HeroesStatTracker.Core.Messaging;
using HeroesStatTracker.Core.User;
using HeroesStatTracker.Core.ViewServices;
using HeroesStatTracker.Data;
using Microsoft.Practices.ServiceLocation;

namespace HeroesStatTracker.Core.ViewModels
{
    public class MainWindowViewModel : ViewModelBase, IMatchSummaryFlyoutService, IMainTabService
    {
        private int _selectedMainTab;
        private long _totalParsedReplays;
        private bool _matchSummaryIsOpen;
        private string _matchSummaryHeader;
        private string _applicationStatus;
        private string _parserStatus;
        private string _parserWatchStatus;
        private string _hotsLogsStatus;

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

            SimpleIoc.Default.Register<IMatchSummaryFlyoutService>(() => this);
            SimpleIoc.Default.Register<IMainTabService>(() => this);
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

        public string MatchSummaryHeader
        {
            get { return _matchSummaryHeader; }
            set
            {
                _matchSummaryHeader = value;
                RaisePropertyChanged();
            }
        }

        public string ApplicationStatus
        {
            get { return _applicationStatus; }
            set
            {
                _applicationStatus = value;
                RaisePropertyChanged();
            }
        }

        public long TotalParsedReplays
        {
            get { return _totalParsedReplays; }
            set
            {
                _totalParsedReplays = value;
                RaisePropertyChanged();
            }
        }

        public string ParserStatus
        {
            get { return _parserStatus; }
            set
            {
                _parserStatus = value;
                RaisePropertyChanged();
            }
        }

        public string ParserWatchStatus
        {
            get { return _parserWatchStatus; }
            set
            {
                _parserWatchStatus = value;
                RaisePropertyChanged();
            }
        }

        public string HotsLogsStatus
        {
            get { return _hotsLogsStatus; }
            set
            {
                _hotsLogsStatus = value;
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