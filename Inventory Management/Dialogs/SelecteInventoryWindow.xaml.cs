using Inventory_Management.Models;
using System;
using System.Collections;
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
    /// Interaction logic for SelecteInventoryWindow.xaml
    /// </summary>
    public partial class SelecteInventoryWindow : Window
    {
        public Inventory Inventory { get; set; }

        public SelecteInventoryWindow()
        {
            InitializeComponent();
            searchTextbox.Focus();
            searchTextbox.SelectAll();
        }

        private void searchTextbox_TextChanged(object sender, TextChangedEventArgs e)
        {
            dataGrid.ItemsSource =
                Global.DataSource.Inventories.Where(i =>
                    (i.Category?.StartsWith(searchTextbox.Text, StringComparison.OrdinalIgnoreCase) ?? false)
                    || (i.SubCategory?.StartsWith(searchTextbox.Text, StringComparison.OrdinalIgnoreCase) ?? false)
                    || (i.Name?.StartsWith(searchTextbox.Text, StringComparison.OrdinalIgnoreCase) ?? false))
                .ToList();
        }

        private void dataGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (dataGrid.SelectedItem as Inventory == null) return;

            Inventory = dataGrid.SelectedItem as Inventory;
            DialogResult = true;
            Close();
        }

        private void Window_KeyUp(object sender, KeyEventArgs e)
        {
            if(e.Key == Key.Down)
            {
                var list = dataGrid.ItemsSource as IList;
                if (list == null || list.Count == 0) return;
                
                if (dataGrid.SelectedItem == null)
                {
                    dataGrid.SelectedItem = list[0];
                }
                else
                {
                    for (int i = 0; i < list.Count; i++)
                    {
                        if (list[i] == dataGrid.SelectedItem)
                        {
                            var index = (i + 1) % list.Count;
                            if (index < 0) index = list.Count + index;
                            dataGrid.SelectedItem = list[index];
                            return;
                        }
                    }
                }
            }
            if (e.Key == Key.Up)
            {
                var list = dataGrid.ItemsSource as IList;
                if (list == null || list.Count == 0) return;

                if (dataGrid.SelectedItem == null)
                {
                    dataGrid.SelectedItem = list[0];
                }
                else
                {
                    for (int i = 0; i < list.Count; i++)
                    {
                        if (list[i] == dataGrid.SelectedItem)
                        {
                            var index = (i - 1) % list.Count;
                            if (index < 0) index = list.Count + index;
                            dataGrid.SelectedItem = list[index];
                            return;
                        }
                    }
                }
            }
            if(e.Key == Key.Return)
            {
                if (dataGrid.SelectedItem as Inventory == null) return;

                Inventory = dataGrid.SelectedItem as Inventory;
                DialogResult = true;
                Close();
            }
        }
    }
}
