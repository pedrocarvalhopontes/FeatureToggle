using System;
using System.Collections.Generic;
using System.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using ToggleAPI.Controllers;
using ToggleAPI.Interface;
using ToggleAPI.Mapping;
using ToggleAPI.Models;
using ToggleAPI.Models.DTO;

namespace ToggleAPI.UnitTests
{
    [TestClass]
    public class ToggleControllerTests
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
        public void GetAll_Ok()
        {
            _repositoryMock.Setup(rep => rep.GetAll()).Returns( new List<Toggle>());
            var controller = new ToggleController(_repositoryMock.Object);

            var result = controller.Get(null);
            var actionResult = result as OkObjectResult;
            var model = actionResult?.Value as IEnumerable<ToggleDtoOutput>;

            Assert.IsNotNull(actionResult);
            Assert.IsNotNull(model);
            _repositoryMock.Verify(rep => rep.GetAll(), Times.Once);
            Assert.AreEqual((int)HttpStatusCode.OK, actionResult.StatusCode);
        }

        [TestMethod]
        public void GetFilteredBySystemName_Ok()
        {
            var systemName = "systemName";
            _repositoryMock.Setup(rep => rep.GetAll()).Returns(new List<Toggle>());
            var controller = new ToggleController(_repositoryMock.Object);

            var result = controller.Get(systemName);
            var actionResult = result as OkObjectResult;
            var model = actionResult?.Value as IEnumerable<ToggleDtoOutput>;

            Assert.IsNotNull(actionResult);
            Assert.IsNotNull(model);
            _repositoryMock.Verify(rep => rep.GetTogglesForSystem(systemName), Times.Once);
            Assert.AreEqual((int)HttpStatusCode.OK, actionResult.StatusCode);
        }

        [TestMethod]
        public void GetById_Ok()
        {
            var id = new Guid();
            var toggle = new Toggle
            {
                Id = id,
                Name = "test"
            };
            _repositoryMock.Setup(rep => rep.Get(id)).Returns(toggle);
            var controller = new ToggleController(_repositoryMock.Object);

            var result = controller.Get(id);
            var actionResult = result as OkObjectResult;
            var model = actionResult?.Value as ToggleDtoOutput;

            Assert.IsNotNull(actionResult);
            Assert.IsNotNull(model);
            Assert.AreEqual((int)HttpStatusCode.OK, actionResult.StatusCode);
        }

        [TestMethod]
        public void GetById_NotFound()
        {
            var controller = new ToggleController(_repositoryMock.Object);

            var result = controller.Get(new Guid());
            var actionResult = result as NotFoundResult;

            Assert.IsNotNull(actionResult);
            Assert.AreEqual((int)HttpStatusCode.NotFound, actionResult.StatusCode);
        }

        [TestMethod]
        public void Post_Ok()
        {
            var toggleInput = new ToggleDtoInput();
            var controller = new ToggleController(_repositoryMock.Object);

            var result = controller.Post(toggleInput);
            var actionResult = result as CreatedAtRouteResult;
            var model = actionResult?.Value as ToggleDtoOutput;

            Assert.IsNotNull(actionResult);
            Assert.IsNotNull(model);
            _repositoryMock.Verify(rep => rep.Save(), Times.Once);
            Assert.AreEqual((int)HttpStatusCode.Created, actionResult.StatusCode);
        }

        [TestMethod]
        public void Post_EmptyToggle_BadRequest()
        {
            var controller = new ToggleController(_repositoryMock.Object);

            var result = controller.Post(null);
            var objectResult = result as BadRequestResult;

            Assert.IsNotNull(objectResult);
            Assert.AreEqual((int)HttpStatusCode.BadRequest, objectResult.StatusCode);
        }

        [TestMethod]
        public void Put_Successful()
        {
            var id = new Guid();
            var toggle = new Toggle
            {
                Id = id,
                Name = "test"
            };
            _repositoryMock.Setup(rep => rep.Get(id)).Returns(toggle);
            var toggleInput = new ToggleDtoInput();
            var controller = new ToggleController(_repositoryMock.Object);

            var result = controller.Put(id, toggleInput);
            var actionResult = result as NoContentResult;

            Assert.IsNotNull(actionResult);
            _repositoryMock.Verify(rep => rep.Save(), Times.Once);
            Assert.AreEqual((int)HttpStatusCode.NoContent, actionResult.StatusCode);
        }

        [TestMethod]
        public void Put_EmptyToggle_BadRequest()
        {
            var controller = new ToggleController(_repositoryMock.Object);

            var result = controller.Put(new Guid(), null);
            var objectResult = result as BadRequestResult;

            Assert.IsNotNull(objectResult);
            Assert.AreEqual((int)HttpStatusCode.BadRequest, objectResult.StatusCode);
        }


        [TestMethod]
        public void Put_InvalidId_BadRequest()
        {
            var controller = new ToggleController(_repositoryMock.Object);

            var result = controller.Put(new Guid(), new ToggleDtoInput());
            var objectResult = result as BadRequestResult;

            Assert.IsNotNull(objectResult);
            Assert.AreEqual((int)HttpStatusCode.BadRequest, objectResult.StatusCode);
        }

        [TestMethod]
        public void Delete_Successful()
        {
            var id = new Guid();
            _repositoryMock.Setup(rep => rep.Contains(id)).Returns(true);
            var controller = new ToggleController(_repositoryMock.Object);

            var result = controller.Delete(id);
            var actionResult = result as NoContentResult;

            Assert.IsNotNull(actionResult);
            _repositoryMock.Verify(rep => rep.Remove(id), Times.Once);
            _repositoryMock.Verify(rep => rep.Save(), Times.Once);
            Assert.AreEqual((int)HttpStatusCode.NoContent, actionResult.StatusCode);
        }

        [TestMethod]
        public void Delete_NotFound()
        {
            var controller = new ToggleController(_repositoryMock.Object);

            var result = controller.Delete(new Guid());
            var objectResult = result as NotFoundResult;

            Assert.IsNotNull(objectResult);
            Assert.AreEqual((int)HttpStatusCode.NotFound, objectResult.StatusCode);
        }

    }
}
