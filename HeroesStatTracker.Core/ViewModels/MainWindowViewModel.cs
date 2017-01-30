using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using HeroesStatTracker.Core.Messaging;
using HeroesStatTracker.Core.User;
using HeroesStatTracker.Core.ViewServices;
using HeroesStatTracker.Data;
using Microsoft.Practices.ServiceLocation;

namespace HeroesStatTracker.Core.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        private IDatabaseService Database;
        private IUserProfileService UserProfile;

        public MainWindowViewModel(IDatabaseService database, IUserProfileService userProfile)
        {
            Database = database;
            UserProfile = userProfile;

            Messenger.Default.Register<NotificationMessage>(this, (message) => UpdateUserBattleTag(message));
        }

        public IDatabaseService GetDatabaseService { get { return Database; } }

        public string AppVersion { get { return AssemblyVersions.HeroesStatTrackerVersion().ToString(); } }

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

        private void OpenWhatsNewWindow()
        {
            WhatsNewWindow.CreateWhatsNewWindow();
        }

        private void OpenProfile()
        {
            ProfileWindow.CreateProfileWindow();
        }

        private void UpdateUserBattleTag(NotificationMessage message)
        {
            if (message.Notification == StaticMessage.UpdateUserBattleTag)
                UserBattleTag = UserProfile.BattleTagName;
        }
    }
}