using GalaSoft.MvvmLight;
using HeroesMatchTracker.Data;
using System;
using System.Linq;

namespace HeroesMatchTracker.Core.ViewModels.TitleBar
{
    public class WhatsNewWindowViewModel : ViewModelBase
    {
        private IDatabaseService Database;
        private string _releaseNotesMarkdownText;

        public WhatsNewWindowViewModel(IDatabaseService database)
        {
            Database = database;

            SetReleaseNotesLog();
        }

        public string ReleaseNotesMarkdownText
        {
            get => _releaseNotesMarkdownText;
            set
            {
                _releaseNotesMarkdownText = value;
                RaisePropertyChanged();
            }
        }

        private void SetReleaseNotesLog()
        {
            var listOfReleases = Database.ReleaseNotesDb().ReleaseNotes.ReadAllRecords().OrderByDescending(x => x.DateReleased).ToList();

            if (listOfReleases.Count <= 0)
                return;

            Version versionTemp = AssemblyVersions.HeroesMatchTrackerVersion().Version;
            Version currentVersion = new Version(versionTemp.Major, versionTemp.Minor, versionTemp.Build);

            if (new Version(listOfReleases[0].Version) < currentVersion)
            {
                if (listOfReleases[0].PreRelease)
                    ReleaseNotesMarkdownText += $"## [Pre-release] Heroes Match Tracker {currentVersion} (Unknown) {Environment.NewLine}";
                else
                    ReleaseNotesMarkdownText += $"## Heroes Match Tracker {currentVersion} (Unknown) {Environment.NewLine}";

                ReleaseNotesMarkdownText += "WARNING - Could not download the latest releases logs. Visit the [releases](https://github.com/koliva8245/HeroesMatchTracker/releases) page for release notes.";
                ReleaseNotesMarkdownText += $"{Environment.NewLine} *** {Environment.NewLine}";
            }

            foreach (var release in listOfReleases)
            {
                var versionNums = release.Version.Split('.');

                if (Convert.ToInt32(versionNums[0]) > 1 || Convert.ToInt32(versionNums[1]) > 99) // version 2.x.x. or 1.100.x (beta for 2.x.x)
                {
                    if (release.PreRelease)
                        ReleaseNotesMarkdownText += $"## [Pre-release] Heroes Match Tracker {release.Version} ({release.DateReleased.ToLocalTime().ToString("MMMM dd, yyyy")}) {Environment.NewLine}";
                    else
                        ReleaseNotesMarkdownText += $"## Heroes Match Tracker {release.Version} ({release.DateReleased.ToLocalTime().ToString("MMMM dd, yyyy")}) {Environment.NewLine}";
                }
                else // legacy
                {
                    if (release.PreRelease)
                        ReleaseNotesMarkdownText += $"## [Pre-release] Heroes Parser Data {release.Version} ({release.DateReleased.ToLocalTime().ToString("MMMM dd, yyyy")}) {Environment.NewLine}";
                    else
                        ReleaseNotesMarkdownText += $"## Heroes Parser Data {release.Version} ({release.DateReleased.ToLocalTime().ToString("MMMM dd, yyyy")}) {Environment.NewLine}";
                }

                ReleaseNotesMarkdownText += release.PatchNote;
                ReleaseNotesMarkdownText += $"{Environment.NewLine} *** {Environment.NewLine}";
            }
        }
    }
}
