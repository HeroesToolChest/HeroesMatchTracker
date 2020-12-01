using System;
using System.ComponentModel.DataAnnotations;

namespace HeroesMatchTracker.Shared.Entities
{
    /// <summary>
    /// Contains information for an old player's battle tag.
    /// </summary>
    public class ReplayOldPlayerInfo : IEntity
    {
        /// <summary>
        /// Gets or sets the unique id.
        /// </summary>
        [Key]
        public long ReplayOldPlayerInfoId { get; set; }

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
        /// Gets or sets the datetime in UTC of the replay that this entry was added.
        /// </summary>
        /// <remarks>
        /// This is the earliest we have found this entry. This date will be updated to an earlier
        /// time if an older replay is found.
        /// </remarks>
        public DateTime DateAdded { get; set; }

        public virtual ReplayPlayer? ReplayPlayer { get; set; }
    }
}
