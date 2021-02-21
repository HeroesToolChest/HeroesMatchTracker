namespace HeroesMatchTracker.Data.Models.Replays
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class ReplayHeroesProfileUpload : IRawDataDisplay, INonContextModels
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long ReplaysHeroesProfileUploadId { get; set; }

        [Index(IsUnique = true)]
        public long ReplayId { get; set; }

        public DateTime? ReplayFileTimeStamp { get; set; }

        public int Status { get; set; }

        public virtual ReplayMatch Replay { get; set; }
    }
}
