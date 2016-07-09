namespace HeroesParserData.Models.DbModels
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public partial class ReplayMatchTeamExperience
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long MatchTeamExperienceId { get; set; }

        public long ReplayId { get; set; }

        public TimeSpan? Time { get; set; }

        public int? Team0CreepXP { get; set; }

        public int? Team0HeroXP { get; set; }

        public int? Team0MinionXP { get; set; }

        public int? Team0StructureXP { get; set; }

        public int? Team0TeamLevel { get; set; }

        public int? Team0TrickleXP { get; set; }

        public int? Team1CreepXP { get; set; }

        public int? Team1HeroXP { get; set; }

        public int? Team1MinionXP { get; set; }

        public int? Team1StructureXP { get; set; }

        public int? Team1TeamLevel { get; set; }

        public int? Team1TrickleXP { get; set; }

        public virtual Replay Replay { get; set; }
    }
}
