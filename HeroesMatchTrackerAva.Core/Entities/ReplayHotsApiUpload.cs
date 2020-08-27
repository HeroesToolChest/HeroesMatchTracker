using System;
using System.ComponentModel.DataAnnotations;

namespace HeroesMatchTracker.Core.Entities
{
    public class ReplayHotsApiUpload
    {
        [Key]
        public long ReplaysHotsApiUploadId { get; set; }

        public long ReplayId { get; set; }

        public DateTime? ReplayFileTimeStamp { get; set; }

        public int Status { get; set; }

        public virtual ReplayMatch ReplayPlayer { get; set; } = null!;
    }
}
