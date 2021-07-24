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

    public class OpeningBalance : BindableBaseFody
    {
        public string InventoryId { get; set; }
        public double? Quantity { get; set; }
        public double? Amount { get; set; }
        public DateTime? OnDate { get; set; }

        public Inventory Clone() => JsonConvert.DeserializeObject<Inventory>(JsonConvert.SerializeObject(this));
    }

    public class Transaction : BindableBaseFody
    {
        public string Id { get; set; }

        public DateTime? Date { get; set; }
        public List<TranscationLine> Lines { get; set; } = new List<TranscationLine>();
        public string Remarks { get; set; }

        public Inventory Clone() => JsonConvert.DeserializeObject<Inventory>(JsonConvert.SerializeObject(this));
    }

    public class TranscationLine : BindableBaseFody
    {
        public string InventoryId { get; set; }
        public double? Amount { get; set; }
        public double? Quantity { get; set; }

        public Inventory Clone() => JsonConvert.DeserializeObject<Inventory>(JsonConvert.SerializeObject(this));
    }
}
