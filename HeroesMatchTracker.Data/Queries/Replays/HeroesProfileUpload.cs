using HeroesMatchTracker.Data.Databases;
using HeroesMatchTracker.Data.Models.Replays;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SQLite;
using System.Linq;

namespace HeroesMatchTracker.Data.Queries.Replays
{
    public class HeroesProfileUpload : QueriesBase, IRawDataQueries<ReplayHeroesProfileUpload>
    {
        public IEnumerable<ReplayHeroesProfileUpload> ReadAllRecords()
        {
            using (var db = new ReplaysContext())
            {
                return db.ReplayHeroesProfileUploads.AsNoTracking().ToList();
            }
        }

        public IEnumerable<ReplayHeroesProfileUpload> ReadLastRecords(int amount)
        {
            using (var db = new ReplaysContext())
            {
                return db.ReplayHeroesProfileUploads.AsNoTracking().OrderByDescending(x => x.ReplayId).Take(amount).ToList();
            }
        }

        public IEnumerable<ReplayHeroesProfileUpload> ReadRecordsCustomTop(int amount, string columnName, string orderBy)
        {
            if (string.IsNullOrEmpty(columnName) || string.IsNullOrEmpty(orderBy))
                return new List<ReplayHeroesProfileUpload>();

            if (amount == 0)
                amount = 1;

            using (var db = new ReplaysContext())
            {
                return db.ReplayHeroesProfileUploads.SqlQuery($"SELECT * FROM ReplayHeroesProfileUploads ORDER BY {columnName} {orderBy} LIMIT {amount}").AsNoTracking().ToList();
            }
        }

        public IEnumerable<ReplayHeroesProfileUpload> ReadRecordsWhere(string columnName, string operand, string input)
        {
            if (string.IsNullOrEmpty(columnName) || string.IsNullOrEmpty(operand))
                return new List<ReplayHeroesProfileUpload>();

            if (LikeOperatorInputCheck(operand, input))
                input = $"%{input}%";
            else if (input == null)
                input = string.Empty;

            using (var db = new ReplaysContext())
            {
                return db.ReplayHeroesProfileUploads.SqlQuery($"SELECT * FROM ReplayHeroesProfileUploads WHERE {columnName} {operand} @Input", new SQLiteParameter("@Input", input)).AsNoTracking().ToList();
            }
        }

        public IEnumerable<ReplayHeroesProfileUpload> ReadTopRecords(int amount)
        {
            using (var db = new ReplaysContext())
            {
                return db.ReplayHeroesProfileUploads.AsNoTracking().Take(amount).ToList();
            }
        }

        public long CreateRecord(ReplayHeroesProfileUpload replayHeroesProfileUpload)
        {
            using (var db = new ReplaysContext())
            {
                db.ReplayHeroesProfileUploads.Add(replayHeroesProfileUpload);
                db.SaveChanges();
            }

            return replayHeroesProfileUpload.ReplaysHeroesProfileUploadId;
        }

        public bool IsExistingRecord(ReplayHeroesProfileUpload replayHotsApiUpload)
        {
            using (var db = new ReplaysContext())
            {
                return db.ReplayHeroesProfileUploads.AsNoTracking().Any(x => x.ReplayId == replayHotsApiUpload.ReplayId);
            }
        }

        /// <summary>
        /// Reads the current status of the given ReplayHeroesProfileUpload.  Call IsExistingRecord() to ensure record exists.
        /// </summary>
        /// <param name="replayHeroesProfileUploads"></param>
        /// <returns></returns>
        public int? ReadUploadStatus(ReplayHeroesProfileUpload replayHeroesProfileUploads)
        {
            using (var db = new ReplaysContext())
            {
                // check to see if a duplicate record got inserted
                if (db.ReplayHeroesProfileUploads.AsNoTracking().Count(x => x.ReplayId == replayHeroesProfileUploads.ReplayId) > 1)
                {
                    var record = db.ReplayHeroesProfileUploads.AsNoTracking().First(x => x.ReplayId == replayHeroesProfileUploads.ReplayId);
                    db.Entry(record).State = EntityState.Deleted; // delete the duplicate record

                    db.SaveChanges();
                }

                return db.ReplayHeroesProfileUploads.AsNoTracking().SingleOrDefault(x => x.ReplayId == replayHeroesProfileUploads.ReplayId).Status;
            }
        }

        public void UpdateReplayHeroesProfileUploadedDateTime(ReplayHeroesProfileUpload replayHeroesProfileUpload)
        {
            using (var db = new ReplaysContext())
            {
                var record = db.ReplayHeroesProfileUploads.SingleOrDefault(x => x.ReplayId == replayHeroesProfileUpload.ReplayId);

                if (record != null)
                {
                    if (replayHeroesProfileUpload.Status == 0 || replayHeroesProfileUpload.Status == 1) // 0 = success | 1 = duplicate
                        record.ReplayFileTimeStamp = replayHeroesProfileUpload.ReplayFileTimeStamp;

                    record.Status = replayHeroesProfileUpload.Status;
                    db.SaveChanges();
                }
            }
        }

        public DateTime ReadLatestReplayHeroesProfileUploadedByDateTime()
        {
            using (var db = new ReplaysContext())
            {
                var record = db.ReplayHeroesProfileUploads.AsNoTracking().OrderByDescending(x => x.ReplayFileTimeStamp).FirstOrDefault();

                if (record != null && record.ReplayFileTimeStamp.HasValue)
                    return record.ReplayFileTimeStamp.Value;
                else
                    return DateTime.Today;
            }
        }

        public DateTime ReadLastReplayHeroesProfileUploaded()
        {
            using (var db = new ReplaysContext())
            {
                var record = db.ReplayHeroesProfileUploads.AsNoTracking().OrderByDescending(x => x.ReplaysHeroesProfileUploadId).FirstOrDefault();

                if (record != null && record.ReplayFileTimeStamp.HasValue)
                    return record.ReplayFileTimeStamp.Value;
                else
                    return DateTime.Today;
            }
        }
    }
}
