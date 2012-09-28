using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PharmaNet.Infrastructure.Repository
{
    public class FakeRepository<T> : IRepository<T>
    {
        private List<T> _collection = new List<T>();

        public void Add(T item)
        {
            _collection.Add(item);
        }

        public void Remove(T item)
        {
            _collection.Remove(item);
        }

        public IQueryable<T> GetAll()
        {
            return _collection.AsQueryable();
        }
    }
}
