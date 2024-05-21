using DurableFunctions.Services;
using DurableFunctions.Services.Interfaces;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;

[assembly: FunctionsStartup(typeof(DurableFunctions.Startup))]

namespace DurableFunctions
{
    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            builder.Services.AddTransient<ITableStorageService, TableStorageService>();
            builder.Services.AddTransient<ICosmosDBService, CosmosDBService>();
            builder.Services.AddAutoMapper(typeof(Startup));
        }
    }
}
