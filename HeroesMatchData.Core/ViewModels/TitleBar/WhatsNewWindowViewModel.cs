using GalaSoft.MvvmLight;
using HeroesMatchData.Data;
using System;
using System.Linq;

namespace HeroesMatchData.Core.ViewModels.TitleBar
{
    public class WhatsNewWindowViewModel : ViewModelBase
    {
        private IDatabaseService Database;
        private string ReleaseNotesMarkdown;

        public WhatsNewWindowViewModel(IDatabaseService database)
        {
            Database = database;
            SetReleaseNotesLog();
        }

        public string ReleaseNotesMarkdownText => ReleaseNotesMarkdown;

        private void SetReleaseNotesLog()
        {
            var listOfReleases = Database.ReleaseNotesDb().ReleaseNotes.ReadAllRecords().OrderByDescending(x => x.DateReleased).ToList();

            if (listOfReleases.Count <= 0)
                return;

            Version versionTemp = AssemblyVersions.HeroesMatchDataVersion().Version;
            Version currentVersion = new Version(versionTemp.Major, versionTemp.Minor, versionTemp.Build);

            if (new Version(listOfReleases[0].Version) < currentVersion)
            {
                if (listOfReleases[0].PreRelease)
                    ReleaseNotesMarkdown += $"##[Pre-release] Heroes Match Data {currentVersion} (Unknown) {Environment.NewLine}";
                else
                    ReleaseNotesMarkdown += $"##Heroes Match Data {currentVersion} (Unknown) {Environment.NewLine}";

                ReleaseNotesMarkdown += "WARNING - Could not download the latest releases logs. Visit the [releases](https://github.com/koliva8245/HeroesParserData/releases) page for release notes.";
                ReleaseNotesMarkdown += $"{Environment.NewLine} *** {Environment.NewLine}";
            }

            foreach (var release in listOfReleases)
            {
                var versionNums = release.Version.Split('.');

                if (Convert.ToInt32(versionNums[0]) > 1 || Convert.ToInt32(versionNums[1]) > 99) // version 2.x.x. or 1.100.x (beta for 2.x.x)
                {
                    if (release.PreRelease)
                        ReleaseNotesMarkdown += $"##[Pre-release] Heroes Match Data {release.Version} ({release.DateReleased.ToString("MMMM dd, yyyy")}) {Environment.NewLine}";
                    else
                        ReleaseNotesMarkdown += $"##Heroes Match Data {release.Version} ({release.DateReleased.ToString("MMMM dd, yyyy")}) {Environment.NewLine}";
                }
                else
                {
                    if (release.PreRelease)
                        ReleaseNotesMarkdown += $"##[Pre-release] Heroes Parser Data {release.Version} ({release.DateReleased.ToString("MMMM dd, yyyy")}) {Environment.NewLine}";
                    else
                        ReleaseNotesMarkdown += $"##Heroes Parser Data {release.Version} ({release.DateReleased.ToString("MMMM dd, yyyy")}) {Environment.NewLine}";
                }

                ReleaseNotesMarkdown += release.PatchNote;
                ReleaseNotesMarkdown += $"{Environment.NewLine} *** {Environment.NewLine}";
            }
        }
    }
}
