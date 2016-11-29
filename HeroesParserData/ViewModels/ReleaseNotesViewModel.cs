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
