using System.ComponentModel.DataAnnotations;

namespace Heroes.Icons.Models
{
    public enum HeroDifficulty
    {
        Unknown,
        Easy,
        Medium,
        Hard,
        [Display(Name = "Very Hard")]
        VeryHard,
    }
}
