using HeroesStatTracker.Data;
using MaterialDesignColors;
using MaterialDesignThemes.Wpf;
using NLog;
using System;
using System.Linq;
using System.Windows;

namespace HeroesStatTracker.Core
{
    public static class StylePalette
    {
        private static Logger WarningLogFile = LogManager.GetLogger(LogFileNames.StartupLogFileName);

        public static void ApplyStyle(bool alternate)
        {
            var resourceDictionary = new ResourceDictionary
            {
                Source = new Uri(@"pack://application:,,,/Dragablz;component/Themes/materialdesign.xaml")
            };

            var styleKey = alternate ? "MaterialDesignAlternateTabablzControlStyle" : "MaterialDesignTabablzControlStyle";
            var style = (Style)resourceDictionary[styleKey];

            foreach (var tabablzControl in Dragablz.TabablzControl.GetLoadedInstances())
            {
                tabablzControl.Style = style;
            }
        }

        public static void ApplyBase(bool isDark)
        {
            new PaletteHelper().SetLightDark(isDark);
        }

        public static void ApplyPrimary(Swatch swatch)
        {
            new PaletteHelper().ReplacePrimaryColor(swatch);
        }

        public static void ApplyAccent(Swatch swatch)
        {
            new PaletteHelper().ReplaceAccentColor(swatch);
        }

        public static Swatch GetSwatchByName(string name)
        {
            var swatches = new SwatchesProvider().Swatches;

            foreach (var swatch in swatches)
            {
                if (swatch.Name == name)
                {
                    return swatch;
                }
            }

            WarningLogFile.Log(LogLevel.Info, $"Could not find palette color of {name}");
            return swatches.ToList()[0];
        }

        public static void SetDefaultPalette()
        {
            ApplyPrimary(GetSwatchByName("blue"));
            ApplyAccent(GetSwatchByName("lightblue"));

            QueryDb.SettingsDb.UserSettings.MainStylePrimary = "blue";
            QueryDb.SettingsDb.UserSettings.MainStyleAccent = "lightblue";
        }
    }
}
