using HotelComercy_WebAPI.Pagination;
using System.Linq.Expressions;

namespace HotelComercy_WebAPI.Repository.IRepository
{
    public interface IRepository<T> where T : class
    {
        Task<IEnumerable<T>> GetAllAsync(Expression<Func<T,bool>>? filter = null, string? includeProperties = null, DefaultPagination? pagination = null);
        Task AddAsync(T entity);
        Task<T> GetFirstOrDefaultAsync(Expression<Func<T,bool>> filter, bool tracked = true, string? includeProperties = null);
        Task<T> GetByIdAsync(int id);
        void Remove(T entity);
        void RemoveRange(IEnumerable<T> entities);
    }
}
