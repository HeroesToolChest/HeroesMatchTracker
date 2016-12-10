namespace HeroesParserData.Models.DbModels
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public partial class ReplayMatchTeamObjective : IReplayDataTable
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long MatchTeamTeamObjectiveId { get; set; }

        public long ReplayId { get; set; }

        public int Team { get; set; }

        public long? PlayerId { get; set; }

        [StringLength(255)]
        public string TeamObjectiveType { get; set; }

        public long? TimeStampTicks { get; set; }

        [NotMapped]
        public TimeSpan? TimeStamp
        {
            get { return TimeStampTicks.HasValue ? TimeSpan.FromTicks(TimeStampTicks.Value) : (TimeSpan?)null; }
            set { TimeStampTicks = value.HasValue ? value.Value.Ticks : (long?)null; }
        }

        public int? Value { get; set; }

        public virtual Replay Replay { get; set; }

        public virtual ReplayAllHotsPlayer ReplayAllHotsPlayer { get; set; }
    }
}