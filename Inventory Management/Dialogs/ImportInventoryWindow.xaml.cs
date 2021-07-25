using Inventory_Management.Models;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
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
    /// Interaction logic for ImportInventoryWindow.xaml
    /// </summary>
    public partial class ImportInventoryWindow : Window
    {
        public List<Inventory> Inventories { get; set; } = new List<Inventory>();
        public List<Balance> ActualOpening { get; set; } = new List<Balance>();
        public List<Balance> PortalOpening { get; set; } = new List<Balance>();


        public ImportInventoryWindow()
        {
            InitializeComponent();
        }

        private void importButton_Click(object sender, RoutedEventArgs e)
        {
            var diag = new OpenFileDialog();
            diag.Multiselect = false;
            if (diag.ShowDialog() != true) return;

            var filePath = diag.FileName;
            var splitedLines = File.ReadAllLines(filePath)
                .Skip(1) // skip header
                .Select(l => l.Split(','));

            foreach (var d in splitedLines)
            {
                var x = 0;
                var item = new Inventory
                {
                    Id = Guid.NewGuid().ToString("n"),
                    Category = d[x],
                    SubCategory = d[++x],
                    Name = d[++x],
                    Hsn = d[++x],
                    Unit = d[++x],
                    TaxRate = double.TryParse(d[++x], out var _t) ? _t : default,
                    Rate = double.TryParse(d[++x], out var _r) ? _r : default,
                };
                Inventories.Add(item);

                ActualOpening.Add(new Balance
                {
                    InventoryId = item.Id,
                    Quantity = double.TryParse(d[++x], out var _aq) ? _aq : default,
                    Amount = double.TryParse(d[++x], out var _aa) ? _aa : default,
                });

                PortalOpening.Add(new Balance
                {
                    InventoryId = item.Id,
                    Quantity = double.TryParse(d[++x], out var _pq) ? _pq : default,
                    Amount = double.TryParse(d[++x], out var _pa) ? _pa : default,
                });
            }
            dataGrid.ItemsSource = Inventories;
        }

        private async void saveImportButton_Click(object sender, RoutedEventArgs e)
        {
            if (Global.DataSource.Inventories == null)
                Global.DataSource.Inventories = new List<Inventory>();
            if (Global.DataSource.PortalOpening == null)
                Global.DataSource.PortalOpening = new List<Balance>();
            if (Global.DataSource.ActualOpening == null)
                Global.DataSource.ActualOpening = new List<Balance>();

            Global.DataSource.Inventories.AddRange(Inventories);
            Global.DataSource.PortalOpening.AddRange(PortalOpening);
            Global.DataSource.ActualOpening.AddRange(ActualOpening);

            await Global.DataSource.Save();
            Close();
        }

        private void dataGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (dataGrid.SelectedItem is Inventory inv)
            {
                new InventoryWindow()
                {
                    DataContext = new ViewModels.InventoryWindowViewModel
                    {
                        Inventory = inv,

                        IsReadOnly = false,
                        IsCreateAllow = false
                    }
                }.ShowDialog();
            }
        }
    }
}
