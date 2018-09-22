using Heroes.Icons;
using Heroes.Icons.Models;
using NLog;
using System;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace HeroesMatchTracker.Core.Converters
{
    public class TalentTooltipTextConverter : IValueConverter
    {
        private static Logger WarningLog = LogManager.GetLogger(LogFileNames.WarningLogFileName);

        // if this algorithm is changed then TalentTooltipStripNonText method in HeroIconsTest.cs needs to be changed as well
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string text = value as string;
            if (string.IsNullOrEmpty(text))
                return text;

            var span = new Span();

            while (text.Length > 0)
            {
                int startIndex = text.IndexOf("<");

                if (startIndex > -1)
                {
                    int endIndex = text.IndexOf(">", startIndex) + 1;

                    // example <c val="#TooltipNumbers">
                    string startTag = text.Substring(startIndex, endIndex - startIndex);

                    if (startTag == "<n/>" || startTag == "</n>")
                    {
                        span.Inlines.Add(new Run(text.Substring(0, startIndex)));
                        span.Inlines.Add(new Run("\n"));

                        // remove, this part of the string is not needed anymore
                        text = text.Remove(0, endIndex);

                        continue;
                    }
                    else if (startTag.ToLower().StartsWith("<c val="))
                    {
                        string colorValue = startTag.Substring(8, startTag.Length - 10);

                        int offset = 4;
                        int closingCTagIndex = text.ToLower().IndexOf("</c>", endIndex);

                        // check if an ending tag exists
                        if (closingCTagIndex > 0)
                        {
                            span.Inlines.Add(new Run(text.Substring(0, startIndex)));
                            span.Inlines.Add(new Run(text.Substring(endIndex, closingCTagIndex - endIndex)) { Foreground = SetTooltipColors(colorValue), FontSize = 15, FontWeight = FontWeights.SemiBold });

                            // remove, this part of the string is not needed anymore
                            text = text.Remove(0, closingCTagIndex + offset);
                        }
                        else
                        {
                            span.Inlines.Add(new Run(text.Substring(0, startIndex)));

                            // add the rest of the text
                            span.Inlines.Add(new Run(text.Substring(endIndex, text.Length - endIndex)) { Foreground = SetTooltipColors(colorValue), FontSize = 15, FontWeight = FontWeights.SemiBold });

                            // none left
                            text = string.Empty;
                        }
                    }
                    else if (startTag.ToLower().StartsWith("<s val="))
                    {
                        string colorValue = startTag.Substring(8, startTag.Length - 10);

                        int offset = 4;
                        int closingCTagIndex = text.ToLower().IndexOf("</s>", endIndex);

                        // check if an ending tag exists
                        if (closingCTagIndex > 0)
                        {
                            span.Inlines.Add(new Run(text.Substring(0, startIndex)));
                            span.Inlines.Add(new Run(text.Substring(endIndex, closingCTagIndex - endIndex)) { Foreground = SetTooltipColors(colorValue), FontSize = 15, FontWeight = FontWeights.SemiBold });

                            // remove, this part of the string is not needed anymore
                            text = text.Remove(0, closingCTagIndex + offset);
                        }
                        else
                        {
                            span.Inlines.Add(new Run(text.Substring(0, startIndex)));

                            // add the rest of the text
                            span.Inlines.Add(new Run(text.Substring(endIndex, text.Length - endIndex)) { Foreground = SetTooltipColors(colorValue), FontSize = 15, FontWeight = FontWeights.SemiBold });

                            // none left
                            text = string.Empty;
                        }
                    }
                    else if (startTag.StartsWith("<img path=\"@UI/StormTalentInTextQuestIcon\"") || startTag.StartsWith("<img  path=\"@UI/StormTalentInTextQuestIcon\""))
                    {
                        int closingTag = text.IndexOf("/>");

                        span.Inlines.Add(new Run(text.Substring(0, startIndex)));

                        Image image = new Image()
                        {
                            Source = CreateImage(HeroesIcons.HeroImages().OtherIconImage(OtherIcon.Quest)),
                            Height = 22,
                            Width = 20,
                            Margin = new Thickness(0, 0, 4, -2),
                            VerticalAlignment = VerticalAlignment.Center,
                        };

                        InlineUIContainer container = new InlineUIContainer(image);

                        span.Inlines.Add(container);

                        // remove, this part of the string is not needed anymore
                        text = text.Remove(0, closingTag + 2);
                    }
                    else if (startTag.StartsWith("<img path=\"@UI/StormTalentInTextArmorIcon\" alignment=\"uppermiddle\""))
                    {
                        int closingTag = text.IndexOf("/>");

                        span.Inlines.Add(new Run(text.Substring(0, startIndex)));

                        BitmapImage bitmapImage = null;

                        if (startTag.Contains("color=\"e12bfc\""))
                            bitmapImage = CreateImage(HeroesIcons.HeroImages().OtherIconImage(OtherIcon.StatusResistShieldSpell));
                        else if (startTag.Contains("color=\"ff6600\""))
                            bitmapImage = CreateImage(HeroesIcons.HeroImages().OtherIconImage(OtherIcon.StatusResistShieldPhysical));
                        else // if (startTag.Contains("color=\"BBBBBB\""))
                            bitmapImage = CreateImage(HeroesIcons.HeroImages().OtherIconImage(OtherIcon.StatusResistShieldDefault));

                        bitmapImage.Freeze();

                        Image image = new Image()
                        {
                            Source = bitmapImage,
                            Height = 20,
                            Width = 18,
                            Margin = new Thickness(0, 0, 4, -2),
                            VerticalAlignment = VerticalAlignment.Center,
                        };

                        InlineUIContainer container = new InlineUIContainer(image);

                        span.Inlines.Add(container);

                        // remove, this part of the string is not needed anymore
                        text = text.Remove(0, closingTag + 2);
                    }
                    else
                    {
                        span.Inlines.Add(new Run(text));
                        text = string.Empty;

                        WarningLog.Log(LogLevel.Warn, $"[{nameof(TalentTooltipTextConverter)}] Unknown tag: {startTag} - FullText: {value.ToString()}");
                    }
                }
                else
                {
                    // add the rest
                    if (text.Length > 0)
                        span.Inlines.Add(new Run(text));

                    text = string.Empty;
                }
            }

            return span;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        private SolidColorBrush SetTooltipColors(string colorValue)
        {
            Color color;

            if (colorValue.All(char.IsLetterOrDigit))
            {
                color = (Color)ColorConverter.ConvertFromString($"#{colorValue}");
            }
            else
            {
                switch (colorValue.ToUpper())
                {
                    case "#TOOLTIPNUMBERS":
                        color = (Color)ColorConverter.ConvertFromString("#BFD4FD");
                        break;
                    case "#STANDARDTOOLTIPHEADER":
                        color = (Color)ColorConverter.ConvertFromString("#FFFFFF");
                        break;
                    case "#TOOLTIPQUEST": // yellow-gold
                        color = (Color)ColorConverter.ConvertFromString("#E4B800");
                        break;
                    case "#ABILITYPASSIVE":
                        color = (Color)ColorConverter.ConvertFromString("#00FF90");
                        break;
                    case "#COLORVIOLET":
                        color = (Color)ColorConverter.ConvertFromString("#D65CFF");
                        break;
                    case "#COLORCREAMYELLOW":
                        color = (Color)ColorConverter.ConvertFromString("#FFFF80");
                        break;
                    case "#MALTHAELTRAIT":
                        color = (Color)ColorConverter.ConvertFromString("#00DFDF");
                        break;
                    case "#GLOWCOLORRED":
                        color = (Color)ColorConverter.ConvertFromString("#FF5858");
                        break;
                    case "#WHITEMANEDESPERATION":
                        color = (Color)ColorConverter.ConvertFromString("#FF8B8B");
                        break;
                    case "#WHITEMANEZEAL":
                        color = (Color)ColorConverter.ConvertFromString("#FFF5C2");
                        break;
                    default:
                        WarningLog.Log(LogLevel.Warn, $"[TalentDescriptionTextStyleConverter] Unknown color value: {colorValue}");
                        color = Colors.Gray;
                        break;
                }
            }

            return new SolidColorBrush(color);
        }

        private BitmapImage CreateImage(Stream image)
        {
            BitmapImage bitmapImage = new BitmapImage();
            bitmapImage.BeginInit();
            bitmapImage.StreamSource = image;
            bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
            bitmapImage.EndInit();
            bitmapImage.Freeze();

            return bitmapImage;
        }
    }
}
