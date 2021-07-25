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

namespace Inventory_Management.Dialogs
{
    /// <summary>
    /// Interaction logic for ListInventoryWindow.xaml
    /// </summary>
    public partial class ListInventoryWindow : Window
    {
        public ListInventoryWindow()
        {
            InitializeComponent();

            dataGrid.ItemsSource = Global.DataSource.Inventories;
        }

        private void dataGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if(dataGrid.SelectedItem is Inventory inventory)
            {
                new Dialogs.InventoryWindow()
                {
                    Owner = this,
                    DataContext = new ViewModels.InventoryWindowViewModel
                    {
                        Inventory = inventory,
                        IsCreateAllow = false
                    }
                }.ShowDialog();
            }
        }
    }
}
