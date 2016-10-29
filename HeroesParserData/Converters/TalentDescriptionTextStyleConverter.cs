using NLog;
using System;
using System.Windows.Controls;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace HeroesParserData.Converters
{
    public class TalentDescriptionTextStyleConverter : IValueConverter
    {
        private static Logger WarningLog = LogManager.GetLogger("WarningLogFile");

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

                    if (startTag == "<n/>")
                    {
                        span.Inlines.Add(new Run(text.Substring(0, startIndex)));
                        span.Inlines.Add(new Run("\n"));

                        // remove, this part of the string is not needed anymore
                        text = text.Remove(0, endIndex);

                        continue;
                    }
                    else if (startTag.StartsWith("<c val="))
                    {
                        string colorValue = startTag.Substring(8, startTag.Length - 10);

                        int offset = 4;
                        int closingCTagIndex = text.IndexOf("</c>", endIndex);

                        // check if an ending tag exists
                        if (closingCTagIndex > 0)
                        {
                            // </c>
                            string closingCTag = text.Substring(closingCTagIndex, offset);

                            span.Inlines.Add(new Run(text.Substring(0, startIndex)));
                            span.Inlines.Add(new Run(text.Substring(endIndex, closingCTagIndex - endIndex)) { Foreground = SetTooltipColors(colorValue) });

                            // remove, this part of the string is not needed anymore
                            text = text.Remove(0, closingCTagIndex + offset);
                        }
                        else
                        {
                            span.Inlines.Add(new Run(text.Substring(0, startIndex)));
                            // add the rest of the text
                            span.Inlines.Add(new Run(text.Substring(endIndex, text.Length - endIndex)) { Foreground = SetTooltipColors(colorValue) });

                            // none left
                            text = string.Empty;
                        }
                    }
                    else if (startTag.StartsWith("<img path=\"@UI/StormTalentInTextQuestIcon\""))
                    {
                        int closingTag = text.IndexOf("/>");

                        span.Inlines.Add(new Run(text.Substring(0, startIndex)));

                        BitmapImage bitmapImage = App.HeroesInfo.GetOtherIcon(HeroesIcons.OtherIcon.Quest);
                        Image image = new Image();
                        image.Source = bitmapImage;
                        image.Height = 15;
                        image.Width = 15;
                        image.Margin = new System.Windows.Thickness(0, 0, 4, -2);
                        image.VerticalAlignment = System.Windows.VerticalAlignment.Center;
                        InlineUIContainer container = new InlineUIContainer(image);

                        span.Inlines.Add(container);

                        // remove, this part of the string is not needed anymore
                        text = text.Remove(0, closingTag + 2);
                    }
                    else
                    {
                        span.Inlines.Add(new Run(text));
                        text = string.Empty;

                        WarningLog.Log(LogLevel.Warn, $"[TalentDescriptionTextStyleConverter] Unknown tag: {startTag}");
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
            switch (colorValue)
            {
                case "#TooltipNumbers":
                    color = Colors.GhostWhite;
                    break;
                case "#TooltipQuest":
                    color = (Color)ColorConverter.ConvertFromString("#AE9C54");
                    break;
                case "ffff8a":
                    color = (Color)ColorConverter.ConvertFromString("#ffff8a");
                    break;
                default:
                    WarningLog.Log(LogLevel.Warn, $"[TalentDescriptionTextStyleConverter] Unknown color value: {colorValue}");
                    color = Colors.Gray;
                    break;
            }

            return new SolidColorBrush(color);
        }
    }
}
