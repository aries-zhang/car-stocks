using CarStocks.Common;
using CarStocks.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarStocks.Repositories
{
    public interface ICarRepository : IRepository<Car>
    {
        List<Car> GetAll(int dealder, string make, string model);
    }
}
