namespace HeroesParserData.Models.DbModels
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public partial class ReleaseNote
    {
        [Key]
        public string Version { get; set; }

        public bool PreRelease { get; set; }

        public DateTime DateReleased { get; set; }

        public string PatchNote { get; set; }
    }
}
