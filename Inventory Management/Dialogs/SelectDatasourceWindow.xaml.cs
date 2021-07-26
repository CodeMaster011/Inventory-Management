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
    /// Interaction logic for SelectDatasourceWindow.xaml
    /// </summary>
    public partial class SelectDatasourceWindow : Window
    {
        private readonly IDataService dataService;

        public SelectDatasourceWindow()
        {
            InitializeComponent();

            dataService = Global.Services.GetService<IDataService>();
            datagrid.ItemsSource = dataService.GetDataSources();
        }

        private void dataGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if(datagrid.SelectedItem is DataSourceMetadata metadata)
            {
                var source = dataService.GetDataSource(metadata);
                Global.DataSource = source;
                // MessageBox.Show("Data source successfully changed.");
                Close();
            }
        }

        private void addNewSourceButton_Click(object sender, RoutedEventArgs e)
        {
            new CreateDatasourceWindow() { Owner = this }.ShowDialog();
        }
    }
}
