using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ToogleAPI.Models
{
    public class ToogleConfiguration
    {
        public Guid Id { get; set; }
        public string SystemName { get; set; }
        public bool Value { get; set; }
    }
}
