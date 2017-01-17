namespace HeroesStatTracker.Data.Models.Replays
{
    using Heroes.ReplayParser;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("Replay")]
    public class ReplayMatch : IRawDataDisplay, INonContextModels
    {
        public ReplayMatch()
        {
            ReplayMatchTeamExperiences = new HashSet<ReplayMatchTeamExperience>();
            ReplayMatchPlayers = new HashSet<ReplayMatchPlayer>();
            ReplayMatchPlayerScoreResults = new HashSet<ReplayMatchPlayerScoreResult>();
            ReplayMatchPlayerTalents = new HashSet<ReplayMatchPlayerTalent>();
            ReplayMatchTeamLevels = new HashSet<ReplayMatchTeamLevel>();
            ReplayMatchTeamObjectives = new HashSet<ReplayMatchTeamObjective>();
            ReplayMatchMessage = new HashSet<ReplayMatchMessage>();
            ReplayMatchAward = new HashSet<ReplayMatchAward>();
            ReplayHotsLogsUpload = new HashSet<ReplayHotsLogsUpload>();
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

        [NotMapped]
        public string Result { get; set; }

        public virtual ICollection<ReplayMatchTeamExperience> ReplayMatchTeamExperiences { get; set; }

        public virtual ICollection<ReplayMatchPlayer> ReplayMatchPlayers { get; set; }

        public virtual ICollection<ReplayMatchPlayerScoreResult> ReplayMatchPlayerScoreResults { get; set; }

        public virtual ICollection<ReplayMatchPlayerTalent> ReplayMatchPlayerTalents { get; set; }

        public virtual ReplayMatchTeamBan ReplayMatchTeamBan { get; set; }

        public virtual ICollection<ReplayMatchTeamLevel> ReplayMatchTeamLevels { get; set; }

        public virtual ICollection<ReplayMatchTeamObjective> ReplayMatchTeamObjectives { get; set; }

        public virtual ICollection<ReplayMatchMessage> ReplayMatchMessage { get; set; }

        public virtual ICollection<ReplayMatchAward> ReplayMatchAward { get; set; }

        public virtual ICollection<ReplayHotsLogsUpload> ReplayHotsLogsUpload { get; set; }
    }
}
