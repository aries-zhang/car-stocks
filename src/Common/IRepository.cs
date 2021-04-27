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
