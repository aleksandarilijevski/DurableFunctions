using DurableFunctions.Models;
using System.Threading.Tasks;

namespace DurableFunctions.Services.Interfaces
{
    public interface ICosmosDBService
    {
        public Task AddObject(DummyObject dummyObject);

        public Task<DummyObject> GetObjectByID(string id);
    }
}
