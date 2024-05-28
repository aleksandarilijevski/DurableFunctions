using DurableFunctions.Models;
using DurableFunctions.Models.Dto;
using DurableFunctions.Services.Interfaces;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Threading.Tasks;

namespace DurableFunctions.Triggers
{
    public class AddObjectToCosmosDBServiceBusTrigger
    {
        private ICosmosDBService _cosmosService;

        public AddObjectToCosmosDBServiceBusTrigger(ICosmosDBService cosmosService)
        {
            _cosmosService = cosmosService;
        }

        [FunctionName(nameof(AddObjectToCosmosDBServiceBusTrigger))]
        public async Task ServiceBusTriggerFunction([ServiceBusTrigger("servicebusqueue", Connection = "serviceBus")] string myQueueItem, ILogger log)
        {
            DummyObjectTableEntity dummyObjectDto = JsonConvert.DeserializeObject<DummyObjectTableEntity>(myQueueItem);
            await _cosmosService.AddObject(dummyObjectDto);
        }
    }
}
