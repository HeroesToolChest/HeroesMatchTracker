using Heroes.StormReplayParser.MessageEvent;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HeroesMatchTracker.Core.Entities
{
    public class ReplayMatchMessage : IEntity
    {
        /// <summary>
        /// Gets or sets the unique id.
        /// </summary>
        [Key]
        public long MessageId { get; set; }

        /// <summary>
        /// Gets or sets the replay id (foreign key).
        /// </summary>
        public long ReplayId { get; set; }

        /// <summary>
        /// Gets or sets the match player id (foreign key).
        /// </summary>
        public long? MatchPlayerId { get; set; }

        /// <summary>
        /// Gets or setst the message event type.
        /// </summary>
        public StormMessageEventType MessageEventType { get; set; }

        /// <summary>
        /// Gets or sets the time the message took place in ticks. Use <see cref="TimeStamp"/> to get a <see cref="TimeSpan"/>.
        /// </summary>
        public long TimeStampTicks { get; set; }

        /// <summary>
        /// Gets or sets the time the message took place.
        /// </summary>
        [NotMapped]
        public TimeSpan TimeStamp
        {
            get { return TimeSpan.FromTicks(TimeStampTicks); }
            set { TimeStampTicks = value.Ticks; }
        }

        /// <summary>
        /// Gets or sets the target of the message.
        /// </summary>
        public StormMessageTarget MessageTarget { get; set; }

        /// <summary>
        /// Gets or sets the chat message.
        /// </summary>
        public string? Message { get; set; } = string.Empty;

        public virtual ReplayMatch Replay { get; set; } = null!;

        public virtual ReplayMatchPlayer? ReplayMatchPlayer { get; set; } = null!;
    }
}
