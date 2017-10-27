using System;

namespace ToogleAPI.Models
{
    public class ConfigurationDtoOutput
    {
        public Guid Id { get; set; }
        public string SystemName { get; set; }
        public bool Value { get; set; }
        public Guid ToggleId { get; set; }
    }
}
