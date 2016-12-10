namespace HeroesParserData.Models.DbModels
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public partial class ReplayMatchPlayerScoreResult : IReplayDataTable
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long MatchPlayerScoreResultId { get; set; }

        public long ReplayId { get; set; }

        public long PlayerId { get; set; }

        public int? SoloKills { get; set; }

        public int? TakeDowns { get; set; }

        public int? Assists { get; set; }

        public int? Deaths { get; set; }

        public int? SiegeDamage { get; set; }

        public int? CreepDamage { get; set; }

        public int? MinionDamage { get; set; }

        public int? SummonDamage { get; set; }

        public int? StructureDamage { get; set; }

        public int? HeroDamage { get; set; }

        public int? DamageTaken { get; set; }

        public int? Healing { get; set; }

        public int? SelfHealing { get; set; }

        public int? ExperienceContribution { get; set; }

        public int? MetaExperience { get; set; }

        public int? MercCampCaptures { get; set; }

        public int? TownKills { get; set; }

        public int? WatchTowerCaptures { get; set; }

        public long? TimeCCdEnemyHeroes { get; set; }

        public long? TimeSpentDeadTicks { get; set; }

        [NotMapped]
        public TimeSpan? TimeSpentDead
        {
            get { return TimeSpentDeadTicks.HasValue? TimeSpan.FromTicks(TimeSpentDeadTicks.Value) : (TimeSpan?)null; }
            set { TimeSpentDeadTicks = value.HasValue? value.Value.Ticks : (long?)null; }
        }

        public virtual Replay Replay { get; set; }

        public virtual ReplayAllHotsPlayer ReplayAllHotsPlayer { get; set; }
    }
}
