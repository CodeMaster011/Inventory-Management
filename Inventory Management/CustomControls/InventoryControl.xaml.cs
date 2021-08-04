using Inventory_Management.Models;
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

namespace Inventory_Management.CustomControls
{
    /// <summary>
    /// Interaction logic for InventoryControl.xaml
    /// </summary>
    /// 
    [PropertyChanged.DoNotNotify]
    public partial class InventoryControl : UserControl, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        
        public Inventory Inventory
        {
            get { return (Inventory)GetValue(InventoryProperty); }
            set
            {
                SetValue(InventoryProperty, value);
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Inventory)));
            }
        }

        public static readonly DependencyProperty InventoryProperty =
            DependencyProperty.Register("Inventory", typeof(Inventory), typeof(InventoryControl), new PropertyMetadata());

        public InventoryControl()
        {
            InitializeComponent();
            root.DataContext = this;
        }
    }
}
