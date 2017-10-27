using System;

namespace ToogleAPI.Models
{
    /// <summary>
    /// Represents a configuration entity.
    /// A configuration belongs to a unique toggle
    /// </summary>
    public class Configuration
    {
        public Guid Id { get; set; }
        public string SystemName { get; set; }
        public bool Value { get; set; }

        //Navigation Property
        public Toggle Toggle { get; set; }
    }
}
