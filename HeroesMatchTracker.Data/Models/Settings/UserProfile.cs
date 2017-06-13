namespace HeroesMatchTracker.Data.Models.Settings
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class UserProfile
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int UserProfileId { get; set; }

        public string UserBattleTagName { get; set; }

        public int UserRegion { get; set; }
    }
}
