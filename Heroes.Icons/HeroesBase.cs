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
        public const int MinimumBuild = 47479;
        public const int DescriptionsAddedBuild = 55844;
        public const int HeroUnitsAddedBuild = 57797;

        protected string XmlMainFolderName => "Xml";
        protected string DefaultFileExtension => ".xml";
        protected string ImageMissingLogName => "_ImageMissingLog.txt";
        protected string ReferenceLogName => "_ReferenceNameLog.txt";
        protected string XmlErrorsLogName => "_XmlErrorsLog.txt";
        protected string LogFileName => "Logs";
        protected string ApplicationImagePath => "Heroes.Icons.Images";

        protected string SetImageStreamString(string imageFolderName, string fileName)
        {
            return $"{ApplicationImagePath}.{imageFolderName}.{fileName}";
        }
    }
}
