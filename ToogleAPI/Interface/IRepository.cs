using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace ToogleAPI.Interface
{
    public interface IRepository<TEntity> where TEntity:class
    {
        TEntity Get(long key);
        IQueryable<TEntity> GetAll();
        TEntity Add(TEntity item);
        TEntity Update(TEntity item);
        void Remove(long key);
        void Save();
    }
}
