using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using ToggleAPI.Interface;
using ToggleAPI.Models;

namespace ToggleAPI.DAL
{
    public class ToggleRepository : IToggleRepository
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

        public IEnumerable<Toggle> GetTogglesForSystem(string systemName)
        {
            var tooglesWithSystemName =  _context.ToggleItems.Include(t => t.Configurations)
                .Where(t => t.Configurations.Any(c => systemName.Equals(c.SystemName) || "*".Equals(c.SystemName))).ToList();

            return FilterConfigurationsBySystemNameOrDefault(systemName, tooglesWithSystemName);
        }

        private List<Toggle> FilterConfigurationsBySystemNameOrDefault(string systemName, List<Toggle> toggles)
        {
            toggles.ForEach(t =>
            {
                if (t.Configurations.Count > 1)
                {
                    var specificConfiguration = t.Configurations.FirstOrDefault(c => c.SystemName == systemName) ??
                                                t.Configurations.FirstOrDefault(c => c.SystemName == "*");
                    t.Configurations = new List<Configuration> {specificConfiguration};
                }
            });

            return toggles;
        }

        public Toggle Update(Toggle item)
        {
            item.Version++;
            return item;
        }
    }
}
