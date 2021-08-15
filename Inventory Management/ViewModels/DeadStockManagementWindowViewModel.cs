using Inventory_Management.Models;
using Inventory_Management.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Inventory_Management.ViewModels
{
    public class DeadStockManagementWindowViewModel : BindableBaseFody
    {
        public DeadStock DeadStock { get; set; } = new DeadStock();
        public DeadStockTransaction DeadStockTransaction { get; set; } = new DeadStockTransaction();
        public bool IsReadOnly { get; set; }
        public bool IsCreateAllow { get; set; }


        public StandardCommand AddNewDeadStockTransactionCommand { get; set; }
        public StandardCommand DeleteDeadStockTransactionCommand { get; set; }
        public StandardCommand ResetDeadStockTransactionCommand { get; set; }
        public StandardCommand CreateDeadStockCommand { get; set; }

        public DeadStockManagementWindowViewModel()
        {
            if (!DeadStock.Date.HasValue) DeadStock.Date = DateTime.Now;

            AddNewDeadStockTransactionCommand = new StandardCommand(_ => addNewDeadStockTransactionCommand());
            DeleteDeadStockTransactionCommand = new StandardCommand(deleteDeadStockTransactionCommand);
            ResetDeadStockTransactionCommand = new StandardCommand(_ => resetDeadStockTransactionCommand());
            CreateDeadStockCommand = new StandardCommand(_ => createDeadStockCommand());
        }

        private void createDeadStockCommand()
        {
            if (string.IsNullOrEmpty(DeadStock.InventoryId))
            {
                MessageBox.Show("Please choose an inventory. Press F9 to do search.");
                return;
            }

            if(DeadStock.Transactions.Count == 0)
            {
                MessageBox.Show("Please enter at least one transaction for cost. Use (-) value for expenses. Use (+) for income.");
                return;
            }

            var deadStockService = Global.Services.GetServiceHard<IDeadStockService>();
            deadStockService.MarkAsDeadStock(Global.DataSource, DeadStock);
            deadStockService.Save(Global.DataSource);

            DeadStock = new DeadStock();
            MessageBox.Show("Done");
        }

        private void resetDeadStockTransactionCommand()
        {
            DeadStockTransaction = new DeadStockTransaction();
        }

        private void deleteDeadStockTransactionCommand(object obj)
        {
            if (obj is DeadStockTransaction line)
            {
                DeadStock.Transactions.Remove(line);
                Global.Services.GetServiceHard<IDeadStockService>().CalculateClosing(DeadStock);
                NotifyPropertyChanged(nameof(Transaction));
                DeadStock.NotifyPropertyChanged(nameof(DeadStock.Transactions));
            }
        }

        private void addNewDeadStockTransactionCommand()
        {
            if (string.IsNullOrEmpty(DeadStockTransaction.Remarks))
            {
                MessageBox.Show("Please enter remarks to add transaction.");
                return;
            }
            if (string.IsNullOrWhiteSpace(DeadStockTransaction.Id))
                DeadStockTransaction.Id = Guid.NewGuid().ToString("n");
            
            Global.Services.GetServiceHard<IDeadStockService>().AddTransactionToDeadStock(Global.DataSource, DeadStock, DeadStockTransaction);
            
            DeadStockTransaction = new DeadStockTransaction();
            DeadStock.NotifyPropertyChanged(nameof(DeadStock.Transactions));
        }
    }
}
