using DurableFunctions.Exceptions;
using DurableFunctions.Models;
using DurableFunctions.Suborchestrations;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using System;
using System.Threading.Tasks;

namespace DurableFunctions.Orchestrations
{
    public class AddObjectToTableStorageOrchestration
    {
        [FunctionName(nameof(AddObjectToTableStorageOrchestration))]
        public async Task Run([OrchestrationTrigger] IDurableOrchestrationContext context)
        {
            DummyObject dummyObject = context.GetInput<DummyObject>();

            try
            {
                await context.CallSubOrchestratorAsync<DummyObject>(nameof(AddObjectToTableStorageSuborchestration), dummyObject);
            }
            catch (EntityAlreadyExistsInTableStorageException e)
            {
                throw;
            }
        }
    }
}
