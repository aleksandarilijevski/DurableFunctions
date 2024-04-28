using Azure;
using Azure.Data.Tables;
using DurableFunctions.Models;
using DurableFunctions.Services.Interfaces;
using Microsoft.WindowsAzure.Storage.Table;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DurableFunctions.Services
{
    public class TableStorageService : ITableStorageService
    {
        private string _connectionString = "DefaultEndpointsProtocol=https;AccountName=development2024;AccountKey=NwhakNgdRM9fGB06a8M04FGjmVVNovTB0RkyOaTk6sCljwsK5+YFnmbiL0YZu8Efszjcupfxc1tL+AStbABL/Q==;EndpointSuffix=core.windows.net";

        public async Task AddObject(DummyObject dummyObject)
        {
            TableClient tableClient = new TableClient(_connectionString, "developmentTable");

            await tableClient.CreateIfNotExistsAsync();
            await tableClient.AddEntityAsync(dummyObject);
        }

        public async Task<List<DummyObject>> GetAllObjects()
        {
            TableClient tableClient = new TableClient(_connectionString, "developmentTable");

            AsyncPageable<DummyObject> data = tableClient.QueryAsync<DummyObject>();
            List<DummyObject> dummyObjects = new List<DummyObject>();

            await foreach (DummyObject dummyObject in data)
            {
                dummyObjects.Add(dummyObject);
            }

            return dummyObjects;
        }

        public async Task DeleteObject(DummyObject dummyObject)
        {
            TableClient tableClient = new TableClient(_connectionString, "developmentTable");
            await tableClient.DeleteEntityAsync(dummyObject.PartitionKey,dummyObject.RowKey);
        }
    }
}
