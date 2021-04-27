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
        }
    }
}