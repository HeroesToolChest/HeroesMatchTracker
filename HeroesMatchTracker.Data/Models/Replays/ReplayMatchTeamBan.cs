namespace HeroesMatchTracker.Data.Models.Replays
{
    using System.ComponentModel.DataAnnotations;

    public class ReplayMatchTeamBan : IRawDataDisplay, INonContextModels
    {
        [Key]
        public long ReplayId { get; set; }

        [StringLength(50)]
        public string Team0Ban0 { get; set; }

        [StringLength(50)]
        public string Team0Ban1 { get; set; }

        [StringLength(50)]
        public string Team0Ban2 { get; set; }

        [StringLength(50)]
        public string Team1Ban0 { get; set; }

        [StringLength(50)]
        public string Team1Ban1 { get; set; }

        [StringLength(50)]
        public string Team1Ban2 { get; set; }

        public virtual ReplayMatch Replay { get; set; }
    }
}
