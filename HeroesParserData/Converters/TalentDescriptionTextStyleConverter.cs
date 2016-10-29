using NLog;
using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Media;

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

                        // </c>
                        string closingCTag = text.Substring(closingCTagIndex, offset);

                        span.Inlines.Add(new Run(text.Substring(0, startIndex)));
                        span.Inlines.Add(new Run(text.Substring(endIndex, closingCTagIndex - endIndex)) { Foreground = SetTooltipColors(colorValue) });

                        // remove, this part of the string is not needed anymore
                        text = text.Remove(0, closingCTagIndex + offset);
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
            if (colorValue == "#TooltipNumbers")
                return new SolidColorBrush(Colors.Green);
            else
            {
                WarningLog.Log(LogLevel.Warn, $"[TalentDescriptionTextStyleConverter] Unknown color value: {colorValue}");
                return new SolidColorBrush(Colors.Gray);
            }
        }
    }
}
