using HeroesParserData.DataQueries;
using HeroesParserData.Models.DbModels;
using Octokit;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HeroesParserData
{
    public class ReleaseNoteHandler
    {
        private IReadOnlyList<Release> Releases;

        public ReleaseNoteHandler()
        {

        }

        public async Task InitializeClient()
        {
            var client = new GitHubClient(new ProductHeaderValue("HeroesParserData", HPDVersion.GetVersionAsString()));
            Releases = await client.Repository.Release.GetAll("koliva8245", "HeroesParserData");
        }

        /// <summary>
        /// Adds the newest release notes to the database and also updates the latest release note
        /// </summary>
        public void AddApplyReleasesReleaseNotes()
        {
            var latestVersion = new Version(Query.ReleaseNotes.ReadLastReleaseNote().Version);

            foreach (var release in Releases)
            {
                if (new Version(release.TagName.TrimStart('v')) >= latestVersion)
                    AddReleaseNote(release);
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
            Version versionTemp = HPDVersion.GetVersion();
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
                PatchNote = release.Body.Replace("\r\n", Environment.NewLine),
                PreRelease = release.Prerelease,
                Version = version
            };

            if (Query.ReleaseNotes.IsExistingReleaseNote(releaseNote))
                Query.ReleaseNotes.UpdateReleaseNote(releaseNote);
            else
                Query.ReleaseNotes.CreateRecord(releaseNote);
        }
    }
}
