using Inventory_Management.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

namespace Inventory_Management.ViewModels
{
    public class InventoryWindowViewModel : BindableBaseFody
    {
        public Inventory Inventory { get; set; } = new Inventory();
        public bool IsReadOnly { get; set; }
        public bool IsCreateAllow { get; set; }
        public StandardCommand CreateNewCommand { get; set; }

        public InventoryWindowViewModel()
        {
            CreateNewCommand = new StandardCommand(_ => createNewInventory());
        }

        private void createNewInventory()
        {
            Inventory.Id = Guid.NewGuid().ToString("n");
            Global.DataSource.Inventories.Add(Inventory.Clone());
            ThreadPool.QueueUserWorkItem(_ =>
            {
                Global.DataSource.Save().GetAwaiter().GetResult();
                Global.Dispatcher.Invoke(() =>
                {
                    MessageBox.Show("Inventory created");
                    Inventory = new Inventory();
                });
            });
        }
    }
}
