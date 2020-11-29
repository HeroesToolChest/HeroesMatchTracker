using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HeroesMatchTracker.Shared.Entities
{
    public class ReplayMatchTeamLevel : IEntity
    {
        /// <summary>
        /// Gets or sets the unique id.
        /// </summary>
        [Key]
        public long MatchTeamLevelId { get; set; }

        /// <summary>
        /// Gets or sets the replay id (foreign key).
        /// </summary>
        public long ReplayId { get; set; }

        /// <summary>
        /// Gets or sets the team level.
        /// </summary>
        public int? Team0Level { get; set; }

        /// <summary>
        /// Gets or sets the time in ticks that the team leveled up. Use <see cref="TeamTime0"/> to get a <see cref="TimeSpan"/>.
        /// </summary>
        public long? TeamTime0Ticks { get; set; }

        /// <summary>
        /// Gets or sets the time that the team levelup up.
        /// </summary>
        [NotMapped]
        public TimeSpan? TeamTime0
        {
            get { return TeamTime0Ticks.HasValue ? TimeSpan.FromTicks(TeamTime0Ticks.Value) : (TimeSpan?)null; }
            set { TeamTime0Ticks = value.HasValue ? value.Value.Ticks : (long?)null; }
        }

        /// <summary>
        /// Gets or sets the team level.
        /// </summary>
        public int? Team1Level { get; set; }

        /// <summary>
        /// Gets or sets the time in ticks that the team leveled up. Use <see cref="TeamTime0"/> to get a <see cref="TimeSpan"/>.
        /// </summary>
        public long? TeamTime1Ticks { get; set; }

        /// <summary>
        /// Gets or sets the team level.
        /// </summary>
        [NotMapped]
        public TimeSpan? TeamTime1
        {
            get { return TeamTime1Ticks.HasValue ? TimeSpan.FromTicks(TeamTime1Ticks.Value) : (TimeSpan?)null; }
            set { TeamTime1Ticks = value.HasValue ? value.Value.Ticks : (long?)null; }
        }

        public virtual ReplayMatch Replay { get; set; } = null!;
    }
}
