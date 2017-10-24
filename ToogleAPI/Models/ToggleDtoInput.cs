using System;
using System.Collections.Generic;

namespace ToogleAPI.Models
{
    public class ToggleDtoInput
    {
        public long Version { get; set; }
        public String Name { get; set; }

        public ICollection<ConfigurationDtoInput> Configurations { get; set; }
    }
}
