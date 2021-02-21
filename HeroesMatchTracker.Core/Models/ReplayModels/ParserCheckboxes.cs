using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Messaging;
using HeroesMatchTracker.Core.Messaging;
using HeroesMatchTracker.Data;

namespace HeroesMatchTracker.Core.Models.ReplayModels
{
    public class ParserCheckboxes : ObservableObject
    {
        public const int LatestParsedIndex = 0;
        public const int LastParsedIndex = 1;
        public const int LatestHeroesProfileUploaderIndex = 2;
        public const int LastHeroesProfileUploaderIndex = 3;

        private IDatabaseService Database;

        private bool[] ScanDateTimeCheckboxes = new bool[4] { false, false, false, false };

        public ParserCheckboxes(IDatabaseService database)
        {
            Database = database;

            ScanDateTimeCheckboxes[Database.SettingsDb().UserSettings.SelectedScanDateTimeIndex] = true;

            Messenger.Default.Register<NotificationMessage>(this, (message) => ReceivedMessage(message));
        }

        public bool LatestParsedChecked
        {
            get => ScanDateTimeCheckboxes[LatestParsedIndex];
            set
            {
                if (Database.SettingsDb().UserSettings.SelectedScanDateTimeIndex != LatestParsedIndex)
                {
                    ScanDateTimeCheckboxes[LatestParsedIndex] = value;
                    if (value)
                    {
                        Database.SettingsDb().UserSettings.SelectedScanDateTimeIndex = LatestParsedIndex;
                        LastParsedChecked = false;
                        LatestHeroesProfileUploaderChecked = false;
                        LastHeroesProfileUploaderChecked = false;
                    }
                }

                RaisePropertyChanged();
            }
        }

        public bool LastParsedChecked
        {
            get => ScanDateTimeCheckboxes[LastParsedIndex];
            set
            {
                if (Database.SettingsDb().UserSettings.SelectedScanDateTimeIndex != LastParsedIndex)
                {
                    ScanDateTimeCheckboxes[LastParsedIndex] = value;
                    if (value)
                    {
                        Database.SettingsDb().UserSettings.SelectedScanDateTimeIndex = LastParsedIndex;
                        LatestParsedChecked = false;
                        LatestHeroesProfileUploaderChecked = false;
                        LastHeroesProfileUploaderChecked = false;
                    }
                }

                RaisePropertyChanged();
            }
        }

        public bool LatestHeroesProfileUploaderChecked
        {
            get => ScanDateTimeCheckboxes[LatestHeroesProfileUploaderIndex];
            set
            {
                if (Database.SettingsDb().UserSettings.SelectedScanDateTimeIndex != LatestHeroesProfileUploaderIndex)
                {
                    ScanDateTimeCheckboxes[LatestHeroesProfileUploaderIndex] = value;
                    if (value)
                    {
                        Database.SettingsDb().UserSettings.SelectedScanDateTimeIndex = LatestHeroesProfileUploaderIndex;
                        LatestParsedChecked = false;
                        LastParsedChecked = false;
                        LastHeroesProfileUploaderChecked = false;
                    }
                }

                RaisePropertyChanged();
            }
        }

        public bool LastHeroesProfileUploaderChecked
        {
            get => ScanDateTimeCheckboxes[LastHeroesProfileUploaderIndex];
            set
            {
                if (Database.SettingsDb().UserSettings.SelectedScanDateTimeIndex != LastHeroesProfileUploaderIndex)
                {
                    ScanDateTimeCheckboxes[LastHeroesProfileUploaderIndex] = value;
                    if (value)
                    {
                        Database.SettingsDb().UserSettings.SelectedScanDateTimeIndex = LastHeroesProfileUploaderIndex;
                        LatestParsedChecked = false;
                        LastParsedChecked = false;
                        LatestHeroesProfileUploaderChecked = false;
                    }
                }

                RaisePropertyChanged();
            }
        }

        private void ReceivedMessage(NotificationMessage message)
        {
            if (message.Notification == StaticMessage.HotsApiUploaderDisabled)
            {
                if (LatestHeroesProfileUploaderChecked || LastHeroesProfileUploaderChecked)
                    LatestParsedChecked = true;

                LatestHeroesProfileUploaderChecked = false;
                LastHeroesProfileUploaderChecked = false;
            }
        }
    }
}
