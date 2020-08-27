using System.ComponentModel.DataAnnotations;

namespace HeroesMatchTracker.Core.Entities
{
    public class ReplayMatchPlayerLoadout
    {
        /// <summary>
        /// Gets or sets the unique id (foreign key).
        /// </summary>
        [Key]
        public long MatchPlayerId { get; set; }

        /// <summary>
        /// Gets or sets the skin id.
        /// </summary>
        [StringLength(50)]
        public string? SkinAndSkinTintId { get; set; } = null;

        /// <summary>
        /// Gets or sets the mount id.
        /// </summary>
        [StringLength(50)]
        public string? MountAndMountTintId { get; set; } = null;

        /// <summary>
        /// Gets or sets the banner id.
        /// </summary>
        [StringLength(50)]
        public string? BannerId { get; set; } = null;

        /// <summary>
        /// Gets or sets the spray id.
        /// </summary>
        [StringLength(50)]
        public string? SprayId { get; set; } = null;

        /// <summary>
        /// Gets or sets the announcer pack id.
        /// </summary>
        [StringLength(50)]
        public string? AnnouncerPackId { get; set; } = null;

        /// <summary>
        /// Gets or sets the voiceline id.
        /// </summary>
        [StringLength(50)]
        public string? VoiceLineId { get; set; } = null;

        public virtual ReplayMatchPlayer ReplayMatchPlayer { get; set; } = null!;
    }
}
