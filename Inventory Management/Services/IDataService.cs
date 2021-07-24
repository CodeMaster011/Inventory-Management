using Inventory_Management.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory_Management.Services
{
    public interface IDataService
    {
        DataSourceMetadata[] GetDataSources();
        DataSource GetDataSource(DataSourceMetadata metadata);

        Task SaveDataSource(DataSourceMetadata metadata, DataSource source);

        Task AddDatasource(DataSource source);
    }

    public class DataSourceMetadata
    {
        public string Id { get; set; }

        public DateTime CreatedOn { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }

        public string SourceFileName { get; set; }
    }

    public class DataSource
    {
        private IDataService dataService;
        private DataSourceMetadata metadata;

        public string Id { get; set; }

        public DateTime CreatedOn { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }

        public List<Inventory> Inventories { get; set; } = new List<Inventory>();
        public List<OpeningBalance> ActualOpening { get; set; } = new List<OpeningBalance>();
        public List<Transaction> ActualTransactions { get; set; } = new List<Transaction>();

        // PORTAL
        public List<OpeningBalance> PortalOpening { get; set; } = new List<OpeningBalance>();
        public List<Transaction> PortalTransactions { get; set; } = new List<Transaction>();


        public DataSource() { }
        public DataSource(DataSourceMetadata metadata, IDataService dataService)
        {
            this.metadata = metadata;
            this.dataService = dataService;
        }

        public Task Save() => dataService.SaveDataSource(metadata, this);

        public DataSource SetDataService(IDataService dataService)
        {
            this.dataService = dataService;
            return this;
        }

        public DataSource SetMetadata(DataSourceMetadata metadata)
        {
            this.metadata = metadata;
            return this;
        }
    }
}
