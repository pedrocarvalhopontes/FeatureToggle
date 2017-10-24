using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using ToogleAPI.Interface;
using ToogleAPI.Models;

namespace ToogleAPI.DAL
{
    public class ToggleRepository : IRepository<Toggle>
    {
        private readonly ToggleContext _context;

        public ToggleRepository(ToggleContext context)
        {
            _context = context;
        }

        public Toggle Add(Toggle item)
        {
            var entityEntry = _context.ToggleItems.Add(item);
            return entityEntry.Entity;
        }

        public Toggle Get(Guid key)
        {
            return _context.ToggleItems.Include(t => t.Configurations).FirstOrDefault(t => t.Id == key);
        }

        public IQueryable<Toggle> GetAll()
        {
            return _context.ToggleItems.Include(t => t.Configurations);
        }

        public void Remove(Guid key)
        {
            var item = Get(key);
            _context.Remove(item);
        }

        public bool Contains(Guid key)
        {
            return _context.ToggleItems.Any(t => t.Id == key);
        }

        public void Save()
        {
            _context.SaveChanges();
        }

        //TODO:review
        public Toggle Update(Toggle item)
        {
            item.Version = item.Version++;
            return item;
        }
    }
}
