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

namespace Inventory_Management.Dialogs
{
    /// <summary>
    /// Interaction logic for CreateDatasourceWindow.xaml
    /// </summary>
    public partial class CreateDatasourceWindow : Window
    {
        public CreateDatasourceWindow()
        {
            InitializeComponent();
        }

        private async void makeDatasourceButton_Click(object sender, RoutedEventArgs e)
        {
            

            if (!fromDate.SelectedDate.HasValue)
            {
                MessageBox.Show("From date or To date is missing or both.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (Global.DataSource == null)
            {
                Global.DataSource = new DataSource
                {
                    CreatedOn = DateTime.Now,
                    FromDate = fromDate.SelectedDate,
                    ToDate = toDate.SelectedDate,
                };
                await Global.Services.GetServiceHard<IDataService>().AddDatasource(Global.DataSource);
                MessageBox.Show("New data source created.");
                return;
            }

            var inventoryService = Global.Services.GetServiceHard<IInventoryService>();
            var newActualBalances = inventoryService.CalculateClosingBalances(Global.DataSource.ActualOpening, Global.DataSource.ActualTransactions);
            var newPortalBalances = inventoryService.CalculateClosingBalances(Global.DataSource.PortalOpening, Global.DataSource.PortalTransactions);
            var datasource = new DataSource
            {
                
            };
        }
    }
}
