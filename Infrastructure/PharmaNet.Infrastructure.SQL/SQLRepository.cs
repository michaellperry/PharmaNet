using System.Data.Entity;
using System.Linq;
using PharmaNet.Infrastructure.Repository;

namespace PharmaNet.Infrastructure.SQL
{
    public class SQLRepository<T> : IRepository<T>
        where T : class
    {
        private readonly DbContext _context;
        private readonly DbSet<T> _dbSet;

        public SQLRepository(DbContext context, DbSet<T> dbSet)
        {
            _context = context;
            _dbSet = dbSet;
        }

        public T Add(T item)
        {
            _dbSet.Add(item);
            return item;
        }

        public IQueryable<T> GetAll()
        {
            return _dbSet;
        }

        public void Remove(T item)
        {
            _dbSet.Remove(item);
        }

        public void SaveChanges()
        {
            _context.SaveChanges();
        }
    }
}
