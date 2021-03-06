using System;
using System.Linq;

using Microsoft.Extensions.Configuration;

using CarStocks.Common;
using CarStocks.Entities;

using Dapper;

namespace CarStocks.Repositories
{
    public class DealerRepository : BaseRepository, IDealerRepository
    {
        public DealerRepository(IConfiguration config) : base(config)
        {
        }

        public Dealer GetByToken(string token)
        {
            using var db = this.GetDbconnection();

            return db.Query<Dealer>("SELECT * FROM Dealer WHERE Token = @token", new { token }).SingleOrDefault();
        }

        public void Delete(Dealer entity)
        {
            throw new NotImplementedException();
        }

        public Dealer Get(int id)
        {
            throw new NotImplementedException();
        }

        public Dealer Insert(Dealer entity)
        {
            throw new NotImplementedException();
        }

        public Dealer Update(Dealer entity)
        {
            throw new NotImplementedException();
        }
    }
}
