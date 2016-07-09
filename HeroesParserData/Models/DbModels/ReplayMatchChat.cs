namespace HeroesParserData.Models.DbModels
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public partial class ReplayMatchChat
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long MatchChatId { get; set; }

        public long ReplayId { get; set; }

        public int PlayerNumber { get; set; }

        [StringLength(50)]
        public string MessageTarget { get; set; }

        [Column(TypeName = "text")]
        public string Message { get; set; }

        // has to be long, due to errors
        public long TimeStamp { get; set; }

        public virtual Replay Replay { get; set; }
    }
}