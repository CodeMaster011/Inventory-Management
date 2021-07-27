using Inventory_Management.Dialogs;
using Inventory_Management.Models;
using Inventory_Management.Services;
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

        protected override void OnClosing(CancelEventArgs e)
        {
            base.OnClosing(e);
            if (MessageBox.Show("Are you sure to close?", "Confirm", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.No)
            {
                e.Cancel = true;
            }
        }

        private void chooseInventoryButton_Click(object sender, RoutedEventArgs e)
        {
            var selecteInventory = new SelecteInventoryWindow() { Owner = this };
            if (selecteInventory.ShowDialog() != true) return;
            if (selecteInventory.Inventory == null) return;
            Inventory = selecteInventory.Inventory;
            RefershViewData();
        }

        private void RefershViewData()
        {
            var inventoryService = Global.Services.GetServiceHard<IInventoryService>();

            double? actualOpeningBalance = inventoryService.GetOpeningQuantity(Inventory.Id, Global.DataSource.ActualOpening);
            var actualTransactions = new List<InventoryTransaction>();
            actualTransactions.Add(new InventoryTransaction
            {
                Line = new TranscationLine
                {
                    Quantity = actualOpeningBalance
                }
            });
            actualTransactions.AddRange(GetInventoryTransactions(Inventory.Id, Global.DataSource.ActualTransactions) ?? new List<InventoryTransaction>() { });
            dataGridActual.ItemsSource = actualTransactions;


            var portalTransactions = new List<InventoryTransaction>();
            double? portalOpeningBalance = inventoryService.GetOpeningQuantity(Inventory.Id, Global.DataSource.PortalOpening);
            portalTransactions.Add(new InventoryTransaction
            {
                Line = new TranscationLine
                {
                    Quantity = portalOpeningBalance
                }
            });
            portalTransactions.AddRange(GetInventoryTransactions(Inventory.Id, Global.DataSource.PortalTransactions)?? new List<InventoryTransaction>() { });
            dataGridPortal.ItemsSource = portalTransactions;

            invName.Text = $"{Inventory.Category} - {Inventory.SubCategory} - {Inventory.Name}";
            
            actualTransactionText.Text = $"Actual Transaction\nQuantity: {inventoryService.CalculateClosingQuantity(Inventory.Id, actualOpeningBalance.GetValueOrDefault(0), Global.DataSource.ActualTransactions):n2}";
            portalTransactionText.Text = $"Portal Transaction\nQuantity: {inventoryService.CalculateClosingQuantity(Inventory.Id, portalOpeningBalance.GetValueOrDefault(0), Global.DataSource.PortalTransactions):n2}";
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
            if (dataGridActual.SelectedItem is InventoryTransaction inventoryTransaction)
            {
                new Dialogs.TransactionWindow()
                {
                    Owner = this,
                    DataContext = new ViewModels.TransactionWindowViewModel
                    {
                        Transaction = inventoryTransaction.Transaction,
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
            if (dataGridPortal.SelectedItem is InventoryTransaction inventoryTransaction)
            {
                new Dialogs.TransactionWindow()
                {
                    Owner = this,
                    DataContext = new ViewModels.TransactionWindowViewModel
                    {
                        Transaction = inventoryTransaction.Transaction,
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
            MessageBox.Show("Save successfully", "Save");
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
                    MessageBox.Show("Removed successfully", "Success");
                }
                else
                {
                    MessageBox.Show("Failed to removed", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                RefershViewData();
            }
        }

        private void addToActualButton_Click(object sender, RoutedEventArgs e)
        {
            if ((sender as Button)?.DataContext is InventoryTransaction inventoryTransaction)
            {
                var transaction = inventoryTransaction.Transaction.Clone();
                // transaction.Id = Guid.NewGuid().ToString("n");
                Global.DataSource.ActualTransactions.Add(transaction);
                RefershViewData();
            }
        }

        private void moveToActualButton_Click(object sender, RoutedEventArgs e)
        {
            if ((sender as Button)?.DataContext is InventoryTransaction inventoryTransaction)
            {
                if (Global.DataSource.PortalTransactions.Remove(inventoryTransaction.Transaction))
                {
                    var transaction = inventoryTransaction.Transaction.Clone();
                    // transaction.Id = Guid.NewGuid().ToString("n");
                    Global.DataSource.ActualTransactions.Add(transaction);
                    RefershViewData();
                }
                else
                {
                    MessageBox.Show("Failed to removed", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void deleteInPortalButton_Click(object sender, RoutedEventArgs e)
        {
            if ((sender as Button)?.DataContext is InventoryTransaction inventoryTransaction)
            {
                if (Global.DataSource.PortalTransactions.Remove(inventoryTransaction.Transaction))
                {
                    MessageBox.Show("Removed successfully", "Success");
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
