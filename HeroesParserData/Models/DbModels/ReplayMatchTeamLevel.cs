namespace HeroesParserData.Models.DbModels
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public partial class ReplayMatchTeamLevel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long MatchTeamLevelId { get; set; }

        public long ReplayId { get; set; }

        public int? Team0Level { get; set; }

        public long? Team0TimeTicks { get; set; }

        [NotMapped]
        public TimeSpan? Team0Time
        {
            get { return Team0TimeTicks.HasValue ? TimeSpan.FromTicks(Team0TimeTicks.Value) : (TimeSpan?)null; }
            set { Team0TimeTicks = value.HasValue ? value.Value.Ticks : (long?)null; }
        }

        public int? Team1Level { get; set; }

        public long? Team1TimeTicks { get; set; }

        [NotMapped]
        public TimeSpan? Team1Time
        {
            get { return Team1TimeTicks.HasValue ? TimeSpan.FromTicks(Team1TimeTicks.Value) : (TimeSpan?)null; }
            set { Team1TimeTicks = value.HasValue ? value.Value.Ticks : (long?)null; }
        }

        public virtual Replay Replay { get; set; }
    }
}
