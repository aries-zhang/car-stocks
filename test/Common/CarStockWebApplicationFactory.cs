using System;
using CarStocks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using CarStocks.Common;

namespace CarStocks.Test.Common
{
    public class CarStockWebApplicationFactory<TStartup> : WebApplicationFactory<Startup>
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureAppConfiguration(config =>
            {
                var integrationConfig = new ConfigurationBuilder()
                .AddJsonFile("appsettings.Integration.json")
                .Build();

                config.AddConfiguration(integrationConfig);
            });

            builder.ConfigureServices(services =>
            {
                // Build the service provider.
                var sp = services.BuildServiceProvider();

                // Create a scope to obtain a reference to the database contexts
                using (var scope = sp.CreateScope())
                {
                    var scopedServices = scope.ServiceProvider;

                    var logger = scopedServices.GetRequiredService<ILogger<CarStockWebApplicationFactory<TStartup>>>();
                    var config = scopedServices.GetRequiredService<IConfiguration>();

                    DatabaseInitialiser.Initialise(config);
                }
            });
        }
    }
}