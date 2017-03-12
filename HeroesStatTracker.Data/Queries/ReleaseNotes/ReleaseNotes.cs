using HeroesMatchData.Data.Databases;
using HeroesMatchData.Data.Models.ReleaseNotes;
using System.Collections.Generic;
using System.Linq;

namespace HeroesMatchData.Data.Queries.ReleaseNotes
{
    public class ReleaseNotes
    {
        public void CreateRecord(ReleaseNote releaseNote)
        {
            using (var db = new ReleaseNotesContext())
            {
                db.ReleaseNotes.Add(releaseNote);
                db.SaveChanges();
            }
        }

        public List<ReleaseNote> ReadAllRecords()
        {
            using (var db = new ReleaseNotesContext())
            {
                return db.ReleaseNotes.ToList();
            }
        }

        public ReleaseNote ReadLastReleaseNote()
        {
            using (var db = new ReleaseNotesContext())
            {
                var list = db.ReleaseNotes.OrderByDescending(x => x.DateReleased).Take(1).ToList();

                if (list.Count > 0)
                    return list[0];
                else
                    return new ReleaseNote() { Version = "0.0.0" };
            }
        }

        public bool IsExistingReleaseNote(ReleaseNote releaseNote)
        {
            using (var db = new ReleaseNotesContext())
            {
                return db.ReleaseNotes.Any(x => x.Version == releaseNote.Version);
            }
        }

        public bool IsCurrentVersionPreReleaes()
        {
            using (var db = new ReleaseNotesContext())
            {
                var list = db.ReleaseNotes.OrderByDescending(x => x.DateReleased).Take(1).ToList();
                if (list.Count > 0)
                    return list[0].PreRelease;
                else
                    return false;
            }
        }

        public void UpdateReleaseNote(ReleaseNote releaseNote)
        {
            using (var db = new ReleaseNotesContext())
            {
                var record = db.ReleaseNotes.SingleOrDefault(x => x.Version == releaseNote.Version);

                if (record != null)
                {
                    record.PreRelease = releaseNote.PreRelease;
                    record.PatchNote = releaseNote.PatchNote;

                    db.SaveChanges();
                }
            }
        }
    }
}
