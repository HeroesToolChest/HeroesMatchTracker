namespace HeroesParserData.Models.DbModels
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public partial class ReplayAllHotsPlayerHero
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long PlayerHeroesId { get; set; }

        public long ReplayId { get; set; }

        public long PlayerId { get; set; }

        [StringLength(50)]
        public string HeroName { get; set; }
        
        public bool IsUsable { get; set; }

        public virtual Replay Replay { get; set; }

        public virtual ReplayAllHotsPlayer ReplayAllHotsPlayer { get; set; }
    }
}
