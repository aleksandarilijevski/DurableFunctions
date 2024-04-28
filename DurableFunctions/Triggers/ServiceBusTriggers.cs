using DurableFunctions.Models;
using DurableFunctions.Services.Interfaces;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Threading.Tasks;

namespace DurableFunctions.Triggers
{
    public class ServiceBusTriggers
    {
        private ICosmosDBService _cosmosService;

        public ServiceBusTriggers(ICosmosDBService cosmosService)
        {
            _cosmosService = cosmosService;
        }

        [FunctionName("ServiceBusTrigger")]
        public async Task ServiceBusTriggerFunction([ServiceBusTrigger("servicebusfunction", Connection = "serviceBus")] string myQueueItem, ILogger log)
        {
            DummyObject dummyObject = JsonConvert.DeserializeObject<DummyObject>(myQueueItem);
            await _cosmosService.AddObject(dummyObject);
        }
    }
}
