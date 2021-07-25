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
            Inventories = File.ReadAllLines(filePath)
                .Skip(1) // skip header
                .Select(l => l.Split(','))
                .Select(d => new Inventory
                {
                    Id = Guid.NewGuid().ToString("n"),
                    Category = d[0],
                    SubCategory = d[1],
                    Name = d[2],
                    Hsn = d[3],
                    Unit = d[4],
                    TaxRate = double.TryParse(d[5], out var _t) ? _t : default,
                    Rate = double.TryParse(d[6], out var _r) ? _r : default,
                })
                .ToList();
            dataGrid.ItemsSource = Inventories;
        }

        private async void saveImportButton_Click(object sender, RoutedEventArgs e)
        {
            if (Global.DataSource.Inventories == null)
                Global.DataSource.Inventories = new List<Inventory>();
            
            Global.DataSource.Inventories.AddRange(Inventories);
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
