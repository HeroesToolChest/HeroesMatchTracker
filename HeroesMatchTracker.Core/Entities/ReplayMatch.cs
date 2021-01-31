using Heroes.StormReplayParser.Player;
using Heroes.StormReplayParser.Replay;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HeroesMatchTracker.Core.Entities
{
    [Table("Replays")]
    public class ReplayMatch : IEntity
    {
        /// <summary>
        /// Gets or sets the unique id.
        /// </summary>
        [Key]
        public long ReplayId { get; set; }

        /// <summary>
        /// Gets or sets the player id of the owner of this replay (foreign key).
        /// </summary>
        public long? OwnerPlayerId { get; set; }

        /// <summary>
        /// Gets or sets the random value associated with the replay.
        /// </summary>
        public long? RandomValue { get; set; }

        /// <summary>
        /// Gets or sets the custom generated hash.
        /// </summary>
        public string Hash { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the name of the map (localized).
        /// </summary>
        public string MapName { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the map id.
        /// </summary>
        public string? MapId { get; set; } = null;

        /// <summary>
        /// Gets or sets the full version number.
        /// </summary>
        public string ReplayVersion { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the replay length in ticks. Use <see cref="ReplayLength"/> to get a <see cref="TimeSpan"/>.
        /// </summary>
        public long ReplayLengthTicks { get; set; }

        /// <summary>
        /// Gets or sets the replay length.
        /// </summary>
        [NotMapped]
        public TimeSpan ReplayLength
        {
            get { return TimeSpan.FromTicks(ReplayLengthTicks); }
            set { ReplayLengthTicks = value.Ticks; }
        }

        /// <summary>
        /// Gets or sets the <see cref="Heroes.StormReplayParser.Replay.GameMode"/> type.
        /// </summary>
        public StormGameMode GameMode { get; set; }

        /// <summary>
        /// Gets or sets the time stamp of the replay (UTC).
        /// </summary>
        public DateTime? TimeStamp { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the replay has AI players.
        /// </summary>
        public bool HasAI { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the replay has observers.
        /// </summary>
        public bool HasObservers { get; set; }

        /// <summary>
        /// Gets or sets the region of the replay.
        /// </summary>
        public StormRegion Region { get; set; }

        /// <summary>
        /// Gets or sets the winning team.
        /// </summary>
        public StormTeam WinningTeam { get; set; }

        /// <summary>
        /// Gets or sets the file path to the physical replay file.
        /// </summary>
        public string? ReplayFilePath { get; set; }

        [NotMapped]
        public string Result { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the the owner of this replay file.
        /// </summary>
        public virtual ReplayPlayer? OwnerReplayPlayer { get; set; }

        public virtual ICollection<ServerReplayUpload>? ServerReplayUploads { get; set; }

        public virtual ICollection<ReplayMatchPlayer>? ReplayMatchPlayers { get; set; }

        public virtual ICollection<ReplayMatchTeamBan>? ReplayMatchTeamBans { get; set; }

        public virtual ICollection<ReplayMatchDraftPick>? ReplayMatchDraftPicks { get; set; }

        public virtual ICollection<ReplayMatchTeamLevel>? ReplayMatchTeamLevels { get; set; }

        public virtual ICollection<ReplayMatchTeamExperience>? ReplayMatchTeamExperiences { get; set; }

        public virtual ICollection<ReplayMatchMessage>? ReplayMatchMessages { get; set; } = null!;
    }
}
