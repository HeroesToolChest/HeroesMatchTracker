namespace HeroesMatchTracker.Data.Models.Replays
{
    using Heroes.ReplayParser;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class ReplayMatchDraftPick : IRawDataDisplay, INonContextModels
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long DraftPickId { get; set; }

        public long ReplayId { get; set; }

        public int PlayerSlotId { get; set; }

        public string HeroSelected { get; set; }

        public DraftPickType PickType { get; set; }

        public virtual ReplayMatch Replay { get; set; }
    }
}
