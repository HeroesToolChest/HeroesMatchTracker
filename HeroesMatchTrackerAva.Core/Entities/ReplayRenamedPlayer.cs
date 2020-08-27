using System;
using System.ComponentModel.DataAnnotations;

namespace HeroesMatchTracker.Core.Entities
{
    /// <summary>
    /// Contains information about an old player's battle tag.
    /// </summary>
    public class ReplayRenamedPlayer
    {
        /// <summary>
        /// Gets or sets the unique id.
        /// </summary>
        [Key]
        public long RenamedPlayerId { get; set; }

        /// <summary>
        /// Gets or sets the playerId (foreign key).
        /// </summary>
        public long PlayerId { get; set; }

        /// <summary>
        /// Gets or sets the old battle tag name of the player.
        /// </summary>
        [StringLength(50)]
        public string? BattleTagName { get; set; }

        /// <summary>
        /// Gets or sets the local data that this entry was added.
        /// </summary>
        public DateTime DateAdded { get; set; }

        public virtual ReplayPlayer ReplayPlayer { get; set; } = null!;
    }
}
