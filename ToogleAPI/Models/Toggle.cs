using System;

namespace ToogleAPI.Models
{
    public class Toggle
    {
        public long Id { get; set; }
        public long Version { get; set; }
        public String Name { get; set; }
        public bool Value { get; set; }
    }
}
