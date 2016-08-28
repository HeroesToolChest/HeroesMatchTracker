namespace HeroesParserData.Models.DbModels
{
    using Heroes.ReplayParser;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public partial class Replay
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Replay()
        {
            ReplayMatchTeamExperiences = new HashSet<ReplayMatchTeamExperience>();
            ReplayMatchPlayers = new HashSet<ReplayMatchPlayer>();
            ReplayMatchPlayerScoreResults = new HashSet<ReplayMatchPlayerScoreResult>();
            ReplayMatchPlayerTalents = new HashSet<ReplayMatchPlayerTalent>();
            ReplayMatchTeamLevels = new HashSet<ReplayMatchTeamLevel>();
            ReplayMatchTeamObjectives = new HashSet<ReplayMatchTeamObjective>();
            ReplayMatchMessage = new HashSet<ReplayMatchMessage>();
            ReplayAllHotsPlayerHeroes = new HashSet<ReplayAllHotsPlayerHero>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long ReplayId { get; set; }

        public long? RandomValue { get; set; }

        [StringLength(50)]
        public string MapName { get; set; }

        public int? ReplayBuild { get; set; }

        [StringLength(20)]
        public string ReplayVersion { get; set; }

        public long ReplayLengthTicks { get; set; }

        [NotMapped]
        public TimeSpan ReplayLength
        {
            get { return TimeSpan.FromTicks(ReplayLengthTicks); }
            set { ReplayLengthTicks = value.Ticks; }
        }

        public GameMode GameMode { get; set; }

        [StringLength(50)]
        public string GameSpeed { get; set; }

        public bool IsGameEventsParsed { get; set; }

        public int? Frames { get; set; }

        [StringLength(10)]
        public string TeamSize { get; set; }

        public DateTime? TimeStamp { get; set; }

        [StringLength(260)]
        public string FileName { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ReplayMatchTeamExperience> ReplayMatchTeamExperiences { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ReplayMatchPlayer> ReplayMatchPlayers { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ReplayMatchPlayerScoreResult> ReplayMatchPlayerScoreResults { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ReplayMatchPlayerTalent> ReplayMatchPlayerTalents { get; set; }

        public virtual ReplayMatchTeamBan ReplayMatchTeamBan { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ReplayMatchTeamLevel> ReplayMatchTeamLevels { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ReplayMatchTeamObjective> ReplayMatchTeamObjectives { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ReplayMatchMessage> ReplayMatchMessage { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ReplayAllHotsPlayerHero> ReplayAllHotsPlayerHeroes { get; set; }
    }
}
