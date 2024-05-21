using AutoMapper;
using DurableFunctions.Activities;
using DurableFunctions.Exceptions;
using DurableFunctions.Models;
using DurableFunctions.Models.Dto;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using System;
using System.Threading.Tasks;

namespace DurableFunctions.Suborchestrations
{
    public class AddObjectToTableStorageSuborchestration
    {
        private IMapper _mapper;

        public AddObjectToTableStorageSuborchestration(IMapper mapper)
        {
            _mapper = mapper;
        }

        [FunctionName(nameof(AddObjectToTableStorageSuborchestration))]
        public async Task Run([OrchestrationTrigger] IDurableOrchestrationContext context)
        {
            DummyObject dummyObject = context.GetInput<DummyObject>();
            DummyObjectTableEntity dummyObjectDto = _mapper.Map<DummyObjectTableEntity>(dummyObject);

            try
            {
                await context.CallActivityAsync<DummyObject>(nameof(AddObjectToTableStorageActivity), dummyObjectDto);
            }
            catch (EntityAlreadyExistsInTableStorageException e)
            {
                throw;
            }
        }
    }
}
