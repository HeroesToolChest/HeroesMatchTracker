using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using HeroesMatchTracker.Core.User;
using HeroesMatchTracker.Data;
using HeroesMatchTracker.Data.Models.Settings;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using static Heroes.Helpers.HeroesHelpers.Regions;

namespace HeroesMatchTracker.Core.ViewModels.Home
{
    public class UserProfileWindowViewModel : ViewModelBase
    {
        private int _selectedProfileRegion;
        private string _profileBattleTag;
        private string _addProfileResultText;
        private string _selectedUserBattleTag;

        private ObservableCollection<string> _userProfilesList = new ObservableCollection<string>();
        private ObservableCollection<UserProfile> _userProfilesCollection = new ObservableCollection<UserProfile>();

        private IDatabaseService Database;
        private ISelectedUserProfileService SelectedUserProfile;

        public UserProfileWindowViewModel(IDatabaseService database, ISelectedUserProfileService selectedUserProfile)
        {
            Database = database;
            SelectedUserProfile = selectedUserProfile;

            UserProfilesCollection = new ObservableCollection<UserProfile>(Database.SettingsDb().UserProfiles.ReadAllProfiles());

            SelectedProfileRegion = RegionsList[0];
            SetUserProfileList();
        }

        public List<UserProfile> SelectedProfiles { get; private set; } = new List<UserProfile>();

        public List<string> RegionsList
        {
            get { return GetRegionsList(); }
        }

        public string SelectedProfileRegion
        {
            get => ((Region)_selectedProfileRegion).ToString();
            set
            {
                if (Enum.TryParse(value, out Region region))
                    _selectedProfileRegion = (int)region;
                else
                    _selectedProfileRegion = 99;
                RaisePropertyChanged();
            }
        }

        public string ProfileBattleTag
        {
            get => _profileBattleTag ?? string.Empty;
            set
            {
                _profileBattleTag = value;
                RaisePropertyChanged();
            }
        }

        public string AddProfileResultText
        {
            get => _addProfileResultText;
            set
            {
                _addProfileResultText = value;
                RaisePropertyChanged();
            }
        }

        public string SelectedUserBattleTag
        {
            get => _selectedUserBattleTag;
            set
            {
                _selectedUserBattleTag = value;
                RaisePropertyChanged();
            }
        }

        public RelayCommand<object> SelectedProfileCommand => new RelayCommand<object>((list) => SetSelectedProfile(list));
        public RelayCommand AddProfileCommand => new RelayCommand(AddProfile);
        public RelayCommand RemovedSelectedProfileCommand => new RelayCommand(RemovedSelectedProfile);
        public RelayCommand SetUserProfileCommand => new RelayCommand(SetUserProfile);

        public ObservableCollection<string> UserProfilesList
        {
            get => _userProfilesList;
            set
            {
                _userProfilesList = value;
                RaisePropertyChanged();
            }
        }

        public ObservableCollection<UserProfile> UserProfilesCollection
        {
            get => _userProfilesCollection;
            set
            {
                _userProfilesCollection = value;
                RaisePropertyChanged();
            }
        }

        private void AddProfile()
        {
            AddProfileResultText = string.Empty;

            if (string.IsNullOrEmpty(ProfileBattleTag.Trim()))
            {
                AddProfileResultText = "BattleTag required";
                return;
            }

            if (string.IsNullOrEmpty(SelectedProfileRegion))
            {
                AddProfileResultText = "Region required";
                return;
            }

            if (!Enum.TryParse(SelectedProfileRegion, out Region region))
                region = Region.XX;

            UserProfile profile = new UserProfile()
            {
                UserBattleTagName = ProfileBattleTag.Trim(),
                UserRegion = (int)region,
            };

            if (Database.SettingsDb().UserProfiles.IsExistingUserProfile(profile))
            {
                AddProfileResultText = "Profile already added";
                return;
            }

            if (Database.ReplaysDb().MatchReplay.GetTotalReplayCount() < 1)
            {
                AddProfileResultText = "Cannot find. There are no replays in the database.";
                return;
            }

            if (Database.ReplaysDb().HotsPlayer.ReadPlayerIdFromBattleTagName(profile.UserBattleTagName, profile.UserRegion) < 1)
            {
                AddProfileResultText = "BattleTag/Region not found in database";
                return;
            }

            Database.SettingsDb().UserProfiles.CreateUserProfile(profile);
            UserProfilesCollection.Add(profile);
            UserProfilesList.Add(GetBattleTagRegionName(profile.UserBattleTagName, profile.UserRegion));

            ProfileBattleTag = string.Empty;
            AddProfileResultText = string.Empty;
        }

        private void SetSelectedProfile(object list)
        {
            SelectedProfiles = ((IEnumerable)list).Cast<UserProfile>().ToList();
        }

        private void RemovedSelectedProfile()
        {
            foreach (var profile in SelectedProfiles)
            {
                string battleTag = GetBattleTagRegionName(profile.UserBattleTagName, profile.UserRegion);

                // check to see if its currently set, if it is we don't want to remove it
                if (battleTag != GetBattleTagRegionName(Database.SettingsDb().UserSettings.UserBattleTagName, Database.SettingsDb().UserSettings.UserRegion))
                {
                    Database.SettingsDb().UserProfiles.DeleteUserProfile(profile.UserProfileId);
                    UserProfilesCollection.Remove(profile);
                    UserProfilesList.Remove(GetBattleTagRegionName(profile.UserBattleTagName, profile.UserRegion));
                }
            }
        }

        private void SetUserProfile()
        {
            var items = SelectedUserBattleTag.Split('|');

            if (items.Length == 2)
            {
                if (!Enum.TryParse(items[1].Trim(), out Region region))
                    region = Region.XX;

                SelectedUserProfile.SetProfile(items[0].Trim(), (int)region);
            }
            else
            {
                SelectedUserProfile.SetProfile(string.Empty, 0);
            }
        }

        private void SetUserProfileList()
        {
            UserProfilesList.Add("-- No BattleTag Set --");
            foreach (var profile in UserProfilesCollection)
            {
                UserProfilesList.Add(GetBattleTagRegionName(profile.UserBattleTagName, profile.UserRegion));
            }

            if (Database.SettingsDb().UserSettings.UserPlayerId > 0)
            {
                string selected = GetBattleTagRegionName(Database.SettingsDb().UserSettings.UserBattleTagName, Database.SettingsDb().UserSettings.UserRegion);

                if (UserProfilesList.Contains(selected))
                {
                    SelectedUserBattleTag = selected;
                }
                else
                {
                    SelectedUserBattleTag = UserProfilesList[0];
                }
            }
            else
            {
                SelectedUserBattleTag = UserProfilesList[0];
            }

            SetUserProfile();
        }

        private string GetBattleTagRegionName(string userBattleTagName, int userRegion)
        {
            return $"{userBattleTagName} | {(Region)userRegion}";
        }
    }
}
