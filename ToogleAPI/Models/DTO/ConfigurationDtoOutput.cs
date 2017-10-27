using System;

namespace ToggleAPI.Models.DTO
{
    /// <summary>
    /// Dto used for output operations.
    /// Mapped between DTOs and entities is performed using Automapper.
    /// </summary>
    public class ConfigurationDtoOutput
    {
        public Guid Id { get; set; }
        public string SystemName { get; set; }
        public bool Value { get; set; }
        public Guid ToggleId { get; set; }
    }
}
