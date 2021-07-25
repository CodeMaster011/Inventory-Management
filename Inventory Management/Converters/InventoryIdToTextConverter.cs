using System;
using System.Globalization;
using System.Windows.Data;

namespace Inventory_Management.Converters
{
    public class InventoryIdToTextConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if(value is string inventoryId)
            {
                return Global.DataSource.Inventories.Find(i => i.Id == inventoryId);
            }
            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
