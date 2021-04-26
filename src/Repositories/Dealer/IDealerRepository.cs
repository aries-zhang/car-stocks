using CarStocks.Common;
using CarStocks.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarStocks.Repositories
{
    public interface IDealerRepository : IRepository<Dealer>
    {
        Dealer GetByToken(string token);
    }
}
