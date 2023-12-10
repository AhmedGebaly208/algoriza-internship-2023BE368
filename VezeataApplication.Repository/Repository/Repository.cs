using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using VezeataApplication.Core.Interfaces;

namespace VezeataApplication.Repository.Repository
{
    public class Repository<T> : IRepository<T> where T : class
    {
        protected ApplicationDbContext _context;
        private readonly DbSet<T> _dbSet;

        public Repository(ApplicationDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _dbSet = _context.Set<T>();
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _dbSet.ToListAsync();
        }

        public async Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate)
        {
            return await _dbSet.Where(predicate).ToListAsync();
        }

        public async Task<T> GetByIdAsync(int id)
        {
            return await _dbSet.FindAsync(id);
        }

        public async Task AddAsync(T entity)
        {
            await _dbSet.AddAsync(entity);
        }

        public async Task AddRangeAsync(IEnumerable<T> entities)
        {
            await _dbSet.AddRangeAsync(entities);
        }

        public void Update(T entity)
        {
            _context.Entry(entity).State = EntityState.Modified;
        }

        public async Task<bool> Remove(int id)
        {
            var result = await _dbSet.FindAsync(id);
            if (result == null)
                return false;

            _context.Remove(result);
            return true;
        }

        public void RemoveRange(IEnumerable<T> entities)
        {
            _dbSet.RemoveRange(entities);
        }

        public async Task<int> GetCountAsync()
        {
            var result = await _dbSet.ToListAsync();
            return result.Count;
        }
    }
}
