using System;
using System.Linq;

namespace ToogleAPI.Interface
{
    public interface IRepository<TEntity> where TEntity:class
    {
        TEntity Get(Guid key);
        IQueryable<TEntity> GetAll();
        TEntity Add(TEntity item);
        TEntity Update(TEntity item);
        void Remove(Guid key);
        bool Contains(Guid key);
        void Save();
    }
}
