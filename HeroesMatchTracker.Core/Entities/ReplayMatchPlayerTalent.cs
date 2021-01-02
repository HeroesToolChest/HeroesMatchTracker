using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HeroesMatchTracker.Core.Entities
{
    public class ReplayMatchPlayerTalent : IEntity
    {
        /// <summary>
        /// Gets or sets the unique id (foreign key).
        /// </summary>
        [Key]
        public long MatchPlayerId { get; set; }

        /// <summary>
        /// Gets or sets the level 1 talent id.
        /// </summary>
        [StringLength(100)]
        public string? TalentId1 { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the time in ticks which the talent 1 was selected. Use <see cref="TimeSpanSelected1"/> to get a <see cref="TimeSpan"/>.
        /// </summary>
        public long? TimeSpanSelected1Ticks { get; set; }

        /// <summary>
        /// Gets or sets the time when talent 1 was selected.
        /// </summary>
        [NotMapped]
        public TimeSpan? TimeSpanSelected1
        {
            get { return TimeSpanSelected1Ticks.HasValue ? TimeSpan.FromTicks(TimeSpanSelected1Ticks.Value) : (TimeSpan?)null; }
            set { TimeSpanSelected1Ticks = value.HasValue ? value.Value.Ticks : (long?)null; }
        }

        /// <summary>
        /// Gets or sets the level 4 talent id.
        /// </summary>
        [StringLength(100)]
        public string? TalentId4 { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the time in ticks which the talent 4 was selected. Use <see cref="TimeSpanSelected4"/> to get a <see cref="TimeSpan"/>.
        /// </summary>
        public long? TimeSpanSelected4Ticks { get; set; }

        /// <summary>
        /// Gets or sets the time when talent 4 was selected.
        /// </summary>
        [NotMapped]
        public TimeSpan? TimeSpanSelected4
        {
            get { return TimeSpanSelected4Ticks.HasValue ? TimeSpan.FromTicks(TimeSpanSelected4Ticks.Value) : (TimeSpan?)null; }
            set { TimeSpanSelected4Ticks = value.HasValue ? value.Value.Ticks : (long?)null; }
        }

        /// <summary>
        /// Gets or sets the level 7 talent id.
        /// </summary>
        [StringLength(100)]
        public string? TalentId7 { get; set; }

        /// <summary>
        /// Gets or sets the time in ticks which the talent 7 was selected. Use <see cref="TimeSpanSelected7"/> to get a <see cref="TimeSpan"/>.
        /// </summary>
        public long? TimeSpanSelected7Ticks { get; set; }

        /// <summary>
        /// Gets or sets the time when talent 7 was selected.
        /// </summary>
        [NotMapped]
        public TimeSpan? TimeSpanSelected7
        {
            get { return TimeSpanSelected7Ticks.HasValue ? TimeSpan.FromTicks(TimeSpanSelected7Ticks.Value) : (TimeSpan?)null; }
            set { TimeSpanSelected7Ticks = value.HasValue ? value.Value.Ticks : (long?)null; }
        }

        /// <summary>
        /// Gets or sets the level 10 talent id.
        /// </summary>
        [StringLength(100)]
        public string? TalentId10 { get; set; }

        /// <summary>
        /// Gets or sets the time in ticks which the talent 10 was selected. Use <see cref="TimeSpanSelected10"/> to get a <see cref="TimeSpan"/>.
        /// </summary>
        public long? TimeSpanSelected10Ticks { get; set; }

        /// <summary>
        /// Gets or sets the time when talent 10 was selected.
        /// </summary>
        [NotMapped]
        public TimeSpan? TimeSpanSelected10
        {
            get { return TimeSpanSelected10Ticks.HasValue ? TimeSpan.FromTicks(TimeSpanSelected10Ticks.Value) : (TimeSpan?)null; }
            set { TimeSpanSelected10Ticks = value.HasValue ? value.Value.Ticks : (long?)null; }
        }

        /// <summary>
        /// Gets or sets the level 13 talent id.
        /// </summary>
        [StringLength(100)]
        public string? TalentId13 { get; set; }

        /// <summary>
        /// Gets or sets the time in ticks which the talent 13 was selected. Use <see cref="TimeSpanSelected13"/> to get a <see cref="TimeSpan"/>.
        /// </summary>
        public long? TimeSpanSelected13Ticks { get; set; }

        /// <summary>
        /// Gets or sets the time when talent 13 was selected.
        /// </summary>
        [NotMapped]
        public TimeSpan? TimeSpanSelected13
        {
            get { return TimeSpanSelected13Ticks.HasValue ? TimeSpan.FromTicks(TimeSpanSelected13Ticks.Value) : (TimeSpan?)null; }
            set { TimeSpanSelected13Ticks = value.HasValue ? value.Value.Ticks : (long?)null; }
        }

        /// <summary>
        /// Gets or sets the level 16 talent id.
        /// </summary>
        [StringLength(100)]
        public string? TalentId16 { get; set; }

        /// <summary>
        /// Gets or sets the time in ticks which the talent 16 was selected. Use <see cref="TimeSpanSelected16"/> to get a <see cref="TimeSpan"/>.
        /// </summary>
        public long? TimeSpanSelected16Ticks { get; set; }

        /// <summary>
        /// Gets or sets the time when talent 16 was selected.
        /// </summary>
        [NotMapped]
        public TimeSpan? TimeSpanSelected16
        {
            get { return TimeSpanSelected16Ticks.HasValue ? TimeSpan.FromTicks(TimeSpanSelected16Ticks.Value) : (TimeSpan?)null; }
            set { TimeSpanSelected16Ticks = value.HasValue ? value.Value.Ticks : (long?)null; }
        }

        /// <summary>
        /// Gets or sets the level 20 talent id.
        /// </summary>
        [StringLength(100)]
        public string? TalentId20 { get; set; }

        /// <summary>
        /// Gets or sets the time in ticks which the talent 20 was selected. Use <see cref="TimeSpanSelected20"/> to get a <see cref="TimeSpan"/>.
        /// </summary>
        public long? TimeSpanSelected20Ticks { get; set; }

        /// <summary>
        /// Gets or sets the time when talent 20 was selected.
        /// </summary>
        [NotMapped]
        public TimeSpan? TimeSpanSelected20
        {
            get { return TimeSpanSelected20Ticks.HasValue ? TimeSpan.FromTicks(TimeSpanSelected20Ticks.Value) : (TimeSpan?)null; }
            set { TimeSpanSelected20Ticks = value.HasValue ? value.Value.Ticks : (long?)null; }
        }

        public virtual ReplayMatchPlayer? ReplayMatchPlayer { get; set; }
    }
}
