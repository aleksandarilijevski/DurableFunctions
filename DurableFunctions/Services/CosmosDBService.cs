using Castle.Core.Logging;
using DurableFunctions.Models.Dto;
using DurableFunctions.Services.Interfaces;
using Microsoft.Azure.Cosmos;
using System.Threading.Tasks;

namespace DurableFunctions.Services
{
    public class CosmosDBService : ICosmosDBService
    {
        private string _databaseName = "CosmosDB";
        private string _containerName = "DummyObjects";

        private Container _container;

        public CosmosDBService()
        {
            CosmosClient cosmosClient = new CosmosClient("AccountEndpoint=https://cosmosdbexample2024.documents.azure.com:443/;AccountKey=s4js44ef80L4S6acN8t2hSBI9QqWkIBdARkoxcbnTA1sOjUqMmzUWdPWa4XZYCYdXef9TSpzzN7jACDbmrSEEg==;");
            _container = cosmosClient.GetContainer(_databaseName, _containerName);
        }

        public async Task AddObject(DummyObjectTableEntity dummyObjectDto)
        {
            await _container.CreateItemAsync(dummyObjectDto, new PartitionKey(dummyObjectDto.PartitionKey));
        }

        public async Task<DummyObjectTableEntity> GetObjectByID(string id)
        {
            using FeedIterator<DummyObjectTableEntity> feed = _container.GetItemQueryIterator<DummyObjectTableEntity>($"SELECT * FROM c WHERE c.id = \"{id}\"");

            while (feed.HasMoreResults)
            {
                FeedResponse<DummyObjectTableEntity> response = await feed.ReadNextAsync();

                foreach (DummyObjectTableEntity item in response)
                {
                    return item;
                }
            }

            return null;
        }
    }
}
