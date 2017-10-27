using System;
using System.Collections.Generic;
using ToogleAPI.Models;

namespace ToggleAPI.Models.DTO
{
    /// <summary>
    /// Dto used for output operations.
    /// Mapped between DTOs and entities is performed using Automapper.
    /// </summary>
    public class ToggleDtoOutput
    {
        public Guid Id { get; set; }
        public long Version { get; set; }
        public String Name { get; set; }
        public ICollection<ConfigurationDtoOutput> Configurations { get; set; }
    }
}
