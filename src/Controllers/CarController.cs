using CarStocks.Common;
using CarStocks.Entities;
using CarStocks.Repositories;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;


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

        [HttpGet]
        [Route("{id}")]
        public Car Get(int id) {
            var car = this._carRepository.Get(id);

            if(car==null) {
                Response.StatusCode = (int)HttpStatusCode.NotFound;

                return null;
            }

            if(car.DealerId != _authDealerId) {
                Response.StatusCode = (int)HttpStatusCode.Unauthorized;

                return null;
            }

            return car;
        }

        [HttpGet]
        [Route("search")]
        public IEnumerable<Car> Search([FromQuery]string make, [FromQuery]string model)
        {
            return this._carRepository.GetAll(_authDealerId, make, model);
        }

        [HttpPost]
        public void Post([FromBody] Car car)
        {
            // TODO: verify values -> make, model non-empty, year is valid year, stock does not go negative?

            car.DealerId = _authDealerId;

            this._carRepository.Insert(car);

            Response.StatusCode = (int)HttpStatusCode.Created;
        }

        [HttpPut("{id}")]
        public void Put(int id, [FromBody] int stock)
        {
            var existingRecord = this._carRepository.Get(id);

            if(existingRecord==null) {
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

        [HttpDelete("{id}")]
        public void Delete(int id)
        {
            var existingRecord = this._carRepository.Get(id);
            
            if(existingRecord==null) {
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
