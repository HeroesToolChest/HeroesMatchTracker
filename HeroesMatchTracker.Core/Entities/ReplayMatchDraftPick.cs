using Heroes.StormReplayParser.Replay;
using System.ComponentModel.DataAnnotations;

namespace HeroesMatchTracker.Core.Entities
{
    public class ReplayMatchDraftPick : IEntity
    {
        /// <summary>
        /// Gets or sets the unqiue id.
        /// </summary>
        [Key]
        public long DraftPickId { get; set; }

        /// <summary>
        /// Gets or sets the replay id (foreign key).
        /// </summary>
        public long ReplayId { get; set; }

        /// <summary>
        /// Gets or sets the player id (foreign key).
        /// </summary>
        public long? PlayerId { get; set; }

        /// <summary>
        /// Gets or sets the selected hero (hero id).
        /// </summary>
        public string HeroSelected { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the type of pick.
        /// </summary>
        public StormDraftPickType PickType { get; set; }

        /// <summary>
        /// Gets or sets the player who performed the <see cref="PickType"/>.
        /// </summary>
        public StormTeam? Team { get; set; }

        public virtual ReplayMatch Replay { get; set; } = null!;

        public virtual ReplayPlayer? ReplayPlayer { get; set; } = null!;
    }
}
