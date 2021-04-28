
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using CarStocks.Entities;
using CarStocks.Test.Common;
using Newtonsoft.Json;

using Xunit;

namespace CarStocks.Test.IntegrationTests
{
    [Collection("CarCrud")]
    public class StockUpdateTest : IClassFixture<TestAppFactory<Startup>>
    {
        private readonly HttpClient _client;

        public StockUpdateTest(TestAppFactory<Startup> factory)
        {
            _client = factory.CreateClient();
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Token", "abc");
        }

        [Fact]
        public async Task Update_Stock_Works()
        {
            var car = new Car()
            {
                Make = "Toyota",
                Model = "Coralla",
                Year = 2015,
                Stock = 1
            };

            var content = new StringContent(JsonConvert.SerializeObject(car));
            content.Headers.ContentType = MediaTypeHeaderValue.Parse("application/json");

            var response = await _client.PostAsync("/car", content);

            response.EnsureSuccessStatusCode();

            var responseBody = await response.Content.ReadAsStringAsync();

            car = JsonConvert.DeserializeObject<Car>(responseBody);

            Assert.NotNull(car);
            Assert.NotEqual(0, car.Id);

            var updatedStock = 1234;
            var updateContent = new StringContent(updatedStock.ToString());
            updateContent.Headers.ContentType = MediaTypeHeaderValue.Parse("application/json");

            var updateResponse = await _client.PutAsync($"/car/{car.Id}", updateContent);

            var getResponse = await _client.GetAsync($"/car/{car.Id}");
            var fetchedCar = JsonConvert.DeserializeObject<Car>(await getResponse.Content.ReadAsStringAsync());

            // Assert.NotNull(fetchedCar);
            Assert.Equal(updatedStock, fetchedCar.Stock);
        }
    }
}