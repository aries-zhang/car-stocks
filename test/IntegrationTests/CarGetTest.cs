
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using CarStocks.Entities;
using CarStocks.Test.Common;
using Moq;
using Newtonsoft.Json;

using Xunit;

namespace CarStocks.Test.IntegrationTests
{
    [Collection("CarCrud")]
    public class CarGetTest : IClassFixture<TestAppFactory<Startup>>
    {
        private readonly HttpClient _client;

        public CarGetTest(TestAppFactory<Startup> factory)
        {
            _client = factory.CreateClient();
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Token", "abc");

            PrepareTestData();
        }

        [Fact]
        public async Task Get_Returns()
        {
            var id = 1;
            var httpResponse = await _client.GetAsync($"/car/{id}");

            httpResponse.EnsureSuccessStatusCode();
            var result = JsonConvert.DeserializeObject<Car>(await httpResponse.Content.ReadAsStringAsync());

            Assert.NotNull(result);
            Assert.Equal(id, result.Id);
        }

        private void PrepareTestData()
        {
            var cars = new[] { new Car()
            {
                Make = "Toyota",
                Year = 2015,
                Stock = It.IsAny<int>()
            }};

            cars.ToList().ForEach(async car =>
            {
                var content = new StringContent(JsonConvert.SerializeObject(car));
                content.Headers.ContentType = MediaTypeHeaderValue.Parse("application/json");

                await _client.PostAsync("/car", content);
            });
        }
    }
}