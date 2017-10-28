using System;
using System.Linq;

namespace ToggleAPI.Interface
{
    /// <summary>
    /// Definition of the contract to the implementation of the repository pattern.
    /// </summary>
    /// <typeparam name="TEntity">Type of entities supported by the repository</typeparam>
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
