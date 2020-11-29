using System.ComponentModel.DataAnnotations;

namespace HeroesMatchTracker.Shared.Entities
{
    public class ReplayMatchTeamBan : IEntity
    {
        /// <summary>
        /// Gets or sets the replay id (foreign key).
        /// </summary>
        [Key]
        public long ReplayId { get; set; }

        /// <summary>
        /// Gets or sets the first banned hero attribute id of team 0.
        /// </summary>
        [StringLength(50)]
        public string? Team0Ban0 { get; set; } = null;

        /// <summary>
        /// Gets or sets the second banned hero attribute id of team 0.
        /// </summary>
        [StringLength(50)]
        public string? Team0Ban1 { get; set; } = null;

        /// <summary>
        /// Gets or sets the third banned hero attribute id of team 0.
        /// </summary>
        [StringLength(50)]
        public string? Team0Ban2 { get; set; } = null;

        /// <summary>
        /// Gets or sets the first banned hero attribute id of team 1.
        /// </summary>
        [StringLength(50)]
        public string? Team1Ban0 { get; set; } = null;

        /// <summary>
        /// Gets or sets the second banned hero attribute id of team 1.
        /// </summary>
        [StringLength(50)]
        public string? Team1Ban1 { get; set; } = null;

        /// <summary>
        /// Gets or sets the third banned hero attribute id of team 1.
        /// </summary>
        [StringLength(50)]
        public string? Team1Ban2 { get; set; } = null;

        public virtual ReplayMatch Replay { get; set; } = null!;
    }
}
