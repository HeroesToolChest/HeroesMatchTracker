namespace HeroesParserData.Models.DbModels
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public partial class ReplayMatchTeamBan : IReplayDataTable
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

        public virtual Replay Replay { get; set; }
    }
}
