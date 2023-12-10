using System.Linq.Expressions;
using System.Threading.Tasks.Dataflow;

namespace VezeataApplication.Core.Interfaces
{
    public interface IRepository<T> where T : class
    {
        Task<IEnumerable<T>> GetAllAsync();
        Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate);
        Task<T> GetByIdAsync(int id);
        Task AddAsync(T entity);
        Task AddRangeAsync(IEnumerable<T> entities);
        void Update(T entity);
        Task<bool> Remove(int id);
        void RemoveRange(IEnumerable<T> entities);
        Task<int> GetCountAsync();
    }
}
