﻿using System;
using System.Collections.Generic;

namespace ToggleAPI.Models
{
    /// <summary>
    /// Represents a Toggle entity.
    /// A toogle can have multiple configurations.
    /// </summary>
    public class Toggle
    {
        public Guid Id { get; set; }
        public long Version { get; set; }
        public String Name { get; set; }
        public ICollection<Configuration> Configurations { get; set; }
        public SystemUser User { get; set; }

        public Toggle()
        {
            Configurations = new List<Configuration>();
        }
    }
}
