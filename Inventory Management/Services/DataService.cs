using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Inventory_Management.Services
{
    public class DataService : IDataService
    {
        public string _dataDir = Path.Combine(Environment.CurrentDirectory, "Data");
        public string _metadataFilePath = Path.Combine(Environment.CurrentDirectory, "Data", "metadata.json");

        public async Task AddDatasource(DataSource source)
        {
            var _sources = GetDataSources();
            var sources = new List<DataSourceMetadata>();
            if (_sources != null) sources.AddRange(_sources);

            var metadata = new DataSourceMetadata
            {
                CreatedOn = source.CreatedOn,
                FromDate = source.FromDate,
                ToDate = source.ToDate,
                Id = source.Id ?? Guid.NewGuid().ToString("n"),
                SourceFileName = $"{Guid.NewGuid():n}.json"
            };
            source.Id = metadata.Id;
            sources.Add(metadata);
            await UpdateMetadata(sources);

            source.SetMetadata(metadata);
            source.SetDataService(this);

            var dataFilePath = Path.Combine(_dataDir, metadata.SourceFileName);
            await Task.Run(() =>
                File.WriteAllText(dataFilePath, Newtonsoft.Json.JsonConvert.SerializeObject(source)));
        }

        public DataSource GetDataSource(DataSourceMetadata metadata)
        {
            var dataFilePath = Path.Combine(_dataDir, metadata.SourceFileName);
            var source = Newtonsoft.Json.JsonConvert.DeserializeObject<DataSource>(File.ReadAllText(dataFilePath));
            source.SetMetadata(metadata);
            source.SetDataService(this);
            return source;
        }

        public DataSourceMetadata[] GetDataSources()
        {
            if (!File.Exists(_metadataFilePath)) return null;
            return Newtonsoft.Json.JsonConvert.DeserializeObject<DataSourceMetadata[]>(File.ReadAllText(_metadataFilePath))
                .OrderByDescending(m => m.CreatedOn)
                .ToArray();
        }

        public Task SaveDataSource(DataSourceMetadata metadata, DataSource source)
        {
            var dataFilePath = Path.Combine(_dataDir, metadata.SourceFileName);
            return Task.Run(() =>
                File.WriteAllText(dataFilePath, Newtonsoft.Json.JsonConvert.SerializeObject(source)));
        }

        protected Task UpdateMetadata(List<DataSourceMetadata> metadatas)
        {
            return Task.Run(() => 
                File.WriteAllText(_metadataFilePath, Newtonsoft.Json.JsonConvert.SerializeObject(metadatas)));
        }

        public DataService()
        {
            if (!Directory.Exists(_dataDir))
                Directory.CreateDirectory(_dataDir);
        }
    }
}
