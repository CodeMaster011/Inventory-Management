using Inventory_Management.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory_Management.Services
{
    public class InventoryService : IInventoryService
    {
        public IEnumerable<Balance> CalculateClosingBalances(IEnumerable<Balance> balances, IEnumerable<Transaction> transactions)
        {
            var calculatedOn = DateTime.Now;
            var dic = balances.Select(b =>
            {
                var newBalance = b.Clone();
                newBalance.Amount = 0;
                newBalance.Quantity = 0;
                newBalance.OnDate = calculatedOn;
                return newBalance;
            }).ToDictionary(b => b.InventoryId);
            foreach (var transaction in transactions)
            {
                var factor = 1;
                if (transaction.Nature == TransactionNature.Sales || transaction.Nature == TransactionNature.DebitNote)
                    factor = -1;

                foreach (var line in transaction.Lines)
                {
                    if (dic.TryGetValue(line.InventoryId, out var balance))
                    {
                        balance.Amount += line.Amount.GetValueOrDefault(0) * factor;
                        balance.Quantity += line.Quantity.GetValueOrDefault(0) * factor;
                    }
                    else
                    {
                        dic.Add(line.InventoryId, new Balance
                        {
                            InventoryId = line.InventoryId,
                            Amount = line.Amount.GetValueOrDefault(0) * factor,
                            Quantity = line.Quantity.GetValueOrDefault(0) * factor
                        });
                    }
                }
            }
            return dic.Values.ToArray();
        }

        public double? CalculateClosingQuantity(string inventoryId, double openingQuantity, IEnumerable<Transaction> transactions)
        {
            var closingBalance = openingQuantity;
            foreach (var transaction in transactions)
            {
                var factor = 1;
                if (transaction.Nature == TransactionNature.Sales || transaction.Nature == TransactionNature.DebitNote)
                    factor = -1;
                foreach (var line in transaction.Lines)
                {
                    if (line.InventoryId == inventoryId)
                        closingBalance += line.Quantity.GetValueOrDefault(0) * factor;
                }
            }
            return closingBalance;
        }

        public double? CalculateClosingAmount(string inventoryId, double openingAmount, IEnumerable<Transaction> transactions)
        {
            var closingBalance = openingAmount;
            foreach (var transaction in transactions)
            {
                var factor = 1;
                if (transaction.Nature == TransactionNature.Sales || transaction.Nature == TransactionNature.DebitNote)
                    factor = -1;
                foreach (var line in transaction.Lines)
                {
                    if (line.InventoryId == inventoryId)
                        closingBalance += line.Amount.GetValueOrDefault(0) * factor;
                }
            }
            return closingBalance;
        }
    }
}
