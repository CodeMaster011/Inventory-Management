using Inventory_Management.Models;
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
    /// Interaction logic for ListTransactionsWindow.xaml
    /// </summary>
    public partial class ListTransactionsWindow : Window
    {
        public ListTransactionsWindow()
        {
            InitializeComponent();
        }

        private void actualRadioButton_Checked(object sender, RoutedEventArgs e)
        {
            dataGrid.ItemsSource = Global.DataSource.ActualTransactions;
        }

        private void portalRadioButton_Checked(object sender, RoutedEventArgs e)
        {
            dataGrid.ItemsSource = Global.DataSource.PortalTransactions;
        }

        private void dataGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if(dataGrid.SelectedItem is Transaction transaction)
            {
                new Dialogs.TransactionWindow()
                {
                    Owner = this,
                    DataContext = new ViewModels.TransactionWindowViewModel
                    {
                        Transaction = transaction,
                        IsReadOnly = false,
                        IsCreateAllow = false,
                        IsActual = actualRadioButton.IsChecked.GetValueOrDefault(false),
                        IsPortal = portalRadioButton.IsChecked.GetValueOrDefault(false)
                    }
                }.ShowDialog();
            }
        }
    }
}
