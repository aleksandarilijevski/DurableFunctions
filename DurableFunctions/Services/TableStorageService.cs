using Azure;
using Azure.Data.Tables;
using DurableFunctions.Models.Dto;
using DurableFunctions.Services.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DurableFunctions.Services
{
    public class TableStorageService : ITableStorageService
    {
        private string _connectionString = "DefaultEndpointsProtocol=https;AccountName=development123;AccountKey=ucKdMDljrCjuHNqniqVHHEObaF/sShkgjcacJFLQwQa5X2vASUkQc+oxTicVc9xjaV0f5wa4ThKo+AStbTPoZg==;EndpointSuffix=core.windows.net";

        public async Task AddObject(DummyObjectTableEntity dummyObjectDto)
        {
            TableClient tableClient = new TableClient(_connectionString, "development123");

            await tableClient.CreateIfNotExistsAsync();
            await tableClient.AddEntityAsync(dummyObjectDto);
        }

        public async Task<List<DummyObjectTableEntity>> GetAllObjects()
        {
            TableClient tableClient = new TableClient(_connectionString, "development123");

            AsyncPageable<DummyObjectTableEntity> data = tableClient.QueryAsync<DummyObjectTableEntity>();
            List<DummyObjectTableEntity> dummyObjects = new List<DummyObjectTableEntity>();

            await foreach (DummyObjectTableEntity dummyObject in data)
            {
                dummyObjects.Add(dummyObject);
            }

            return dummyObjects;
        }

        public async Task DeleteObject(DummyObjectTableEntity dummyObjectDto)
        {
            TableClient tableClient = new TableClient(_connectionString, "development123");
            await tableClient.DeleteEntityAsync(dummyObjectDto.PartitionKey, dummyObjectDto.RowKey);
        }
    }
}
