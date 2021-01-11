using Heroes.StormReplayParser.Replay;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HeroesMatchTracker.Core.Entities
{
    public class ReplayMatchTeamExperience : IEntity
    {
        /// <summary>
        /// Gets or sets the unique id.
        /// </summary>
        [Key]
        public long MatchTeamExperienceId { get; set; }

        /// <summary>
        /// Gets or sets the replay id (foreign key).
        /// </summary>
        public long ReplayId { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="StormTeam"/>.
        /// </summary>
        public StormTeam Team { get; set; }

        /// <summary>
        /// Gets or sets the time in ticks that this experience breakdown took place. Use <see cref="Time"/> to get a <see cref="TimeSpan"/>.
        /// </summary>
        public long? TimeTicks { get; set; }

        /// <summary>
        /// Gets or sets the time that this experience breakdown took place.
        /// </summary>
        [NotMapped]
        public TimeSpan? Time
        {
            get { return TimeTicks.HasValue ? TimeSpan.FromTicks(TimeTicks.Value) : (TimeSpan?)null; }
            set { TimeTicks = value.HasValue ? value.Value.Ticks : (long?)null; }
        }

        /// <summary>
        /// Gets or sets the experience earned from defending mercenaries.
        /// </summary>
        public int? TeamCreepXP { get; set; }

        /// <summary>
        /// Gets or sets the experience earned from heroes.
        /// </summary>
        public int? TeamHeroXP { get; set; }

        /// <summary>
        /// Gets or sets the experience earned from minions.
        /// </summary>
        public int? TeamMinionXP { get; set; }

        /// <summary>
        /// Gets or sets the experience earned from structures.
        /// </summary>
        public int? TeamStructureXP { get; set; }

        /// <summary>
        /// Gets or sets the level of the team.
        /// </summary>
        public int? TeamTeamLevel { get; set; }

        /// <summary>
        /// Gets or sets the passive experience gain.
        /// </summary>
        public int? TeamPassiveXP { get; set; }

        public virtual ReplayMatch Replay { get; set; } = null!;
    }
}
