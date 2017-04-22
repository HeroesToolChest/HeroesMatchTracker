using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using Heroes.Helpers;
using HeroesMatchTracker.Core.User;
using System.Collections.Generic;
using static Heroes.Helpers.HeroesHelpers.Regions;

namespace HeroesMatchTracker.Core.ViewModels
{
    public class ProfileWindowViewModel : ViewModelBase
    {
        private IUserProfileService UserProfile;
        private string _userBattleTag;
        private int _userRegion;

        public ProfileWindowViewModel(IUserProfileService userProfile)
        {
            UserProfile = userProfile;

            UserBattleTag = UserProfile.BattleTagName;
            SelectedRegion = ((Region)UserProfile.RegionId).ToString();
        }

        public RelayCommand SetUserCommand => new RelayCommand(SetUser);

        public string UserBattleTag
        {
            get => _userBattleTag;
            set
            {
                _userBattleTag = value;
                RaisePropertyChanged();
            }
        }

        public string SelectedRegion
        {
            get => ((Region)_userRegion).ToString();
            set
            {
                _userRegion = (int)HeroesHelpers.EnumParser.ConvertRegionStringToEnum(value);
                RaisePropertyChanged();
            }
        }

        public List<string> RegionsList
        {
            get { return GetRegionsList(); }
        }

        private void SetUser()
        {
            UserBattleTag = UserBattleTag.Trim();
            if (string.IsNullOrEmpty(SelectedRegion))
                return;

            UserProfile.SetProfile(_userBattleTag, _userRegion);
        }
    }
}
