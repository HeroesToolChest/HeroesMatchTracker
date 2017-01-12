namespace HeroesStatTracker.Data.Replays.Models
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class ReplayMatchTeamBan : IReplayModelDataTable
    {
        [Key]
        public long ReplayId { get; set; }

        [StringLength(50)]
        public string Team0Ban0 { get; set; }

        [StringLength(50)]
        public string Team0Ban1 { get; set; }

        [StringLength(50)]
        public string Team1Ban0 { get; set; }

        [StringLength(50)]
        public string Team1Ban1 { get; set; }

        public virtual ReplayMatch Replay { get; set; }
    }
}
