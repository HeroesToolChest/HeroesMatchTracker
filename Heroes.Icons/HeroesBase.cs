using System;
using System.Windows.Media.Imaging;

namespace Heroes.Icons
{
    public class HeroesBase
    {
        public const string NoPortraitPick = "storm_ui_ingame_heroselect_btn_nopick.dds";
        public const string NoPortraitFound = "storm_ui_ingame_heroselect_btn_notfound.dds";
        public const string NoLoadingScreenPick = "storm_ui_ingame_hero_loadingscreen_nopick.dds";
        public const string NoLoadingScreenFound = "storm_ui_ingame_hero_loadingscreen_notfound.dds";
        public const string NoLeaderboardPick = "storm_ui_ingame_hero_leaderboard_nopick.dds";
        public const string NoLeaderboardFound = "storm_ui_ingame_hero_loadingscreen_notfound.dds";
        public const string NoTalentIconPick = "storm_ui_ingame_leader_talent_unselected.png";
        public const string NoTalentIconFound = "storm_ui_icon_default.dds";

        protected static string ImageMissingLogName => "_ImageMissingLog.txt";
        protected static string ReferenceLogName => "_ReferenceNameLog.txt";
        protected static string XmlErrorsLogName => "_XmlErrorsLog.txt";
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
