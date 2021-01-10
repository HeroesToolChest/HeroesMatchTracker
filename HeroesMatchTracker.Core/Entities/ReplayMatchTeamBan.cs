using Heroes.StormReplayParser.Replay;
using System.ComponentModel.DataAnnotations;

namespace HeroesMatchTracker.Core.Entities
{
    public class ReplayMatchTeamBan : IEntity
    {
        /// <summary>
        /// Gets or sets the unqiue id.
        /// </summary>
        [Key]
        public long TeamBanId { get; set; }

        /// <summary>
        /// Gets or sets the replay id (foreign key).
        /// </summary>
        public long ReplayId { get; set; }

        /// <summary>
        /// Gets or sets the hero attribute id of the banned hero.
        /// </summary>
        public string? TeamBan { get; set; } = null;

        /// <summary>
        /// Gets or sets the <see cref="StormTeam"/> of that performed the ban.
        /// </summary>
        public StormTeam? Team { get; set; } = null;

        /// <summary>
        /// Gets or sets the zero-index order of the ban pick of the team.
        /// </summary>
        public int Order { get; set; }

        public virtual ReplayMatch Replay { get; set; } = null!;
    }
}
