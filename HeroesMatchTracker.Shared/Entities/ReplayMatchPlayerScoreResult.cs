using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HeroesMatchTracker.Shared.Entities
{
    public class ReplayMatchPlayerScoreResult : IEntity
    {
        /// <summary>
        /// Gets or sets the unique id (foreign key).
        /// </summary>
        [Key]
        public long MatchPlayerId { get; set; }

        /// <summary>
        /// Gets or sets the amount of solo kills.
        /// </summary>
        public int? SoloKills { get; set; }

        /// <summary>
        /// Gets or sets the amount of takedowns.
        /// </summary>
        public int? TakeDowns { get; set; }

        /// <summary>
        /// Gets or sets the amount of assists.
        /// </summary>
        public int? Assists { get; set; }

        /// <summary>
        /// Gets or sets the amount of deaths.
        /// </summary>
        public int? Deaths { get; set; }

        /// <summary>
        /// Gets or sets the total amount of damage dealt to minions, structures, and summons.
        /// </summary>
        public int? SiegeDamage { get; set; }

        /// <summary>
        /// Gets or sets the amount of damage dealt to defending mercenaries.
        /// </summary>
        public int? CreepDamage { get; set; }

        /// <summary>
        /// Gets or sets the amount of damage dealt to minions.
        /// </summary>
        public int? MinionDamage { get; set; }

        /// <summary>
        /// Gets or sets the amount of damage dealt to summon units.
        /// </summary>
        public int? SummonDamage { get; set; }

        /// <summary>
        /// Gets or sets the amount of damage dealt to structures.
        /// </summary>
        public int? StructureDamage { get; set; }

        /// <summary>
        /// Gets or sets the amount of damage dealt to heroes.
        /// </summary>
        public int? HeroDamage { get; set; }

        /// <summary>
        /// Gets or sets the amount of damage taken.
        /// </summary>
        public int? DamageTaken { get; set; }

        /// <summary>
        /// Gets or sets the amount of damage soaked.
        /// </summary>
        public int? DamageSoaked { get; set; }

        /// <summary>
        /// Gets or sets the amount of healing.
        /// </summary>
        public int? Healing { get; set; }

        /// <summary>
        /// Gets or sets the amount of healing done to self.
        /// </summary>
        public int? SelfHealing { get; set; }

        /// <summary>
        /// Gets or sets the amount of experience earned.
        /// </summary>
        public int? ExperienceContribution { get; set; }

        /// <summary>
        /// Gets or sets the amount experience that will be added to the players account and level.
        /// This is the total experience for the player's team.
        /// </summary>
        public int? MetaExperience { get; set; }

        /// <summary>
        /// Gets or sets the amount of times a mercenary camp was captured.
        /// </summary>
        public int? MercCampCaptures { get; set; }

        /// <summary>
        /// Gets or sets the amount of forts and keeps destroyed.
        /// </summary>
        public int? TownKills { get; set; }

        /// <summary>
        /// Gets or sets the amount of times a watch tower was captured.
        /// </summary>
        public int? WatchTowerCaptures { get; set; }

        /// <summary>
        /// Gets or sets the time spent dead in ticks. Use <see cref="TimeSpentDeadTicks"/> to get a <see cref="TimeSpan"/>.
        /// </summary>
        public long? TimeSpentDeadTicks { get; set; }

        /// <summary>
        /// Gets or sets the amount of time spent while being dead.
        /// </summary>
        [NotMapped]
        public TimeSpan? TimeSpentDead
        {
            get { return TimeSpentDeadTicks.HasValue ? TimeSpan.FromTicks(TimeSpentDeadTicks.Value) : (TimeSpan?)null; }
            set { TimeSpentDeadTicks = value.HasValue ? value.Value.Ticks : (long?)null; }
        }

        /// <summary>
        /// Gets or sets the amount of spell damage dealt.
        /// </summary>
        public int? SpellDamage { get; set; }

        /// <summary>
        /// Gets or sets the amount of physical damage dealt.
        /// </summary>
        public int? PhysicalDamage { get; set; }

        /// <summary>
        /// Gets or sets the total time on fire in ticks. Use <see cref="OnFireTimeonFire"/> to get a <see cref="TimeSpan"/>.
        /// </summary>
        public long? OnFireTimeonFireTicks { get; set; }

        /// <summary>
        /// Gets or sets the amount of time the player was on fire.
        /// </summary>
        [NotMapped]
        public TimeSpan? OnFireTimeonFire
        {
            get { return OnFireTimeonFireTicks.HasValue ? TimeSpan.FromTicks(OnFireTimeonFireTicks.Value) : (TimeSpan?)null; }
            set { OnFireTimeonFireTicks = value.HasValue ? value.Value.Ticks : (long?)null; }
        }

        /// <summary>
        /// Gets or sets the amount of minion kills.
        /// </summary>
        public int? MinionKills { get; set; }

        /// <summary>
        /// Gets or sets the amount of regeneration globes collected.
        /// </summary>
        public int? RegenGlobes { get; set; }

        /// <summary>
        /// Gets or sets the amount of the highest streak of kills.
        /// </summary>
        public int? HighestKillStreak { get; set; }

        /// <summary>
        /// Gets or sets the amount of healing given to allies.
        /// </summary>
        public int? ProtectionGivenToAllies { get; set; }

        /// <summary>
        /// Gets or sets the total amount of time CC'ing heroes. Use <see cref="TimeCCdEnemyHeroes"/> to get a <see cref="TimeSpan"/>.
        /// </summary>
        public long? TimeCCdEnemyHeroesTicks { get; set; }

        /// <summary>
        /// Gets or sets the amount of time CC'ing heroes.
        /// </summary>
        [NotMapped]
        public TimeSpan? TimeCCdEnemyHeroes
        {
            get { return TimeCCdEnemyHeroesTicks.HasValue ? TimeSpan.FromTicks(TimeCCdEnemyHeroesTicks.Value) : (TimeSpan?)null; }
            set { TimeCCdEnemyHeroesTicks = value.HasValue ? value.Value.Ticks : (long?)null; }
        }

        /// <summary>
        /// Gets or sets the amount of ticks rooting enemy heroes. Use <see cref="TimeRootingEnemyHeroes"/> to get a <see cref="TimeSpan"/>.
        /// </summary>
        public long? TimeRootingEnemyHeroesTicks { get; set; }

        /// <summary>
        /// Gets or sets the amount of time rooting enemey heroes.
        /// </summary>
        [NotMapped]
        public TimeSpan? TimeRootingEnemyHeroes
        {
            get { return TimeRootingEnemyHeroesTicks.HasValue ? TimeSpan.FromTicks(TimeRootingEnemyHeroesTicks.Value) : (TimeSpan?)null; }
            set { TimeRootingEnemyHeroesTicks = value.HasValue ? value.Value.Ticks : (long?)null; }
        }

        /// <summary>
        /// Gets or sets the amount of ticks stunning enemy heroes. Use <see cref="TimeStunningEnemyHeroes"/> to get a <see cref="TimeSpan"/>.
        /// </summary>
        public long? TimeStunningEnemyHeroesTicks { get; set; }

        /// <summary>
        /// Gets or sets the amount of time stunning enemy heroes.
        /// </summary>
        [NotMapped]
        public TimeSpan? TimeStunningEnemyHeroes { get; set; }

        /// <summary>
        /// Gets or sets the amount of clutch heals performed.
        /// </summary>
        public int? ClutchHealsPerformed { get; set; }

        /// <summary>
        /// Gets or sets the amount of times escapes were performed.
        /// </summary>
        public int? EscapesPerformed { get; set; }

        /// <summary>
        /// Gets or sets the amount of times a vengeance was performed.
        /// </summary>
        public int? VengeancesPerformed { get; set; }

        /// <summary>
        /// Gets or sets the amount times of outnumbered deaths.
        /// </summary>
        public int? OutnumberedDeaths { get; set; }

        /// <summary>
        /// Gets or sets the amount of team fight escapes performed.
        /// </summary>
        public int? TeamfightEscapesPerformed { get; set; }

        /// <summary>
        /// Gets or sets the amount of team fight healing.
        /// </summary>
        public int? TeamfightHealingDone { get; set; }

        /// <summary>
        /// Gets or sets the amount of team fight damage taken.
        /// </summary>
        public int? TeamfightDamageTaken { get; set; }

        /// <summary>
        /// Gets or sets the amount of team fight hero damage.
        /// </summary>
        public int? TeamfightHeroDamage { get; set; }

        /// <summary>
        /// Gets or sets the amount of multi-kills performs.
        /// </summary>
        public int? Multikill { get; set; }

        public virtual ReplayMatchPlayer ReplayMatchPlayer { get; set; } = null!;
    }
}
