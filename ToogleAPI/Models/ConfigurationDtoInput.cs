using System;

namespace ToogleAPI.Models
{
    public class ConfigurationDTO
    {
        public Guid Id { get; set; }
        public string SystemName { get; set; }
        public bool Value { get; set; }
        public Guid toggleId { get; set; }
    }
}
