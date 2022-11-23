using System.Linq.Expressions;

namespace HotelComercy_WebAPI.Repository.IRepository
{
    public interface IRepository<T> where T : class
    {
        IEnumerable<T> GetAll(Expression<Func<T,bool>>? filter = null, string? includeProperties = null);
        void Add(T entity);
        T GetFirstOrDefault(Expression<Func<T,bool>> filter, string? includeProperties = null);
        T GetById(int id);
        void Remove(T entity);
        void RemoveRange(IEnumerable<T> entities);
    }
}
