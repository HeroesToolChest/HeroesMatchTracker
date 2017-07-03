using System;
using System.Windows.Media.Imaging;

namespace Heroes.Icons
{
    public class HeroesBase
    {
        protected static string ImageMissingLogName => "_ImageMissingLog.txt";
        protected static string ReferenceLogName => "_ReferenceNameLog.txt";
        protected static string BuildVerificationLogName => "_BuildVerificationLog.txt";
        protected static string LogFileName => "Logs";
        protected static string ApplicationIconsPath => "pack://application:,,,/Heroes.Icons;component/Icons";

        protected BitmapImage HeroesBitmapImage(string iconPath)
        {
            if (string.IsNullOrEmpty(iconPath))
                throw new ArgumentNullException(nameof(iconPath));

            if (iconPath[0] != '\\')
                iconPath = '\\' + iconPath;

            return new BitmapImage(new Uri($@"{ApplicationIconsPath}{iconPath}", UriKind.Absolute));
        }
    }
}
