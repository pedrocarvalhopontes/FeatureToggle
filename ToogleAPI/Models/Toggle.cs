using System;
using System.Collections.Generic;

namespace ToogleAPI.Models
{
    public class Toggle
    {
        public Guid Id { get; set; }
        public long Version { get; set; }
        public String Name { get; set; }

        public ICollection<Configuration> Configurations { get; set; }
    }
}
