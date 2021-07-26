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

namespace Inventory_Management.Reports
{
    /// <summary>
    /// Interaction logic for DailyReportWindow.xaml
    /// </summary>
    public partial class DailyReportWindow : Window
    {
        public DailyReportWindow()
        {
            InitializeComponent();
        }

        private void searchButton_Click(object sender, RoutedEventArgs e)
        {

        }

        public class DataView : BindableBaseFody
        {
            public Inventory Inventory { get; set; }
            
        }
    }
}
