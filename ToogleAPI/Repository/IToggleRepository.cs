﻿using System.Collections.Generic;
using ToggleAPI.Models;

namespace ToggleAPI.Repository
{
    /// <summary>
    /// Specialization of the IRepository interface for a toggle.
    /// </summary>
    public interface IToggleRepository:IRepository<Toggle>
    {
        IEnumerable<Toggle> GetTogglesForSystem(string systemName);
    }
}
