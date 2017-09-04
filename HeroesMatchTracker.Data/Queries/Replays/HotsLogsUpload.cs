using HeroesMatchTracker.Data.Databases;
using HeroesMatchTracker.Data.Models.Replays;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SQLite;
using System.Linq;

namespace HeroesMatchTracker.Data.Queries.Replays
{
    public class HotsLogsUpload : QueriesBase, IRawDataQueries<ReplayHotsLogsUpload>
    {
        public IEnumerable<ReplayHotsLogsUpload> ReadAllRecords()
        {
            using (var db = new ReplaysContext())
            {
                return db.ReplayHotsLogsUploads.AsNoTracking().ToList();
            }
        }

        public IEnumerable<ReplayHotsLogsUpload> ReadLastRecords(int amount)
        {
            using (var db = new ReplaysContext())
            {
                return db.ReplayHotsLogsUploads.AsNoTracking().OrderByDescending(x => x.ReplayId).Take(amount).ToList();
            }
        }

        public IEnumerable<ReplayHotsLogsUpload> ReadRecordsCustomTop(int amount, string columnName, string orderBy)
        {
            if (string.IsNullOrEmpty(columnName) || string.IsNullOrEmpty(orderBy))
                return new List<ReplayHotsLogsUpload>();

            if (amount == 0)
                amount = 1;

            using (var db = new ReplaysContext())
            {
                return db.ReplayHotsLogsUploads.SqlQuery($"SELECT * FROM ReplayHotsLogsUploads ORDER BY {columnName} {orderBy} LIMIT {amount}").AsNoTracking().ToList();
            }
        }

        public IEnumerable<ReplayHotsLogsUpload> ReadRecordsWhere(string columnName, string operand, string input)
        {
            if (string.IsNullOrEmpty(columnName) || string.IsNullOrEmpty(operand))
                return new List<ReplayHotsLogsUpload>();

            if (LikeOperatorInputCheck(operand, input))
                input = $"%{input}%";
            else if (input == null)
                input = string.Empty;

            using (var db = new ReplaysContext())
            {
                return db.ReplayHotsLogsUploads.SqlQuery($"SELECT * FROM ReplayHotsLogsUploads WHERE {columnName} {operand} @Input", new SQLiteParameter("@Input", input)).AsNoTracking().ToList();
            }
        }

        public IEnumerable<ReplayHotsLogsUpload> ReadTopRecords(int amount)
        {
            using (var db = new ReplaysContext())
            {
                return db.ReplayHotsLogsUploads.AsNoTracking().Take(amount).ToList();
            }
        }

        public long CreateRecord(ReplayHotsLogsUpload replayHotsLogsUpload)
        {
            using (var db = new ReplaysContext())
            {
                db.ReplayHotsLogsUploads.Add(replayHotsLogsUpload);
                db.SaveChanges();
            }

            return replayHotsLogsUpload.ReplaysHotsLogsUploadId;
        }

        public bool IsExistingRecord(ReplayHotsLogsUpload replayHotsLogsUpload)
        {
            using (var db = new ReplaysContext())
            {
                return db.ReplayHotsLogsUploads.AsNoTracking().Any(x => x.ReplayId == replayHotsLogsUpload.ReplayId);
            }
        }

        /// <summary>
        /// Reads the current status of the given ReplayHotsLogsUpload.  Call IsExistingRecord() to ensure record exists.
        /// </summary>
        /// <param name="replayHotsLogsUpload"></param>
        /// <returns></returns>
        public int? ReadUploadStatus(ReplayHotsLogsUpload replayHotsLogsUpload)
        {
            using (var db = new ReplaysContext())
            {
                // check to see if a duplicate record got inserted
                if (db.ReplayHotsLogsUploads.AsNoTracking().Count(x => x.ReplayId == replayHotsLogsUpload.ReplayId) > 1)
                {
                    var record = db.ReplayHotsLogsUploads.AsNoTracking().First(x => x.ReplayId == replayHotsLogsUpload.ReplayId);
                    db.Entry(record).State = EntityState.Deleted; // delete the duplicate record

                    db.SaveChanges();
                }

                return db.ReplayHotsLogsUploads.AsNoTracking().SingleOrDefault(x => x.ReplayId == replayHotsLogsUpload.ReplayId).Status;
            }
        }

        public void UpdateHotsLogsUploadedDateTime(ReplayHotsLogsUpload replayHotsLogsUpload)
        {
            using (var db = new ReplaysContext())
            {
                var record = db.ReplayHotsLogsUploads.SingleOrDefault(x => x.ReplayId == replayHotsLogsUpload.ReplayId);

                if (record != null)
                {
                    if (replayHotsLogsUpload.Status == 0 || replayHotsLogsUpload.Status == 1) // 0 = success | 1 = duplicate
                        record.ReplayFileTimeStamp = replayHotsLogsUpload.ReplayFileTimeStamp;

                    record.Status = replayHotsLogsUpload.Status;
                    db.SaveChanges();
                }
            }
        }

        public DateTime ReadLatestReplayHotsLogsUploadedByDateTime()
        {
            using (var db = new ReplaysContext())
            {
                var record = db.ReplayHotsLogsUploads.AsNoTracking().OrderByDescending(x => x.ReplayFileTimeStamp).FirstOrDefault();

                if (record != null && record.ReplayFileTimeStamp.HasValue)
                    return record.ReplayFileTimeStamp.Value;
                else
                    return DateTime.Today;
            }
        }

        public DateTime ReadLastReplayHotsLogsUploaded()
        {
            using (var db = new ReplaysContext())
            {
                var record = db.ReplayHotsLogsUploads.AsNoTracking().OrderByDescending(x => x.ReplaysHotsLogsUploadId).FirstOrDefault();

                if (record != null && record.ReplayFileTimeStamp.HasValue)
                    return record.ReplayFileTimeStamp.Value;
                else
                    return DateTime.Today;
            }
        }
    }
}
