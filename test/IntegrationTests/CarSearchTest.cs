
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using CarStocks.Entities;
using CarStocks.Test.Common;
using Newtonsoft.Json;
using System.Collections;

using Xunit;
using System.Linq;

namespace CarStocks.Test.IntegrationTests
{
    [Collection("CarCrud")]
    public class CarSearchTest : IClassFixture<TestAppFactory<Startup>>
    {
        private readonly HttpClient _client;

        public CarSearchTest(TestAppFactory<Startup> factory)
        {
            _client = factory.CreateClient();
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Token", "abc");
            PrepareTestData();
        }

        [Fact]
        public async Task Search_Returns()
        {
            var response = await _client.GetAsync("/car/search");

            response.EnsureSuccessStatusCode();

            var searchResult = JsonConvert.DeserializeObject<Car[]>(await response.Content.ReadAsStringAsync());

            Assert.True(searchResult.All(car => car.DealerId == 1));
            Assert.True(searchResult.Any(car => _testData.Any(t => t.Make == car.Make && t.Model == car.Model && t.Year == car.Year)));
        }

        [Fact]
        public async Task Search_Returns_With_Conditions()
        {
            var response = await _client.GetAsync($"/car/search?make={_testData[0].Make}&model={_testData[0].Model}");

            // response.EnsureSuccessStatusCode();

            var searchResult = JsonConvert.DeserializeObject<Car[]>(await response.Content.ReadAsStringAsync());

            Assert.True(searchResult.Length > 0);
            Assert.Equal(_testData[0].Make, searchResult[0].Make);
            Assert.Equal(_testData[0].Model, searchResult[0].Model);
        }

        private Car[] _testData = new[] {
            new Car() {Make="BMW", Model="X5", Year=2000},
            new Car() {Make="Tesla", Model="Model X", Year=2000},
            new Car() {Make="Toyota", Model="Camry", Year=2000},
            new Car() {Make="Toyota", Model="Kluger", Year=2000},
            new Car() {Make="Mini", Model="Cooper", Year=2000},
        };

        private void PrepareTestData()
        {
            _testData.ToList().ForEach(async car => await _client.SendAsync(ConstructRequest(car, "abc")));
            _testData.ToList().ForEach(async car => await _client.SendAsync(ConstructRequest(car, "def")));
        }


        private HttpRequestMessage ConstructRequest(Car car, string dealerToken)
        {
            var content = new StringContent(JsonConvert.SerializeObject(car));
            content.Headers.ContentType = MediaTypeHeaderValue.Parse("application/json");

            var request = new HttpRequestMessage(HttpMethod.Post, "/car")
            {
                Content = content
            };
            request.Headers.Authorization = new AuthenticationHeaderValue("Token", dealerToken);

            return request;
        }
    }
}