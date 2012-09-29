using System.Data.Entity;
using System.Linq;
using PharmaNet.Infrastructure.Repository;

namespace PharmaNet.Fulfillment.SQL
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
            _context.SaveChanges();
            return item;
        }

        public IQueryable<T> GetAll()
        {
            return _dbSet;
        }

        public void Remove(T item)
        {
            _dbSet.Remove(item);
            _context.SaveChanges();
        }
    }
}
