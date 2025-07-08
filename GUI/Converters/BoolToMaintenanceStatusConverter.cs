using System;
using System.Globalization;
using System.Windows.Data;

namespace GUI.Converters
{
    public class BoolToMaintenanceStatusConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
            => (value is bool b && b) ? "In Maintenance" : "Online";

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            => throw new NotImplementedException();
    }
}
