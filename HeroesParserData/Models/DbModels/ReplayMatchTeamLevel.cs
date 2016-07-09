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

        public TimeSpan? Team0Time { get; set; }

        public int? Team1Level { get; set; }

        public TimeSpan? Team1Time { get; set; }

        public virtual Replay Replay { get; set; }
    }
}
