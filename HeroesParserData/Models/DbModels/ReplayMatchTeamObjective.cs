namespace HeroesParserData.Models.DbModels
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public partial class ReplayMatchTeamObjective
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long MatchTeamTeamObjectiveId { get; set; }

        public long ReplayId { get; set; }

        public int Team { get; set; }

        public long? PlayerId { get; set; }

        [StringLength(255)]
        public string TeamObjectiveType { get; set; }

        public TimeSpan? TimeStamp { get; set; }

        public int? Value { get; set; }

        public virtual Replay Replay { get; set; }

        public virtual ReplayAllHotsPlayer ReplayAllHotsPlayer { get; set; }
    }
}