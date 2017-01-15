namespace HeroesStatTracker.Data.Models.Replays
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class ReplayMatchAward : IRawDataDisplay, INonContextModels
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long MatchAwardId { get; set; }

        public long ReplayId { get; set; }

        public long PlayerId { get; set; }

        public string Award { get; set; }

        public virtual ReplayMatch Replay { get; set; }

        public virtual ReplayAllHotsPlayer ReplayAllHotsPlayer { get; set; }
    }
}
