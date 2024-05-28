using Castle.Core.Logging;
using DurableFunctions.Models.Dto;
using DurableFunctions.Services.Interfaces;
using Microsoft.Azure.Cosmos;
using System.Threading.Tasks;

namespace DurableFunctions.Services
{
    public class CosmosDBService : ICosmosDBService
    {
        private string _databaseName = "DummyObjects";
        private string _containerName = "containerId";

        private Container _container;

        public CosmosDBService()
        {
            CosmosClient cosmosClient = new CosmosClient("AccountEndpoint=https://cosmosdbexample24.documents.azure.com:443/;AccountKey=2VvfyWl18K1QRuEvRt6E7c3LeWwpdTGQGjpRRSZIAVh4jKfX2jr6mykSGsa7D40TzhrpCvkGEOPxACDbk3mqCg==;");
            _container = cosmosClient.GetContainer(_databaseName, _containerName);
        }

        public async Task AddObject(DummyObjectTableEntity dummyObjectDto)
        {
            await _container.CreateItemAsync(dummyObjectDto, new PartitionKey("InputKey"));
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
