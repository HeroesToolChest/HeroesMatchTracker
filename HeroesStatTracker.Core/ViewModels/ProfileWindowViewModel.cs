using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using Heroes.Helpers;
using HeroesStatTracker.Core.User;
using System.Collections.Generic;
using static Heroes.Helpers.HeroesHelpers.Regions;

namespace HeroesStatTracker.Core.ViewModels
{
    public class ProfileWindowViewModel : ViewModelBase
    {
        private IUserProfileService UserProfile;

        public ProfileWindowViewModel(IUserProfileService userProfile)
        {
            UserProfile = userProfile;
        }

        public RelayCommand SetUserCommand => new RelayCommand(SetUser);

        public string UserBattleTag
        {
            get { return UserProfile.BattleTagName; }
            set
            {
                UserProfile.BattleTagName = value;
                RaisePropertyChanged();
            }
        }

        public string SelectedRegion
        {
            get { return ((Region)UserProfile.RegionId).ToString(); }
            set
            {
                UserProfile.RegionId = (int)HeroesHelpers.EnumParser.ConvertRegionStringtoEnum(value);
                RaisePropertyChanged();
            }
        }

        public List<string> RegionsList
        {
            get { return GetRegionsList(); }
        }

        private void SetUser()
        {
            if (string.IsNullOrEmpty(UserBattleTag) || string.IsNullOrWhiteSpace(UserBattleTag) || string.IsNullOrEmpty(SelectedRegion))
                return;

            UserProfile.SetProfile();
        }
    }
}
