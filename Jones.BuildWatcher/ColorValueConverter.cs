using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace Jones.BuildWatcher
{
    public class ColorValueConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool result;

            if (!bool.TryParse(value.ToString(), out result))
            {
                return null;
            }

            return result
                ? new SolidColorBrush(Colors.MediumSeaGreen)
                : new SolidColorBrush(Colors.Red);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}