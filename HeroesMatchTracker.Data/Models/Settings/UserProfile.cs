namespace HeroesMatchTracker.Data.Models.Settings
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using static Heroes.Helpers.HeroesHelpers.Regions;

    public class UserProfile
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int UserProfileId { get; set; }

        public string UserBattleTagName { get; set; }

        public int UserRegion { get; set; }

        [NotMapped]
        public string GetUserRegion
        {
            get { return ((Region)UserRegion).ToString(); }
        }
    }
}
