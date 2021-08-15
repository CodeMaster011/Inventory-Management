using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace Inventory_Management.Converters
{
    public class PositiveNegativeColorConverter : IValueConverter
    {
        public SolidColorBrush NegativeValueColor { get; set; } = new SolidColorBrush(Color.FromRgb(255, 48, 48));
        public SolidColorBrush PositiveValueColor { get; set; } = new SolidColorBrush(Color.FromRgb(69, 139, 0));

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var dValue = 0d;
            if (value.GetType() == typeof(double?))
                dValue = ((double?)value).Value;
            if (value.GetType() == typeof(double))
                dValue = (double)value;
            if (value.GetType() == typeof(string))
                if (double.TryParse(value as string, out dValue)) { }

            if (dValue < 0)
                return NegativeValueColor;
            return PositiveValueColor;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
