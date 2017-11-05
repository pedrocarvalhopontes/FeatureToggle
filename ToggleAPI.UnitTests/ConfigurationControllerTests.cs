using System;
using System.Collections.Generic;
using System.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using ToggleAPI.Controllers;
using ToggleAPI.Mapping;
using ToggleAPI.Models;
using ToggleAPI.Models.DTO;

namespace ToggleAPI.UnitTests
{
    [TestClass]
    public class ConfigurationControllerTests
    {
        private Mock<IToggleRepository> _repositoryMock;

        [TestInitialize]
        public void Initialize()
        {
            _repositoryMock = new Mock<IToggleRepository>();

            var cfg = new ToggleMappingConfiguration();
            AutoMapper.Mapper.Initialize(cfg.ConfigurationAction);
        }

        [TestMethod]
        public void GetConfigurationsForToggle_Ok()
        {
            var id = new Guid();
            var toggle = new Toggle
            {
                Id = id,
                Name = "test",
                Configurations = new List<Configuration>()
            };
            _repositoryMock.Setup(rep => rep.Get(id)).Returns(toggle);
            var controller = new ConfigurationController(_repositoryMock.Object);

            var result = controller.GetConfigurationsForToggle(id);
            var actionResult = result as OkObjectResult;
            var model = actionResult?.Value as IEnumerable<ConfigurationDtoOutput>;

            Assert.IsNotNull(actionResult);
            Assert.IsNotNull(model);
            Assert.AreEqual((int)HttpStatusCode.OK, actionResult.StatusCode);

        }

        [TestMethod]
        public void GetConfigurationsForToggle_ToggleNotFound()
        {
            var controller = new ConfigurationController(_repositoryMock.Object);

            var result = controller.GetConfigurationsForToggle(new Guid());
            var objectResult = result as NotFoundResult;

            Assert.IsNotNull(objectResult);
            Assert.AreEqual((int)HttpStatusCode.NotFound, objectResult.StatusCode);
        }

        [TestMethod]
        public void GetConfigurationForToggle_Ok()
        {
            var toggleId = new Guid();
            var configurationId = new Guid();
            var toggle = new Toggle
            {
                Id = toggleId,
                Name = "test",
                Configurations = new List<Configuration> { new Configuration { Id = configurationId } }
            };
            _repositoryMock.Setup(rep => rep.Get(toggleId)).Returns(toggle);
            var controller = new ConfigurationController(_repositoryMock.Object);

            var result = controller.GetConfigurationForToggle(toggleId, configurationId);
            var actionResult = result as OkObjectResult;
            var model = actionResult?.Value as ConfigurationDtoOutput;

            Assert.IsNotNull(actionResult);
            Assert.IsNotNull(model);
            Assert.AreEqual((int)HttpStatusCode.OK, actionResult.StatusCode);
        }

        [TestMethod]
        public void GetConfigurationForToggle_ToggleNotFound()
        {
            var controller = new ConfigurationController(_repositoryMock.Object);

            var result = controller.GetConfigurationForToggle(new Guid(), new Guid());
            var objectResult = result as NotFoundResult;

            Assert.IsNotNull(objectResult);
            Assert.AreEqual((int)HttpStatusCode.NotFound, objectResult.StatusCode);
        }

        [TestMethod]
        public void GetConfigurationForToggle_ConfigurationNotFound()
        {
            var toggleId = new Guid();
            var configurationId = new Guid();
            var toggle = new Toggle
            {
                Id = toggleId,
                Name = "test",
                Configurations = new List<Configuration>()
            };
            _repositoryMock.Setup(rep => rep.Get(toggleId)).Returns(toggle);
            var controller = new ConfigurationController(_repositoryMock.Object);

            var result = controller.GetConfigurationForToggle(toggleId, configurationId);
            var objectResult = result as NotFoundResult;

            Assert.IsNotNull(objectResult);
            Assert.AreEqual((int)HttpStatusCode.NotFound, objectResult.StatusCode);
        }

        [TestMethod]
        public void CreateConfigurationForToggle_Ok()
        {
            var toggleId = new Guid();
            var toggle = new Toggle();
            var configurationInput = new ConfigurationDtoInput();
            _repositoryMock.Setup(rep => rep.Get(toggleId)).Returns(toggle);
            var controller = new ConfigurationController(_repositoryMock.Object);

            var result = controller.CreateConfigurationForToggle(toggleId, configurationInput);
            var actionResult = result as CreatedAtRouteResult;
            var model = actionResult?.Value as ConfigurationDtoOutput;

            Assert.IsNotNull(actionResult);
            Assert.IsNotNull(model);
            _repositoryMock.Verify(rep => rep.Save(), Times.Once);
            Assert.AreEqual((int)HttpStatusCode.Created, actionResult.StatusCode);
        }

        [TestMethod]
        public void CreateConfigurationForToggle_BadRequest()
        {
            var controller = new ConfigurationController(_repositoryMock.Object);

            var result = controller.CreateConfigurationForToggle(new Guid(), null);
            var objectResult = result as BadRequestResult;

            Assert.IsNotNull(objectResult);
            Assert.AreEqual((int)HttpStatusCode.BadRequest, objectResult.StatusCode);
        }

        [TestMethod]
        public void CreateConfigurationForToggle_ToggleNotFound()
        {
            var controller = new ConfigurationController(_repositoryMock.Object);

            var result = controller.CreateConfigurationForToggle(new Guid(), new ConfigurationDtoInput());
            var objectResult = result as NotFoundResult;

            Assert.IsNotNull(objectResult);
            Assert.AreEqual((int)HttpStatusCode.NotFound, objectResult.StatusCode);
        }

        [TestMethod]
        public void DeleteConfigurationForToggle_Successful()
        {
            var toggleId = new Guid();
            var configurationId = new Guid();
            var configuration = new Configuration {Id = configurationId};
            var toggle = new Toggle
            {
                Id = toggleId,
                Name = "test",
                Configurations = new List<Configuration> { configuration }
            };
            _repositoryMock.Setup(rep => rep.Get(toggleId)).Returns(toggle);
            var controller = new ConfigurationController(_repositoryMock.Object);

            var result = controller.DeleteConfigurationForToggle(toggleId, configurationId);
            var actionResult = result as NoContentResult;

            Assert.IsNotNull(actionResult);
            _repositoryMock.Verify(rep => rep.Save(), Times.Once);
            Assert.AreEqual((int)HttpStatusCode.NoContent, actionResult.StatusCode);
        }

        [TestMethod]
        public void DeleteConfigurationForToggle_ToggleNotFound()
        {
            var controller = new ConfigurationController(_repositoryMock.Object);

            var result = controller.DeleteConfigurationForToggle(new Guid(), new Guid());
            var objectResult = result as NotFoundResult;

            Assert.IsNotNull(objectResult);
            Assert.AreEqual((int)HttpStatusCode.NotFound, objectResult.StatusCode);
        }

        [TestMethod]
        public void DeleteConfigurationForToggle_ConfigurationNotFound()
        {
            var toggleId = new Guid();
            var configurationId = new Guid();
            var toggle = new Toggle
            {
                Id = toggleId,
                Name = "test",
                Configurations = new List<Configuration>()
            };
            _repositoryMock.Setup(rep => rep.Get(toggleId)).Returns(toggle);
            var controller = new ConfigurationController(_repositoryMock.Object);

            var result = controller.DeleteConfigurationForToggle(toggleId, configurationId);
            var objectResult = result as NotFoundResult;

            Assert.IsNotNull(objectResult);
            Assert.AreEqual((int)HttpStatusCode.NotFound, objectResult.StatusCode);
        }

        [TestMethod]
        public void UpdateConfigurationForToggle_Successful()
        {
            var toggleId = new Guid();
            var configurationId = new Guid();
            var configuration = new Configuration { Id = configurationId };
            var toggle = new Toggle
            {
                Id = toggleId,
                Name = "test",
                Configurations = new List<Configuration> { configuration }
            };
            _repositoryMock.Setup(rep => rep.Get(toggleId)).Returns(toggle);
            var controller = new ConfigurationController(_repositoryMock.Object);

            var result =
                controller.UpdateConfigurationForToggle(toggleId, configurationId, new ConfigurationDtoInput());
            var objectResult = result as NoContentResult;

            Assert.IsNotNull(objectResult);
            Assert.AreEqual((int)HttpStatusCode.NoContent, objectResult.StatusCode);
        }

        [TestMethod]
        public void UpdateConfigurationForToggle_BadRequest()
        {
            var controller = new ConfigurationController(_repositoryMock.Object);

            var result = controller.UpdateConfigurationForToggle(new Guid(), new Guid(), null);
            var objectResult = result as BadRequestResult;

            Assert.IsNotNull(objectResult);
            Assert.AreEqual((int)HttpStatusCode.BadRequest, objectResult.StatusCode);
        }

        [TestMethod]
        public void UpdateConfigurationForToggle_ToggleNotFound()
        {
            var controller = new ConfigurationController(_repositoryMock.Object);

            var result = controller.UpdateConfigurationForToggle(new Guid(), new Guid(), new ConfigurationDtoInput());
            var objectResult = result as NotFoundResult;

            Assert.IsNotNull(objectResult);
            Assert.AreEqual((int)HttpStatusCode.NotFound, objectResult.StatusCode);
        }

        [TestMethod]
        public void UpdateConfigurationForToggle_ConfigurationNotFound()
        {
            var toggleId = new Guid();
            var configurationId = new Guid();
            var toggle = new Toggle
            {
                Id = toggleId,
                Name = "test",
                Configurations = new List<Configuration>()
            };
            _repositoryMock.Setup(rep => rep.Get(toggleId)).Returns(toggle);
            var controller = new ConfigurationController(_repositoryMock.Object);

            var result = controller.UpdateConfigurationForToggle(toggleId, configurationId, new ConfigurationDtoInput());
            var objectResult = result as NotFoundResult;

            Assert.IsNotNull(objectResult);
            Assert.AreEqual((int)HttpStatusCode.NotFound, objectResult.StatusCode);
        }
    }
}