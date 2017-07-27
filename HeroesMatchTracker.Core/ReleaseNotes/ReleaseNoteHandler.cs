using HeroesMatchTracker.Data;
using HeroesMatchTracker.Data.Models.ReleaseNotes;
using Octokit;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace HeroesMatchTracker.Core.ReleaseNotes
{
    public class ReleaseNoteHandler
    {
        private IReadOnlyList<Release> Releases;
        private IDatabaseService Database;

        public ReleaseNoteHandler(IDatabaseService database)
        {
            Database = database;
        }

        public async Task InitializeClient()
        {
            var client = new GitHubClient(new ProductHeaderValue("HeroesMatchTracker", AssemblyVersions.HeroesMatchTrackerVersion().ToString()));
            Releases = await client.Repository.Release.GetAll("koliva8245", "HeroesMatchTracker");
        }

        /// <summary>
        /// Adds the newest release notes to the database and also updates the latest release note
        /// </summary>
        public void AddReleaseNotes()
        {
            Version versionInDatabase = new Version(Database.ReleaseNotesDb().ReleaseNotes.ReadLastReleaseNote().Version);

            if (Releases.Count > 0)
            {
                // check to see if the release note version we have in the database is newer than the one available on github
                // this happens because of a test release that has since been removed
                while (versionInDatabase > new Version(Releases[0].TagName.TrimStart('v')))
                {
                    // remove the release that no longer exists
                    Database.ReleaseNotesDb().ReleaseNotes.DeleteReleaseNote(new ReleaseNote { Version = versionInDatabase.ToString() });
                    versionInDatabase = new Version(Database.ReleaseNotesDb().ReleaseNotes.ReadLastReleaseNote().Version);
                }
            }

            foreach (var release in Releases)
            {
                Version versionRelease = new Version(release.TagName.TrimStart('v'));

                if (versionRelease >= versionInDatabase)
                {
                    // don't add newer release notes newer than the current version the app is on
                    if (versionRelease <= AssemblyVersions.HeroesMatchTrackerVersion().Version)
                        AddReleaseNote(release);
                }
                else
                {
                    // add one more so its updated
                    AddReleaseNote(release);
                    break;
                }
            }
        }

        /// <summary>
        /// Adds all the Releases to the database
        /// </summary>
        public void AddAllReleaseNotes()
        {
            foreach (var releaseNote in Releases)
                AddReleaseNote(releaseNote);
        }

        /// <summary>
        /// Adds all the Releases up to the current version of the application to the database
        /// </summary>
        public void AddAllReleasesUpToCurrentVersion()
        {
            Version versionTemp = AssemblyVersions.HeroesMatchTrackerVersion().Version;
            Version currentVersion = new Version(versionTemp.Major, versionTemp.Minor, versionTemp.Build);

            foreach (var releaseNote in Releases)
            {
                if (new Version(releaseNote.TagName.TrimStart('v')) <= currentVersion)
                    AddReleaseNote(releaseNote);
            }
        }

        // adds the release to the database
        private void AddReleaseNote(Release release)
        {
            string version = release.TagName.TrimStart('v');

            ReleaseNote releaseNote = new ReleaseNote
            {
                DateReleased = release.PublishedAt.Value.DateTime,
                PatchNote = ModifyReleaseBody(release.Body),
                PreRelease = release.Prerelease,
                Version = version,
            };

            if (Database.ReleaseNotesDb().ReleaseNotes.IsExistingReleaseNote(releaseNote))
                Database.ReleaseNotesDb().ReleaseNotes.UpdateReleaseNote(releaseNote);
            else
                Database.ReleaseNotesDb().ReleaseNotes.CreateRecord(releaseNote);
        }

        private string ModifyReleaseBody(string releaseBody)
        {
            releaseBody = releaseBody.Replace("\r\n", Environment.NewLine);

            string pattern = @"#\d+";
            string replacement = "[$&](https://github.com/koliva8245/HeroesMatchTracker/issues/$&)";

            releaseBody = Regex.Replace(releaseBody, pattern, replacement);
            return releaseBody.Replace(@"/issues/#", @"/issues/");
        }
    }
}
