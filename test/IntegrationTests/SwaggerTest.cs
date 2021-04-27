
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using CarStocks.Test.Common;
using Newtonsoft.Json;

using Xunit;

namespace CarStocks.Test.IntegrationTests
{
    public class SwaggerTest : IClassFixture<CarStockWebApplicationFactory<Startup>>
    {
        private readonly HttpClient _client;

        public SwaggerTest(CarStockWebApplicationFactory<Startup> factory)
        {
            _client = factory.CreateClient();
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Token", "abc");
        }

        [Fact]
        public async Task Swagger_Works()
        {
            var httpResponse = await _client.GetAsync("/swagger");

            httpResponse.EnsureSuccessStatusCode();
        }
    }
}