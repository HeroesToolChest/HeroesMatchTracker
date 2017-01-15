namespace HeroesStatTracker.Data.Models.Replays
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class ReplayMatchPlayerTalent : IRawDataDisplay, INonContextModels
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long MatchPlayerTalentId { get; set; }

        public long ReplayId { get; set; }

        public long PlayerId { get; set; }

        [StringLength(50)]
        public string Character { get; set; }

        public int? TalentId1 { get; set; }

        [StringLength(75)]
        public string TalentName1 { get; set; }

        public long? TimeSpanSelected1Ticks { get; set; }

        [NotMapped]
        public TimeSpan? TimeSpanSelected1
        {
            get { return TimeSpanSelected1Ticks.HasValue ? TimeSpan.FromTicks(TimeSpanSelected1Ticks.Value) : (TimeSpan?)null; }
            set { TimeSpanSelected1Ticks = value.HasValue ? value.Value.Ticks : (long?)null; }
        }

        public int? TalentId4 { get; set; }

        [StringLength(75)]
        public string TalentName4 { get; set; }

        public long? TimeSpanSelected4Ticks { get; set; }

        [NotMapped]
        public TimeSpan? TimeSpanSelected4
        {
            get { return TimeSpanSelected4Ticks.HasValue ? TimeSpan.FromTicks(TimeSpanSelected4Ticks.Value) : (TimeSpan?)null; }
            set { TimeSpanSelected4Ticks = value.HasValue ? value.Value.Ticks : (long?)null; }
        }

        public int? TalentId7 { get; set; }

        [StringLength(75)]
        public string TalentName7 { get; set; }

        public long? TimeSpanSelected7Ticks { get; set; }

        [NotMapped]
        public TimeSpan? TimeSpanSelected7
        {
            get { return TimeSpanSelected7Ticks.HasValue ? TimeSpan.FromTicks(TimeSpanSelected7Ticks.Value) : (TimeSpan?)null; }
            set { TimeSpanSelected7Ticks = value.HasValue ? value.Value.Ticks : (long?)null; }
        }

        public int? TalentId10 { get; set; }

        [StringLength(75)]
        public string TalentName10 { get; set; }

        public long? TimeSpanSelected10Ticks { get; set; }

        [NotMapped]
        public TimeSpan? TimeSpanSelected10
        {
            get { return TimeSpanSelected10Ticks.HasValue ? TimeSpan.FromTicks(TimeSpanSelected10Ticks.Value) : (TimeSpan?)null; }
            set { TimeSpanSelected10Ticks = value.HasValue ? value.Value.Ticks : (long?)null; }
        }

        public int? TalentId13 { get; set; }

        [StringLength(75)]
        public string TalentName13 { get; set; }

        public long? TimeSpanSelected13Ticks { get; set; }

        [NotMapped]
        public TimeSpan? TimeSpanSelected13
        {
            get { return TimeSpanSelected13Ticks.HasValue ? TimeSpan.FromTicks(TimeSpanSelected13Ticks.Value) : (TimeSpan?)null; }
            set { TimeSpanSelected13Ticks = value.HasValue ? value.Value.Ticks : (long?)null; }
        }

        public int? TalentId16 { get; set; }

        [StringLength(75)]
        public string TalentName16 { get; set; }

        public long? TimeSpanSelected16Ticks { get; set; }

        [NotMapped]
        public TimeSpan? TimeSpanSelected16
        {
            get { return TimeSpanSelected16Ticks.HasValue ? TimeSpan.FromTicks(TimeSpanSelected16Ticks.Value) : (TimeSpan?)null; }
            set { TimeSpanSelected16Ticks = value.HasValue ? value.Value.Ticks : (long?)null; }
        }

        public int? TalentId20 { get; set; }

        [StringLength(75)]
        public string TalentName20 { get; set; }

        public long? TimeSpanSelected20Ticks { get; set; }

        [NotMapped]
        public TimeSpan? TimeSpanSelected20
        {
            get { return TimeSpanSelected20Ticks.HasValue ? TimeSpan.FromTicks(TimeSpanSelected20Ticks.Value) : (TimeSpan?)null; }
            set { TimeSpanSelected20Ticks = value.HasValue ? value.Value.Ticks : (long?)null; }
        }

        public virtual ReplayMatch Replay { get; set; }

        public virtual ReplayAllHotsPlayer ReplayAllHotsPlayer { get; set; }
    }
}
