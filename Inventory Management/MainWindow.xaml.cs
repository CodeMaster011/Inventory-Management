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

        protected override void OnClosing(CancelEventArgs e)
        {
            base.OnClosing(e);
            if (MessageBox.Show("Are you sure to exit?", "Confirm", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.No)
            {
                e.Cancel = true;
            }
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
            new Dialogs.SelectDatasourceWindow() { Owner = this }.ShowDialog();
            RefreshDashboardData();
        }

        private void exitMenu_Click(object sender, RoutedEventArgs e)
        {
            Environment.Exit(0);
        }

        private void addNewInventoryMenu_Click(object sender, RoutedEventArgs e)
        {
            new Dialogs.InventoryWindow()
            {
                Owner = this,
                DataContext = new ViewModels.InventoryWindowViewModel
                {
                    IsCreateAllow = true
                }
            }.ShowDialog();
            RefreshDashboardData();
        }

        private void listInventoryMenu_Click(object sender, RoutedEventArgs e)
        {
            new Dialogs.ListInventoryWindow() { Owner = this }.ShowDialog();
            RefreshDashboardData();
        }

        private void addNewActualTransactionMenu_Click(object sender, RoutedEventArgs e)
        {
            new Dialogs.TransactionWindow()
            {
                Owner = this,
                DataContext = new ViewModels.TransactionWindowViewModel
                {
                    IsReadOnly = false,
                    IsCreateAllow = true,
                    IsActual = true
                }
            }.ShowDialog();
            RefreshDashboardData();
        }

        private void listActualTransactionMenu_Click(object sender, RoutedEventArgs e)
        {
            var wind = new Dialogs.ListTransactionsWindow { Owner = this };
            wind.actualRadioButton.IsChecked = true;
            wind.ShowDialog();
            RefreshDashboardData();
        }

        private void addNewPortalTransactionMenu_Click(object sender, RoutedEventArgs e)
        {
            new Dialogs.TransactionWindow()
            {
                Owner = this,
                DataContext = new ViewModels.TransactionWindowViewModel
                {
                    IsReadOnly = false,
                    IsCreateAllow = true,
                    IsPortal = true
                }
            }.ShowDialog();
            RefreshDashboardData();
        }

        private void listPortalTransactionMenu_Click(object sender, RoutedEventArgs e)
        {
            var wind = new Dialogs.ListTransactionsWindow { Owner = this };
            wind.portalRadioButton.IsChecked = true;
            wind.ShowDialog();
            RefreshDashboardData();
        }

        private void importMaster_Click(object sender, RoutedEventArgs e)
        {
            new Dialogs.ImportInventoryWindow() { Owner = this }.ShowDialog();
            RefreshDashboardData();
        }

        private async void saveMenu_Click(object sender, RoutedEventArgs e)
        {
            await Global.DataSource.Save();
            MessageBox.Show("Saved successfully");
        }

        private void invTransactionReportMenu_Click(object sender, RoutedEventArgs e)
        {
            new Reports.InventoryTransactionReportWindow { Owner = this }.ShowDialog();
            RefreshDashboardData();
        }

        private void comparisonReportMenu_Click(object sender, RoutedEventArgs e)
        {
            new Reports.ComparisonWindow { Owner = this }.ShowDialog();
            RefreshDashboardData();
        }

        private void aboutMenu_Click(object sender, RoutedEventArgs e)
        {
            new Dialogs.AboutWindow { Owner = this }.ShowDialog();
        }

        public void RefreshDashboardData()
        {
            var list = new List<DashboardCard>();
            if (Global.DataSource == null)
            {
                list = null;
                return;
            }

            list.Add(new DashboardCard
            {
                Title = "Inventory",
                BigText = Global.DataSource.Inventories.Count.ToString(),
                OpenOnClick = () => listInventoryMenu_Click(null, null)
            });

            list.Add(new DashboardCard
            {
                Title = "Transaction",
                Subtitle = "Actual",
                BigText = Global.DataSource.ActualTransactions.Count.ToString(),
                OpenOnClick = () => listActualTransactionMenu_Click(null, null)
            });

            list.Add(new DashboardCard
            {
                Title = "Transaction",
                Subtitle = "Portal",
                BigText = Global.DataSource.PortalTransactions.Count.ToString(),
                OpenOnClick = () => listPortalTransactionMenu_Click(null, null)
            });

            dashbordItems.ItemsSource = list;
        }

        public class DashboardCard : BindableBaseFody
        {
            public string Title { get; set; }
            public string Subtitle { get; set; }
            public string BigText { get; set; }
            public Action OpenOnClick { get; set; }
        }
    }
}
