using HeroesParserData.HotsLogs;
using HeroesParserData.Models.DbModels;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;

namespace HeroesParserData.DataQueries
{
    public static partial class Query
    {
        internal static class HotsLogsUpload
        {
            public static long CreateRecord(ReplayHotsLogsUpload replayHotsLogsUpload)
            {
                using (var db = new HeroesParserDataContext())
                {
                    db.ReplayHotsLogsUploads.Add(replayHotsLogsUpload);
                    db.SaveChanges();
                }

                return replayHotsLogsUpload.ReplaysHotsLogsUploadId;
            }

            public static bool IsExistingRecord(ReplayHotsLogsUpload replayHotsLogsUpload)
            {
                using (var db = new HeroesParserDataContext())
                {
                    return db.ReplayHotsLogsUploads.Any(x => x.ReplayId == replayHotsLogsUpload.ReplayId);
                }
            }

            /// <summary>
            /// Reads the current status of the given ReplayHotsLogsUpload.  Call IsExistingRecord() to ensure record exists.
            /// </summary>
            /// <param name="replayHotsLogsUpload"></param>
            /// <returns></returns>
            public static ReplayHotsLogStatus? ReadUploadStatus(ReplayHotsLogsUpload replayHotsLogsUpload)
            {
                using (var db = new HeroesParserDataContext())
                {
                    return db.ReplayHotsLogsUploads.SingleOrDefault(x => x.ReplayId == replayHotsLogsUpload.ReplayId).Status;
                }
            }

            public static List<ReplayHotsLogsUpload> ReadTopRecords(int num)
            {
                using (var db = new HeroesParserDataContext())
                {
                    return db.ReplayHotsLogsUploads.Take(num).ToList();
                }
            }

            public static List<ReplayHotsLogsUpload> ReadLastRecords(int num)
            {
                using (var db = new HeroesParserDataContext())
                {
                    return db.ReplayHotsLogsUploads.OrderByDescending(x => x.ReplayId).Take(num).ToList();
                }
            }

            public static List<ReplayHotsLogsUpload> ReadRecordsCustomTop(int count, string columnName, string orderBy)
            {
                if (string.IsNullOrEmpty(columnName) || string.IsNullOrEmpty(orderBy))
                    return new List<ReplayHotsLogsUpload>();

                if (count == 0)
                    count = 1;

                using (var db = new HeroesParserDataContext())
                {
                    return db.ReplayHotsLogsUploads.SqlQuery($"SELECT * FROM ReplayHotsLogsUploads ORDER BY {columnName} {orderBy} LIMIT {count}").ToList();
                }
            }

            public static List<ReplayHotsLogsUpload> ReadRecordsWhere(string columnName, string operand, string input)
            {
                if (string.IsNullOrEmpty(columnName) || string.IsNullOrEmpty(operand))
                    return new List<ReplayHotsLogsUpload>();

                if (Utilities.LikeOperatorInputCheck(operand, input))
                    input = $"%{input}%";
                else if (input == null)
                    input = string.Empty;

                using (var db = new HeroesParserDataContext())
                {
                    return db.ReplayHotsLogsUploads.SqlQuery($"SELECT * FROM ReplayHotsLogsUploads WHERE {columnName} {operand} @Input", new SQLiteParameter("@Input", input)).ToList();
                }
            }

            public static DateTime ReadLatestReplayHotsLogsUploadedByDateTime()
            {
                using (var db = new HeroesParserDataContext())
                {
                    var record = db.ReplayHotsLogsUploads.OrderByDescending(x => x.ReplayFileTimeStamp).FirstOrDefault();

                    if (record != null && record.ReplayFileTimeStamp.HasValue)
                        return record.ReplayFileTimeStamp.Value;
                    else
                        return new DateTime(2014, 1, 1);
                }
            }

            public static DateTime ReadLastReplayHotsLogsUploadedByDateTime()
            {
                using (var db = new HeroesParserDataContext())
                {
                    var record = db.ReplayHotsLogsUploads.OrderByDescending(x => x.ReplaysHotsLogsUploadId).FirstOrDefault();

                    if (record != null && record.ReplayFileTimeStamp.HasValue)
                        return record.ReplayFileTimeStamp.Value;
                    else
                        return new DateTime(2014, 1, 1);
                }
            }

            public static void UpdateHotsLogsUploadedDateTime(ReplayHotsLogsUpload replayHotsLogsUpload)
            {
                using (var db = new HeroesParserDataContext())
                {
                    var record = db.ReplayHotsLogsUploads.SingleOrDefault(x => x.ReplayId == replayHotsLogsUpload.ReplayId);

                    if (record != null)
                    {
                        if (replayHotsLogsUpload.Status == ReplayHotsLogStatus.Success)
                        {
                            record.ReplayFileTimeStamp = replayHotsLogsUpload.ReplayFileTimeStamp;
                        }

                        record.Status = replayHotsLogsUpload.Status;
                        db.SaveChanges();
                    }
                }
            }
        }
    }
}
