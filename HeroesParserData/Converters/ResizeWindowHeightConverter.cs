using System;
using System.Globalization;
using System.Windows.Data;

namespace HeroesParserData.Converters
{
    public class ResizeWindowHeightConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            double? number = value as double?;
            if (!number.HasValue)
                return null;

            if (number <= 200)
                return number.Value;
            else
                return number - 135;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
