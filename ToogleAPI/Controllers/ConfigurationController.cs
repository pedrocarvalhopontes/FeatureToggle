using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ToogleAPI.Interface;
using ToogleAPI.Models;

namespace ToogleAPI.Controllers
{
    [Route("api/toggles/{toggleId}/configurations")]
    public class ConfigurationController : Controller
    {
        private readonly IRepository<Toggle> _repository;

        public ConfigurationController(IRepository<Toggle> repository)
        {
            _repository = repository;
        }

        [HttpGet]
        public IActionResult GetConfigurationsForToggle(Guid toggleId)
        {
            var toggle = _repository.Get(toggleId);

            if (toggle == null)
            {
                return NotFound();
            }

            var toggleConfigurations = AutoMapper.Mapper.Map<IEnumerable<ConfigurationDtoOutput>>(toggle.Configurations);

            return Ok(toggleConfigurations);
        }

        [HttpGet("{id}", Name ="GetConfigurationForToggle")]
        public IActionResult GetConfigurationForToggle(Guid toggleId, Guid id)
        {
            var toggle = _repository.Get(toggleId);
            if (toggle == null)
            {
                return NotFound();
            }

            var configuration = toggle.Configurations.FirstOrDefault(c => c.Id == id);
            if (configuration == null)
            {
                return NotFound();
            }

            var toggleConfiguration = AutoMapper.Mapper.Map<ConfigurationDtoOutput>(configuration);
            return Ok(toggleConfiguration);
        }

        [HttpPost]
        public IActionResult CreateConfigurationForToggle(Guid toggleId, [FromBody]ConfigurationDtoInput configuration)
        {
            if (configuration == null)
            {
                return BadRequest();
            }

            var toggle = _repository.Get(toggleId);

            if (toggle == null)
            {
                return NotFound();
            }

            var configurationEntity = AutoMapper.Mapper.Map<Configuration>(configuration);
            toggle.Configurations.Add(configurationEntity);
            _repository.Save();

            var configurationOutput = AutoMapper.Mapper.Map<ConfigurationDtoOutput>(configurationEntity);

            return CreatedAtRoute("GetConfigurationForToggle",
                new { toggleId = toggleId, id = configurationOutput.Id },
                configurationOutput);
        }

    }
}