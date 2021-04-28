
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using CarStocks.Entities;
using CarStocks.Test.Common;
using Moq;
using Newtonsoft.Json;

using Xunit;

namespace CarStocks.Test.IntegrationTests
{
    [Collection("CarCrud")]
    public class CarCreateTest : IClassFixture<TestAppFactory<Startup>>
    {
        private readonly HttpClient _client;

        public CarCreateTest(TestAppFactory<Startup> factory)
        {
            _client = factory.CreateClient();
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Token", "abc");
        }

        [Fact]
        public async Task Create_Returns_201()
        {
            var car = new Car()
            {
                Make = "Toyota",
                Model = "RAV",
                Year = 2015
            };

            var httpResponse = await _client.PostAsync("/car", ConstructPostData(car));

            Assert.Equal(HttpStatusCode.Created, httpResponse.StatusCode);
        }

        [Fact]
        public async Task Create_Works()
        {
            var testedCar = new Car()
            {
                Make = "Toyota",
                Model = "RAV",
                Year = 2015,
                Stock = It.IsAny<int>()
            };

            var creationResponse = await _client.PostAsync("/car", ConstructPostData(testedCar));

            Assert.Equal(HttpStatusCode.Created, creationResponse.StatusCode);

            var createdCar = JsonConvert.DeserializeObject<Car>(await creationResponse.Content.ReadAsStringAsync());

            Assert.NotEqual(0, createdCar.Id);

            var getResponse = await _client.GetAsync($"/car/{createdCar.Id}");

            Assert.Equal(HttpStatusCode.OK, getResponse.StatusCode);

            var persistedCar = JsonConvert.DeserializeObject<Car>(await getResponse.Content.ReadAsStringAsync());

            Assert.Equal(testedCar.Make, persistedCar.Make);
            Assert.Equal(testedCar.Model, persistedCar.Model);
            Assert.Equal(testedCar.Year, persistedCar.Year);
            Assert.Equal(testedCar.Stock, persistedCar.Stock);
            Assert.Equal(1, persistedCar.DealerId);
        }

        [Fact]
        public async Task Create_Returns_400_With_Empty_Make()
        {
            var car = new Car()
            {
                Model = "RAV",
                Year = 2015,
                Stock = It.IsAny<int>()
            };

            var response = await _client.PostAsync("/car", ConstructPostData(car));

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);

            var responseBody = JsonConvert.DeserializeObject<dynamic>(await response.Content.ReadAsStringAsync());

            Assert.NotNull(responseBody);
            Assert.Equal("The Make field is required.", responseBody.errors.Make[0].ToString());
        }


        [Fact]
        public async Task Create_Returns_400_With_Empty_Model()
        {
            var car = new Car()
            {
                Make = "Toyota",
                Year = 2015,
                Stock = It.IsAny<int>()
            };

            var response = await _client.PostAsync("/car", ConstructPostData(car));

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);

            var responseBody = JsonConvert.DeserializeObject<dynamic>(await response.Content.ReadAsStringAsync());

            Assert.NotNull(responseBody);
            Assert.Equal("The Model field is required.", responseBody.errors.Model[0].ToString());
        }


        [Fact]
        public async Task Create_Returns_400_With_Incorrect_Year()
        {
            var car = new Car()
            {
                Model = "RAV",
                Make = "Toyota",
                Year = 999,
                Stock = It.IsAny<int>()
            };

            var response = await _client.PostAsync("/car", ConstructPostData(car));

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);

            var responseBody = JsonConvert.DeserializeObject<dynamic>(await response.Content.ReadAsStringAsync());

            Assert.NotNull(responseBody);
            Assert.True(responseBody.errors.Year[0].ToString().StartsWith("The field Year must be between"));
        }

        private HttpContent ConstructPostData(Car car)
        {
            var content = new StringContent(JsonConvert.SerializeObject(car));
            content.Headers.ContentType = MediaTypeHeaderValue.Parse("application/json");

            return content;
        }
    }
}