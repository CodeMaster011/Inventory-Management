using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory_Management.Models
{
    public class Inventory : BindableBaseFody
    {
        public string Id { get; set; }
        public string Category { get; set; }
        public string SubCategory { get; set; }
        public string Name { get; set; }
        public string Hsn { get; set; }
        public string Unit { get; set; }
        public double? TaxRate { get; set; }
        public double? Rate { get; set; }

        public Inventory Clone() => JsonConvert.DeserializeObject<Inventory>(JsonConvert.SerializeObject(this));
    }

    public class Balance : BindableBaseFody
    {
        public string InventoryId { get; set; }
        public double? Quantity { get; set; }
        public double? Amount { get; set; }
        public DateTime? OnDate { get; set; }

        public Balance Clone() => JsonConvert.DeserializeObject<Balance>(JsonConvert.SerializeObject(this));
    }

    public class Transaction : BindableBaseFody
    {
        public string Id { get; set; }

        public TransactionNature Nature { get; set; }
        public DateTime? Date { get; set; }
        public List<TranscationLine> Lines { get; set; } = new List<TranscationLine>();
        public string Remarks { get; set; }

        public Transaction Clone() => JsonConvert.DeserializeObject<Transaction>(JsonConvert.SerializeObject(this));
    }

    public enum TransactionNature
    {
        Sales,
        Purchase,
        CreditNote,
        DebitNote
    }

    public class TranscationLine : BindableBaseFody
    {
        public string InventoryId { get; set; }
        public double? Amount { get; set; }
        public double? Quantity { get; set; }

        public TranscationLine Clone() => JsonConvert.DeserializeObject<TranscationLine>(JsonConvert.SerializeObject(this));
    }
}
