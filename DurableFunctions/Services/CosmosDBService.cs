using DurableFunctions.Models;
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
            CosmosClient cosmosClient = new CosmosClient("AccountEndpoint=https://cosmosdbexample2024.documents.azure.com:443/;AccountKey=WBcrNC3zZKTrmPyPRHWsAQc5VERg2XrtT0LkfVjPmwWJIeHVKtLlp8jNw5t49zoNQhnxCsG56vhbACDbhTnbbg==;");
            _container = cosmosClient.GetContainer(_databaseName, _containerName);
        }

        public async Task AddObject(DummyObject dummyObject)
        {
            await _container.CreateItemAsync(dummyObject, new PartitionKey(dummyObject.PartitionKey));
        }

        public async Task<DummyObject> GetObjectByID(string id)
        {
            using FeedIterator<DummyObject> feed = _container.GetItemQueryIterator<DummyObject>($"SELECT * FROM c WHERE c.id = \"{id}\"");

            while (feed.HasMoreResults)
            {
                FeedResponse<DummyObject> response = await feed.ReadNextAsync();

                foreach (DummyObject item in response)
                {
                    return item;
                }
            }

            return null;
        }
    }
}
