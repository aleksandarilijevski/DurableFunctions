using DurableFunctions.Models;
using DurableFunctions.Models.Dto;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DurableFunctions.Services.Interfaces
{
    public interface ITableStorageService
    {
        public Task AddObject(DummyObjectTableEntity dummyObjectDto);

        public Task<List<DummyObjectTableEntity>> GetAllObjects();

        public Task DeleteObject(DummyObjectTableEntity dummyObjectDto);
    }
}
