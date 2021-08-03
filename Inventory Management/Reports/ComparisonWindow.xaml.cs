using Inventory_Management.Models;
using Inventory_Management.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    /// Interaction logic for ComparisonWindow.xaml
    /// </summary>
    public partial class ComparisonWindow : Window
    {
        // TODO: a search box support for filtering

        public ComparisonWindow()
        {
            InitializeComponent();

            LoadData();
        }

        private void LoadData()
        {
            var result = new List<ViewData>();
            

            var inventoryService = Global.Services.GetServiceHard<IInventoryService>();
            var actualClosingBalances = inventoryService.CalculateClosingBalances(Global.DataSource.ActualOpening, Global.DataSource.ActualTransactions);
            var portalClosingBalances = inventoryService.CalculateClosingBalances(Global.DataSource.PortalOpening, Global.DataSource.PortalTransactions);
            foreach (var inv in Global.DataSource.Inventories)
            {
                var acClBalance = actualClosingBalances.FirstOrDefault(b => b.InventoryId == inv.Id);
                var poClBalance = portalClosingBalances.FirstOrDefault(b => b.InventoryId == inv.Id);
                var diff = new Balance
                {
                    Amount = acClBalance?.Amount.GetValueOrDefault(0) - poClBalance?.Amount.GetValueOrDefault(0),
                    Quantity = acClBalance?.Quantity.GetValueOrDefault(0) - poClBalance?.Quantity.GetValueOrDefault(0),
                };
                result.Add(new ViewData
                {
                    Inventory = inv,
                    ActualBalance = acClBalance,
                    PortalBalance = poClBalance,
                    Difference = diff,
                    Action = "Unknown",
                    IsNegativeAmount = diff.Amount.GetValueOrDefault(0) < 0,
                    IsNegativeQuantity = diff.Quantity.GetValueOrDefault(0) < 0,
                });
            }
            datagrid.ItemsSource = result;
        }

        public class ViewData : BindableBaseFody
        {
            public Inventory Inventory { get; set; }
            public Balance ActualBalance { get; set; }
            public Balance PortalBalance { get; set; }
            public Balance Difference { get; set; }
            public bool IsNegativeQuantity { get; set; }
            public bool IsNegativeAmount { get; set; }
            public string Action { get; set; }
        }

        private void addTransactionButton_Click(object sender, RoutedEventArgs e)
        {
            new Dialogs.TransactionWindow()
            {
                Owner = this,
                DataContext = new ViewModels.TransactionWindowViewModel
                {
                    IsReadOnly = false,
                    IsCreateAllow = true,
                }
            }.ShowDialog();
            LoadData();
        }

        private void addInventoryButton_Click(object sender, RoutedEventArgs e)
        {
            new Dialogs.InventoryWindow()
            {
                Owner = this,
                DataContext = new ViewModels.InventoryWindowViewModel
                {
                    IsCreateAllow = true
                }
            }.ShowDialog();
        }
    }
}
