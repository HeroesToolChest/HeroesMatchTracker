using Heroes.StormReplayParser.Replay;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HeroesMatchTracker.Core.Entities
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
        public int? TeamLevel { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="StormTeam"/>.
        /// </summary>
        public StormTeam? Team { get; set; }

        /// <summary>
        /// Gets or sets the time in ticks that the team leveled up. Use <see cref="TeamTime"/> to get a <see cref="TimeSpan"/>.
        /// </summary>
        public long? TeamTimeTicks { get; set; }

        /// <summary>
        /// Gets or sets the time that the team leveled up.
        /// </summary>
        [NotMapped]
        public TimeSpan? TeamTime
        {
            get { return TeamTimeTicks.HasValue ? TimeSpan.FromTicks(TeamTimeTicks.Value) : (TimeSpan?)null; }
            set { TeamTimeTicks = value.HasValue ? value.Value.Ticks : (long?)null; }
        }

        public virtual ReplayMatch Replay { get; set; } = null!;
    }
}
