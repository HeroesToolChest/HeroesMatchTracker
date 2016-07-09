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

        public int? TalentId2 { get; set; }

        [StringLength(75)]
        public string TalentName2 { get; set; }

        public TimeSpan? TimeSpanSelected2 { get; set; }

        public int? TalentId3 { get; set; }

        [StringLength(75)]
        public string TalentName3 { get; set; }

        public TimeSpan? TimeSpanSelected3 { get; set; }

        public int? TalentId4 { get; set; }

        [StringLength(75)]
        public string TalentName4 { get; set; }

        public TimeSpan? TimeSpanSelected4 { get; set; }

        public int? TalentId5 { get; set; }

        [StringLength(75)]
        public string TalentName5 { get; set; }

        public TimeSpan? TimeSpanSelected5 { get; set; }

        public int? TalentId6 { get; set; }

        [StringLength(75)]
        public string TalentName6 { get; set; }

        public TimeSpan? TimeSpanSelected6 { get; set; }

        public int? TalentId7 { get; set; }

        [StringLength(75)]
        public string TalentName7 { get; set; }

        public TimeSpan? TimeSpanSelected7 { get; set; }

        public virtual Replay Replay { get; set; }

        public virtual ReplayAllHotsPlayer ReplayAllHotsPlayer { get; set; }
    }
}
