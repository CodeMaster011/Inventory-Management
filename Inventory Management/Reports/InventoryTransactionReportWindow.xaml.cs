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

                dataGridActual.ItemsSource = GetInventoryTransactions(Inventory.Id, Global.DataSource.ActualTransactions);
                dataGridPortal.ItemsSource = GetInventoryTransactions(Inventory.Id, Global.DataSource.PortalTransactions);

                invName.Text = $"{Inventory.Category}-{Inventory.SubCategory}-{Inventory.Name}";
                
                var inventoryService = Global.Services.GetServiceHard<IInventoryService>();

                actualTransactionText.Text = $"Actual Transaction\n{inventoryService.CalculateClosingQuantity(Inventory.Id, inventoryService.GetOpeningQuantity(Inventory.Id, Global.DataSource.ActualOpening).GetValueOrDefault(0), Global.DataSource.ActualTransactions)}";
                portalTransactionText.Text = $"Portal Transaction\n{inventoryService.CalculateClosingQuantity(Inventory.Id, inventoryService.GetOpeningQuantity(Inventory.Id, Global.DataSource.PortalOpening).GetValueOrDefault(0), Global.DataSource.PortalTransactions)}";
            }
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

        }

        private void dataGridPortal_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {

        }
    }
}
