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
        public Balance ActualOpeningBalance { get; set; } = new Balance();
        public Balance PortalOpeningBalance { get; set; } = new Balance();
        public bool IsReadOnly { get; set; }
        public bool IsCreateAllow { get; set; }
        public StandardCommand CreateNewCommand { get; set; }

        public InventoryWindowViewModel()
        {
            CreateNewCommand = new StandardCommand(_ => createNewInventory());
            AddOnPropertyChangedCallback(nameof(Inventory), () =>
            {
                if (string.IsNullOrWhiteSpace(Inventory?.Id)) return;
                ActualOpeningBalance = Global.DataSource.ActualOpening.Find(b => b.InventoryId == Inventory.Id) ?? new Balance();
                PortalOpeningBalance = Global.DataSource.PortalOpening.Find(b => b.InventoryId == Inventory.Id) ?? new Balance();
            });
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
