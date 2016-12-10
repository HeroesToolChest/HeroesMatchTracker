namespace HeroesParserData.Models.DbModels
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public partial class ReplayMatchTeamLevel : IReplayDataTable
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long MatchTeamLevelId { get; set; }

        public long ReplayId { get; set; }

        public int? Team0Level { get; set; }

        public long? TeamTime0Ticks { get; set; }

        [NotMapped]
        public TimeSpan? TeamTime0
        {
            get { return TeamTime0Ticks.HasValue ? TimeSpan.FromTicks(TeamTime0Ticks.Value) : (TimeSpan?)null; }
            set { TeamTime0Ticks = value.HasValue ? value.Value.Ticks : (long?)null; }
        }

        public int? Team1Level { get; set; }

        public long? TeamTime1Ticks { get; set; }

        [NotMapped]
        public TimeSpan? TeamTime1
        {
            get { return TeamTime1Ticks.HasValue ? TimeSpan.FromTicks(TeamTime1Ticks.Value) : (TimeSpan?)null; }
            set { TeamTime1Ticks = value.HasValue ? value.Value.Ticks : (long?)null; }
        }

        public virtual Replay Replay { get; set; }
    }
}
