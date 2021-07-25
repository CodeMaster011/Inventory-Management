using Inventory_Management.Models;
using System.Collections.Generic;

namespace Inventory_Management.Services
{
    public interface IInventoryService
    {
        IEnumerable<Balance> CalculateClosingBalances(IEnumerable<Balance> balances, IEnumerable<Transaction> transactions);
        double? CalculateClosingQuantity(string inventoryId, double openingQuantity, IEnumerable<Transaction> transactions);
        double? CalculateClosingAmount(string inventoryId, double openingAmount, IEnumerable<Transaction> transactions);
    }
}
