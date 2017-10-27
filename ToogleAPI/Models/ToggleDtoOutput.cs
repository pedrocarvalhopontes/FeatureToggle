using System;
namespace ToogleAPI.Models
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
    }
}
