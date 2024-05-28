using DurableFunctions.Exceptions;
using DurableFunctions.Models.Dto;
using DurableFunctions.Services.Interfaces;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using System;
using System.Threading.Tasks;

namespace DurableFunctions.Activities
{
    public class AddObjectToTableStorageActivity
    {
        private ITableStorageService _tableStorageService;

        public AddObjectToTableStorageActivity(ITableStorageService tableStorageService)
        {
            _tableStorageService = tableStorageService;
        }

        [FunctionName(nameof(AddObjectToTableStorageActivity))]
        public async Task Run([ActivityTrigger] DummyObjectTableEntity dummyObjectDto)
        {

            try
            {
                await _tableStorageService.AddObject(dummyObjectDto);
            }
            catch (Exception e)
            {
                if (e.Message.Contains("This specified entity already exists"))
                {
                    throw new EntityAlreadyExistsInTableStorageException("Entity with entered ID already exists!");
                }
                else
                {
                    throw;
                }
            }
        }
    }
}
