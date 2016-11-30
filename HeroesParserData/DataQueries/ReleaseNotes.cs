using HeroesParserData.Models.DbModels;
using System.Collections.Generic;
using System.Linq;

namespace HeroesParserData.DataQueries
{
    public static partial class Query
    {
        internal static class ReleaseNotes
        {
            public static void CreateRecord(ReleaseNote releaseNote)
            {
                using (var db = new HeroesParserDataContext())
                {
                    db.ReleaseNotes.Add(releaseNote);
                    db.SaveChanges();
                }
            }

            public static List<ReleaseNote> ReadAllRecords()
            {
                using (var db = new HeroesParserDataContext())
                {
                    return db.ReleaseNotes.ToList();
                }
            }

            public static ReleaseNote ReadLastReleaseNote()
            {
                using (var db = new HeroesParserDataContext())
                {
                    var list = db.ReleaseNotes.OrderByDescending(x => x.DateReleased).Take(1).ToList();

                    if (list.Count > 0)
                        return list[0];
                    else
                        return new ReleaseNote() { Version = "0.0.0" };
                }
            }

            public static bool IsExistingReleaseNote(ReleaseNote releaseNote)
            {
                using (var db = new HeroesParserDataContext())
                {
                    return db.ReleaseNotes.Any(x => x.Version == releaseNote.Version);
                }
            }

            public static bool IsCurrentVersionPreReleaes()
            {
                using (var db = new HeroesParserDataContext())
                {
                    var list = db.ReleaseNotes.OrderByDescending(x => x.DateReleased).Take(1).ToList();
                    if (list.Count > 0)
                        return list[0].PreRelease;
                    else
                        return false;
                }
            }

            public static void UpdateReleaseNote(ReleaseNote releaseNote)
            {
                using (var db = new HeroesParserDataContext())
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
}
