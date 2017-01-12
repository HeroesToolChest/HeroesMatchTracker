using System;
using System.Windows.Media.Imaging;

namespace Heroes.Icons
{
    public class HeroesBase
    {
        protected readonly string ImageMissingLogName = "_ImageMissingLog.txt";
        protected readonly string ReferenceLogName = "_ReferenceNameLog.txt";
        protected readonly string LogFileName = "Logs";
        protected readonly string ApplicationIconsPath = "pack://application:,,,/Heroes.Icons;component/Icons";

        protected BitmapImage HeroesBitmapImage(string iconPath)
        {
            if (string.IsNullOrEmpty(iconPath))
                throw new ArgumentNullException();

            if (iconPath[0] != '\\')
                iconPath = '\\' + iconPath;

            return new BitmapImage(new Uri($@"{ApplicationIconsPath}{iconPath}", UriKind.Absolute));
        }
    }
}
