namespace HeroesParserData.Models.DbModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public partial class ReplayAllHotsPlayer : IReplayDataTable
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public ReplayAllHotsPlayer()
        {
            ReplayMatchPlayers = new HashSet<ReplayMatchPlayer>();
            ReplayMatchPlayerScoreResults = new HashSet<ReplayMatchPlayerScoreResult>();
            ReplayMatchPlayerTalents = new HashSet<ReplayMatchPlayerTalent>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
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

        public DateTime LastSeen { get; set; }

        public int Seen { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ReplayMatchPlayer> ReplayMatchPlayers { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ReplayMatchPlayerScoreResult> ReplayMatchPlayerScoreResults { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ReplayMatchPlayerTalent> ReplayMatchPlayerTalents { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ReplayAllHotsPlayerHero> ReplayAllHotsPlayerHeroes { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ReplayMatchAward> ReplayMatchAwards { get; set; }
    }
}
