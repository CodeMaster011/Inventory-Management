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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Inventory_Management
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            new Dialogs.InventoryWindow() 
            { 
                DataContext = new ViewModels.InventoryWindowViewModel
                {
                    Inventory = Global.DataSource.Inventories.FirstOrDefault(),
                    IsReadOnly = false
                }
            }.ShowDialog();
        }

        private void importMasterButton_Click(object sender, RoutedEventArgs e)
        {
            new Dialogs.ImportInventoryWindow().ShowDialog();
        }

        private void selectInventoryButton_Click(object sender, RoutedEventArgs e)
        {
            var selecteInventory = new Dialogs.SelecteInventoryWindow();
            selecteInventory.searchTextbox.Text = "G";
            if (selecteInventory.ShowDialog() != true) return;
            if (selecteInventory.Inventory != null)
                MessageBox.Show(selecteInventory.Inventory.Name);
        }

        private void addTransactionButton_Click(object sender, RoutedEventArgs e)
        {
            new Dialogs.TransactionWindow()
            {
                DataContext = new ViewModels.TransactionWindowViewModel
                {
                    IsReadOnly = false,
                    IsCreateAllow = true
                }
            }.ShowDialog();
        }

        private void openData_Click(object sender, RoutedEventArgs e)
        {
            new Dialogs.SelectDatasourceWindow().ShowDialog();
        }
    }
}
