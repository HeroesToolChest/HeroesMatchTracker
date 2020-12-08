using System.ComponentModel.DataAnnotations;

namespace HeroesMatchTracker.Shared.Entities
{
    public class ReplayMatchPlayerLoadout : IEntity
    {
        /// <summary>
        /// Gets or sets the unique id (foreign key).
        /// </summary>
        [Key]
        public long MatchPlayerId { get; set; }

        /// <summary>
        /// Gets or sets the skin id.
        /// </summary>
        public string? SkinAndSkinTintId { get; set; } = null;

        /// <summary>
        /// Gets or sets the skin attribute id.
        /// </summary>
        public string? SkinAndSkinTintAttributeId { get; set; } = null;

        /// <summary>
        /// Gets or sets the mount id.
        /// </summary>
        public string? MountAndMountTintId { get; set; } = null;

        /// <summary>
        /// Gets or sets the mount attribute id.
        /// </summary>
        public string? MountAndMountTintAttributeId { get; set; } = null;

        /// <summary>
        /// Gets or sets the banner id.
        /// </summary>
        public string? BannerId { get; set; } = null;

        /// <summary>
        /// Gets or sets the banner attribute id.
        /// </summary>
        public string? BannerAttributeId { get; set; } = null;

        /// <summary>
        /// Gets or sets the spray id.
        /// </summary>
        public string? SprayId { get; set; } = null;

        /// <summary>
        /// Gets or sets the spray attribute id.
        /// </summary>
        public string? SprayAttributeId { get; set; } = null;

        /// <summary>
        /// Gets or sets the announcer pack id.
        /// </summary>
        public string? AnnouncerPackId { get; set; } = null;

        /// <summary>
        /// Gets or sets the announcer pack attribute id.
        /// </summary>
        public string? AnnouncerPackAttributeId { get; set; } = null;

        /// <summary>
        /// Gets or sets the voiceline id.
        /// </summary>
        public string? VoiceLineId { get; set; } = null;

        /// <summary>
        /// Gets or sets the voiceline attribute id.
        /// </summary>
        public string? VoiceLineAttributeId { get; set; } = null;

        public virtual ReplayMatchPlayer? ReplayMatchPlayer { get; set; }
    }
}
