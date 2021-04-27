using System.Collections.Generic;

using CarStocks.Common;
using CarStocks.Entities;

namespace CarStocks.Repositories
{
    public interface ICarRepository : IRepository<Car>
    {
        List<Car> GetAll(int dealder, string make, string model);
    }
}
