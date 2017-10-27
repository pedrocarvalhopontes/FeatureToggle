using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using ToggleAPI.Models.DTO;
using ToogleAPI.Interface;
using ToogleAPI.Models;

namespace ToogleAPI.Controllers
{
    [Route("api/toggles/{ToggleId}/configurations")]
    public class ConfigurationController : Controller
    {
        private readonly IRepository<Toggle> _repository;

        public ConfigurationController(IRepository<Toggle> repository)
        {
            _repository = repository;
        }

        /// <summary>
        /// Retrieves a collection of all configurations available for a given toggle that maches the <paramref name="toggleId"/>.
        /// </summary>
        /// <param name="toggleId">Unique identifier of a toggle.</param>
        /// <returns>Ok if successful, NotFound if the toggle for the given identifier doesn't exists.</returns>
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

        /// <summary>
        /// Retrieves the collection that matches the <paramref name="id"/> from configurations available for a given toggle that maches the <paramref name="toggleId"/>.
        /// </summary>
        /// <param name="toggleId">Unique identifier of a toggle.</param>
        /// <param name="id">Unique identifier of a configuration.</param>
        /// <returns>Ok if successful, NotFound if the toggle or configuration don't exist.</returns>
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

        /// <summary>
        /// Creates a new configuration for the toggle that maches the <paramref name="toggleId"/>.
        /// </summary>
        /// <param name="toggleId">Unique identifier of a toggle.</param>
        /// <param name="configuration">Input configuration</param>
        /// <returns>CreatedAtRoute if successful, NotFound if the toggle doesn't exist.</returns>
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

        /// <summary>
        /// Deletes a configuration that matches the <paramref name="id"/> from configurations available for a toggle that maches the <paramref name="toggleId"/>.
        /// </summary>
        /// <param name="toggleId">Unique identifier of a toggle.</param>
        /// <param name="id">Unique identifier of the configuration.</param>
        /// <returns>NoContent if successful, NotFound if the toggle doesn't exist.</returns>
        [HttpDelete("{id}")]
        public IActionResult DeleteConfigurationForToggle(Guid toggleId, Guid id)
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

            toggle.Configurations.Remove(configuration);
            _repository.Save();

            return NoContent();
        }

        /// <summary>
        /// Performs a full update for an existing configuration that matches the <paramref name="id"/> from configurations available for a toggle that maches the <paramref name="toggleId"/>.
        /// </summary>
        /// <param name="toggleId">Unique identifier of a toggle.</param>
        /// <param name="id">Unique identifier of a configuration.</param>
        /// <param name="newConfiguration"></param>
        /// <returns>NoContent if successful, NotFound if the toggle doesn't exist.</returns>
        [HttpPut("{id}")]
        public IActionResult UpdateConfigurationForToggle(Guid toggleId, Guid id, [FromBody]ConfigurationDtoInput newConfiguration)
        {
            var toggle = _repository.Get(toggleId);
            if (toggle == null)
            {
                return NotFound();
            }

            var existingConfiguration = toggle.Configurations.FirstOrDefault(c => c.Id == id);
            if (existingConfiguration == null)
            {
                return NotFound();
            }

            AutoMapper.Mapper.Map(newConfiguration, existingConfiguration);
            _repository.Save();

            return NoContent();
        }

    }
}