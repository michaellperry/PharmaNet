using System;
using System.Collections.Generic;
using System.Linq;

namespace PharmaNet.Infrastructure.Repository
{
    public interface IRepository<T>
    {
        T Add(T item);
        void Remove(T item);
        IQueryable<T> GetAll();
        void SaveChanges();
    }
}
