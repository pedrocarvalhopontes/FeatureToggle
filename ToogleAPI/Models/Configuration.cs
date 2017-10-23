using System;

namespace ToogleAPI.Models
{
    public class Configuration
    {
        public Guid Id { get; set; }
        public string SystemName { get; set; }
        public bool Value { get; set; }

        //Navigation Property
        public Toggle toggle { get; set; }
    }
}
