using DurableFunctions.Exceptions;
using DurableFunctions.Models;
using DurableFunctions.Orchestrations;
using DurableFunctions.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Threading.Tasks;
using System.Web.Http;

namespace DurableFunctions.Triggers
{
    public class AddObjectToTableStorageHttpTrigger
    {
        private ICosmosDBService _cosmosService;
        private ITableStorageService _tableStorageService;

        public AddObjectToTableStorageHttpTrigger(ICosmosDBService cosmosService, ITableStorageService tableStorageService)
        {
            _cosmosService = cosmosService;
            _tableStorageService = tableStorageService;
        }

        [FunctionName(nameof(AddObjectToTableStorageHttpTrigger))]
        public async Task<IActionResult> AddObjectToTableStorageFunction(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post")] HttpRequest req,
            ILogger log, [DurableClient] IDurableOrchestrationClient starter)
        {
            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            DummyObject dummyObject = JsonConvert.DeserializeObject<DummyObject>(requestBody);

            try
            {
                await starter.StartNewAsync(nameof(AddObjectToTableStorageOrchestration), dummyObject);
                return new OkObjectResult("Object was successfully added to table storage!");
            }
            catch (EntityAlreadyExistsInTableStorageException e)
            {
                return new BadRequestObjectResult("Entity with entered ID already exists!");
            }
            catch(Exception e)
            {
                return new InternalServerErrorResult();
            }

        }
    }
}
