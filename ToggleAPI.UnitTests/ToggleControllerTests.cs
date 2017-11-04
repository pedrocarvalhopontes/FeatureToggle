using System;
using System.Collections.Generic;
using System.Linq;
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
    public class ToggleControllerTests
    {
        private IToggleRepository _repository;

        [TestInitialize]
        public void Initialize()
        {
            DbContextOptions<ToggleContext> options = new DbContextOptionsBuilder<ToggleContext>()
                .UseInMemoryDatabase(databaseName: "Toggle API")
                .Options;

            var context = new ToggleContext(options);
            //context.EnsureSeedDataForContext();

            _repository = new ToggleRepository(context);

            var cfg = new ToggleMappingConfiguration();
            AutoMapper.Mapper.Initialize(cfg.ConfigurationAction);
        }

        [TestMethod]
        public void GetAll_Ok()
        {
            var controller = new ToggleController(_repository);

            var result = controller.Get(null);
            var okResult = result as OkObjectResult;
            var model = okResult?.Value as IEnumerable<ToggleDtoOutput>;

            Assert.IsNotNull(okResult);
            Assert.IsNotNull(model);
            Assert.AreEqual(model.Count(), 3);
            Assert.AreEqual(200, okResult.StatusCode);
        }

        [TestMethod]
        public void GetFilteredBySystemName_Ok()
        {
            var controller = new ToggleController(_repository);

            var result = controller.Get("*");
            var okResult = result as OkObjectResult;
            var model = okResult?.Value as IEnumerable<ToggleDtoOutput>;

            Assert.IsNotNull(okResult);
            Assert.IsNotNull(model);
            Assert.AreEqual(model.Count(), 2);
            Assert.AreEqual(200, okResult.StatusCode);
        }

        [TestMethod]
        public void GetById_Ok()
        {
            Assert.Fail();
        }

        [TestMethod]
        public void GetById_NotFound()
        {
            var controller = new ToggleController(_repository);

            var result = controller.Get(new Guid());
            var objectResult = result as NotFoundResult;

            Assert.IsNotNull(objectResult);
            Assert.AreEqual(404, objectResult.StatusCode);
        }

        [TestMethod]
        public void Post_Ok()
        {
            Assert.Fail();
        }

        [TestMethod]
        public void Post_EmptyToggle_BadRequest()
        {
            var controller = new ToggleController(_repository);

            var result = controller.Post(null);
            var objectResult = result as BadRequestResult;

            Assert.IsNotNull(objectResult);
            Assert.AreEqual(400, objectResult.StatusCode);
        }

        [TestMethod]
        public void Put_Successful()
        {
            Assert.Fail();
        }

        [TestMethod]
        public void Put_EmptyToggle_BadRequest()
        {
            var controller = new ToggleController(_repository);

            var result = controller.Put(new Guid(), null);
            var objectResult = result as BadRequestResult;

            Assert.IsNotNull(objectResult);
            Assert.AreEqual(400, objectResult.StatusCode);
        }


        [TestMethod]
        public void Put_InvalidId_BadRequest()
        {
            var controller = new ToggleController(_repository);

            var result = controller.Put(new Guid(), new ToggleDtoInput());
            var objectResult = result as BadRequestResult;

            Assert.IsNotNull(objectResult);
            Assert.AreEqual(400, objectResult.StatusCode);
        }

        [TestMethod]
        public void Delete_Successful()
        {
            Assert.Fail();
        }

        [TestMethod]
        public void Delete_NotFound()
        {
            var controller = new ToggleController(_repository);

            var result = controller.Delete(new Guid());
            var objectResult = result as NotFoundResult;

            Assert.IsNotNull(objectResult);
            Assert.AreEqual(404, objectResult.StatusCode);
        }

    }
}
