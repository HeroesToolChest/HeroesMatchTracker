namespace HeroesMatchTracker.Data.Models.Settings
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class UnparsedReplay
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int UnparsedReplaysId { get; set; }

        public DateTime TimeStamp { get; set; }

        public int Build { get; set; }

        public string Status { get; set; }

        public string FilePath { get; set; }
    }
}
