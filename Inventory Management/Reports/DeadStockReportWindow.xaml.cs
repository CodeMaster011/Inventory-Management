using Inventory_Management.Converters;
using Inventory_Management.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Inventory_Management.Reports
{
    /// <summary>
    /// Interaction logic for DeadStockReportWindow.xaml
    /// </summary>
    public partial class DeadStockReportWindow : Window
    {
        private PositiveNegativeColorConverter positiveNegativeColorConverter;

        public DeadStockReportWindow()
        {
            InitializeComponent();

            positiveNegativeColorConverter = new PositiveNegativeColorConverter();

            dataGrid.ItemsSource = Global.DataSource.DeadStocks;
        }

        private void dataGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (dataGrid.SelectedItem is DeadStock deadStock)
            {
                new Dialogs.DeadStockManagementWindow()
                {
                    Owner = this,
                    DataContext = new ViewModels.DeadStockManagementWindowViewModel
                    {
                        DeadStock = deadStock,
                        IsCreateAllow = false
                    }
                }.ShowDialog();
            }
        }

        private void quantityColDatagrid_TextChanged(object sender, TextChangedEventArgs e)
        {
            if(sender is TextBox textBox)
            {
                textBox.Foreground = positiveNegativeColorConverter.Convert(textBox.DataContext, null, null, null) as Brush;
            }
        }
    }
}
