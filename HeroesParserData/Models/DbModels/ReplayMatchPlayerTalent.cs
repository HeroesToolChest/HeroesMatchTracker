namespace HeroesParserData.Models.DbModels
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public partial class ReplayMatchPlayerTalent
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long MatchPlayerTalentId { get; set; }

        public long ReplayId { get; set; }

        public long PlayerId { get; set; }

        [StringLength(50)]
        public string Character { get; set; }

        public int? TalentId1 { get; set; }

        [StringLength(75)]
        public string TalentName1 { get; set; }

        public TimeSpan? TimeSpanSelected1 { get; set; }

        public int? TalentId4 { get; set; }

        [StringLength(75)]
        public string TalentName4 { get; set; }

        public TimeSpan? TimeSpanSelected4 { get; set; }

        public int? TalentId7 { get; set; }

        [StringLength(75)]
        public string TalentName7 { get; set; }

        public TimeSpan? TimeSpanSelected7 { get; set; }

        public int? TalentId10 { get; set; }

        [StringLength(75)]
        public string TalentName10 { get; set; }

        public TimeSpan? TimeSpanSelected10 { get; set; }

        public int? TalentId13 { get; set; }

        [StringLength(75)]
        public string TalentName13 { get; set; }

        public TimeSpan? TimeSpanSelected13 { get; set; }

        public int? TalentId16 { get; set; }

        [StringLength(75)]
        public string TalentName16 { get; set; }

        public TimeSpan? TimeSpanSelected16 { get; set; }

        public int? TalentId20 { get; set; }

        [StringLength(75)]
        public string TalentName20 { get; set; }

        public TimeSpan? TimeSpanSelected20 { get; set; }

        public virtual Replay Replay { get; set; }

        public virtual ReplayAllHotsPlayer ReplayAllHotsPlayer { get; set; }
    }
}
