using Microsoft.VisualStudio.TestTools.UnitTesting;
using ToggleAPI.Mapping;

namespace ToggleAPI.UnitTests
{
    [TestClass]
    public class ToggleMappingConfigurationTests
    {
        [TestMethod]
        public void ValidateMapperConfigurations()
        {
            var config = new ToggleMappingConfiguration().Configure();
            config.AssertConfigurationIsValid();
        }
    }
}
