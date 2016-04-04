using System;
using System.Globalization;
using System.Windows.Data;

namespace Jones.BuildWatcher
{
    public class TimeStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(value is DateTime))
            {
                return null;
            }

            var dt = (DateTime) value;


            string text;
            if (dt.Date == DateTime.Today)
            {
                text = "Today";
            }
            else if (dt.Date == DateTime.Today.AddDays(-1))
            {
                text = "Yesterday";
            }
            else
            {
                text = dt.ToString("MM/dd/yyyy");
            }

            return $"{text} at {dt.ToString("h:mm tt")}";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}