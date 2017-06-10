using NLog;
using System;
using System.Globalization;
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

        private readonly Uri QuestIcon = new Uri($"pack://application:,,,/Heroes.Icons;component/Icons/Other/storm_ui_ingame_talentpanel_upgrade_quest_icon.dds", UriKind.Absolute);

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string text = value as string;
            if (text == null)
                return null;
            if (text == string.Empty)
                return string.Empty;

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
                            span.Inlines.Add(new Run(text.Substring(endIndex, closingCTagIndex - endIndex)) { Foreground = SetTooltipColors(colorValue), FontSize = 15, FontWeight = FontWeights.DemiBold });

                            // remove, this part of the string is not needed anymore
                            text = text.Remove(0, closingCTagIndex + offset);
                        }
                        else
                        {
                            span.Inlines.Add(new Run(text.Substring(0, startIndex)));

                            // add the rest of the text
                            span.Inlines.Add(new Run(text.Substring(endIndex, text.Length - endIndex)) { Foreground = SetTooltipColors(colorValue), FontSize = 15, FontWeight = FontWeights.DemiBold });

                            // none left
                            text = string.Empty;
                        }
                    }
                    else if (startTag.StartsWith("<img path=\"@UI/StormTalentInTextQuestIcon\"") || startTag.StartsWith("<img  path=\"@UI/StormTalentInTextQuestIcon\""))
                    {
                        int closingTag = text.IndexOf("/>");

                        span.Inlines.Add(new Run(text.Substring(0, startIndex)));

                        BitmapImage bitmapImage = new BitmapImage(QuestIcon);
                        Image image = new Image()
                        {
                            Source = bitmapImage,
                            Height = 15,
                            Width = 15,
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
                        color = Colors.WhiteSmoke;
                        break;
                    case "#TOOLTIPQUEST": // yellow-gold
                        color = (Color)ColorConverter.ConvertFromString("#B48E4C");
                        break;
                    case "#ABILITYPASSIVE":
                        color = (Color)ColorConverter.ConvertFromString("#16D486");
                        break;
                    case "#COLORVIOLET":
                        color = (Color)ColorConverter.ConvertFromString("#A85EC6");
                        break;
                    case "#COLORCREAMYELLOW":
                        color = (Color)ColorConverter.ConvertFromString("#CED077");
                        break;
                    case "#MALTHAELTRAIT":
                        color = (Color)ColorConverter.ConvertFromString("#23CFD1");
                        break;
                    default:
                        WarningLog.Log(LogLevel.Warn, $"[TalentDescriptionTextStyleConverter] Unknown color value: {colorValue}");
                        color = Colors.Gray;
                        break;
                }
            }

            return new SolidColorBrush(color);
        }
    }
}
