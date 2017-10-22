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

            ///TODO:review
            if (!context.ToggleItems.Any())
            {
                ContextInitializer.Initialize(context);
            }
        }

        public Toggle Add(Toggle item)
        {
            var entityEntry = _context.ToggleItems.Add(item);
            return entityEntry.Entity;
        }

        public Toggle Get(long key)
        {
            return _context.ToggleItems.FirstOrDefault(t => t.Id == key);
        }

        public IQueryable<Toggle> GetAll()
        {
            return _context.ToggleItems;
        }

        public void Remove(long key)
        {
            var item = Get(key);
            _context.Remove(item);
        }

        public void Save()
        {
            _context.SaveChanges();
        }

        public Toggle Update(Toggle item)
        {
            var toggle = Get(item.Id);

            toggle.Name = item.Name;
            toggle.Value = item.Value;
            toggle.Version = toggle.Version++;

            _context.ToggleItems.Update(item);

            return toggle;
        }
    }
}
