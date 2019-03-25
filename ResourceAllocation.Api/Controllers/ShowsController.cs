using System;
using Microsoft.AspNetCore.Mvc;
using ResourceAllocation.Domain;
using ResourceAllocation.Services.Shows;

namespace ResourceAllocation.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ShowsController : ControllerBase
    {
        private readonly IShowsService _showsService;

        public ShowsController(IShowsService showsService)
        {
            _showsService = showsService;
        }

        [HttpGet]
        public IActionResult Get()
        {
            var result = _showsService.GetAll();
            return Ok(result);
        }

        [HttpGet]
        [Route("{id:Guid}")]
        public IActionResult Get(Guid id)
        {
            var result = _showsService.GetById(id);
            return Ok(result);
        }

        [HttpPut]
        public IActionResult Put(Show entity)
        {
            _showsService.Add(entity);
            return Ok();
        }

        [HttpPatch]
        public IActionResult Patch(Show entity)
        {
            _showsService.Update(entity);
            return Ok();
        }

        [HttpDelete]
        [Route("{id:Guid}")]
        public IActionResult Delete(Guid id)
        {
            _showsService.Delete(id);
            return Ok();
        }
    }
}