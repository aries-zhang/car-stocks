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

        private int _dealerId
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
        public IEnumerable<Car> Get([FromQuery]string make, [FromQuery]string model)
        {
            return this._carRepository.GetAll(_dealerId, make, model);
        }

        [HttpPost]
        public void Post([FromBody] Car car)
        {
            car.DealerId = _dealerId;

            this._carRepository.Insert(car);
        }

        [HttpPut("{id}")]
        public void Put(int id, [FromBody] int stock)
        {
            var existingRecord = this._carRepository.Get(id);

            if (existingRecord.DealerId != _dealerId)
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

            if (existingRecord.DealerId != _dealerId)
            {
                Response.StatusCode = (int)HttpStatusCode.Unauthorized;

                return;
            }

            this._carRepository.Delete(existingRecord);
        }

    }
}
