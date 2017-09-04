using HeroesMatchTracker.Data.Databases;
using HeroesMatchTracker.Data.Models.Replays;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SQLite;
using System.Linq;

namespace HeroesMatchTracker.Data.Queries.Replays
{
    public class HotsApiUpload : QueriesBase, IRawDataQueries<ReplayHotsApiUpload>
    {
        public IEnumerable<ReplayHotsApiUpload> ReadAllRecords()
        {
            using (var db = new ReplaysContext())
            {
                return db.ReplayHotsApiUploads.AsNoTracking().ToList();
            }
        }

        public IEnumerable<ReplayHotsApiUpload> ReadLastRecords(int amount)
        {
            using (var db = new ReplaysContext())
            {
                return db.ReplayHotsApiUploads.AsNoTracking().OrderByDescending(x => x.ReplayId).Take(amount).ToList();
            }
        }

        public IEnumerable<ReplayHotsApiUpload> ReadRecordsCustomTop(int amount, string columnName, string orderBy)
        {
            if (string.IsNullOrEmpty(columnName) || string.IsNullOrEmpty(orderBy))
                return new List<ReplayHotsApiUpload>();

            if (amount == 0)
                amount = 1;

            using (var db = new ReplaysContext())
            {
                return db.ReplayHotsApiUploads.SqlQuery($"SELECT * FROM ReplayHotsApiUploads ORDER BY {columnName} {orderBy} LIMIT {amount}").AsNoTracking().ToList();
            }
        }

        public IEnumerable<ReplayHotsApiUpload> ReadRecordsWhere(string columnName, string operand, string input)
        {
            if (string.IsNullOrEmpty(columnName) || string.IsNullOrEmpty(operand))
                return new List<ReplayHotsApiUpload>();

            if (LikeOperatorInputCheck(operand, input))
                input = $"%{input}%";
            else if (input == null)
                input = string.Empty;

            using (var db = new ReplaysContext())
            {
                return db.ReplayHotsApiUploads.SqlQuery($"SELECT * FROM ReplayHotsApiUploads WHERE {columnName} {operand} @Input", new SQLiteParameter("@Input", input)).AsNoTracking().ToList();
            }
        }

        public IEnumerable<ReplayHotsApiUpload> ReadTopRecords(int amount)
        {
            using (var db = new ReplaysContext())
            {
                return db.ReplayHotsApiUploads.AsNoTracking().Take(amount).ToList();
            }
        }

        public long CreateRecord(ReplayHotsApiUpload replayHotsApiUpload)
        {
            using (var db = new ReplaysContext())
            {
                db.ReplayHotsApiUploads.Add(replayHotsApiUpload);
                db.SaveChanges();
            }

            return replayHotsApiUpload.ReplaysHotsApiUploadId;
        }

        public bool IsExistingRecord(ReplayHotsApiUpload replayHotsApiUpload)
        {
            using (var db = new ReplaysContext())
            {
                return db.ReplayHotsApiUploads.AsNoTracking().Any(x => x.ReplayId == replayHotsApiUpload.ReplayId);
            }
        }

        /// <summary>
        /// Reads the current status of the given ReplayHotsApiUpload.  Call IsExistingRecord() to ensure record exists.
        /// </summary>
        /// <param name="replayHotsApiUpload"></param>
        /// <returns></returns>
        public int? ReadUploadStatus(ReplayHotsApiUpload replayHotsApiUpload)
        {
            using (var db = new ReplaysContext())
            {
                // check to see if a duplicate record got inserted
                if (db.ReplayHotsApiUploads.AsNoTracking().Count(x => x.ReplayId == replayHotsApiUpload.ReplayId) > 1)
                {
                    var record = db.ReplayHotsApiUploads.AsNoTracking().First(x => x.ReplayId == replayHotsApiUpload.ReplayId);
                    db.Entry(record).State = EntityState.Deleted; // delete the duplicate record

                    db.SaveChanges();
                }

                return db.ReplayHotsApiUploads.AsNoTracking().SingleOrDefault(x => x.ReplayId == replayHotsApiUpload.ReplayId).Status;
            }
        }

        public void UpdateHotsApiUploadedDateTime(ReplayHotsApiUpload replayHotsApiUpload)
        {
            using (var db = new ReplaysContext())
            {
                var record = db.ReplayHotsApiUploads.SingleOrDefault(x => x.ReplayId == replayHotsApiUpload.ReplayId);

                if (record != null)
                {
                    if (replayHotsApiUpload.Status == 0 || replayHotsApiUpload.Status == 1) // 0 = success | 1 = duplicate
                        record.ReplayFileTimeStamp = replayHotsApiUpload.ReplayFileTimeStamp;

                    record.Status = replayHotsApiUpload.Status;
                    db.SaveChanges();
                }
            }
        }

        public DateTime ReadLatestReplayHotsApiUploadedByDateTime()
        {
            using (var db = new ReplaysContext())
            {
                var record = db.ReplayHotsApiUploads.AsNoTracking().OrderByDescending(x => x.ReplayFileTimeStamp).FirstOrDefault();

                if (record != null && record.ReplayFileTimeStamp.HasValue)
                    return record.ReplayFileTimeStamp.Value;
                else
                    return DateTime.Today;
            }
        }

        public DateTime ReadLastReplayHotsApiUploaded()
        {
            using (var db = new ReplaysContext())
            {
                var record = db.ReplayHotsApiUploads.AsNoTracking().OrderByDescending(x => x.ReplaysHotsApiUploadId).FirstOrDefault();

                if (record != null && record.ReplayFileTimeStamp.HasValue)
                    return record.ReplayFileTimeStamp.Value;
                else
                    return DateTime.Today;
            }
        }
    }
}
