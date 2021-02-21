namespace HeroesMatchTracker.Data.Models.Replays
{
    using Heroes.Helpers;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("Replays")]
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
            ReplayHotsApiUpload = new HashSet<ReplayHotsApiUpload>();
            ReplayMatchDraftPicks = new HashSet<ReplayMatchDraftPick>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long ReplayId { get; set; }

        public long? RandomValue { get; set; }

        public string Hash { get; set; }

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

        public virtual ICollection<ReplayMatchDraftPick> ReplayMatchDraftPicks { get; set; }

        public virtual ICollection<ReplayMatchTeamObjective> ReplayMatchTeamObjectives { get; set; }

        public virtual ICollection<ReplayMatchMessage> ReplayMatchMessage { get; set; }

        public virtual ICollection<ReplayMatchAward> ReplayMatchAward { get; set; }

        public virtual ICollection<ReplayHotsApiUpload> ReplayHotsApiUpload { get; set; }
        public virtual ICollection<ReplayHeroesProfileUpload> ReplayHeroesProfileUpload { get; set; }
    }
}
