using System;
using System.Collections.Generic;
using System.Linq;

namespace PharmaNet.Infrastructure.Repository
{
    public interface IRepository<T>
    {
        void Add(T item);
        void Remove(T item);
        IQueryable<T> GetAll();
    }
}
