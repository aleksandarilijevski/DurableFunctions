using Azure.Messaging.ServiceBus;
using DurableFunctions.Models;
using DurableFunctions.Services.Interfaces;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DurableFunctions.Triggers
{
    public class TimerTriggers
    {
        private ITableStorageService _tableStorageService;

        public TimerTriggers(ITableStorageService tableStorageService, ICosmosDBService cosmosService)
        {
            _tableStorageService = tableStorageService;
        }

        [FunctionName("TimerTrigger")]
        public async Task TimerTriggerFunction([TimerTrigger("0 * * * * *", RunOnStartup = false)] TimerInfo timerInfo, ILogger log)
        {
            List<DummyObject> dummyObjects = await _tableStorageService.GetAllObjects();

            foreach (DummyObject dummyObject in dummyObjects)
            {
                ServiceBusClient client = new ServiceBusClient("Endpoint=sb://servicebusexample2024.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=14Y8qCyQN6Rqr3oXs/Bn+E6fS9XbEMrQV+ASbCd8E10=");
                ServiceBusSender sender = client.CreateSender("servicebusfunction");

                string body = JsonConvert.SerializeObject(dummyObject);
                ServiceBusMessage serviceBusMessage = new ServiceBusMessage(body);
                await sender.SendMessageAsync(serviceBusMessage);
                await _tableStorageService.DeleteObject(dummyObject);
            }
        }
    }
}
