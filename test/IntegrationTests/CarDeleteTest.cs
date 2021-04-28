
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
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
    public class CarDeleteTest : IClassFixture<TestAppFactory<Startup>>
    {
        private readonly HttpClient _client;

        public CarDeleteTest(TestAppFactory<Startup> factory)
        {
            _client = factory.CreateClient();
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Token", "abc");
        }

        [Fact]
        public async Task Delete_Works()
        {
            var car = new Car()
            {
                Make = "Toyota",
                Model = "Coralla",
                Year = 2015,
                Stock = It.IsAny<int>()
            };

            var content = new StringContent(JsonConvert.SerializeObject(car));
            content.Headers.ContentType = MediaTypeHeaderValue.Parse("application/json");

            var response = await _client.PostAsync("/car", content);
            response.EnsureSuccessStatusCode();

            var responseBody = await response.Content.ReadAsStringAsync();

            car = JsonConvert.DeserializeObject<Car>(responseBody);

            Assert.NotNull(car);
            Assert.NotEqual(0, car.Id);


            var deletionResponse = await _client.DeleteAsync($"/car/{car.Id}");

            deletionResponse.EnsureSuccessStatusCode();

            var getResponse = await _client.GetAsync($"/car/{car.Id}");

            Assert.Equal(HttpStatusCode.NotFound, getResponse.StatusCode);
        }
    }
}