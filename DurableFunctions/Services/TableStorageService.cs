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
        private string _connectionString = "DefaultEndpointsProtocol=https;AccountName=development2024;AccountKey=NwhakNgdRM9fGB06a8M04FGjmVVNovTB0RkyOaTk6sCljwsK5+YFnmbiL0YZu8Efszjcupfxc1tL+AStbABL/Q==;EndpointSuffix=core.windows.net";

        public async Task AddObject(DummyObjectTableEntity dummyObjectDto)
        {
            TableClient tableClient = new TableClient(_connectionString, "developmentTable");

            await tableClient.CreateIfNotExistsAsync();
            await tableClient.AddEntityAsync(dummyObjectDto);
        }

        public async Task<List<DummyObjectTableEntity>> GetAllObjects()
        {
            TableClient tableClient = new TableClient(_connectionString, "developmentTable");

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
            TableClient tableClient = new TableClient(_connectionString, "developmentTable");
            await tableClient.DeleteEntityAsync(dummyObjectDto.PartitionKey, dummyObjectDto.RowKey);
        }
    }
}
