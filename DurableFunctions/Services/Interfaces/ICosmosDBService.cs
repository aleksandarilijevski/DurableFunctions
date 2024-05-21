using DurableFunctions.Models;
using DurableFunctions.Models.Dto;
using System.Threading.Tasks;

namespace DurableFunctions.Services.Interfaces
{
    public interface ICosmosDBService
    {
        public Task AddObject(DummyObjectTableEntity dummyObjectDto);

        public Task<DummyObjectTableEntity> GetObjectByID(string id);
    }
}
