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
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
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

        public HttpResponseMessage CreateFunctionResponse(HttpRequest request, IDurableOrchestrationClient starter, string instanceId)
        {
            Uri requestUri = new Uri(request.Scheme + "://" + request.Host + request.Path + request.QueryString);
            HttpRequestMessage requestMessage = new HttpRequestMessage(new HttpMethod(request.Method), requestUri);

            // Create the status response
            var response = starter.CreateCheckStatusResponse(requestMessage, instanceId);

            // Set the status code to 202 Accepted
            response.StatusCode = HttpStatusCode.Accepted;

            // Add a Retry-After header to the response
            response.Headers.RetryAfter = new RetryConditionHeaderValue(TimeSpan.FromSeconds(10));

            return response;
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
                string instanceId = await starter.StartNewAsync(nameof(AddObjectToTableStorageOrchestration), dummyObject);

                var response = starter.CreateCheckStatusResponse(req, instanceId);
                HttpResponseMessage httpResponseMessage = CreateFunctionResponse(req.HttpContext.Request, starter, instanceId);
                log.LogInformation(httpResponseMessage.ToString());

                // Wait for the orchestration to complete
                TimeSpan timeout = TimeSpan.FromSeconds(30);
                TimeSpan retryInterval = TimeSpan.FromSeconds(2);
                DateTime endTime = DateTime.UtcNow + timeout;

                while (DateTime.UtcNow < endTime)
                {
                    DurableOrchestrationStatus status = await starter.GetStatusAsync(instanceId);

                    if (status != null)
                    {
                        if (status.RuntimeStatus == OrchestrationRuntimeStatus.Completed)
                        {
                            return new OkObjectResult("OK");
                        }
                        else if (status.RuntimeStatus == OrchestrationRuntimeStatus.Failed)
                        {
                            var errorDetails = status.Output?.ToString();
                            log.LogError($"Orchestration {instanceId} failed: {errorDetails}");

                            // Check if the error message contains the custom exception details
                            if (errorDetails != null && errorDetails.Contains("EntityAlreadyExists"))
                            {
                                return new ConflictObjectResult("Entity with entered ID already exists!");
                            }

                            return new StatusCodeResult(StatusCodes.Status500InternalServerError);
                        }
                    }

                    await Task.Delay(retryInterval);
                }

                // Timeout
                return new StatusCodeResult(StatusCodes.Status202Accepted);
            }
            catch (Exception ex)
            {
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }
    }
}
