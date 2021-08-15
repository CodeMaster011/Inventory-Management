using Inventory_Management.Models;
using Inventory_Management.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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
    /// Interaction logic for DeadStockManagementWindow.xaml
    /// </summary>
    public partial class DeadStockManagementWindow : Window
    {
        public DeadStockManagementWindow()
        {
            InitializeComponent();
        }

        private void searchInventoryNameTextbox_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.F9)
            {
                // search
                var selecteInventory = new SelecteInventoryWindow() { Owner = this };
                selecteInventory.searchTextbox.Text = searchInventoryNameTextbox.Text;
                if (selecteInventory.ShowDialog() != true) return;
                if (selecteInventory.Inventory != null)
                {
                    var vm = this.DataContext as DeadStockManagementWindowViewModel;
                    if (vm == null)
                    {
                        MessageBox.Show("Internal error. View Model not found.");
                        return;
                    }
                    searchInventoryNameTextbox.Text = selecteInventory.Inventory.Name;
                    vm.DeadStock.InventoryId = selecteInventory.Inventory.Id;
                }
            }
        }


        private void addTransactionButton_Click(object sender, RoutedEventArgs e)
        {
            var vm = this.DataContext as DeadStockManagementWindowViewModel;
            if (vm == null)
            {
                MessageBox.Show("Internal Error. View Model not found.");
                return;
            }
            vm.AddNewDeadStockTransactionCommand.Execute(null);
            datagrid.ItemsSource = null;
            datagrid.ItemsSource = vm.DeadStock.Transactions;
            transactionRemarksTextbox.Text = null;
            transactionRemarksTextbox.Focus();
        }

        private void transactionLineDeleteButton_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button)
            {
                var vm = this.DataContext as DeadStockManagementWindowViewModel;
                var line = button.CommandParameter as DeadStockTransaction;
                if (MessageBox.Show($"Are you sure to remove item,\n{line.Remarks}?", "Confirm", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.No)
                    return;
                vm.ResetDeadStockTransactionCommand.Execute(line);
                datagrid.ItemsSource = null;
                datagrid.ItemsSource = vm.DeadStock.Transactions;
            }
        }

        private void createNewDeadStockButton_Click(object sender, RoutedEventArgs e)
        {
            var vm = this.DataContext as DeadStockManagementWindowViewModel;
            vm.CreateDeadStockCommand.Execute(null);
            datagrid.ItemsSource = vm.DeadStock.Transactions;
            datePicker.Focus();
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            base.OnClosing(e);
        }

        private void updateDeadStockButton_Click(object sender, RoutedEventArgs e)
        {
            Global.DataSource.Save();
            MessageBox.Show("Done");
        }
    }
}
