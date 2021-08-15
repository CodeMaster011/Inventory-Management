using Inventory_Management.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory_Management.Services
{
    public interface IDeadStockService
    {
        void MarkAsDeadStock(DataSource source, DeadStock deadStock);
        bool AddTransactionToDeadStock(DataSource source, DeadStock deadStock, DeadStockTransaction transaction);
        bool MarkAsDisposed(DataSource source, DeadStock deadStock);
        void CalculateClosing(DeadStock deadStock);
        Task Save(DataSource source);
    }

    public class DeadStockService : IDeadStockService
    {
        public bool AddTransactionToDeadStock(DataSource source, DeadStock deadStock, DeadStockTransaction transaction)
        {
            deadStock.Transactions.Add(transaction);
            CalculateClosing(deadStock);
            return true;
        }

        public void MarkAsDeadStock(DataSource source, DeadStock deadStock)
        {
            if (string.IsNullOrWhiteSpace(deadStock.Id))
                deadStock.Id = Guid.NewGuid().ToString("n");
            else if (source.DeadStocks.Find(d => d.Id == deadStock.Id) == deadStock)
                return;

            deadStock.Amount = deadStock.Transactions.Sum(d => d.Amount.GetValueOrDefault(0));
            deadStock.Quantity = deadStock.Transactions.Sum(d => d.Quantity.GetValueOrDefault(0));

            source.DeadStocks.Add(deadStock);
        }

        public bool MarkAsDisposed(DataSource source, DeadStock deadStock)
        {
            if (source.DeadStocks.Find(d => d.Id == deadStock.Id) != deadStock)
                return false;
            deadStock.IsDisposed = true;
            return true;
        }

        public void CalculateClosing(DeadStock deadStock)
        {
            deadStock.Amount = deadStock.Transactions.Sum(d => d.Amount.GetValueOrDefault(0));
            deadStock.Quantity = deadStock.Transactions.Sum(d => d.Quantity.GetValueOrDefault(0));
        }

        public Task Save(DataSource source) => source.Save();
    }
}
