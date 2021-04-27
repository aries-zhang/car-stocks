using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;

namespace CarStocks.Common
{
    public interface IRepository<T> where T : IEntity
    {
        T Insert(T entity);
        void Delete(T entity);
        T Update(T entity);
        T Get(int id);
    }
}
