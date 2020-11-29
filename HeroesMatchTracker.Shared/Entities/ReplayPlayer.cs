using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace HeroesMatchTracker.Shared.Entities
{
    /// <summary>
    /// Contains information for a Heroes of the Storm player.
    /// </summary>
    public class ReplayPlayer : IEntity
    {
        /// <summary>
        /// Gets or sets the unique id.
        /// </summary>
        [Key]
        public long PlayerId { get; set; }

        /// <summary>
        /// Gets or sets the player's battle tag name. Contains the # followed by numbers.
        /// </summary>
        [StringLength(50)]
        public string? BattleTagName { get; set; }

        /// <summary>
        /// Gets or sets the id name. This is usally in the format of T:XXXXXXXX#XXX.
        /// </summary>
        [StringLength(50)]
        public string? ShortcutId { get; set; }

        /// <summary>
        /// Gets or sets the player's account level.
        /// </summary>
        public int AccountLevel { get; set; }

        /// <summary>
        /// Gets or sets the local datetime of the last time this player was seen.
        /// </summary>
        public DateTime LastSeen { get; set; }

        /// <summary>
        /// Gets or sets the local datetime that the player was seen before <see cref="LastSeen"/>.
        /// </summary>
        public DateTime? LastSeenBefore { get; set; }

        /// <summary>
        /// Gets or sets the number of times this player was seen.
        /// </summary>
        /// <remarks>This is the number of times this player is found in the database.</remarks>
        public int Seen { get; set; }

        /// <summary>
        /// Gets or sets the custom user notes associated with this player.
        /// </summary>
        public string? Notes { get; set; }

        public virtual ReplayPlayerToon? ReplayPlayerToon { get; set; }

        public virtual ICollection<ReplayOldPlayerInfo>? ReplayOldPlayerInfos { get; set; }

        public virtual ICollection<ReplayMatchPlayer>? ReplayMatchPlayers { get; set; }

       // public virtual ICollection<ReplayMatchPlayerScoreResult> ReplayMatchPlayerScoreResults { get; } = null!;

        //public virtual ICollection<ReplayMatchPlayerTalent> ReplayMatchPlayerTalents { get; set; } = null!;

       // public virtual ICollection<ReplayMatchAward> ReplayMatchAwards { get; set; } = null!;
    }
}
