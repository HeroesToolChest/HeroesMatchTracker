using System;
using System.Globalization;
using System.Windows.Data;

namespace HeroesParserData.Converters
{
    public class IntToStringThousandSeperator : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            int? number = value as int?;
            if (!number.HasValue)
                return null;
            else
            {
                return number.Value.ToString("#,0");
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
