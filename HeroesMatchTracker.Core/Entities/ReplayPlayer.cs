using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace HeroesMatchTracker.Core.Entities
{
    [SuppressMessage("Usage", "CA2227:Collection properties should be read only", Justification = "ef entity")]
    /// <summary>
    /// Contains information about all of the Heroes of the Storm players.
    /// </summary>
    public class ReplayPlayer
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
        /// Gets or sets the id name of the <see cref="BattleTagName"/>.
        /// </summary>
        [StringLength(50)]
        public string? BattleTagId { get; set; }

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
        public int Seen { get; set; }

        /// <summary>
        /// Gets or sets the custom user notes associated with this player.
        /// </summary>
        public string? Notes { get; set; }

        public virtual ReplayPlayerToon ReplayPlayerToon { get; set; } = null!;

        public virtual ReplayRenamedPlayer ReplayRenamedPlayer { get; set; } = null!;

        public virtual ICollection<ReplayMatchPlayer> ReplayMatchPlayers { get; set; } = null!;

        public virtual ICollection<ReplayMatchPlayerScoreResult> ReplayMatchPlayerScoreResults { get; } = null!;

        public virtual ICollection<ReplayMatchPlayerTalent> ReplayMatchPlayerTalents { get; set; } = null!;

        public virtual ICollection<ReplayMatchAward> ReplayMatchAwards { get; set; } = null!;
    }
}
