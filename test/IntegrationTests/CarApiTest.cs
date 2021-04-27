
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
    public class CarApiTest : IClassFixture<CarStockWebApplicationFactory<Startup>>
    {
        private readonly HttpClient _client;

        public CarApiTest(CarStockWebApplicationFactory<Startup> factory)
        {
            _client = factory.CreateClient();
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Token", "abc");
        }

        [Fact]
        public async Task Search_Returns()
        {
            // The endpoint or route of the controller action.
            var httpResponse = await _client.GetAsync("/car/search");

            // Must be successful.
            httpResponse.EnsureSuccessStatusCode();
        }
    }
}