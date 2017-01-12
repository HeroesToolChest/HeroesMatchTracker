namespace HeroesStatTracker.Data.Replays.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class ReplayAllHotsPlayerHero : IReplayModelDataTable
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long PlayerHeroesId { get; set; }

        public long PlayerId { get; set; }

        [StringLength(50)]
        public string HeroName { get; set; }
        
        public bool IsUsable { get; set; }

        public DateTime LastUpdated { get; set; }

        public virtual ReplayAllHotsPlayer ReplayAllHotsPlayer { get; set; }
    }
}
