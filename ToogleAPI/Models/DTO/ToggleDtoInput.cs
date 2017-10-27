using System;
using System.Collections.Generic;

namespace ToggleAPI.Models.DTO
{
    /// <summary>
    /// Dto used for input operations.
    /// Mapped between DTOs and entities is performed using Automapper.
    /// </summary>
    public class ToggleDtoInput
    {
        public long Version { get; set; }
        public String Name { get; set; }
        public ICollection<ConfigurationDtoInput> Configurations { get; set; }
    }
}
