namespace HeroesParserData.Models.DbModels
{
    using HotsLogs;
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public partial class ReplayHotsLogsUpload : IReplayDataTable
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long ReplaysHotsLogsUploadId { get; set; }

        public long ReplayId { get; set; }

        public DateTime? ReplayFileTimeStamp { get; set; }

        public ReplayHotsLogStatus Status { get; set; }

        public virtual Replay Replay { get; set; }
    }
}
