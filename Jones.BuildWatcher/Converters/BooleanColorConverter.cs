using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace Jones.BuildWatcher
{
    public sealed class BooleanColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool result;
            if(!(value is bool))
            {
                return false;
            }

            result = (bool) value;

            return result ? Brushes.Green : Brushes.Red;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}