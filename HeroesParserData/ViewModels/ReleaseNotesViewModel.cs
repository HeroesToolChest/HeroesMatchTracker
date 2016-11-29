using HeroesParserData.DataQueries;
using System;
using System.Linq;

namespace HeroesParserData.ViewModels
{
    public class ReleaseNotesViewModel : ViewModelBase
    {
        private string ReleaseNotesMarkdown;

        public string ReleaseNotesMarkdownText
        {
            get { return ReleaseNotesMarkdown; }
        }

        public ReleaseNotesViewModel()
        {
            SetReleaseNotesLog();
        }

        private void SetReleaseNotesLog()
        {
            var listOfReleases = Query.ReleaseNotes.ReadAllRecords().OrderByDescending(x => x.DateReleased).ToList();

            if (listOfReleases.Count <= 0)
                return;

            Version currentVersion = HPDVersion.GetVersion();
            if (new Version(listOfReleases[0].Version) < currentVersion)
            {
                if (listOfReleases[0].PreRelease)
                    ReleaseNotesMarkdown += $"##[Pre-release] Heroes Parser Data {currentVersion} (Unknown) {Environment.NewLine}";
                else
                    ReleaseNotesMarkdown += $"##Heroes Parser Data {currentVersion} (Unknown) {Environment.NewLine}";

                ReleaseNotesMarkdown += "WARNING - Could not download the latest releases logs. Visit the [releases](https://github.com/koliva8245/HeroesParserData/releases) page for release notes.";
                ReleaseNotesMarkdown += $"{Environment.NewLine} *** {Environment.NewLine}";
            }


            foreach (var release in listOfReleases)
            {
                if (release.PreRelease)
                    ReleaseNotesMarkdown += $"##[Pre-release] Heroes Parser Data {release.Version} ({release.DateReleased.ToString("MMMM dd, yyyy")}) {Environment.NewLine}";
                else
                    ReleaseNotesMarkdown += $"##Heroes Parser Data {release.Version} ({release.DateReleased.ToString("MMMM dd, yyyy")}) {Environment.NewLine}";

                ReleaseNotesMarkdown += release.PatchNote;
                ReleaseNotesMarkdown += $"{Environment.NewLine} *** {Environment.NewLine}";
            }
        }
    }
}
