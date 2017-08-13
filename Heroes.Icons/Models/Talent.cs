using System;
using System.Windows.Media.Imaging;

namespace Heroes.Icons.Models
{
    public class Talent
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

        public TalentTier Tier { get; set; }

        public Uri IconUri { get; set; }

        public TalentTooltip Tooltip { get; set; } = new TalentTooltip();

        /// <summary>
        /// Returns the BitmapImage of the icon uri
        /// </summary>
        /// <returns></returns>
        public BitmapImage GetIcon()
        {
            if (IconUri == null)
                return null;

            BitmapImage image = new BitmapImage(IconUri);
            image.Freeze();

            return image;
        }
    }
}
