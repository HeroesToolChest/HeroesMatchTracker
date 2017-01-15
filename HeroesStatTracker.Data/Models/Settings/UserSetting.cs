namespace HeroesStatTracker.Data.Models.Settings
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class UserSetting
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int SettingId { get; set; }

        public string Name { get; set; }
 
        public string Value { get; set; }
    }
}
