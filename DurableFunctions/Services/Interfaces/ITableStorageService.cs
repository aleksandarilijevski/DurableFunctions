using DurableFunctions.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DurableFunctions.Services.Interfaces
{
    public interface ITableStorageService
    {
        public Task AddObject(DummyObject dummyObject);

        public Task<List<DummyObject>> GetAllObjects();

        public Task DeleteObject(DummyObject dummyObject);
    }
}
