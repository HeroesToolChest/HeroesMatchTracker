namespace HeroesStatTracker.Data.Replays.Models
{
    //using HotsLogs;
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class ReplayHotsLogsUpload : IReplayModelDataTable
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long ReplaysHotsLogsUploadId { get; set; }

        public long ReplayId { get; set; }

        public DateTime? ReplayFileTimeStamp { get; set; }

        //public ReplayHotsLogStatus Status { get; set; }

        public virtual ReplayMatch Replay { get; set; }
    }
}
