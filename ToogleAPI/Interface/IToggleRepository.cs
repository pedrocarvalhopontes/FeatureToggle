using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ToggleAPI.Models;

namespace ToggleAPI.Interface
{
    public interface IToggleRepository:IRepository<Toggle>
    {
        IEnumerable<Toggle> GetTooglesForSystem(string systemName);
    }
}
