using Azure.Messaging.ServiceBus;
using DurableFunctions.Models;
using DurableFunctions.Models.Dto;
using DurableFunctions.Services.Interfaces;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DurableFunctions.Triggers
{
    public class SendObjectToServiceBusTimerTrigger
    {
        private ITableStorageService _tableStorageService;

        public SendObjectToServiceBusTimerTrigger(ITableStorageService tableStorageService, ICosmosDBService cosmosService)
        {
            _tableStorageService = tableStorageService;
        }

        [FunctionName(nameof(SendObjectToServiceBusTimerTrigger))]
        public async Task TimerTriggerFunction([TimerTrigger("0 * * * * *", RunOnStartup = false)] TimerInfo timerInfo, ILogger log)
        {
            List<DummyObjectTableEntity> dummyObjects = await _tableStorageService.GetAllObjects();

            foreach (DummyObjectTableEntity dummyObject in dummyObjects)
            {
                ServiceBusClient client = new ServiceBusClient("Endpoint=sb://servicebusexample221.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=bRYEE44anAaVVO2FvA8kjV5FFSV1rZEuZ+ASbH7ePWQ=");
                ServiceBusSender sender = client.CreateSender("servicebusqueue");

                string body = JsonConvert.SerializeObject(dummyObject);
                ServiceBusMessage serviceBusMessage = new ServiceBusMessage(body);
                await sender.SendMessageAsync(serviceBusMessage);
                await _tableStorageService.DeleteObject(dummyObject);
            }
        }
    }
}
