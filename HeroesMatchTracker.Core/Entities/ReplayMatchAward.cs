using System.ComponentModel.DataAnnotations;

namespace HeroesMatchTracker.Core.Entities
{
    public class ReplayMatchAward
    {
        /// <summary>
        /// Gets or sets the unique id.
        /// </summary>
        [Key]
        public long MatchAwardId { get; set; }

        /// <summary>
        /// Gets or sets the replay id (foreign key).
        /// </summary>
        public long ReplayId { get; set; }

        /// <summary>
        /// Gets or sets the player id (foreign key).
        /// </summary>
        public long PlayerId { get; set; }

        /// <summary>
        /// Gets or sets the award id.
        /// </summary>
        public string AwardId { get; set; } = string.Empty;

        public virtual ReplayMatch ReplayMatch { get; set; } = null!;

        public virtual ReplayPlayer ReplayPlayer { get; set; } = null!;
    }
}
