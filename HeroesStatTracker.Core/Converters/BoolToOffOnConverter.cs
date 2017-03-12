using System;
using System.Globalization;
using System.Windows.Data;

namespace HeroesMatchData.Core.Converters
{
    public class BoolToOffOnConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                return "Off";
            else
                return (bool)value ? "On" : "Off";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
