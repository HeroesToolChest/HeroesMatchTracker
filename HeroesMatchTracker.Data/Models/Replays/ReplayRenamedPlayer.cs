namespace HeroesMatchTracker.Data.Models.Replays
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class ReplayRenamedPlayer : IRawDataDisplay, INonContextModels
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long RenamedPlayerId { get; set; }

        public long PlayerId { get; set; }

        /// <summary>
        /// [PlayerName]#[Number]
        /// </summary>
        [StringLength(50)]
        public string BattleTagName { get; set; }

        /// <summary>
        /// Unique id associated with battle net account
        /// </summary>
        public int BattleNetId { get; set; }

        public int BattleNetRegionId { get; set; }

        public int BattleNetSubId { get; set; }

        public string BattleNetTId { get; set; }

        public DateTime DateAdded { get; set; }

        public virtual ReplayAllHotsPlayer ReplayAllHotsPlayer { get; set; }
    }
}
