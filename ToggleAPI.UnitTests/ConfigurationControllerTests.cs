using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ToggleAPI.Controllers;
using ToggleAPI.DAL;
using ToggleAPI.Interface;
using ToggleAPI.Mapping;
using ToggleAPI.Models;
using ToggleAPI.Models.DTO;

namespace ToggleAPI.UnitTests
{
    [TestClass]
    public class ConfigurationControllerTests
    {
        private IToggleRepository _repository;

        [TestInitialize]
        public void Initialize()
        {
            DbContextOptions<ToggleContext> options = new DbContextOptionsBuilder<ToggleContext>()
                .UseInMemoryDatabase(databaseName: "Toggle API")
                .Options;

            var context = new ToggleContext(options);
            context.EnsureSeedDataForContext();

            _repository = new ToggleRepository(context);

            var cfg = new ToggleMappingConfiguration();
            AutoMapper.Mapper.Initialize(cfg.ConfigurationAction);
        }

        [TestMethod]
        public void GetConfigurationsForToggle_Ok()
        {
            Assert.Fail();
        }

        [TestMethod]
        public void GetConfigurationsForToggle_ToggleNotFound()
        {
            var controller = new ConfigurationController(_repository);

            var result = controller.GetConfigurationsForToggle(new Guid());
            var objectResult = result as NotFoundResult;

            Assert.IsNotNull(objectResult);
            Assert.AreEqual(404, objectResult.StatusCode);
        }

        [TestMethod]
        public void GetConfigurationForToggle_Ok()
        {
            Assert.Fail();
        }

        [TestMethod]
        public void GetConfigurationForToggle_ToggleNotFound()
        {
            var controller = new ConfigurationController(_repository);

            var result = controller.GetConfigurationForToggle(new Guid(), new Guid());
            var objectResult = result as NotFoundResult;

            Assert.IsNotNull(objectResult);
            Assert.AreEqual(404, objectResult.StatusCode);
        }

        [TestMethod]
        public void GetConfigurationForToggle_ConfigurationNotFound()
        {
            Assert.Fail();
        }

        [TestMethod]
        public void CreateConfigurationForToggle_Ok()
        {
            Assert.Fail();
        }

        [TestMethod]
        public void CreateConfigurationForToggle_BadRequest()
        {
            var controller = new ConfigurationController(_repository);

            var result = controller.CreateConfigurationForToggle(new Guid(), null);
            var objectResult = result as BadRequestResult;

            Assert.IsNotNull(objectResult);
            Assert.AreEqual(400, objectResult.StatusCode);
        }

        [TestMethod]
        public void CreateConfigurationForToggle_ToggleNotFound()
        {
            var controller = new ConfigurationController(_repository);

            var result = controller.CreateConfigurationForToggle(new Guid(), new ConfigurationDtoInput());
            var objectResult = result as NotFoundResult;

            Assert.IsNotNull(objectResult);
            Assert.AreEqual(404, objectResult.StatusCode);
        }

        [TestMethod]
        public void DeleteConfigurationForToggle_Successful()
        {
            Assert.Fail();
        }

        [TestMethod]
        public void DeleteConfigurationForToggle_ToggleNotFound()
        {
            var controller = new ConfigurationController(_repository);

            var result = controller.DeleteConfigurationForToggle(new Guid(), new Guid());
            var objectResult = result as NotFoundResult;

            Assert.IsNotNull(objectResult);
            Assert.AreEqual(404, objectResult.StatusCode);
        }

        [TestMethod]
        public void DeleteConfigurationForToggle_ConfigurationNotFound()
        {
            Assert.Fail();
        }

        [TestMethod]
        public void UpdateConfigurationForToggle_Successful()
        {
            Assert.Fail();
        }

        [TestMethod]
        public void UpdateConfigurationForToggle_BadRequest()
        {
            var controller = new ConfigurationController(_repository);

            var result = controller.UpdateConfigurationForToggle(new Guid(), new Guid(), null);
            var objectResult = result as BadRequestResult;

            Assert.IsNotNull(objectResult);
            Assert.AreEqual(400, objectResult.StatusCode);
        }

        [TestMethod]
        public void UpdateConfigurationForToggle_ToggleNotFound()
        {
            var controller = new ConfigurationController(_repository);

            var result = controller.UpdateConfigurationForToggle(new Guid(), new Guid(), new ConfigurationDtoInput());
            var objectResult = result as NotFoundResult;

            Assert.IsNotNull(objectResult);
            Assert.AreEqual(404, objectResult.StatusCode);
        }

        [TestMethod]
        public void UpdateConfigurationForToggle_ConfigurationNotFound()
        {
            Assert.Fail();
        }
    }
}