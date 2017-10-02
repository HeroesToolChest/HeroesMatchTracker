using System;
using System.IO;

namespace Heroes.Icons
{
    public class HeroesBase
    {
        public const string NoPortraitPick = "storm_ui_ingame_heroselect_btn_nopick.dds";
        public const string NoPortraitFound = "storm_ui_ingame_heroselect_btn_notfound.dds";
        public const string NoLoadingScreenPick = "storm_ui_ingame_hero_loadingscreen_nopick.dds";
        public const string NoLoadingScreenFound = "storm_ui_ingame_hero_loadingscreen_notfound.dds";
        public const string NoLeaderboardPick = "storm_ui_ingame_hero_leaderboard_nopick.dds";
        public const string NoLeaderboardFound = "storm_ui_ingame_hero_leaderboard_notfound.dds";
        public const string NoTalentIconPick = "storm_ui_ingame_leader_talent_unselected.png";
        public const string NoTalentIconFound = "storm_ui_icon_default.dds";

        protected string XmlMainFolderName => "Xml";
        protected string DefaultFileExtension => ".xml";
        protected string ImageMissingLogName => "_ImageMissingLog.txt";
        protected string ReferenceLogName => "_ReferenceNameLog.txt";
        protected string XmlErrorsLogName => "_XmlErrorsLog.txt";
        protected string LogFileName => "Logs";
        protected string ApplicationIconsPath => Path.GetFullPath("Icons");

        protected Uri GetImageUri(string iconFolderName, string fileName)
        {
            return new Uri(Path.Combine(ApplicationIconsPath, iconFolderName, fileName), UriKind.Absolute);
        }

        //protected BitmapImage HeroesBitmapImage(string iconPath)
        //{
        //    if (string.IsNullOrEmpty(iconPath))
        //        throw new ArgumentNullException(nameof(iconPath));

        //    BitmapImage image;

        //    if (iconPath.StartsWith(ApplicationIconsPath))
        //    {
        //        image = new BitmapImage(new Uri(iconPath, UriKind.Absolute));
        //        image.Freeze();

        //        return image;
        //    }
        //    else if (iconPath[0] != '\\')
        //    {
        //        iconPath = '\\' + iconPath;
        //    }

        //    image = new BitmapImage(new Uri($@"{ApplicationIconsPath}{iconPath}", UriKind.Absolute));
        //    image.Freeze();

        //    return image;
        //}

        //protected BitmapImage HeroesBitmapImage(Uri iconPath)
        //{
        //    if (string.IsNullOrEmpty(iconPath.AbsoluteUri))
        //        throw new ArgumentNullException(nameof(iconPath));

        //    BitmapImage image = new BitmapImage(iconPath);
        //    image.Freeze();

        //    return image;
        //}
    }
}
