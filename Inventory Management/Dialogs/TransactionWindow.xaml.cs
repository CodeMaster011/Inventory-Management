using Inventory_Management.ViewModels;
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
    /// Interaction logic for TransactionWindow.xaml
    /// </summary>
    public partial class TransactionWindow : Window
    {
        public TransactionWindow()
        {
            InitializeComponent();
        }

        private void searchInventoryNameTextbox_KeyUp(object sender, KeyEventArgs e)
        {
            if(e.Key == Key.F9)
            {
                // search
                var selecteInventory = new SelecteInventoryWindow();
                selecteInventory.searchTextbox.Text = searchInventoryNameTextbox.Text;
                if (selecteInventory.ShowDialog() != true) return;
                if (selecteInventory.Inventory != null)
                {
                    var vm = this.DataContext as TransactionWindowViewModel;
                    if(vm == null)
                    {
                        MessageBox.Show("Internal error. View Model not found.");
                        return;
                    }
                    searchInventoryNameTextbox.Text = selecteInventory.Inventory.Name;
                    vm.TranscationLine.InventoryId = selecteInventory.Inventory.Id;
                }
            }
        }

        private void addTransactionButton_Click(object sender, RoutedEventArgs e)
        {
            var vm = this.DataContext as TransactionWindowViewModel;
            if(vm == null)
            {
                MessageBox.Show("Internal Error. View Model not found.");
                return;
            }
            vm.AddTransactionLineCommand.Execute(null);
            datagrid.ItemsSource = null;
            datagrid.ItemsSource = vm.Transaction.Lines;
            searchInventoryNameTextbox.Text = null;
            searchInventoryNameTextbox.Focus();
        }

        private void createNewTransactionButton_Click(object sender, RoutedEventArgs e)
        {
            var vm = this.DataContext as TransactionWindowViewModel;
            vm.CreateNewCommand.Execute(null);
            datagrid.ItemsSource = vm.Transaction.Lines;
            datePicker.Focus();
        }
    }
}
