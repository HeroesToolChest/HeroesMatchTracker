using System.ComponentModel.DataAnnotations;

namespace HeroesMatchTracker.Core.Entities
{
    /// <summary>
    /// Contains information about all the players' toon handles.
    /// </summary>
    public class ReplayPlayerToon : IEntity
    {
        /// <summary>
        /// Gets or sets the unique id (foreign key).
        /// </summary>
        [Key]
        public long PlayerId { get; set; }

        /// <summary>
        /// Gets or sets the region value.
        /// </summary>
        public int Region { get; set; }

        /// <summary>
        /// Gets or sets the program id. This id is the same for all player's in this replay.
        /// </summary>
        [StringLength(50)]
        public long ProgramId { get; set; }

        /// <summary>
        /// Gets or sets the realm value.
        /// </summary>
        public int Realm { get; set; }

        /// <summary>
        /// Gets or sets the id unique to the player's account in this region.
        /// </summary>
        public long Id { get; set; }

        public virtual ReplayPlayer? ReplayPlayer { get; set; }
    }
}
