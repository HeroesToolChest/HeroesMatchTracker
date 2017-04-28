namespace HeroesMatchTracker.Data.Models.ReleaseNotes
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public class ReleaseNote
    {
        [Key]
        public string Version { get; set; }

        public bool PreRelease { get; set; }

        public DateTime DateReleased { get; set; }

        public string PatchNote { get; set; }
    }
}
