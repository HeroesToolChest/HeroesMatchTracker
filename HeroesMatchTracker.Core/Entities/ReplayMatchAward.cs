using System.ComponentModel.DataAnnotations;

namespace HeroesMatchTracker.Core.Entities
{
    public class ReplayMatchAward : IEntity
    {
        [Key]
        public long ReplayMatchAwardId { get; set; }

        /// <summary>
        /// Gets or sets the unique id (foreign key).
        /// </summary>
        public long MatchPlayerId { get; set; }

        /// <summary>
        /// Gets or sets the award id.
        /// </summary>
        public string AwardId { get; set; } = string.Empty;

        public virtual ReplayMatchPlayer ReplayMatchPlayer { get; set; } = null!;
    }
}
