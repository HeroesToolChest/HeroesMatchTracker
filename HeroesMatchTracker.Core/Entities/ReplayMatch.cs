using Heroes.StormReplayParser.Replay;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace HeroesMatchTracker.Core.Entities
{
    [SuppressMessage("Usage", "CA2227:Collection properties should be read only", Justification = "ef entity")]
    [Table("Replays")]
    public class ReplayMatch
    {
        /// <summary>
        /// Gets or sets the unique id.
        /// </summary>
        [Key]
        public long ReplayId { get; set; }

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
        [StringLength(50)]
        public string MapName { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the map id.
        /// </summary>
        [StringLength(50)]
        public string? MapId { get; set; } = null;

        /// <summary>
        /// Gets or sets the full version number.
        /// </summary>
        [StringLength(20)]
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
        /// Gets or sets the filename of the replay.
        /// </summary>
        [StringLength(260)]
        public string FileName { get; set; } = string.Empty;

        [NotMapped]
        public string Result { get; set; } = string.Empty;

        public virtual ICollection<ReplayHotsApiUpload> ReplayHotsApiUploads { get; set; } = null!;

        public virtual ICollection<ReplayMatchAward> ReplayMatchAwards { get; set; } = null!;

        public virtual ICollection<ReplayMatchDraftPick> ReplayMatchDraftPicks { get; set; } = null!;

        public virtual ICollection<ReplayMatchMessage> ReplayMatchMessages { get; set; } = null!;

        public virtual ICollection<ReplayMatchPlayer> ReplayMatchPlayers { get; set; } = null!;

        public virtual ICollection<ReplayMatchPlayerScoreResult> ReplayMatchPlayerScoreResults { get; set; } = null!;

        public virtual ICollection<ReplayMatchPlayerTalent> ReplayMatchPlayerTalents { get; set; } = null!;

        public virtual ReplayMatchTeamBan ReplayMatchTeamBans { get; set; } = null!;

        public virtual ICollection<ReplayMatchTeamExperience> ReplayMatchTeamExperiences { get; set; } = null!;

        public virtual ICollection<ReplayMatchTeamLevel> ReplayMatchTeamLevels { get; set; } = null!;
    }
}
