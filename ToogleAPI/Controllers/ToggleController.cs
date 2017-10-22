using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using ToogleAPI.Models;
using ToogleAPI.Interface;

namespace ToogleAPI.Controllers
{
    [Route("api/toggle")]
    public class ToggleController : Controller
    {
        private readonly IRepository<Toggle> _repository;

        public ToggleController(IRepository<Toggle> repository)
        {
            _repository = repository;
        }

        // GET api/toggle
        [HttpGet]
        public IEnumerable<Toggle> Get()
        {
            return _repository.GetAll().ToList();
        }

        // GET api/toggle/5
        [HttpGet("{id}", Name ="GetById")]
        public IActionResult Get(long id)
        {
            var item = _repository.Get(id);

            if(item == null)
            {
                return NotFound();
            }

            return new ObjectResult(item);
        }



        // POST api/toggle
        [HttpPost]
        public IActionResult Post([FromBody]Toggle toggle)
        {
            if(toggle == null)
            {
                return BadRequest() ;
            }

            _repository.Add(toggle);
            _repository.Save();

            return CreatedAtRoute("GetById", new { id = toggle.Id }, toggle);

        }

        // PUT api/toggle/5
        [HttpPut("{id}")]
        public IActionResult Put(long id, [FromBody]Toggle toggle)
        {
            if(toggle == null || toggle.Id != id)
            {
                return BadRequest();
            }

            _repository.Update(toggle);
            _repository.Save();

            return new NoContentResult();
        }

        // DELETE api/toggle/5
        [HttpDelete("{id}")]
        public IActionResult Delete(long id)
        {
            _repository.Remove(id);
            _repository.Save();

            return new NoContentResult();
        }
        
    }
}
