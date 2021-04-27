using System.Collections.Generic;

using System.Net;

using Microsoft.AspNetCore.Mvc;

using CarStocks.Common;
using CarStocks.Entities;
using CarStocks.Repositories;



namespace CarStocks.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class CarController : ControllerBase
    {
        private ICarRepository _carRepository;

        private int _authDealerId
        {
            get
            {
                return (int)HttpContext.Items[Constants.DEALER_ID_CONTEXT_KEY];
            }
        }

        public CarController(ICarRepository carRepository)
        {
            this._carRepository = carRepository;
        }

        /// <summary>
        /// Gets a single car.
        /// </summary>
        /// <param name="id"></param>
        /// <returns>A car with the specified Id.</returns>
        /// <response code="200">Returns the car.</response>
        /// <response code="404">If the specified car does not exist.</response>
        /// <response code="407">If Authorisation header is invalid, or data not in auth scope.</response>
        [HttpGet]
        [Route("{id}")]
        public Car Get(int id)
        {
            var car = this._carRepository.Get(id);

            if (car == null)
            {
                Response.StatusCode = (int)HttpStatusCode.NotFound;

                return null;
            }

            if (car.DealerId != _authDealerId)
            {
                Response.StatusCode = (int)HttpStatusCode.Unauthorized;

                return null;
            }

            return car;
        }

        /// <summary>
        /// Search cars by make and model.
        /// </summary>
        /// <param name="make"></param>
        /// <param name="model"></param>
        /// <returns>List of cars that matches the search parameters.</returns>
        /// <response code="200">Returns the list of cars.</response>
        [HttpGet]
        [Route("search")]
        public IEnumerable<Car> Search([FromQuery] string make, [FromQuery] string model)
        {
            return this._carRepository.GetAll(_authDealerId, make, model);
        }

        /// <summary>
        /// Add a new car.
        /// </summary>
        /// <param name="car"></param>
        /// <response code="201">If the car is successfully created.</response>
        /// <response code="407">If Authorisation header is invalid, or data not in auth scope.</response>
        [HttpPost]
        public void Post([FromBody] Car car)
        {
            // TODO: verify values -> make, model non-empty, year is valid year, stock does not go negative?

            car.DealerId = _authDealerId;

            this._carRepository.Insert(car);

            Response.StatusCode = (int)HttpStatusCode.Created;
        }

        /// <summary>
        /// Update stock level of the specified car by id.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="stock"></param>
        /// <response code="200">If the car is successfully updated.</response>
        /// <response code="407">If Authorisation header is invalid, or data not in auth scope.</response>
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] int stock)
        {
            var existingRecord = this._carRepository.Get(id);

            if (existingRecord == null)
            {
                Response.StatusCode = (int)HttpStatusCode.NotFound;

                return;
            }

            if (existingRecord.DealerId != _authDealerId)
            {
                Response.StatusCode = (int)HttpStatusCode.Unauthorized;

                return;
            }

            existingRecord.Stock = stock;

            this._carRepository.Update(existingRecord);
        }

        /// <summary>
        /// Delete the specified car by id.
        /// </summary>
        /// <param name="id"></param>
        /// <response code="200">If the car is successfully updated.</response>
        /// <response code="404">If the specified car does not exist.</response>
        /// <response code="407">If Authorisation header is invalid, or data not in auth scope.</response>
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
            var existingRecord = this._carRepository.Get(id);

            if (existingRecord == null)
            {
                Response.StatusCode = (int)HttpStatusCode.NotFound;

                return;
            }

            if (existingRecord.DealerId != _authDealerId)
            {
                Response.StatusCode = (int)HttpStatusCode.Unauthorized;

                return;
            }

            this._carRepository.Delete(existingRecord);
        }
    }
}
