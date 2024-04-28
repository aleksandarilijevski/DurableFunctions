using DurableFunctions.Models;
using DurableFunctions.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Threading.Tasks;

namespace DurableFunctions.Triggers
{
    public class HttpTriggers
    {
        private ICosmosDBService _cosmosService;
        private ITableStorageService _tableStorageService;

        public HttpTriggers(ICosmosDBService cosmosService, ITableStorageService tableStorageService)
        {
            _cosmosService = cosmosService;
            _tableStorageService = tableStorageService;
        }

        [FunctionName("AddObjectToTableStorage")]
        public async Task<IActionResult> AddObjectToTableStorageFunction(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post")] HttpRequest req,
            ILogger log)
        {
            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            DummyObject dummyObject = JsonConvert.DeserializeObject<DummyObject>(requestBody);

            DummyObject check = await _cosmosService.GetObjectByID(dummyObject.UniqueID);

            if (check is not null && dummyObject.TriggerDate.Date < DateTime.Now.Date)
            {
                return new BadRequestObjectResult("Object with entered ID already exists, trigger date can not be in the past!");
            }

            if (check is not null)
            {
                return new BadRequestObjectResult("Object with entered unique ID already exists!");
            }

            if (dummyObject.TriggerDate.Date < DateTime.Now.Date)
            {
                return new BadRequestObjectResult("Trigger date can not be in the past!");
            }

            dummyObject.TriggerDate = DateTime.SpecifyKind(dummyObject.TriggerDate, DateTimeKind.Utc);
            await _tableStorageService.AddObject(dummyObject);

            return new OkObjectResult("Object was successfully added to table storage!");
        }
    }
}
