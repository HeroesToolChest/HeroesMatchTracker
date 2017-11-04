using System.IO;

namespace Heroes.Icons.Models
{
    public class TalentBase
    {
        /// <summary>
        /// The name of the talent
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// The unique reference name of the talent
        /// </summary>
        public string ReferenceName { get; set; }

        /// <summary>
        /// Is the icon of the talent generic
        /// </summary>
        public bool IsIconGeneric { get; set; }

        /// <summary>
        /// Is the talent generic
        /// </summary>
        public bool IsGeneric { get; set; }

        /// <summary>
        /// The tooltip's description name
        /// </summary>
        public string TooltipDescriptionName { get; set; }

        public string Icon { get; set; }

        public TalentTooltip Tooltip { get; set; } = new TalentTooltip();

        public Stream GetIcon()
        {
            return HeroesIcons.GetHeroesIconsAssembly().GetManifestResourceStream(Icon);
        }
    }
}
