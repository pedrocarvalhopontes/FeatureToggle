﻿using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using ToggleAPI.Interface;
using ToggleAPI.Models;
using ToggleAPI.Models.DTO;

namespace ToggleAPI.Controllers
{
    [Route("api/toggles")]
    public class ToggleController : Controller
    {
        private readonly IToggleRepository _repository;

        public ToggleController(IToggleRepository repository)
        {
            _repository = repository;
        }

        // GET api/toggles
        [HttpGet]
        public IActionResult Get(string systemName)
        {
            if (systemName != null)
            {
                return GetBySystemName(systemName);
            }

            var toggles = _repository.GetAll().ToList();
            var togglesDto = AutoMapper.Mapper.Map<IEnumerable<ToggleDtoOutput>>(toggles);

            return Ok(togglesDto);
        }

        private IActionResult GetBySystemName(string systemName)
        {
            var tooglesForSystem = _repository.GetTooglesForSystem(systemName);

            if (tooglesForSystem == null)
            {
                return NotFound();
            }
            var toggleToReturn = AutoMapper.Mapper.Map<IEnumerable<ToggleDtoOutput>>(tooglesForSystem).ToList();

            return Ok(toggleToReturn);
        }

        // GET api/toggles/5
        [HttpGet("{id}", Name ="GetById")]
        public IActionResult Get(Guid id)
        {
            var item = _repository.Get(id);

            if(item == null)
            {
                return NotFound();
            }
            var dto = AutoMapper.Mapper.Map<ToggleDtoOutput>(item);

            return Ok(dto);
        }

        // POST api/toggles
        [HttpPost]
        public IActionResult Post([FromBody]ToggleDtoInput toggleDtoInput)
        {
            if(toggleDtoInput == null)
            {
                return BadRequest() ;
            }

            var toggle = AutoMapper.Mapper.Map<Toggle>(toggleDtoInput);
            _repository.Add(toggle);
            _repository.Save();//Todo:review if we should return any error?
            var toggleDtoOutput = AutoMapper.Mapper.Map<ToggleDtoOutput>(toggle);

            return CreatedAtRoute("GetById", new { id = toggleDtoOutput.Id }, toggleDtoOutput);

        }

        // PUT api/toggles/5
        [HttpPut("{id}")]
        public IActionResult Put(Guid id, [FromBody]ToggleDtoInput newtoggle)
        {
            if(newtoggle == null)
            {
                return BadRequest();
            }

            var toggle = _repository.Get(id);
            if (toggle == null)
            {
                return BadRequest();
            }

            AutoMapper.Mapper.Map(newtoggle, toggle);

            _repository.Update(toggle);
            _repository.Save();

            return NoContent();
        }

        // DELETE api/toggles/5
        [HttpDelete("{id}")]
        public IActionResult Delete(Guid id)
        {
            if (!_repository.Contains(id))
            {
                return NotFound();
            }

            _repository.Remove(id);
            _repository.Save();

            return NoContent();
        }
        
    }
}
