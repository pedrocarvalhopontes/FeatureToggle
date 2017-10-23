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

            var toggleConfigurations = AutoMapper.Mapper.Map<IEnumerable<ConfigurationDTO>>(toggle.Configurations);

            return Ok(toggleConfigurations);
        }

    }
}