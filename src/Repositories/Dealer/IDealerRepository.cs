using CarStocks.Common;
using CarStocks.Entities;

namespace CarStocks.Repositories
{
    public interface IDealerRepository : IRepository<Dealer>
    {
        Dealer GetByToken(string token);
    }
}
