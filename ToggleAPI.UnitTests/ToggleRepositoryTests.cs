﻿using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ToggleAPI.Models;
using ToggleAPI.Repository;

namespace ToggleAPI.UnitTests
{
    [TestClass]
    public class ToggleRepositoryTests
    {
        private ToggleContext _context;

        [TestInitialize]
        public void Initialize()
        {
            DbContextOptions<ToggleContext> options = new DbContextOptionsBuilder<ToggleContext>()
                .UseInMemoryDatabase(databaseName: "Toggle API")
                .Options;

            _context = new ToggleContext(options);
            _context.EnsureSeedDataForContext();
        }

        [TestMethod]
        public void Add_Successful()
        {
            var repository = new ToggleRepository(_context);
            var toggle = new Toggle
            {
                Id = new Guid(),
                Name = "isButtonBlue",
                Version = 1,
                Configurations =
                {
                    new Configuration {SystemName = "*", Value = true}
                }
            };
            var expectedSize = _context.ToggleItems.Count() + 1;

            repository.Add(toggle);
            repository.Save(); //TODO:review if we should save here
            var actualSize = _context.ToggleItems.Count();

            Assert.AreEqual(expectedSize, actualSize);
        }

        [TestMethod]
        public void Get_Successful()
        {
            var toggle = _context.ToggleItems.FirstOrDefault();
            var repository = new ToggleRepository(_context);

            var result = repository.Get(toggle.Id);

            Assert.IsNotNull(result);
            Assert.AreEqual(toggle.Id, result.Id);
        }

        //TODO:could be improved
        [TestMethod]
        public void GetAll_Successful()
        {
            var toggles = _context.ToggleItems.ToList();
            var repository = new ToggleRepository(_context);

            var result = repository.GetAll().ToList();

            Assert.IsNotNull(result);
            Assert.AreEqual(toggles.Count, result.Count);
        }

        [TestMethod]
        public void Update_Successful()
        {
            var toggle = _context.ToggleItems.First();
            var expectedVersion = toggle.Version + 1;
            var repository = new ToggleRepository(_context);

            var result = repository.Update(toggle);

            Assert.IsNotNull(result);
            Assert.AreEqual(expectedVersion, result.Version);
        }

        //TODO:could be improved
        [TestMethod]
        public void Remove_Successful()
        {
            var toggle = _context.ToggleItems.First();
            var toggleId = toggle.Id;
            var expectedSize = _context.ToggleItems.Count() - 1;
            var repository = new ToggleRepository(_context);

            repository.Remove(toggleId);
            repository.Save();
            var actualSize = _context.ToggleItems.Count();

            Assert.AreEqual(expectedSize, actualSize);
        }

        [TestMethod]
        public void Contains_isTrue()
        {
            var toggle = _context.ToggleItems.First();
            var repository = new ToggleRepository(_context);

            var result = repository.Contains(toggle.Id);

            Assert.IsTrue(result);
        }

        [TestMethod]
        public void Contains_isFalse()
        {
            var repository = new ToggleRepository(_context);

            var result = repository.Contains(new Guid());

            Assert.IsFalse(result);
        }

        [TestMethod]
        public void GetTogglesForSystem_Abc_Sucessful()
        {
            var repository = new ToggleRepository(_context);
            var systemName = "Abc";

            var result = repository.GetTogglesForSystem(systemName);

            AssertConfigurationAreOfSystem(result, systemName);
        }

        [TestMethod]
        public void GetTogglesForSystem_Default_Sucessful()
        {
            var repository = new ToggleRepository(_context);
            var systemName = "*";

            var result = repository.GetTogglesForSystem(systemName);

            AssertConfigurationAreOfSystem(result, systemName);
        }

        private void AssertConfigurationAreOfSystem(IEnumerable<Toggle> toggles, string systemName)
        {
            Assert.IsNotNull(toggles);
            foreach (var toggle in toggles)
            {
                Assert.AreEqual(1, toggle.Configurations.Count);
                var actualName = toggle.Configurations.First().SystemName;
                Assert.IsTrue(systemName.Equals(actualName) || "*".Equals(actualName));
            }
        }
    }
}
