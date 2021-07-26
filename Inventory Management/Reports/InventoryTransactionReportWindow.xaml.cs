using Inventory_Management.Dialogs;
using Inventory_Management.Models;
using Inventory_Management.Services;
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
    /// Interaction logic for InventoryTransactionReportWindow.xaml
    /// </summary>
    public partial class InventoryTransactionReportWindow : Window
    {
        public Inventory Inventory { get; set; }
        

        public InventoryTransactionReportWindow()
        {
            InitializeComponent();
        }

        private void chooseInventoryButton_Click(object sender, RoutedEventArgs e)
        {
            var selecteInventory = new SelecteInventoryWindow() { Owner = this };
            if (selecteInventory.ShowDialog() != true) return;
            if (selecteInventory.Inventory != null)
            {
                Inventory = selecteInventory.Inventory;
                RefershViewData();
                //dataGridActual.ItemsSource = GetInventoryTransactions(Inventory.Id, Global.DataSource.ActualTransactions);
                //dataGridPortal.ItemsSource = GetInventoryTransactions(Inventory.Id, Global.DataSource.PortalTransactions);

                //invName.Text = $"{Inventory.Category}-{Inventory.SubCategory}-{Inventory.Name}";
                
                //var inventoryService = Global.Services.GetServiceHard<IInventoryService>();

                //actualTransactionText.Text = $"Actual Transaction\n{inventoryService.CalculateClosingQuantity(Inventory.Id, inventoryService.GetOpeningQuantity(Inventory.Id, Global.DataSource.ActualOpening).GetValueOrDefault(0), Global.DataSource.ActualTransactions)}";
                //portalTransactionText.Text = $"Portal Transaction\n{inventoryService.CalculateClosingQuantity(Inventory.Id, inventoryService.GetOpeningQuantity(Inventory.Id, Global.DataSource.PortalOpening).GetValueOrDefault(0), Global.DataSource.PortalTransactions)}";
            }
        }

        private void RefershViewData()
        {
            dataGridActual.ItemsSource = GetInventoryTransactions(Inventory.Id, Global.DataSource.ActualTransactions);
            dataGridPortal.ItemsSource = GetInventoryTransactions(Inventory.Id, Global.DataSource.PortalTransactions);

            invName.Text = $"{Inventory.Category}-{Inventory.SubCategory}-{Inventory.Name}";

            var inventoryService = Global.Services.GetServiceHard<IInventoryService>();

            actualTransactionText.Text = $"Actual Transaction\n{inventoryService.CalculateClosingQuantity(Inventory.Id, inventoryService.GetOpeningQuantity(Inventory.Id, Global.DataSource.ActualOpening).GetValueOrDefault(0), Global.DataSource.ActualTransactions)}";
            portalTransactionText.Text = $"Portal Transaction\n{inventoryService.CalculateClosingQuantity(Inventory.Id, inventoryService.GetOpeningQuantity(Inventory.Id, Global.DataSource.PortalOpening).GetValueOrDefault(0), Global.DataSource.PortalTransactions)}";
        }


        private List<InventoryTransaction> GetInventoryTransactions(string id, IEnumerable<Transaction> transactions)
        {
            var result = new List<InventoryTransaction>();
            foreach (var transaction in transactions)
            {
                foreach (var item in transaction.Lines)
                {
                    if (item.InventoryId == id)
                        result.Add(new InventoryTransaction { Transaction = transaction, Line = item });
                }
            }
            return result;
        }

        public class InventoryTransaction : BindableBaseFody
        {
            public Transaction Transaction { get; set; }
            public TranscationLine Line { get; set; }
        }

        private void dataGridActual_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (dataGridActual.SelectedItem is Transaction transaction)
            {
                new Dialogs.TransactionWindow()
                {
                    Owner = this,
                    DataContext = new ViewModels.TransactionWindowViewModel
                    {
                        Transaction = transaction,
                        IsReadOnly = false,
                        IsCreateAllow = false,
                        IsActual = true,
                        IsPortal = false
                    }
                }.ShowDialog();
            }
        }

        private void dataGridPortal_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (dataGridPortal.SelectedItem is Transaction transaction)
            {
                new Dialogs.TransactionWindow()
                {
                    Owner = this,
                    DataContext = new ViewModels.TransactionWindowViewModel
                    {
                        Transaction = transaction,
                        IsReadOnly = false,
                        IsCreateAllow = false,
                        IsActual = false,
                        IsPortal = true
                    }
                }.ShowDialog();
            }
        }

        private void addToPortalButton_Click(object sender, RoutedEventArgs e)
        {
            if ((sender as Button)?.DataContext is InventoryTransaction inventoryTransaction)
            {
                var transaction = inventoryTransaction.Transaction.Clone();
                // transaction.Id = Guid.NewGuid().ToString("n");
                Global.DataSource.PortalTransactions.Add(transaction);
                RefershViewData();
            }
        }

        private async void saveButton_Click(object sender, RoutedEventArgs e)
        {
            await Global.DataSource.Save();
            MessageBox.Show("Save succefull", "Save");
        }

        private void moveToPortalButton_Click(object sender, RoutedEventArgs e)
        {
            if ((sender as Button)?.DataContext is InventoryTransaction inventoryTransaction)
            {
                if (Global.DataSource.ActualTransactions.Remove(inventoryTransaction.Transaction))
                {
                    var transaction = inventoryTransaction.Transaction.Clone();
                    // transaction.Id = Guid.NewGuid().ToString("n");
                    Global.DataSource.PortalTransactions.Add(transaction);
                    RefershViewData();
                }else
                {
                    MessageBox.Show("Failed to removed", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void deleteInActualButton_Click(object sender, RoutedEventArgs e)
        {
            if ((sender as Button)?.DataContext is InventoryTransaction inventoryTransaction)
            {
                if (Global.DataSource.ActualTransactions.Remove(inventoryTransaction.Transaction))
                {
                    MessageBox.Show("Removed succesfully", "Success");
                }
                else
                {
                    MessageBox.Show("Failed to removed", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                RefershViewData();
            }
        }
    }
}
