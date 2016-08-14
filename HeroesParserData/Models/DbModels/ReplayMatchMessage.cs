namespace HeroesParserData.Models.DbModels
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public partial class ReplayMatchMessage
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long MessageId { get; set; }

        public long ReplayId { get; set; }

        public string MessageEventType { get; set; }

        public long TimeStampTicks { get; set; }

        [NotMapped]
        public TimeSpan TimeStamp
        {
            get { return TimeSpan.FromTicks(TimeStampTicks); }
            set { TimeStampTicks = value.Ticks; }
        }

        [StringLength(20)]
        public string MessageTarget { get; set; }

        [StringLength(20)]
        public string PlayerName { get; set; }

        public string CharacterName { get; set; }

        public string Message { get; set; }

        public virtual Replay Replay { get; set; }
    }
}