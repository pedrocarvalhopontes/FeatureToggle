namespace ToogleAPI.Models
{
    /// <summary>
    /// Dto used for input operations.
    /// Mapped between DTOs and entities is performed using Automapper.
    /// </summary>
    public class ConfigurationDtoInput
    {
        public string SystemName { get; set; }
        public bool Value { get; set; }
    }
}
