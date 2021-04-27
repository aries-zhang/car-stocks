using System.Linq;
using System.Net;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using CarStocks.Common;
using CarStocks.Controllers;
using CarStocks.Entities;
using CarStocks.Repositories;

using Moq;

using Xunit;

namespace CarStocks.Test.UnitTests
{
    public class CarControllerTest
    {
        private Mock<ICarRepository> _mock;

        public CarControllerTest()
        {
            var car = new Car()
            {
                Id = 1,
                Make = "Audi",
                Model = "A6",
                Year = 2010,
                DealerId = 1
            };

            _mock = new Mock<ICarRepository>();
            _mock.Setup(o => o.GetAll(It.IsAny<int>(), It.IsAny<string>(), It.IsAny<string>())).Returns((new[] { car }).ToList());
            _mock.Setup(o => o.Update(It.IsAny<Car>())).Returns<Car>(car => car);
            _mock.Setup(o => o.Get(It.IsAny<int>())).Returns(car);
            _mock.Setup(o => o.Delete(It.IsAny<Car>()));
            _mock.Setup(o => o.Insert(It.IsAny<Car>())).Returns<Car>(car => car);
        }

        private CarController MockIt()
        {
            var controller = new CarController(_mock.Object);

            return controller;
        }

        private void MockHttpContext(ControllerBase controller, int dealerId)
        {
            controller.ControllerContext = new ControllerContext();
            controller.ControllerContext.HttpContext = new DefaultHttpContext();
            controller.ControllerContext.HttpContext.Items[Constants.DEALER_ID_CONTEXT_KEY] = dealerId;
        }

        [Fact]
        public void Get_Returns_Correct_Collection()
        {
            var authDealerId = It.IsAny<int>();

            var controller = MockIt();
            MockHttpContext(controller, authDealerId);

            var cars = controller.Search(string.Empty, string.Empty);

            _mock.Verify(o => o.GetAll(
                It.Is<int>(i => i == authDealerId),
                It.Is<string>(s => string.IsNullOrEmpty(s)),
                It.Is<string>(s => string.IsNullOrEmpty(s))
            ), Times.Once);

            Assert.True(1 == cars.Count());
        }

        [Fact]
        public void Post_Assigns_Correct_Dealer_Id()
        {
            var authDealerId = It.IsAny<int>();

            var controller = MockIt();
            MockHttpContext(controller, authDealerId);

            controller.Post(new Car());

            _mock.Verify(o => o.Insert(It.Is<Car>(c => c.DealerId == authDealerId)), Times.Once);
        }

        [Fact]
        public void Put_Returns_401_With_Mismatching_Dealer_Auth()
        {
            var authDealerId = 2;

            var controller = MockIt();
            MockHttpContext(controller, authDealerId);

            controller.Put(1, 0);

            Assert.Equal((int)HttpStatusCode.Unauthorized, controller.HttpContext.Response.StatusCode);
        }

        [Fact]
        public void Put_Returns_404_When_No_Record_Found()
        {
            var authDealerId = 1;

            var mock = new Mock<ICarRepository>();
            mock.Setup(o => o.Get(It.IsAny<int>())).Returns<Car>(null);

            var controller = new CarController(mock.Object);
            MockHttpContext(controller, authDealerId);

            controller.Put(1, 0);

            mock.Verify(o => o.Get(It.IsAny<int>()), Times.Once);
            mock.Verify(o => o.Update(It.IsAny<Car>()), Times.Never);

            Assert.Equal((int)HttpStatusCode.NotFound, controller.HttpContext.Response.StatusCode);
        }


        [Fact]
        public void Delete_Returns_401_With_Mismatching_Dealer_Auth()
        {
            var authDealerId = 2;

            var controller = MockIt();
            MockHttpContext(controller, authDealerId);

            controller.Delete(1);

            Assert.Equal((int)HttpStatusCode.Unauthorized, controller.HttpContext.Response.StatusCode);
        }

        [Fact]
        public void Delete_Returns_404_When_No_Record_Found()
        {
            var authDealerId = 1;

            var mock = new Mock<ICarRepository>();
            mock.Setup(o => o.Get(It.IsAny<int>())).Returns<Car>(null);

            var controller = new CarController(mock.Object);
            MockHttpContext(controller, authDealerId);

            controller.Delete(It.IsAny<int>());

            mock.Verify(o => o.Get(It.IsAny<int>()), Times.Once);
            mock.Verify(o => o.Delete(It.IsAny<Car>()), Times.Never);

            Assert.Equal((int)HttpStatusCode.NotFound, controller.HttpContext.Response.StatusCode);
        }
    }
}
